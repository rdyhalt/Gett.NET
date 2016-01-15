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
    /// Represent a single share for user.
    /// http://ge.tt/developers - REST API for Ge.tt Web service - share API.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("ShareName = {GettShareInfo.ShareName}, Title = {GettShareInfo.Title}, CreatedUtc = {GettShareInfo.CreatedUtc}")]
    public class GettShare : GettBaseUri
    {
        #region HashCode and Equals overrides
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            // ReSharper disable NonReadonlyFieldInGetHashCode
            return GettShareInfo == null ? base.GetHashCode() : GettShareInfo.ShareName.GetHashCode();
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        public override bool Equals(object obj)
        {
            var share = obj as GettShare;
            return share != null && share.GettShareInfo.ShareName == GettShareInfo.ShareName;
        }

        public override string ToString()
        {
            return string.Format("Title:{0}, Files:{1}, CreatedUtc:{2}", string.IsNullOrEmpty(GettShareInfo.Title) ? GettShareInfo.ShareName : GettShareInfo.Title, GettShareInfo.Files.Length, GettShareInfo.CreatedUtc);
        }
        #endregion

        #region JSON Serialize/Deserialize classes
        [JsonObject(MemberSerialization.OptOut)]
        [System.Diagnostics.DebuggerDisplay("ShareInfo = {ShareName}, Title = {Title}, CreatedUtc = {CreatedUtc}")]
        public class ShareInfo
        {
            [JsonIgnore]
            public readonly DateTime Timestamp = DateTime.Now;
            [JsonProperty("sharename")]
            public string ShareName = null;
            [JsonProperty("readystate")]
            public string ReadyState = null;
            [JsonProperty("created")]
            [JsonConverter(typeof(Newtonsoft.Json.Converters.UnixDateTimeConverter))]
            public DateTime? CreatedUtc = null;
            [JsonProperty("title")]
            public string Title = null;
            [JsonProperty("getturl")]
            public string GettUrl = null;
            [JsonProperty("userid")]
            public string UserId = null;
            [JsonProperty("fullname")]
            public string Fullname = null;
            [JsonProperty("files")]
            public GettFile.FileInfo[] Files = new GettFile.FileInfo[0];
        }
        #endregion

        #region Variables
        /// <summary>
        /// Provides access to GettUser object, Token etc.
        /// </summary>
        private readonly GettUser _gettUser;

        /// <summary>
        /// Provides information on what Share we are working on.
        /// </summary>
        internal ShareInfo GettShareInfo;
        #endregion

        #region ctor
        /// <summary>
        /// Internal ctor, GettShares class creates new instance for this class
        /// </summary>
        /// <param name="gettUser">GettUser class</param>
        /// <param name="gettShareInfo">Information for this share</param>
        internal GettShare(GettUser gettUser, ShareInfo gettShareInfo)
        {
            _gettUser = gettUser;
            GettShareInfo = gettShareInfo;
        }
        #endregion

        #region Common information on share and files in share
        /// <summary>
        /// Parent of this class. GettUser class.
        /// </summary>
        public GettUser Parent { get { return _gettUser; } }

        /// <summary>
        /// Access to share and file information. Can be used to sort, select etc.
        /// </summary>
        public ShareInfo Info { get { return GettShareInfo; } }

        /// <summary>
        /// Access to files information in this share. Can be used to sort, select etc.
        /// </summary>
        public GettFile.FileInfo[] FilesInfo { get { return GettShareInfo.Files; } }
        #endregion

        #region Refresh share information - /shares/{sharename}
        /// <summary>
        /// Refresh share information and all files.
        /// </summary>
        public bool Refresh()
        {
            // Build Uri.
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(ShareList, GettShareInfo.ShareName);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // GET request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            byte[] response = gett.DownloadData(baseUri.Uri);

            // Response.
            var shareInfo = JsonConvert.DeserializeObject<ShareInfo>(Encoding.UTF8.GetString(response));
            GettShareInfo = shareInfo;
            return true;
        }

        /// <summary>
        /// Async version of Refresh
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> RefreshAsync()
        {
            return Task.Factory.StartNew(Refresh);
        }

        /// <summary>
        /// Begins an asynchronous refresh share information and all files.
        /// </summary>
        public IAsyncResult BeginRefresh(AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => Refresh(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous Refresh. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndRefresh(IAsyncResult ar)
        {
            try
            {
                var t = (Task<bool>)ar;

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

        #region Change share information - /shares/{sharename}/update
        /// <summary>
        /// Change title for share.
        /// </summary>
        /// <param name="title">New title</param>
        public bool SetTitle(string title)
        {
            // Argument.
            var jsonArgument = new JObject(new JProperty("title", string.IsNullOrEmpty(title) ? null : title)); // "title":"my new title" or "title":null to delete it

            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(ShareUpdate, GettShareInfo.ShareName);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // POST request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            gett.Headers.Add("Content-Type", "application/json");
            byte[] request = Encoding.UTF8.GetBytes(jsonArgument.ToString());
            byte[] response = gett.UploadData(baseUri.Uri, request);

            // Response.
            var shareInfo = JsonConvert.DeserializeObject<ShareInfo>(Encoding.UTF8.GetString(response));
            GettShareInfo.Title = shareInfo.Title;
            return true;
        }

        /// <summary>
        /// Async version of GetShares
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> SetTitleAsync(string title)
        {
            return Task.Factory.StartNew(() => SetTitle(title));
        }

        /// <summary>
        /// Begins an asynchronous change title for share.
        /// </summary>
        /// <param name="title">New title</param>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        public IAsyncResult BeginSetTitle(string title, AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => SetTitle(title), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous SetTitle. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndSetTitle(IAsyncResult ar)
        {
            try
            {
                var t = (Task<bool>)ar;

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

        #region Delete a share and all of its files. - /shares/{sharename}/destroy
        /// <summary>
        /// Delete the share and all of its files.
        /// </summary>
        public bool Destroy()
        {
            // Build Uri.
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(ShareDestroy, GettShareInfo.ShareName);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // POST request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            var request = new byte[0];
            byte[] response = gett.UploadData(baseUri.Uri, request);

            // Response.
            string jsonResponse = Encoding.UTF8.GetString(response);
            return jsonResponse.Contains("true");
        }

        /// <summary>
        /// Async version of Destroy
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> DestroyAsync()
        {
            return Task.Factory.StartNew(Destroy);
        }

        /// <summary>
        /// Begins an asynchronous delete the share and all of its files.
        /// </summary>
        public IAsyncResult BeginDestroy(AsyncCallback callBack, object state)
        {
            Task t = Task.Factory.StartNew(_ => Destroy(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous Destroy. 
        /// </summary>
        /// <param name="ar"></param>
        public void EndDestroy(IAsyncResult ar)
        {
            try
            {
                var t = (Task)ar;

                if (t.IsFaulted && t.Exception != null)
                    throw t.Exception.InnerException;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }
        #endregion

        #region Create new file - /files/{sharename}/create
        /// <summary>
        /// To upload files to Ge.tt you must first create the file under a given share.
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <param name="session">Optional, only used by the Live API</param>
        /// <returns>New GettFile class for manipulation file</returns>
        public GettFile CreateFile(string fileName, string session = null)
        {
            // Argument.
            var jsonArgument = new JObject(new JProperty("filename", fileName));

            // Optional - Only used by the Live API - https://open.ge.tt/1/doc/rest#files/{sharename}/create
            if (!string.IsNullOrEmpty(session))
                jsonArgument.Add(new JProperty("session", session));

            // Build Uri.
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileCreate, GettShareInfo.ShareName);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // POST request.
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            gett.Headers.Add("Content-Type", "application/json");
            byte[] request = Encoding.UTF8.GetBytes(jsonArgument.ToString());
            byte[] response = gett.UploadData(baseUri.Uri, request);

            // Response.
            var fileInfo = JsonConvert.DeserializeObject<GettFile.FileInfo>(Encoding.UTF8.GetString(response));

            // Create a GettFile class for the newly created file.
            return new GettFile(_gettUser, this, fileInfo);
        }

        /// <summary>
        /// Async version of CreateFile
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<GettFile> CreateFileAsync(string fileName, string session = null)
        {
            return Task.Factory.StartNew(() => CreateFile(fileName, session));
        }

        /// <summary>
        /// Begins an asynchronous upload file.
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <param name="state"></param>
        /// <param name="session">Optional, only used by the Live API</param>
        /// <param name="callBack"></param>
        public IAsyncResult BeginCreateFile(string fileName, AsyncCallback callBack, object state, string session = null)
        {
            Task<GettFile> t = Task.Factory.StartNew(_ => CreateFile(fileName, session), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous CreateFile. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>New GettFile class for manipulation file</returns>
        public GettFile EndCreateFile(IAsyncResult ar)
        {
            try
            {
                var t = (Task<GettFile>)ar;

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

        #region Access to files under this share
        /// <summary>
        /// Find a specefic file from fileid in this share.
        /// Remember, file information is only refreshed when method Refresh() is called.
        /// </summary>
        /// <param name="fileid">File id to find</param>
        /// <returns>New GettFile class for manipulation file</returns>
        /// <exception>Exception will be thrown if file is not found</exception>
        public GettFile FindFileId(string fileid)
        {
            // Find and create a GettFile class from fileid.
            return (from file in GettShareInfo.Files
                    where file.FileId == fileid
                    select new GettFile(_gettUser, this, file)).First();
        }

        /// <summary>
        /// Find a specefic file from filename in this share.
        /// Remember, file information is only refreshed when method Refresh() is called.
        /// </summary>
        /// <param name="fileName">Filename to find</param>
        /// <returns>New GettFile class for manipulation file</returns>
        /// <exception>Exception will be thrown if file is not found</exception>    
        public GettFile FindFileName(string fileName)
        {
            // Find and create a GettFile class from fileid.
            return (from file in GettShareInfo.Files
                    where file.FileName == fileName
                    select new GettFile(_gettUser, this, file)).First();
        }

        /// <summary>
        /// List all files under this share.
        /// Remember, file information is only refreshed when method Refresh() is called.
        /// </summary>
        /// <returns>An array of new GettFile class for manipulation files</returns>
        public GettFile[] Files
        {
            get
            {
                // Create an array of GettFile class for all files in share.
                return (from file in GettShareInfo.Files
                        select new GettFile(_gettUser, this, file)).ToArray();
            }
        }
        #endregion
    }
}
