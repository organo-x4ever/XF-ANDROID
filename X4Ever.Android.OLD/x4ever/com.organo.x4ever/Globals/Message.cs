using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Message))]

namespace com.organo.x4ever.Globals
{
    public class Message : ContentPage, IMessage
    {
        public string GetResource(string resourceKey)
        {
            var translation = new TranslateExtension();
            translation.Text = resourceKey;
            return translation.ProvideValue(null)?.ToString() ?? "";
        }

        public async Task AlertAsync(string title, string message, string actions)
        {
            await DisplayAlert(title.Trim(), message.Trim(), actions.ToString());
        }

        public async Task<bool> AlertAsync(string title, string message, string accept, string cancel)
        {
            return await DisplayAlert(title.Trim(), message.Trim(), accept, cancel);
        }

        public async Task<string> AlertSheetAsync(string title, string buttonAction, Dictionary<string, string> items)
        {
            String[] list = new String[items.Count];

            var i = 0;
            foreach (var item in items)
            {
                list[i] = item.Value;
                i++;
            }

            return await AlertSheetAsync(title, buttonAction, list);
        }

        public async Task<string> AlertSheetAsync(string title, string buttonAction, string[] items)
        {
            return await DisplayActionSheet(title, buttonAction, null, items);
        }
    }
}