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
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Gett.Sharing
{
    /// <summary>
    /// Represent a user on Ge.tt.
    /// http://ge.tt/developers - REST API for Ge.tt Web service - users API.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("UserMe = {Me.FullName}, {Me.Email}, UserStorage Total = {Me.Storage.Total}, UserStorage Free = {Me.Storage.Free}")]
    public class GettUser : GettBaseUri
    {
        #region HashCode and Equals overrides
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            // ReSharper disable once NonReadonlyFieldInGetHashCode
            return Me == null ? base.GetHashCode() : Me.UserId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var user = obj as GettUser;
            return user != null && user.Me.UserId == Me.UserId;
        }
        #endregion

        #region JSON Serialize/Deserialize classes
        /// <summary>
        /// User token from first login
        /// </summary>
        [JsonObject(MemberSerialization.OptOut)]
        internal class UserToken
        {
            [JsonIgnore]
            public readonly DateTime Timestamp = DateTime.Now;
            [JsonProperty("accesstoken", NullValueHandling = NullValueHandling.Ignore)]
            public string AccessToken = null;
            [JsonProperty("refreshtoken")]
            public string RefreshToken = null;
            [JsonProperty("expires", NullValueHandling = NullValueHandling.Ignore)]
            public int? Expires = null;
        }

        /// <summary>
        /// Login token
        /// </summary>
        [JsonObject(MemberSerialization.OptOut)]
        internal class LoginToken
        {
            [JsonProperty("apikey")]
            public string ApiKey;
            [JsonProperty("email")]
            public string Email;
            [JsonProperty("password")]
            public string Password;
        }

        /// <summary>
        /// User informations from Ge.tt
        /// </summary>
        [JsonObject(MemberSerialization.OptOut)]
        public class UserMe
        {
            [JsonIgnore]
            public readonly DateTime Timestamp = DateTime.Now;
            [JsonProperty("userid")]
            public string UserId;
            [JsonProperty("fullname")]
            public string FullName;
            [JsonProperty("email")]
            public string Email;

            [JsonProperty("storage")]
            public UserStorage Storage;
        }

        /// <summary>
        /// Storage information for user.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public class UserStorage
        {
            [JsonProperty("used")]
            public long Used;
            [JsonProperty("limit")]
            public long Limit;
            [JsonProperty("extra")]
            public long Extra;

            public long Free { get { return Limit - Used; } }
            public long Total { get { return Limit; } }
        }
        #endregion

        #region Variables
        /// <summary>
        /// Login token. Used in all REST API.
        /// </summary>
        internal UserToken Token
        {
            get { return _token; }
            set
            {
                _token = value;
                if (_token.Expires != null)
                {
                    SessionExpires = (int)_token.Expires;
                }
            }
        }
        private UserToken _token;

        /// <summary>
        /// When does login session expires.
        /// Call RefreshLogin(), to keep login session alive.
        /// </summary>
        public int SessionExpires { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Public ctor.
        /// </summary>
        /// <param name="maxConcurrentConnections">The maximum number of concurrent connections allowed</param>
        public GettUser(int maxConcurrentConnections = 2)
        {
            // The maximum number of concurrent connections allowed. The default value is 2.
            if (maxConcurrentConnections != 2)
            {
                ServicePointManager.DefaultConnectionLimit = maxConcurrentConnections;
            }

            _shares = new GettShares(this);
        }
        #endregion

        #region Login API - /users/login
        /// <summary>
        /// Login to Ge.tt Web Service.
        /// The first method to call before any other service.
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        public bool Login(string apiKey, string email, string password)
        {
            // Argument
            var loginToken = new LoginToken { ApiKey = apiKey, Email = email, Password = password };
            string jsonArgument = JsonConvert.SerializeObject(loginToken);

            // POST request
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            gett.Headers.Add("Content-Type", "application/json");
            byte[] request = Encoding.UTF8.GetBytes(jsonArgument);
            byte[] response = gett.UploadData(UsersLogin, request);

            // Response
            Token = JsonConvert.DeserializeObject<UserToken>(Encoding.UTF8.GetString(response));
            return Token.Expires.HasValue && Token.Expires > 0;
        }

        /// <summary>
        /// Async version of Login
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> LoginAsync(string apiKey, string email, string password)
        {
            return Task.Factory.StartNew(() => Login(apiKey, email, password));
        }

        /// <summary>
        /// Begins an asynchronous Login to Ge.tt Web Service.
        /// The first method to call before any other service.
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <param name="callBack">Callback</param>
        /// <param name="state">User state</param>
        public IAsyncResult BeginLogin(string apiKey, string email, string password, AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => Login(apiKey, email, password), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous Login. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndLogin(IAsyncResult ar)
        {
            try
            {
                var t = (Task<bool>)ar;
                if (t.IsFaulted && t.Exception != null) throw t.Exception.InnerException;

                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        /// <summary>
        /// Refresh previous login token.
        /// Please see Token.Expires.
        /// </summary>
        public bool RefreshLogin()
        {
            if (Token == null)
                throw new UnauthorizedAccessException("Login is required");

            // Argument
            var refreshToken = new UserToken { RefreshToken = Token.RefreshToken };
            string jsonArgument = JsonConvert.SerializeObject(refreshToken);

            // POST request
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            gett.Headers.Add("Content-Type", "application/json");
            byte[] request = Encoding.UTF8.GetBytes(jsonArgument);
            byte[] response = gett.UploadData(UsersLogin, request);

            // Response
            Token = JsonConvert.DeserializeObject<UserToken>(Encoding.UTF8.GetString(response));
            return Token.Expires.HasValue && Token.Expires > 0;
        }

        /// <summary>
        /// Async version of RefreshLogin
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> RefreshLoginAsync()
        {
            return Task.Factory.StartNew(RefreshLogin);
        }

        /// <summary>
        /// Begins an asynchronous refresh previous login token.
        /// Please see Token.Expires.
        /// </summary>
        public IAsyncResult BeginRefreshLogin(AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => RefreshLogin(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous RefreshLogin. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndRefreshLogin(IAsyncResult ar)
        {
            try
            {
                var t = (Task<bool>)ar;

                if (t.IsFaulted && t.Exception != null) throw t.Exception.InnerException;
                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }
        #endregion

        #region User information API - /users/me
        /// <summary>
        /// Refresh Me informations. User storage etc.
        /// </summary>
        public bool RefreshMe()
        {
            if (Token == null) throw new UnauthorizedAccessException("Login is required");

            // Build Uri
            var baseUri = new UriBuilder(UsersMe) { Query = string.Format("accesstoken={0}", Token.AccessToken) };

            // GET request
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            byte[] response = gett.DownloadData(baseUri.Uri);

            // Response
            Me = JsonConvert.DeserializeObject<UserMe>(Encoding.UTF8.GetString(response));
            return true;
        }

        /// <summary>
        /// Async version of RefreshMe
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> RefreshMeAsync()
        {
            return Task.Factory.StartNew(RefreshMe);
        }

        /// <summary>
        /// Begins an asynchronous refresh Me informations. User storage etc..
        /// </summary>
        public IAsyncResult BeginRefreshMe(AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => RefreshMe(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous RefreshMe. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndRefreshMe(IAsyncResult ar)
        {
            try
            {
                var t = (Task<bool>)ar;

                if (t.IsFaulted && t.Exception != null) throw t.Exception.InnerException;
                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        /// <summary>
        /// User information.
        /// </summary>
        public UserMe Me
        {
            get
            {
                if (Token == null) throw new UnauthorizedAccessException("Login is required");
                return _me;
            }
            private set
            {
                _me = value;
            }
        }
        private UserMe _me;
        #endregion

        #region Access to Shares
        /// <summary>
        /// Get instance for GettShares class. Provides access to Shares and files.
        /// </summary>
        public GettShares Shares
        {
            get
            {
                if (Token == null) throw new UnauthorizedAccessException("Login is required");
                return _shares;
            }
        }
        private readonly GettShares _shares;
        #endregion

        #region Access to Live API
        /// <summary>
        /// Get new instance for GettLive class.
        /// </summary>
        public Live.GettLive GetLive()
        {
            if (Token == null) throw new UnauthorizedAccessException("Login is required");

            return new Live.GettLive(this);
        }
        #endregion
    }
}
