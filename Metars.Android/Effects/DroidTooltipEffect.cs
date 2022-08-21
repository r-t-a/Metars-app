using Android.Views;
using Metars.Views.Effects;
using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Metars.Droid.Effects;
using static Com.Tomergoldst.Tooltips.ToolTipsManager;
using Com.Tomergoldst.Tooltips;

[assembly: ResolutionGroupName("MyApps")]
[assembly: ExportEffect(typeof(DroidTooltipEffect), nameof(TooltipEffect))]
namespace Metars.Droid.Effects
{
    public class DroidTooltipEffect : PlatformEffect
    {
        private ITipListener Listener { get; set; }

        private ToolTip _toolTipView;
        private ToolTipsManager _toolTipsManager;

        public DroidTooltipEffect()
        {
            Listener = new TipListener();
            _toolTipsManager = new ToolTipsManager(Listener);
        }

        private void OnTap(object sender, EventArgs e)
        {
            var control = Control ?? Container;
            var text = TooltipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                ToolTip.Builder builder;
                var parentContent = control.RootView;

                var position = TooltipEffect.GetPosition(Element);
                switch (position)
                {
                    case TooltipPosition.Top:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(90, ' '), ToolTip.PositionAbove);
                        break;
                    case TooltipPosition.Left:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(90, ' '), ToolTip.PositionLeftTo);
                        break;
                    case TooltipPosition.Right:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(90, ' '), ToolTip.PositionRightTo);
                        break;
                    case TooltipPosition.Bottom:
                    default:
                        builder = new ToolTip.Builder(control.Context, control, parentContent as ViewGroup, text.PadRight(90, ' '), ToolTip.PositionBelow);
                        break;
                }

                builder.SetAlign(ToolTip.AlignLeft);
                builder.SetBackgroundColor(TooltipEffect.GetBackgroundColor(Element).ToAndroid());
                builder.SetTextColor(TooltipEffect.GetTextColor(Element).ToAndroid());
                _toolTipView = builder.Build();

                _toolTipsManager?.Show(_toolTipView);
            }
        }

        protected override void OnAttached()
        {
            var control = Control ?? Container;
            control.Click += OnTap;
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Click -= OnTap;
            _toolTipsManager.FindAndDismiss(control);
        }

        private class TipListener : Java.Lang.Object, ITipListener
        {
            public void OnTipDismissed(Android.Views.View p0, int p1, bool p2)
            {
            }
        }
    }
}