using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Globals
{
    public interface IMessage
    {
        string GetResource(string resourceKey);

        //Asynchronization
        Task AlertAsync(string title, string message, string actions);

        Task<bool> AlertAsync(string title, string message, string accept, string cancel);

        Task<string> AlertSheetAsync(string title, string buttonAction, Dictionary<string, string> items);

        Task<string> AlertSheetAsync(string title, string buttonAction, string[] items);
    }
}