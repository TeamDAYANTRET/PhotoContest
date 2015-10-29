namespace PhotoContest.Data.Helper
{
    using Dropbox.Api;

    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System;

    public static class DropboxManager
    {
        private const string DropboxAppAccessToken = "OAURNfHymnAAAAAAAAABZhPHE3VDDBAP8Z06HVrfgcxdxHf39ZxzMlf2S-P-VGeu";
        private const string DropboxURLSuffix = "dl=0";
        private const string RawURLSuffix = "raw=1";

        public static async Task<Dictionary<string, List<Tuple<string, string>>>> GetAllSharedLinks()
        {
            var linkList = new Dictionary<string, List<Tuple<string, string>>>();

            using (var dbx = new DropboxClient(DropboxAppAccessToken))
            {
                //var arg = new Dropbox.Api.Sharing.GetSharedLinksArg("folder");
                var list = await dbx.Sharing.GetSharedLinksAsync();

                foreach(var link in list.Links)
                {
                    var imgName = link.AsPath.Path.Substring(link.AsPath.Path.LastIndexOf('/') + 1);
                    string path = link.AsPath.Path.Replace("/" + imgName, "");

                    string rawUrl = link.Url.Substring(0, link.Url.Length - DropboxURLSuffix.Length) + RawURLSuffix;

                    if (path != string.Empty && imgName != "www.dropbox.com.url")
                    {
                        if (!linkList.ContainsKey(path))
                        {
                            linkList.Add(path, new List<Tuple<string, string>>());
                        }

                        linkList[path].Add(new Tuple<string, string>(rawUrl, imgName));
                    }
                }
            }

            return linkList;
        }
    }
}
