#region License information
/*

  Copyright (c) 2014 ctor (http://www.codeproject.com/Members/Kim-Togo)
 
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

namespace Gett.Sharing
{
    /// <summary>
    /// REST and Live API endpoints for Ge.tt.
    /// </summary>
    public class GettBaseUri
    {
        // REST API endpoints
        internal static Uri BaseUri = new Uri("https://open.ge.tt/1/");

        // Authentication
        internal Uri UsersLogin = new Uri(BaseUri, "users/login"); // POST /1/users/login
        internal Uri UsersMe = new Uri(BaseUri, "users/me"); // GET /1/users/me?accesstoken={at}

        // Shares
        internal Uri ShareListAll = new Uri(BaseUri, "shares"); // GET /1/shares?accesstoken={at}
        internal Uri ShareCreate = new Uri(BaseUri, "shares/create"); // POST /1/shares/create?accesstoken={at}
        internal string ShareList = "shares/{0}"; // GET /1/shares/{sharename}
        internal string ShareUpdate = "shares/{0}/update"; // POST /1/shares/{sharename}/update?accesstoken={at}
        internal string ShareDestroy = "shares/{0}/destroy"; // POST /1/shares/{sharename}/destroy?accesstoken={at}

        // Files
        internal string FileCreate = "files/{0}/create"; // POST /1/files/{sharename}/create?accesstoken={at}
        internal string FileList = "files/{0}/{1}"; // GET /1/files/{sharename}/{fileid}
        internal string FileUpload = "files/{0}/{1}/upload"; // GET /1/files/{sharename}/{fileid}/upload?accesstoken={at}
        internal string FileDestroy = "files/{0}/{1}/destroy"; // POST /1/files/{sharename}/{fileid}/destroy?accesstoken={at}
        internal string FileDownloadBlob = "files/{0}/{1}/blob"; // GET /1/files/{sharename}/{fileid}/blob

        // Live API endpoints
        internal static string BaseUriLive = "wss://open.ge.tt";
    }
}
