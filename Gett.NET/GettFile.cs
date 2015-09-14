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
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Gett.Sharing
{
    /// <summary>
    /// Represent a file in a share for user.
    /// http://ge.tt/developers - REST API for Ge.tt Web service - file API.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("FileId = {_gettFileInfo.FileId}, FileName = {_gettFileInfo.FileName}, CreatedUtc = {_gettFileInfo.CreatedUtc}")]
    public class GettFile : GettBaseUri
    {
        #region HashCode and Equals overrides
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            // ReSharper disable NonReadonlyFieldInGetHashCode
            return _gettFileInfo == null ? base.GetHashCode() : _gettFileInfo.FileId.GetHashCode();
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        public override bool Equals(object obj)
        {
            var file = obj as GettFile;
            return file != null && file._gettFileInfo.FileId == _gettFileInfo.FileId;
        }

        public override string ToString()
        {
            return string.Format("FileName:{0}, Size:{1}, DL:{2}, CreatedUtc:{3}", _gettFileInfo.FileName, _gettFileInfo.Size, _gettFileInfo.Downloads, _gettFileInfo.CreatedUtc);
        }
        #endregion

        #region JSON Serialize/Deserialize classes
        [System.Diagnostics.DebuggerDisplay("FileInfo = {FileId}, FileName = {FileName}, CreatedUtc = {CreatedUtc}")]
        [JsonObject(MemberSerialization.OptOut)]
        public class FileInfo
        {
            [JsonIgnore]
            public readonly DateTime Timestamp = DateTime.Now;
            [JsonProperty("fileid")]
            public string FileId = null;
            [JsonProperty("created")]
            [JsonConverter(typeof(Newtonsoft.Json.Converters.UnixDateTimeConverter))]
            public DateTime? CreatedUtc = null;
            [JsonProperty("filename")]
            public string FileName = null;
            [JsonProperty("size")]
            public long Size = 0;
            [JsonProperty("downloads")]
            public int Downloads = 0;
            [JsonProperty("readystate")]
            public string ReadyState = null;
            [JsonProperty("sharename")]
            public string ShareName = null;
            [JsonProperty("getturl")]
            public string GettUrl = null;
            [JsonProperty("upload")]
            public UploadInfo Upload = null;
        }

        [System.Diagnostics.DebuggerDisplay("PostUrl = {PostUrl}, PutUrl = {PutUrl}")]
        [JsonObject(MemberSerialization.OptOut)]
        public class UploadInfo
        {
            [JsonProperty("puturl")]
            public string PutUrl = null;
            [JsonProperty("posturl")]
            public string PostUrl = null;
        }
        #endregion

        #region Variables
        /// <summary>
        /// Provides access to GettUser object, Token etc.
        /// </summary>
        private readonly GettUser _gettUser;

        /// <summary>
        /// Provides access to GettShare object.
        /// </summary>
        private readonly GettShare _gettShare;

        /// <summary>
        /// Provides information on what File we are working on.
        /// </summary>
        private FileInfo _gettFileInfo;
        #endregion

        #region ctor
        /// <summary>
        /// Internal ctor, GettShare class creates new instance for this class.
        /// </summary>
        /// <param name="gettUser">GettUser class</param>
        /// <param name="gettShare">GettShare class</param>
        /// <param name="gettFileInfo">Information for this share</param>
        internal GettFile(GettUser gettUser, GettShare gettShare, FileInfo gettFileInfo)
        {
            _gettUser = gettUser;
            _gettShare = gettShare;
            _gettFileInfo = gettFileInfo;
        }
        #endregion

        #region Common information about this file
        /// <summary>
        /// Parent of this class. GettShare class.
        /// </summary>
        public GettShare Parent { get { return _gettShare; } }

        /// <summary>
        /// Access to file information.
        /// </summary>
        public FileInfo Info { get { return _gettFileInfo; } }
        #endregion

        #region Refresh file information - /files/{sharename}/{fileid}
        /// <summary>
        /// Refresh file information.
        /// </summary>
        public bool Refresh()
        {
            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileList, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // GET request
            var gett = new WebClient { Encoding = Encoding.UTF8 };
            byte[] response = gett.DownloadData(baseUri.Uri);

            // Response
            var fileInfo = JsonConvert.DeserializeObject<FileInfo>(Encoding.UTF8.GetString(response));
            _gettFileInfo = fileInfo;
            return true;
        }

        /// <summary>
        /// Async version of Refresh
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> RefreshAsync()
        {
            return Task.Factory.StartNew(() => Refresh());
        }

        /// <summary>
        /// Begins an asynchronous refresh file information.
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

        #region Prepare to overwrite file - /files/{sharename}/{fileid}/upload
        /// <summary>
        /// Prepare to overwrite existing file. Get new upload uri.
        /// </summary>
        public bool RefreshUpload()
        {
            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileUpload, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // GET request
            var gett = new WebClient {Encoding = Encoding.UTF8};
            byte[] response = gett.DownloadData(baseUri.Uri);

            // Response
            var uploadInfo = JsonConvert.DeserializeObject<UploadInfo>(Encoding.UTF8.GetString(response));
            _gettFileInfo.Upload = uploadInfo;
            return true;
        }

        /// <summary>
        /// Async version of RefreshUpload
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> RefreshUploadAsync()
        {
            return Task.Factory.StartNew(() => RefreshUpload());
        }

        /// <summary>
        /// Begins an asynchronous prepare to overwrite existing file. Get new upload uri.
        /// </summary>
        public IAsyncResult BeginRefreshUpload(AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => RefreshUpload(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous RefreshUpload. 
        /// </summary>
        /// <param name="ar"></param>
        public bool EndRefreshUpload(IAsyncResult ar)
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

        #region Delete a file and the binary contents - /files/{sharename}/{fileid}/destroy
        /// <summary>
        /// Delete file.
        /// </summary>
        public bool Destroy()
        {
            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileDestroy, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);
            baseUri.Query = string.Format("accesstoken={0}", _gettUser.Token.AccessToken);

            // POST request
            var gett = new WebClient {Encoding = Encoding.UTF8};
            var request = new byte[0];
            byte[] response = gett.UploadData(baseUri.Uri, request);

            // Response
            string jsonResponse = Encoding.UTF8.GetString(response);
            return jsonResponse.Contains("true");
        }

        /// <summary>
        /// Async version of Destroy
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> DestroyAsync()
        {
            return Task.Factory.StartNew(() => Destroy());
        }

        /// <summary>
        /// Begins an asynchronous delete file.
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
        public bool EndDestroy(IAsyncResult ar)
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

        #region Upload and Download - posturl or puturl
        /// <summary>
        /// Upload file content to POST URL.
        /// </summary>
        /// <param name="fileName">File to upload content</param>
        /// <returns>True if uploads is successful</returns>
        public bool UploadFile(string fileName)
        {
            // POST request
            var gett = new WebClient();
            byte[] response = gett.UploadFile(_gettFileInfo.Upload.PostUrl, fileName);

            // Response
            string jsonResponse = Encoding.UTF8.GetString(response);
            return jsonResponse.Contains("computer says yes");
        }

        /// <summary>
        /// Async version of UploadFile
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> UploadFileAsync(string fileName)
        {
            return Task.Factory.StartNew(() => UploadFile(fileName));
        }

        /// <summary>
        /// Begins an asynchronous upload file content to POST URL.
        /// </summary>
        public IAsyncResult BeginUploadFile(string fileName, AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => UploadFile(fileName), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous UploadFile. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>True if uploads is successful</returns>
        public bool EndUploadFile(IAsyncResult ar)
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

        /// <summary>
        /// Upload byte array content to PUT URL.
        /// </summary>
        /// <param name="data">Byte array</param>
        /// <returns>True if uploads is successful</returns>
        public bool UploadData(byte[] data)
        {
            // POST request
            var gett = new WebClient();
            byte[] response = gett.UploadData(_gettFileInfo.Upload.PutUrl, "PUT", data);

            // Response
            string jsonResponse = Encoding.UTF8.GetString(response);
            return jsonResponse.Contains("computer says yes");
        }

        /// <summary>
        /// Async version of UploadData
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> UploadDataAsync(byte[] data)
        {
            return Task.Factory.StartNew(() => UploadData(data));
        }

        /// <summary>
        /// Begins an asynchronous upload byte array content to PUT URL.
        /// </summary>
        public IAsyncResult BeginUploadData(byte[] data, AsyncCallback callBack, object state)
        {
            Task<bool> t = Task.Factory.StartNew(_ => UploadData(data), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous UploadData. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>True if uploads is successful</returns>
        public bool EndUploadData(IAsyncResult ar)
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

        /// <summary>
        /// Download file content.
        /// </summary>
        /// <param name="fileName">File to store content</param>
        public bool DownloadFile(string fileName)
        {
            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileDownloadBlob, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);

            // GET request
            var gett = new WebClient();
            gett.DownloadFile(baseUri.Uri, fileName);
            return true;
        }

        /// <summary>
        /// Async version of DownloadFile
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<bool> DownloadFileAsync(string fileName)
        {
            return Task.Factory.StartNew(() => DownloadFile(fileName));
        }

        /// <summary>
        /// Begins an asynchronous download file content.
        /// </summary>
        /// <param name="fileName">File to store content</param>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        public IAsyncResult BeginDownloadFile(string fileName, AsyncCallback callBack, object state)
        {
            Task t = Task.Factory.StartNew(_ => DownloadFile(fileName), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous DownloadFile. 
        /// </summary>
        /// <param name="ar"></param>
        public void EndDownloadFile(IAsyncResult ar)
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

        /// <summary>
        /// Download file content and return an array of bytes.
        /// </summary>
        /// <returns>Byte array of file content</returns>
        public byte[] DownloadData()
        {
            // Build Uri
            var baseUri = new UriBuilder(BaseUri);
            baseUri.Path = baseUri.Path + string.Format(FileDownloadBlob, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);

            // GET request
            var gett = new WebClient();
            return gett.DownloadData(baseUri.Uri);
        }

        /// <summary>
        /// Async version of DownloadFile
        /// </summary>
        /// <returns>A Task, it can be used for async/await statments</returns>
        public Task<byte[]> DownloadDataAsync()
        {
            return Task.Factory.StartNew(() => DownloadData());
        }

        /// <summary>
        /// Begins an asynchronous download file content and return an array of bytes.
        /// </summary>
        public IAsyncResult BeginDownloadData(AsyncCallback callBack, object state)
        {
            Task<byte[]> t = Task.Factory.StartNew(_ => DownloadData(), state);

            if (callBack != null)
            {
                t.ContinueWith(res => callBack(t));
            }

            return t;
        }

        /// <summary>
        /// Handles the end of an asynchronous UploadData. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>Byte array of file content</returns>
        public byte[] EndDownloadData(IAsyncResult ar)
        {
            try
            {
                var t = (Task<byte[]>)ar;

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

        #region Asynchron Upload and Download using EAP Patterns. - posturl or puturl
        /// <summary>
        /// Object used for thread-safe for WebClient gettAsync.
        /// </summary>
        private readonly object _syncRootAsync = new object();

        /// <summary>
        /// WebClient used for Asynchron operation.
        /// </summary>
        private WebClient _gettAsync;

        /// <summary>
        /// Cancel Async upload or download.
        /// </summary>
        public void CancelEvents()
        {
            lock (_syncRootAsync)
            {
                // Abort other operation.
                if (_gettAsync != null)
                {
                    _gettAsync.DownloadDataCompleted -= Gett_DownloadDataCompleted;
                    _gettAsync.DownloadFileCompleted -= Gett_DownloadFileCompleted;
                    _gettAsync.DownloadProgressChanged -= Gett_DownloadProgressChanged;
                    _gettAsync.UploadDataCompleted -= Gett_UploadDataCompleted;
                    _gettAsync.UploadFileCompleted -= Gett_UploadFileCompleted;
                    _gettAsync.UploadProgressChanged -= Gett_UploadProgressChanged;

                    if (_gettAsync.IsBusy)
                        _gettAsync.CancelAsync();
                }
            }
        }

        /// <summary>
        /// This event is raised each time file upload operation completes.
        /// </summary>
        public event UploadFileCompletedEventHandler UploadFileCompleted;

        /// <summary>
        /// This event is raised each time upload makes progress.
        /// </summary>
        public event UploadProgressChangedEventHandler UploadProgressChanged;

        /// <summary>
        /// Upload file content asynchron.
        /// Progress is signaled via UploadProgressChanged and UploadFileCompleted event.
        /// </summary>
        /// <param name="fileName">File to upload content</param>
        public void UploadFileEvents(string fileName)
        {
            lock (_syncRootAsync)
            {
                // Abort other operation.
                if (_gettAsync != null)
                {
                    _gettAsync.DownloadDataCompleted -= Gett_DownloadDataCompleted;
                    _gettAsync.DownloadFileCompleted -= Gett_DownloadFileCompleted;
                    _gettAsync.DownloadProgressChanged -= Gett_DownloadProgressChanged;
                    _gettAsync.UploadDataCompleted -= Gett_UploadDataCompleted;
                    _gettAsync.UploadFileCompleted -= Gett_UploadFileCompleted;
                    _gettAsync.UploadProgressChanged -= Gett_UploadProgressChanged;

                    if (_gettAsync.IsBusy)
                        _gettAsync.CancelAsync();
                }

                // GET request
                _gettAsync = new WebClient();
                _gettAsync.UploadFileCompleted += Gett_UploadFileCompleted;
                _gettAsync.UploadProgressChanged += Gett_UploadProgressChanged;
                _gettAsync.UploadFileAsync(new Uri(_gettFileInfo.Upload.PostUrl), fileName);
            }
        }

        /// <summary>
        /// This event is raised each time upload makes progress.
        /// </summary>
        private void Gett_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            UploadProgressChangedEventHandler copyUploadProgressChanged = UploadProgressChanged;
            if (copyUploadProgressChanged != null)
                copyUploadProgressChanged(this, e);
        }

        /// <summary>
        /// This event is raised each time file upload operation completes.
        /// </summary>
        private void Gett_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            UploadFileCompletedEventHandler copyUploadFileCompleted = UploadFileCompleted;
            if (copyUploadFileCompleted != null)
                copyUploadFileCompleted(this, e);
        }

        /// <summary>
        /// This event is raised each time an asynchronous data upload operation completes.
        /// </summary>
        public event UploadDataCompletedEventHandler UploadDataCompleted;

        /// <summary>
        /// Upload byte array content asynchron.
        /// Progress is signaled via UploadProgressChanged and UploadDataCompleted event.
        /// </summary>
        /// <param name="data">Byte array</param>
        public void UploadDataEvents(byte[] data)
        {
            lock (_syncRootAsync)
            {
                // Abort other operation.
                if (_gettAsync != null)
                {
                    _gettAsync.DownloadDataCompleted -= Gett_DownloadDataCompleted;
                    _gettAsync.DownloadFileCompleted -= Gett_DownloadFileCompleted;
                    _gettAsync.DownloadProgressChanged -= Gett_DownloadProgressChanged;
                    _gettAsync.UploadDataCompleted -= Gett_UploadDataCompleted;
                    _gettAsync.UploadFileCompleted -= Gett_UploadFileCompleted;
                    _gettAsync.UploadProgressChanged -= Gett_UploadProgressChanged;

                    if (_gettAsync.IsBusy)
                        _gettAsync.CancelAsync();
                }

                // GET request
                _gettAsync = new WebClient();
                _gettAsync.UploadDataCompleted += Gett_UploadDataCompleted;
                _gettAsync.UploadProgressChanged += Gett_UploadProgressChanged;

                _gettAsync.UploadDataAsync(new Uri(_gettFileInfo.Upload.PutUrl), "PUT", data);
            }
        }

        /// <summary>
        /// This event is raised each time an asynchronous data upload operation completes
        /// </summary>
        private void Gett_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            UploadDataCompletedEventHandler copyUploadDataCompleted = UploadDataCompleted;
            if (copyUploadDataCompleted != null)
                copyUploadDataCompleted(this, e);
        }

        /// <summary>
        /// This event is raised each time file download operation completes.
        /// </summary>
        public event System.ComponentModel.AsyncCompletedEventHandler DownloadAsyncFileCompleted;

        /// <summary>
        /// This event is raised each time download makes progress.
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadAsyncProgressChanged;

        /// <summary>
        /// Download file content asynchron.
        /// Progress is signaled via DownloadAsyncProgressChanged and DownloadAsyncFileCompleted event.
        /// </summary>
        /// <param name="fileName">File to store content</param>
        public void DownloadFileEvents(string fileName)
        {
            lock (_syncRootAsync)
            {
                // Abort other operation.
                if (_gettAsync != null)
                {
                    _gettAsync.DownloadDataCompleted -= Gett_DownloadDataCompleted;
                    _gettAsync.DownloadFileCompleted -= Gett_DownloadFileCompleted;
                    _gettAsync.DownloadProgressChanged -= Gett_DownloadProgressChanged;
                    _gettAsync.UploadDataCompleted -= Gett_UploadDataCompleted;
                    _gettAsync.UploadFileCompleted -= Gett_UploadFileCompleted;
                    _gettAsync.UploadProgressChanged -= Gett_UploadProgressChanged;

                    if (_gettAsync.IsBusy)
                        _gettAsync.CancelAsync();
                }

                // Build Uri
                var baseUri = new UriBuilder(BaseUri);
                baseUri.Path = baseUri.Path + string.Format(FileDownloadBlob, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);

                // GET request
                _gettAsync = new WebClient();
                _gettAsync.DownloadFileCompleted += Gett_DownloadFileCompleted;
                _gettAsync.DownloadProgressChanged += Gett_DownloadProgressChanged;
                _gettAsync.DownloadFileAsync(baseUri.Uri, fileName);
            }
        }

        /// <summary>
        /// This event is raised each time download makes progress.
        /// </summary>
        private void Gett_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChangedEventHandler copyDownloadAsyncProgressChanged = DownloadAsyncProgressChanged;
            if (copyDownloadAsyncProgressChanged != null)
                copyDownloadAsyncProgressChanged(this, e);
        }

        /// <summary>
        /// This event is raised each time file download operation completes.
        /// </summary>
        private void Gett_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.ComponentModel.AsyncCompletedEventHandler copyDownloadAsyncFileCompleted = DownloadAsyncFileCompleted;
            if (copyDownloadAsyncFileCompleted != null)
                copyDownloadAsyncFileCompleted(this, e);
        }

        /// <summary>
        /// This event is raised each time an asynchronous data download operation completes.
        /// Downloaded data is in DownloadDataCompletedEventArgs.Result byte array. Remember to check DownloadDataCompletedEventArgs.Error and .Cancel results.
        /// </summary>
        public event DownloadDataCompletedEventHandler DownloadDataCompleted;

        /// <summary>
        /// Download file content asynchron.
        /// Progress is signaled via DownloadAsyncProgressChanged and DownloadAsyncFileCompleted event.
        /// </summary>
        /// <param name="fileName">File to store content</param>
        public void DownloadDataEvents(string fileName)
        {
            lock (_syncRootAsync)
            {
                // Abort other operation.
                if (_gettAsync != null)
                {
                    _gettAsync.DownloadDataCompleted -= Gett_DownloadDataCompleted;
                    _gettAsync.DownloadFileCompleted -= Gett_DownloadFileCompleted;
                    _gettAsync.DownloadProgressChanged -= Gett_DownloadProgressChanged;
                    _gettAsync.UploadDataCompleted -= Gett_UploadDataCompleted;
                    _gettAsync.UploadFileCompleted -= Gett_UploadFileCompleted;
                    _gettAsync.UploadProgressChanged -= Gett_UploadProgressChanged;

                    if (_gettAsync.IsBusy)
                        _gettAsync.CancelAsync();
                }

                // Build Uri
                var baseUri = new UriBuilder(BaseUri);
                baseUri.Path = baseUri.Path + string.Format(FileDownloadBlob, _gettShare.GettShareInfo.ShareName, _gettFileInfo.FileId);

                // GET request
                _gettAsync = new WebClient();
                _gettAsync.DownloadDataCompleted += Gett_DownloadDataCompleted;
                _gettAsync.DownloadProgressChanged += Gett_DownloadProgressChanged;
                _gettAsync.DownloadDataAsync(baseUri.Uri, fileName);
            }
        }

        /// <summary>
        /// This event is raised each time an asynchronous data download operation completes
        /// </summary>
        private void Gett_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            DownloadDataCompletedEventHandler copyDownloadDataCompleted = DownloadDataCompleted;
            if (copyDownloadDataCompleted != null)
                copyDownloadDataCompleted(this, e);
        }
        #endregion
    }
}
