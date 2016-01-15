#region License information
/*

  Copyright (c) 2016 Togocoder (http://www.codeproject.com/Members/Kim-Togo)
 
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
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Gett.Sharing
{
    /// <summary>
    /// Represent all shares on Ge.tt for user.
    /// http://ge.tt/developers - REST API for Ge.tt Web service - share API.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Shares for user = {_gettUser.Me.FullName}, {_gettUser.Me.Email}")]
    public class GettShares : GettBaseUri
    {
        #region HashCode and Equals overrides
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            // ReSharper disable once NonReadonlyFieldInGetHashCode
            return _gettUser == null ? base.GetHashCode() : _gettUser.Me.UserId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var shares = obj as GettShares;
            return shares != null && shares._gettUser.Me.UserId == _gettUser.Me.UserId;
        }
        #endregion

        #region Variables
        /// <summary>
        /// Provides access to GettUser object, Token etc.
        /// </summary>
        private readonly GettUser _gettUser;
        #endregion

        #region ctor
        /// <summary>
        /// Internal ctor, GettUser class creates one instance for this class.
        /// </summary>
        /// <param name="gettUser"></param>
        internal GettShares(GettUser gettUser)
        {
            _gettUser = gettUser;
        }
        #endregion

        #region Create new share - /shares/create
        /// <summary>
        /// Creates a new share.
        /// </summary>
        /// <param name="title">Optional title of the share</param>
        /// <returns>New _gettShare class for manipulation share and files</returns>
        public GettShare CreateShare(string title = null)
        {
            // Argument.
            var jsonArgument = new JObject(new JProperty("title", title));

            // Build Uri.
            var baseUri = new UriBuilder(ShareCreate) { Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken) };

            // POST request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            gett.Headers.Add("Content-Type", "application/json");
            byte[] request = Encoding.UTF8.GetBytes(jsonArgument.ToString());
            byte[] response = gett.UploadData(baseUri.Uri, request);

            // Response.
            var shareInfo = JsonConvert.DeserializeObject<GettShare.ShareInfo>(Encoding.UTF8.GetString(response));

            // Create a _gettShare class for the newly created share.
            return new GettShare(_gettUser, shareInfo);
        }

        /// <summary>
        /// Async version of CreateShare
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<GettShare> CreateShareAsync(string title = null)
        {
            return Task.Factory.StartNew(() => CreateShare(title));
        }

        /// <summary>
        /// Begins an asynchronous creates a new share.
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        /// <param name="title">Optional title of the share</param>
        public IAsyncResult BeginCreateShare(AsyncCallback callBack, object state, string title = null)
        {
            Task<GettShare> t = Task.Factory.StartNew(_ => CreateShare(title), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous CreateShare. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>New _gettShare class for manipulation share and files</returns>
        public GettShare EndCreateShare(IAsyncResult ar)
        {
            try
            {
                var t = (Task<GettShare>)ar;

                if (t.IsFaulted && t.Exception != null)
                    throw t.Exception.InnerException;

                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }
        #endregion

        #region Get access to existing shares - /shares - /shares/{sharename} -
        /// <summary>
        /// Find a specific share by share name.
        /// </summary>
        /// <param name="name">Name of share</param>
        /// <returns>New _gettShare class for manipulation share and files</returns>
        public GettShare GetShare(string name)
        {
            // Build Uri.
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(ShareList, name);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // GET request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            byte[] response = gett.DownloadData(baseUri.Uri);

            // Response.
            var shareInfo = JsonConvert.DeserializeObject<GettShare.ShareInfo>(Encoding.UTF8.GetString(response));

            // Create a _gettShare class.
            return new GettShare(_gettUser, shareInfo);
        }

        /// <summary>
        /// Async version of GetShare
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<GettShare> GetShareAsync(string name)
        {
            return Task.Factory.StartNew(() => GetShare(name));
        }

        /// <summary>
        /// Begins an asynchronous find a specific share by share name.
        /// </summary>
        /// <param name="name">Name of share</param>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        public IAsyncResult BeginGetShare(string name, AsyncCallback callBack, object state)
        {
            Task<GettShare> t = Task.Factory.StartNew(_ => GetShare(name), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous GetShare. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>New _gettShare class for manipulation share and files</returns>
        public GettShare EndGetShare(IAsyncResult ar)
        {
            try
            {
                var t = (Task<GettShare>)ar;

                if (t.IsFaulted && t.Exception != null)
                    throw t.Exception.InnerException;

                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        /// <summary>
        /// Returns all shares and the containing files.
        /// </summary>
        /// <returns>An array of new _gettShare class for manipulation share and files</returns>
        public GettShare[] GetShares()
        {
            // Build Uri.
            var baseUri = new UriBuilder(ShareListAll) { Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken) };

            // GET request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            byte[] response = gett.DownloadData(baseUri.Uri);

            //string s = Encoding.UTF8.GetString(response);

            // Response.
            var shareInfos = JsonConvert.DeserializeObject<GettShare.ShareInfo[]>(Encoding.UTF8.GetString(response));

            // Creates an array of _gettShare class.
            return (from shareInfo in shareInfos
                    select new GettShare(_gettUser, shareInfo)).ToArray();
        }

        /// <summary>
        /// Async version of GetShares
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<GettShare[]> GetSharesAsync()
        {
            return Task.Factory.StartNew(GetShares);
        }

        /// <summary>
        /// Begins an asynchronous returns all shares and the containing files.
        /// </summary>
        public IAsyncResult BeginGetShares(AsyncCallback callBack, object state)
        {
            Task<GettShare[]> t = Task.Factory.StartNew(_ => GetShares(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous GetShares. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>An array of new _gettShare class for manipulation share and files</returns>
        public GettShare[] EndGetShares(IAsyncResult ar)
        {
            try
            {
                var t = (Task<GettShare[]>)ar;

                if (t.IsFaulted && t.Exception != null)
                    throw t.Exception.InnerException;

                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }
        #endregion
    }
}
