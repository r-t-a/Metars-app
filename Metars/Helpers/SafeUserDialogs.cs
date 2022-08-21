using Acr.UserDialogs;
using Metars.Views.Controls;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Metars.Helpers
{
    public class SafeUserDialogs
    {
        private static readonly Lazy<SafeUserDialogs> _instance = new Lazy<SafeUserDialogs>(() => new SafeUserDialogs());
        public static SafeUserDialogs Instance => _instance.Value;

        private static bool IsLoadingScreenShowing { get; set; }
        private static SemaphoreSlim LoadingSemaphore { get; } = new SemaphoreSlim(1, 1);

        public void Alert(string message, string title = null, string okText = null)
        {
            _ = AlertAsync(message, title, okText);
        }

        public async Task HideLoading()
        {
            await LoadingSemaphore.WaitAsync();

            try
            {
                if (!IsLoadingScreenShowing)
                {
                    return;
                }

                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        await PopupNavigation.Instance.PopAsync(false);
                    }
                    catch (Exception e)
                    {
                        Crashes.TrackError(e);
                    }
                });

                IsLoadingScreenShowing = false;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                if (LoadingSemaphore.CurrentCount == 0)
                {
                    LoadingSemaphore.Release();
                }
            }
        }

        public async Task ShowLoading(string message = null)
        {
            await LoadingSemaphore.WaitAsync();

            try
            {
                if (IsLoadingScreenShowing)
                {
                    return;
                }

                IsLoadingScreenShowing = true;

                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup(message ?? "Loading..."));
                    }
                    catch (Exception e)
                    {
                        Crashes.TrackError(e);
                    }
                });
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                if (LoadingSemaphore.CurrentCount == 0)
                {
                    LoadingSemaphore.Release();
                }
            }
        }

        public async Task AlertAsync(string message, string title = null, string okay = null)
        {
            try
            {
                await UserDialogs.Instance.AlertAsync(message, title, okay);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task<string> ShowActionSheetAsync(string title = null, string destructive = null, string[] buttons = null,
            string cancel = "Cancel")
        {
            try
            {
                return await UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, null, buttons);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return string.Empty;
        }
    }
}
