using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Dropbox.Api;
using Dropbox.Api.Files;

using Xamarin.Forms;


namespace jtbPromise
{
    public class DropBoxService
    {
        #region Constants

        private const string AppKeyDropboxtoken = "1mq2p0div8wscow";
        private const string ClientId = "r7znytg7m39bywa";
        private const string RedirectUri = "https://jtbPromise.jtb";

        #endregion

        #region Fields

        /// <summary>
        ///     Occurs when the user was authenticated
        /// </summary>
        public Action OnAuthenticated;

        private string oauth2State;

        #endregion

        #region Properties

        private string AccessToken { get; set; }
        public string AuthenticationURL { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     <para>Runs the Dropbox OAuth authorization process if not yet authenticated.</para>
        ///     <para>Upon completion <seealso cref="OnAuthenticated"/> is called</para>
        /// </summary>
        /// <returns>An asynchronous task.</returns>
        public async Task Authorize()
        {
            if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.GetAccessTokenFromSettings())
            {
                // Found token and set AccessToken 
                return;
            }

            // Run Dropbox authentication
            this.oauth2State = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, ClientId, new Uri(RedirectUri), this.oauth2State);
            AuthenticationURL = authorizeUri.AbsoluteUri.ToString();
            var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
            webView.Navigating += this.WebViewOnNavigating;
            var contentPage = new ContentPage { Content = webView };
            await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
        }

        public async Task<IList<Metadata>> ListFiles()
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var list = await client.Files.ListFolderAsync(string.Empty);
                    return list?.Entries;
                }
            }
            catch (Exception)
            {
  
                return null;
            }
        }

        //public async Task<byte[]> ReadFile(string file)
        //{
        //    try
        //    {
        //        using (var client = this.GetClient())
        //        {
        //            var response = await client.Files.DownloadAsync(file);
        //            var bytes = response?.GetContentAsByteArrayAsync();
        //            return bytes?.Result;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        /// <summary>  
        /// Method to upload files on Dropbox  
        /// </summary>  
        /// <param name="UploadfolderPath"> Dropbox path where we want to upload files</param>  
        /// <param name="UploadfileName"> File name to be created in Dropbox</param>  
        /// <param name="SourceFilePath"> Local file path which we want to upload</param>  
        /// <returns></returns>  
        public bool Upload(string UploadfolderPath, string UploadfileName, string SourceFilePath)
        {
            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(SourceFilePath)))
                {
                    using (var client = this.GetClient())
                    {
                        var response = client.Files.UploadAsync(UploadfolderPath + "/" + UploadfileName, WriteMode.Overwrite.Instance, body: stream);
                        var rest = response.Result; //Added to wait for the result from Async method  
                    }
                        
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
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
        public bool Download(string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var response = client.Files.DownloadAsync(DropboxFolderPath + "/" + DropboxFileName);
                    var result = response.Result.GetContentAsStreamAsync(); //Added to wait for the result from Async method  
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //public async Task<FileMetadata> WriteFile(byte[] fileContent, string filename)
        //{
        //    try
        //    {
        //        var commitInfo = new CommitInfo(filename, WriteMode.Overwrite.Instance, false, DateTime.Now);

        //        using (var client = this.GetClient())
        //        {
        //            var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
        //            return metadata;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region Methods

        /// <summary>
        ///     Saves the Dropbox token to app settings
        /// </summary>
        /// <param name="token">Token received from Dropbox authentication</param>
        private static async Task SaveDropboxToken(string token)
        {
            if (token == null)
            {
                return;
            }

            try
            {
                Application.Current.Properties.Add(AppKeyDropboxtoken, token);
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception)
            {

            }
        }


        /// <summary>  
        /// Method to create new folder on Dropbox  
        /// </summary>  
        /// <param name="path"> path of the folder we want to create on Dropbox</param>  
        /// <returns></returns>  
        public bool CreateFolder(string path)
        {
            try
            {
                if (AccessToken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }

                var folderArg = new CreateFolderArg(path);
                using (var client = this.GetClient())
                {
                    var folder = client.Files.CreateFolderAsync(path);
                    var result = folder.Result;
                }
               
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
                if (AccessToken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }
                using (var client = this.GetClient())
                {
                    var folders = client.Files.ListFolderAsync(path);
                    var result = folders.Result;
                }
                   
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private DropboxClient GetClient()
        {
            return new DropboxClient(this.AccessToken);
        }

        /// <summary>
        ///     Tries to find the Dropbox token in application settings
        /// </summary>
        /// <returns>Token as string or <c>null</c></returns>
        private bool GetAccessTokenFromSettings()
        {
            try
            {
                if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                {
                    return false;
                }

                this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                if (this.AccessToken != null)
                {
                    this.OnAuthenticated.Invoke();
                    return true;
                }

                return false;
            }
            catch (Exception )
            {
   
                return false;
            }
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                // we need to ignore all navigation that isn't to the redirect uri.
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                if (result.State != this.oauth2State)
                {
                    return;
                }

                this.AccessToken = result.AccessToken;

                await SaveDropboxToken(this.AccessToken);
                this.OnAuthenticated?.Invoke();
            }
            catch (ArgumentException)
            {
                // There was an error in the URI passed to ParseTokenFragment
            }
            finally
            {
                e.Cancel = true;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        #endregion
    }
}
