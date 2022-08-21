using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Metars.iOS.Services;
using Metars.Services.Interfaces;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin.Forms;

namespace Metars.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private App _app;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Popup.Init();
            Forms.Init();
            FormsMaterial.Init();

            _app = new App(new iOSInitializer());
            LoadApplication(_app);

            return base.FinishedLaunching(app, options);
        }

        public class iOSInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.Register<IHttpClientBuilder, HttpClientBuilder>();
            }
        }
    }
}
