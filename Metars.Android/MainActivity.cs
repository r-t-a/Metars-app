using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Metars.Services.Interfaces;
using Prism.Ioc;
using Prism;
using Metars.Droid.Services;
using Acr.UserDialogs;
using Rg.Plugins.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Metars.Droid
{
    [Activity(Label = "Metars", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Popup.Init(this);
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            _app = new App(new AndroidInitializer());
            LoadApplication(_app);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.Register<IHttpClientBuilder, HttpClientBuilder>();
            }
        }

        public override void OnBackPressed()
        {
            // if back button pressed when Rg popup don't go back in below stack
            if (!Popup.SendBackPressed(base.OnBackPressed))
            {
                //base.OnBackPressed();
            }
        }
    }
}