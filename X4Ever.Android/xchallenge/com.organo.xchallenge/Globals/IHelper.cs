using System.Threading.Tasks;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Globals
{
    public interface IHelper
    {
        string GetResource(string resourceKey);

        Task DisplayMessage(string result);

        string ReturnMessage(string result);

        Task DisplayMessage(string result, char splitBy);

        Task DisplayMessage(string title, string msg, string action);

        Task DisplayMessageOnly(string msg);

        Task<string> DatetimeStampAsync();

        /// <summary>
        /// The method will return full path with file name.
        /// </summary>
        /// <param name="fileName">The file to display.</param>
        /// <param name="fileType">The file type should be like Image, Video etc.</param>
        /// <returns>The full path of the file.</returns>
        string GetFilePath(string fileName, FileType fileType);

        UriImageSource GetFileUri(string fileName, FileType fileType);

        ImageSource GetFileSource(string fileName, FileType fileType);
        object GetFileObject(string fileName, FileType fileType);

        string Encrypt(string input, string key = "");
        string Decrypt(string input, string key = "");
    }
}