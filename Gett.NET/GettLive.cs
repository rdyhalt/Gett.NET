#region License information
/*

  Copyright (c) 2014 Togocoder (http://www.codeproject.com/Members/Kim-Togo)
 
  This file is part of Gett.NET library that uses the Ge.tt REST API, http://ge.tt/developers
  
  Gett.NET is a free library: you can redistribute it and/or modify as nessery
  it under the terms of The Code Project Open License (CPOL) as published by
  the The Code Project, either version 1.02 of the License, or (at your option) any later version.

  Gett.NET is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY OF ANY KIND. The software is provided "as-is".
 
  Please read the The Code Project Open License (CPOL) at http://www.codeproject.com/info/cpol10.aspx

  I would appreciate getting an email, if you choose to use this library in your own work.
  Send an email to togocoder(at)live.com with a little description of your work, thanks! :-)

  ---
  Gett.NET library makes heavy use of Json.NET - http://json.codeplex.com/ for serialize and deserialize
  of JSON class, Newtonsoft.Json.dll.
 
  License information on Json.NET, see http://json.codeplex.com/license.
  Copyright (c) 2007 James Newton-King

  Permission is hereby granted, free of charge, to any person obtaining a copy of
  this software and associated documentation files (the "Software"), to deal in the Software without restriction,
  including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
  and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
  subject to the following conditions:

  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 
  ---
  Gett.NET library make use of WebSocket protocol client from https://github.com/sta/websocket-sharp
  The library is used to make a connection to Ge.tt Live API.
  Please see WsStream.cs for license information.

*/
#endregion

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

// ReSharper disable once CheckNamespace
namespace Gett.Sharing.Live
{
    /// <summary>
    /// Represent a single connection to Ge.tt Live API
    /// http://ge.tt/developers - Live API for Ge.tt Web service.
    /// </summary>
    public class GettLive
    {
        #region JSON Serialize/Deserialize classes
        [System.Diagnostics.DebuggerDisplay("FileId = {FileId}, FileName = {FileName}, ShareName = {ShareName}")]
        [JsonObject(MemberSerialization.OptOut)]
        public class LiveFileEventInfo
        {
            [JsonIgnore]
            public readonly DateTime Timestamp = DateTime.Now;
            [JsonProperty("type")]
            public string Type = null;
            [JsonProperty("sharename")]
            public string ShareName = null;
            [JsonProperty("filename")]
            public string FileName = null;
            [JsonProperty("fileid")]
            public string FileId = null;
            [JsonProperty("size")]
            public long Size = 0;
            [JsonProperty("reason")]
            public string Reason = null;
        }
        #endregion

        #region Variables
        /// <summary>
        /// Provides access to GettUser object, Token etc.
        /// </summary>
        private readonly GettUser _gettUser;

        /// <summary>
        /// Connection to WebSocket.
        /// </summary>
        private WebSocket _webSocket;

        /// <summary>
        /// Last received from Ge.tt Live.
        /// </summary>
        public DateTime LastReceived { get; private set; }

        /// <summary>
        /// Session id for this Live connection.
        /// </summary>
        public string Session { get; private set; }

        /// <summary>
        /// WatchDog timer. Restarts Live API connection if no "ping" is received for over 2 min.
        /// </summary>
        private System.Threading.Timer _watchDog;

        /// <summary>
        /// Thread-safe handling of WatchDog timer
        /// </summary>
        private readonly object _watchDogSyncRoot = new object();

        /// <summary>
        /// When WatchDog restarts connection
        /// </summary>
        public TimeSpan WatchDogRestartTimeSpan = new TimeSpan(0, 1, 30);
        #endregion

        #region Common information about this file
        /// <summary>
        /// Parent of this class. GettUser class.
        /// </summary>
        public GettUser Parent { get { return _gettUser; } }
        #endregion

        #region ctor
        /// <summary>
        /// Internal ctor, GettUser class creates instances for this class.
        /// </summary>
        /// <param name="gettUser"></param>
        internal GettLive(GettUser gettUser)
        {
            _gettUser = gettUser;
        }
        #endregion

        #region WebSocket events for own data handling, reconnecting etc.
        /// <summary>
        /// Live API open, sending 'connect' login request
        /// </summary>
        public event Action<GettLive> OnOpen;

        /// <summary>
        /// Live API has been closed.
        /// </summary>
        public event Action<GettLive> OnClose;

        /// <summary>
        /// There has been an error.
        /// </summary>
        public event Action<GettLive, string> OnError;

        /// <summary>
        /// Raw data from Live API.
        /// </summary>
        public event Action<GettLive, string> OnMessage;
        #endregion

        #region Starting and ending Live session
        /// <summary>
        /// Start connection to Ge.tt Live with the given session id.
        /// If a file in your share is downloaded, a notifications is received.
        /// </summary>
        /// <param name="session">Session id of you own choice</param>
        /// <param name="useWatchDog">Use a watch dog timer to automatic check if reconnect is required</param>
        public void Start(string session, bool useWatchDog)
        {
            if (_webSocket == null)
            {
                if (useWatchDog)
                {
                    lock (_watchDogSyncRoot)
                    {
                        if (_watchDog == null)
                        {
                            _watchDog = new System.Threading.Timer(WatchDog);
                            _watchDog.Change(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30));
                        }
                    }
                }

                Session = session;
                _webSocket = new WebSocket(GettBaseUri.BaseUriLive, "wss");
                _webSocket.OnOpen += WebSocket_OnOpen;
                _webSocket.OnClose += WebSocket_OnClose;
                _webSocket.OnError += WebSocket_OnError;
                _webSocket.OnMessage += WebSocket_OnMessage;

                _webSocket.Connect();
            }
        }

        /// <summary>
        /// Check last received Date/Time. If time span is over WatchDogRestartTimeSpan, watch dog will restart connection to Live API
        /// </summary>
        /// <param name="state"></param>
        private void WatchDog(object state)
        {
            // Is it time to restart connection ?
            if ((DateTime.Now - LastReceived) > WatchDogRestartTimeSpan)
            {
                System.Diagnostics.Debug.WriteLine("Restart connection, Last receivce:" + LastReceived + ", TimeSpan:" + (DateTime.Now - LastReceived), "WS");
                Restart();
            }
        }

        /// <summary>
        /// Stop connection to Ge.tt Live.
        /// </summary>
        public void Stop()
        {
            if (_webSocket != null)
            {
                lock (_watchDogSyncRoot)
                {
                    if (_watchDog != null)
                    {
                        _watchDog.Dispose();
                        _watchDog = null;
                    }
                }

                _webSocket.OnOpen -= WebSocket_OnOpen;
                _webSocket.OnClose -= WebSocket_OnClose;
                _webSocket.OnError -= WebSocket_OnError;
                _webSocket.OnMessage -= WebSocket_OnMessage;
                _webSocket.Close();
            }
        }

        /// <summary>
        /// Restart connection to Ge.tt Live.
        /// </summary>
        public void Restart()
        {
            if (_webSocket != null)
            {
                _webSocket.Close();
                _webSocket.Connect();
            }
        }
        #endregion

        #region WebSocket events
        /// <summary>
        /// On WebSocket open connection.
        /// Send "connect" argument.
        /// </summary>
        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine("OnOpen, send 'connect' request", "WS");

                // Argument.
                var jsonArgument = new JObject(new JProperty("type", "connect"), new JProperty("accesstoken", _gettUser.Token.AccessToken), new JProperty("session", Session));

                // Send "connect" Json.
                _webSocket.Send(jsonArgument.ToString());
                _webSocket.Send("ping");

                if (OnOpen != null)
                    OnOpen(this);
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("OnOpen, handling exception: " + ex.GetType().ToString() + ", " + ex.Message, "WS");

                if (OnError != null)
                    OnError(this, ex.Message);
            }
        }

        /// <summary>
        /// On WebSocket close connection
        /// </summary>
        private void WebSocket_OnClose(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnClose event");
            if (OnClose != null)
                OnClose(this);
        }

        /// <summary>
        /// On WebSocket error, connection abort etc.
        /// </summary>
        private void WebSocket_OnError(object sender, string eventdata)
        {
            //System.Diagnostics.Debug.WriteLine("OnError event: [" + eventdata + "]", "WS");
            if (OnError != null)
                OnError(this, eventdata);
        }

        /// <summary>
        /// On WebSocket new message received.
        /// </summary>
        private void WebSocket_OnMessage(object sender, string eventdata)
        {
            // Base JSON message format, except for "ping" message.
            //{
            //  "type":"the message type"
            //}
            //System.Diagnostics.Debug.WriteLine("WebSocket_OnMessage event: [" + eventdata + "]");
            try
            {
                // Got a "ping" message from Live API
                if (eventdata == "ping")
                {
                    // Send "pong" message, we are still alive.
                    _webSocket.Send("pong");
                    LastReceived = DateTime.Now;
                }
                else if (eventdata == "pong")
                {
                    // Got a "pong" message for ower own "ping" message.
                    LastReceived = DateTime.Now;
                }
                else
                {
                    try
                    {
                        JObject jsonResponse = JObject.Parse(eventdata);
                        LiveFileEventInfo fileLiveEvent = null;

                        // What type of Live event?
                        switch (jsonResponse["type"].ToString())
                        {
                            case "download": fileLiveEvent = JsonConvert.DeserializeObject<LiveFileEventInfo>(eventdata); break; // Download event.
                            case "storagelimit": fileLiveEvent = JsonConvert.DeserializeObject<LiveFileEventInfo>(eventdata); break; // Storagelimit event.
                            case "filestat": fileLiveEvent = JsonConvert.DeserializeObject<LiveFileEventInfo>(eventdata); break; // Filestatus event.
                            case "violatedterms": fileLiveEvent = JsonConvert.DeserializeObject<LiveFileEventInfo>(eventdata); break; // Violated terms event.
                        }

                        if (fileLiveEvent != null)
                        {
                            LastReceived = DateTime.Now;
                            OnFileEvent(fileLiveEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("WebSocket_OnMessage, Json exception: " + ex.GetType() + ", " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WebSocket_OnMessage, handling exception: " + ex.GetType() + ", " + ex.Message);
            }

            if (OnMessage != null)
            {
                OnMessage(this, eventdata);
            }

        }
        #endregion

        #region Events from Live API
        /// <summary>
        /// Event that is called every time a download, storagelimit, filestat or violatedterms is received.
        /// </summary>
        public event Action<LiveFileEventInfo> LiveFileEvent;

        /// <summary>
        /// Signal Live event on file.
        /// </summary>
        /// <param name="fileLiveEvent">The file and share</param>
        private void OnFileEvent(LiveFileEventInfo fileLiveEvent)
        {
            var copyLiveFileEvent = LiveFileEvent;
            if (copyLiveFileEvent != null)
            {
                copyLiveFileEvent(fileLiveEvent);
            }
        }
        #endregion
    }
}
