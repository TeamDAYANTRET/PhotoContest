namespace PhotoContest.Web.App_Start
{
    using Dropbox.Api;
    using Dropbox.Api.Files;

    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class DropboxManager
    {
        private const string DropboxAppAccessToken = "OAURNfHymnAAAAAAAAABZhPHE3VDDBAP8Z06HVrfgcxdxHf39ZxzMlf2S-P-VGeu";

        /// <summary>Uploads file in existing folder in Dropbox.</summary>
        /// <param name="folder">
        /// Path to the folder. Always begin with "/" - the root Dropbox folder. Example: "/Contests/Active"
        /// </param>
        /// <param name="file">Name of the file with the filename extension. Example: "Abstract5567.jpg"</param>
        /// <param name="content">A generic view of a sequence of bytes. (Stream)</param>
        /// <returns>On success returns shared link of the file, otherwise null.</returns>
        public static async Task<string> Upload(string folder, string file, Stream content)
        {
            string sharedLink = null;

            using (var dbx = new DropboxClient(DropboxAppAccessToken))
            {
                using (var mem = new MemoryStream())
                {
                    content.Position = 0;
                    content.CopyTo(mem);

                    mem.Position = 0;

                    try
                    {
                        var uploaded = await dbx.Files.UploadAsync(
                        folder + "/" + file,
                        WriteMode.Overwrite.Instance,
                        body: mem);

                        var sharedLinkArg = new Dropbox.Api.Sharing.CreateSharedLinkArg(folder + "/" + file);
                        var link = await dbx.Sharing.CreateSharedLinkAsync(sharedLinkArg);

                        sharedLink = link.Url;
                    }
                    catch (Exception)
                    { }
                }
            }

            return sharedLink;
        }

        /// <summary>
        /// Deletes file or entire folder(with or without content). 
        /// </summary>
        /// <param name="path">If the target is a folder - path to the folder. Example: "/Contests/Active".
        /// If the target is file - the path includes the filename and extension. Example: "/Contests/Active/Abstract5567.jpg"
        /// The path is relevant to the root Dropbox folder - i.e. the path always starts with "/" 
        /// </param>
        /// <returns>On success returns true, otherwise false.</returns>
        public static async Task<bool> Delete(string path)
        {
            using (var dbx = new DropboxClient(DropboxAppAccessToken))
            {
                var folderArg = new Dropbox.Api.Files.DeleteArg(path);
                bool success = true;
                try
                {
                    var folder = await dbx.Files.DeleteAsync(folderArg);
                }
                catch (Exception ex)
                {
                    success = false;
                }

                return success;
            }
        }

        /// <summary>
        /// Creates folder 
        /// </summary>
        /// <param name="path">Path to the parent folder. Example: "/Contests/Active".
        /// The path is relevant to the root Dropbox folder - i.e. the path always starts with "/" 
        /// </param>
        /// <returns>On success returns true, otherwise false.</returns>
        public static async Task<bool> CreateFolder(string path)
        {
            using (var dbx = new DropboxClient(DropboxAppAccessToken))
            {
                var folderArg = new Dropbox.Api.Files.CreateFolderArg(path);
                bool success = true;
                try
                {
                    var folder = await dbx.Files.CreateFolderAsync(folderArg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to create folder");
                    success = false;
                }

                return success;
            }
        }

        /// <summary>
        /// Downloads given file in dropbox folder. 
        /// </summary>
        /// <param name="folder">Path to the folder. Always begin with "/" - the root Dropbox folder. Example: "/Contests/Active"</param>
        /// <param name="file">Name of the file with the filename extension. Example: "Abstract5567.jpg"</param>
        /// <returns>On success returns Base64 converted file, otherwise null.</returns>
        public static async Task<string> DownloadFile(string folder, string file)
        {
            string fileContent = null;

            using (var dbx = new DropboxClient(DropboxAppAccessToken))
            {
                using (var response = await dbx.Files.DownloadAsync(folder + "/" + file))
                {
                    try
                    {
                        fileContent = await response.GetContentAsStringAsync();
                    }
                    catch(Exception)
                    { }                    
                }
            }

            return fileContent;
        }
    }
}
