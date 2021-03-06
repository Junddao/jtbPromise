﻿using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace jtbPromise
{
    class CDropBox
    {
        #region Variables  
        private DropboxClient DBClient;
        private ListFolderArg DBFolders;
        private string _strAccessToken = string.Empty;
        private string oauth2State;
        private const string RedirectUri = "https://jtbPromise.com"; // Same as we have configured Under [Application] -> settings -> redirect URIs.  
        #endregion

        #region Constructor  
        public CDropBox(
            string ApiKey = "1mq2p0div8wscow", 
            string ApiSecret = "r7znytg7m39bywa", 
            string ApplicationName = "jtbPromise",
            string token = "gnDZnglvC6AAAAAAAAAAUnlgWkrd1v4YeNNIkQGl3ecBPZT59QBknzRVHgWFwHea")
        {
            try
            {
                AppKey = ApiKey;
                AppSecret = ApiSecret;
                AppName = ApplicationName;
                AccessTocken = token;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Properties  
        public string AppName
        {
            get; private set;
        }
        public string AuthenticationURL
        {
            get; private set;
        }
        public string AppKey
        {
            get; private set;
        }

        public string AppSecret
        {
            get; private set;
        }

        public string AccessTocken
        {
            get; private set;
        }
        public string Uid
        {
            get; private set;
        }
        #endregion

        #region UserDefined Methods  

        /// <summary>  
        /// This method is to generate Authentication URL to redirect user for login process in Dropbox.  
        /// </summary>  
        /// <returns></returns>  
        public string GeneratedAuthenticationURL()
        {
            try
            {
                this.oauth2State = Guid.NewGuid().ToString("N");
                Uri authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, AppKey, new Uri(RedirectUri), state: oauth2State);
                AuthenticationURL = authorizeUri.AbsoluteUri.ToString();
     
                return authorizeUri.AbsoluteUri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

     

        /// <summary>  
        /// This method is to generate Access Token required to access dropbox outside of the environment (in ANy application).  
        /// </summary>  
        /// <returns></returns>  
        public string GenerateAccessToken()
        {
            try
            {
                string _strAccessToken = string.Empty;

                if (CanAuthenticate())
                {
                    if (string.IsNullOrEmpty(AuthenticationURL))
                    {
                        throw new Exception("AuthenticationURL is not generated !");

                    }
                    // Run Dropbox authentication
                    //OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(AuthenticationURL));

                    //Login login = new Login(AppKey, AuthenticationURL, this.oauth2State); // WPF window with Webbrowser control to redirect user for Dropbox login process.  
                    //login.Owner = Application.Current.MainWindow;
                    //login.ShowDialog();

                    
                    _strAccessToken = AccessTocken;

                    //_strAccessToken = result.AccessToken;
                    //AccessTocken = result.AccessToken;
                    DropboxClientConfig CC = new DropboxClientConfig(AppName, 1);
                    HttpClient HTC = new HttpClient(new HttpClientHandler());
                    HTC.Timeout = TimeSpan.FromMinutes(10); // set timeout for each ghttp request to Dropbox API.  
                    CC.HttpClient = HTC;
                    DBClient = new DropboxClient(AccessTocken, CC);
                }

                return _strAccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private DropboxClient GetClient()
        //{
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        return new DropboxClient(AccessTocken, new DropboxClientConfig()
        //        {
        //            HttpClient = new HttpClient(new
        //               HttpClientHandler())
        //        });
        //    }
        //    return new DropboxClient(AccessTocken);
        //}

        /// <summary>  
        /// Method to create new folder on Dropbox  
        /// </summary>  
        /// <param name="path"> path of the folder we want to create on Dropbox</param>  
        /// <returns></returns>  
        public bool CreateFolder(string path)
        {
            try
            {
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }

                var folderArg = new CreateFolderArg(path);
                var folder = DBClient.Files.CreateFolderAsync(folderArg);
                var result = folder.Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>  
        /// Method is to check that whether folder exists on Dropbox or not.  
        /// </summary>  
        /// <param name="path"> Path of the folder we want to check for existance.</param>  
        /// <returns></returns>  
        public bool FolderExists(string path)
        {
            try
            {
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }

                var folders = DBClient.Files.ListFolderAsync(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IList<Metadata>> ListFiles(string path)
        {
            try
            {
                var list = await DBClient.Files.ListFolderAsync(path);
                return list?.Entries;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>  
        /// Method to delete file/folder from Dropbox  
        /// </summary>  
        /// <param name="path">path of file.folder to delete</param>  
        /// <returns></returns>  
        public bool Delete(string path)
        {
            try
            {
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }

                var folders = DBClient.Files.DeleteV2Async(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>  
        /// Method to upload files on Dropbox  
        /// </summary>  
        /// <param name="UploadfolderPath"> Dropbox path where we want to upload files</param>  
        /// <param name="UploadfileName"> File name to be created in Dropbox</param>  
        /// <param name="SourceFilePath"> Local file path which we want to upload</param>  
        /// <returns></returns>  
        public async Task Upload(string UploadfolderPath, string UploadfileName, string SourceFilePath)
        {
            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(SourceFilePath)))
                {
                    var response = await DBClient.Files.UploadAsync(UploadfolderPath + "/" + UploadfileName, WriteMode.Overwrite.Instance, body: stream);
                }
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>  
        /// Method to Download files from Dropbox  
        /// </summary>  
        /// <param name="DropboxFolderPath">Dropbox folder path which we want to download</param>  
        /// <param name="DropboxFileName"> Dropbox File name availalbe in DropboxFolderPath to download</param>  
        /// <param name="DownloadFolderPath"> Local folder path where we want to download file</param>  
        /// <param name="DownloadFileName">File name to download Dropbox files in local drive</param>  
        /// <returns></returns>  
        public async Task Download(string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName)
        {
            try
            {
                var response = await DBClient.Files.DownloadAsync(DropboxFolderPath + "/" + DropboxFileName);
                //var files = response.GetContentAsStreamAsync(); //Added to wait for the result from Async method  

                using (FileStream fileStream = File.Create(DownloadFolderPath + "/" + DownloadFileName))
                {
                    (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
          
            }
        }

        //public async Task Download(string remoteFilePath, string localFilePath)
        //{
        //    using (var response = await DBClient.Files.DownloadAsync(remoteFilePath))
        //    {
        //        using (var fileStream = File.Create(localFilePath))
        //        {
        //            response.GetContentAsStreamAsync().Result.CopyTo(fileStream);
        //        }

        //    }
        //}
        #endregion




        #region Validation Methods  
        /// <summary>  
        /// Validation method to verify that AppKey and AppSecret is not blank.  
        /// Mendatory to complete Authentication process successfully.  
        /// </summary>  
        /// <returns></returns>  
        public bool CanAuthenticate()
        {
            try
            {
                if (AppKey == null)
                {
                    throw new ArgumentNullException("AppKey");
                }
                if (AppSecret == null)
                {
                    throw new ArgumentNullException("AppSecret");
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
