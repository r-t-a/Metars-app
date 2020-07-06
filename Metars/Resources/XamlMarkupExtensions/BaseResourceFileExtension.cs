using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Metars.Resources.XamlMarkupExtensions
{
    [ContentProperty("Text")]
    public abstract class BaseResourceFileExtension<TExtensionType> : IMarkupExtension
    {
        protected abstract string ResourceId { get; }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var temp = new ResourceManager(ResourceId, typeof(TExtensionType).GetTypeInfo().Assembly);

            var translation = string.Empty;
            try
            {
                //todo do the GetPlatformCulture for GetString to get translation resource file
                translation = temp.GetString(Text);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return translation;
        }
    }
}
