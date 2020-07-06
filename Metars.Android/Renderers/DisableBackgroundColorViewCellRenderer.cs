using System;
using Metars.Droid.Renderers;
using Metars.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DisableBackgroundColorViewCell), typeof(DisableBackgroundColorViewCellRenderer))]
namespace Metars.Droid.Renderers
{
    public class DisableBackgroundColorViewCellRenderer : ViewCellRenderer
    {
    }
}
