# Gett.NET - A library for Ge.tt Web Services
Gett.NET is a C# library for Ge.tt Web Services REST API and Live API.

Ge.tt (http://ge.tt) is an instant, real-time file publishing and sharing service.
With Ge.tt, you can turn any type of file into web content and share it instantly. You can share documents, video, music and photos making them instantly available.

Ge.tt is suitable for professional and personal use.
* Real-time sharing:
Your files are ready to be published or shared as soon as you select them! No need to wait for files to upload.
* Real-time statistics:
You can easily track how many people have downloaded or viewed your files.

Read more at http://ge.tt/about.

To get started, you have to create or use an existing user account at Ge.tt.

Go to http://ge.tt and click on “SIGN UP”. It is free to sign up for a basic account, fill out the form and you are off.
After a successful login at http://ge.tt, you then access http://ge.tt/developers and click “Create app”, fill in the form for your own API key and click “Create app” button.

Now you have the most important information, the API key! Without the API key you cannot use Gett.NET library. Just remember that your API key is secret and personal. Use it as such.

###What is So Special about Ge.tt
*What marks Ge.tt special from "all other upload services" on the internet?*

The most unique is the instant upload and download service. When you start to upload a large file via Gett.NET library or the web site, others are able to start downloading the large file immediately, no waiting for the large file to complete the upload.

And if you need the advanced queuing nature of Ge.tt, then say hello to the Live API.
The idea behind Live API is that you create a new file on a share via Gett.NET library, but you do not upload the file.

As long as there is a connection to Live API from Gett.NET library, all files created that way is listed as available for download.
But when a user clicks on the download link from Ge.tt, Live API sent out an event that is picked up by Gett.NET library.

This event enables you to choose what file to upload, and you can share an always updated file. For other files, that is uploaded, Live API sent out an event every time a user clicks on the download link which will enable you to collect real-time statistics.

##Using the Code
Gett.NET library has a base class `GettUser`. From `GettUser`, you have access to all shares from class `GettShares` and access to Live API events from `GettLive`.

`GettShares` can have zero or more share `GettShare` objects and `GettShare` can have zero or more files `GettFile`.

All errors will throw exceptions. Remember to use `try-catch` blocks.

Let us start with logging in at Ge.tt Web Services. Have you API key, login and password information ready.
In all examples, API key is "`apitest`", login information is "`apitest@ge.tt`" and password is "`secret`".
```csharp
// Login to Ge.tt, create a GettUser object.
// You have to provide a valid API key, login and password.
//
Gett.Sharing.GettUser user = new Gett.Sharing.GettUser();

// Try to login
user.Login("apitest", "apitest@ge.tt", "secret");

// Now we are logged in at Ge.tt. Let us retrieve information for our user.
user.RefreshMe();
Console.WriteLine("Login user: {0} ({1])", user.Me.Email, user.Me.FullName);
Console.WriteLine("Storage, you are using {0} of {1} bytes, 
    you still have {2} bytes free.", user.Me.Storage.Used, 
    user.Me.Storage.Total, user.Me.Storage.Free);
```
If you wish to get current storage information, call `GettUser.RefreshMe()` method.

A login session does not last forever, it expires in `GettUser.SessionExpires` seconds. Call `GettUser.RefreshLogin()` method to keep login session alive.

Now, let us create a share and upload a file.
To upload a file, you have to create or use an existing share. Through `GettShares` you can create a new share, when the share is created a `GettShare` object is available.
From `GettShare` you create a new file through `GettFile`.
```csharp
// First create a new share. 
Gett.Sharing.GettShare share1 = user.Shares.CreateShare("My own share");
Console.WriteLine("Share created, with share name: {0} and title:{1}", 
        share1.Info. ShareName, share1.Info.Title);

// Then create a new file.
Gett.Sharing.GettFile file1 = share1.CreateFile("MyDataFile.txt");
Console.WriteLine("New file created at share {0}, with file name: 
  {1} readystate:{2}", file1.Info.ShareName, file1.Info.FileName, file1.Info.ReadyState);

// Upload content of the file to Ge.tt
file1.UploadFile(@"c:\myfolder\MyDataFile.txt");

// Upload is completed. Print out the Ge.tt URL string, that you can give 
// to other users so they can start downloading the file.
Console.WriteLine("Upload completed. Ge.tt URL: {0}", file1.Info.GettUrl);

// Refresh file info
file1.Refresh();
Console.WriteLine("File created at share {0}, with file name: {1} readystate:{2}", 
    file1.Info.ShareName, file1.Info.FileName, file1.Info.ReadyState); 
```
And you are done! First file uploaded.

Shares on Ge.tt has an unique URL, like http://ge.tt/4ddfds, this URL will list all files in a share. A file has its own URL like http://ge.tt/4ddfds/v/0.

Let us download the file again.
```csharp
// First you have to get access to the share you want to download a file from.
Gett.Sharing.GettShare share2 = user.Shares.GetShare("4ddfds"); // the path from the 
                        // http://ge.tt/4ddfds - 4ddfds

// Get access to the file.
Gett.Sharing.GettFile file2 = share2.FindFileId("0"); // Get the first file in the share.
Console.WriteLine("File created at share {0}, with file name: {1} readystate:{2}", 
    file2.Info.ShareName, file2.Info.FileName, file2.Info.ReadyState);

// Download file content from Ge.tt to a local file.
file2.DownloadFile(@"c:\myfolder\MyDownloadDataFile.txt");

// Download is completed.
Console.WriteLine("Download completed.");  
```
To delete a file in a share, you call method `GettFile.Destroy()` or if you want to delete all files and the share, you call method `GettShare.Destroy()`.

To list all shares you have, you access the method `GettUser.Shares.GetShares()`. This will return an array of `GettShare` objects.
```csharp
// List all shares and files under each share.
Gett.Sharing.GettShare[] shares = user.Shares.GetShares();

foreach (Gett.Sharing.GettShare share in shares)
{
  Console.WriteLine("Share {0} with title {1} has {2} files", 
    share.Info.ShareName, share.Info.Title, share.FilesInfo.Length);
  foreach (Gett.Sharing.GettFile.FileInfo fileInfo in share.FileInfo)
  {
    Console.WriteLine("+  File name: {0}, File size: {1}, 
    Download count: {2}", fileInfo.FileName, fileInfo.Size, fileInfo.Downloads);
  }
}  
```
If you just want to look at or search for a share or file information and do not wish to manipulate, use `GettShare.Info` and `GettShare.FileInfo[]` properties.

`GettShare.Info` has information about the share. Like when it was created, the title, Ge.tt URL etc.
`GettShare.FileInfo[]` has information on all files in that share like when it was created, file name, file size, download count, etc.

`GettShare.Info` and `GettShare.FileInfo[]` information can be outdated. To get up-to-date information, call `GettShare.Refresh()` method.
