using Foundation;
using Metars.iOS.Effects;
using Metars.Views.Effects;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("MyApps")]
[assembly: ExportEffect(typeof(iOSTooltipEffect), nameof(TooltipEffect))]
namespace Metars.iOS.Effects
{
    public class iOSTooltipEffect : PlatformEffect
    {
        private EasyTipView.EasyTipView _tooltip;
        private UITapGestureRecognizer _tapGestureRecognizer;

        public iOSTooltipEffect()
        {
            _tooltip = new EasyTipView.EasyTipView();
        }

        private void OnTap(object sender, EventArgs e)
        {
            var control = Control ?? Container;
            var text = TooltipEffect.GetText(Element);
            if (!string.IsNullOrEmpty(text))
            {
                _tooltip.BubbleColor = TooltipEffect.GetBackgroundColor(Element).ToUIColor();
                _tooltip.ForegroundColor = TooltipEffect.GetTextColor(Element).ToUIColor();
                _tooltip.Text = new NSString(text);
                UpdatePosition();

                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }
                _tooltip?.Show(control, vc.View, true);
            }
        }

        protected override void OnAttached()
        {
            var control = Control ?? Container;
            if (control is UIButton)
            {
                var btn = Control as UIButton;
                btn.TouchUpInside += OnTap;
            }
            else
            {
                _tapGestureRecognizer = new UITapGestureRecognizer((UITapGestureRecognizer obj) =>
                {
                    OnTap(obj, EventArgs.Empty);
                });
                control.UserInteractionEnabled = true;
                control.AddGestureRecognizer(_tapGestureRecognizer);
            }
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            if (control is UIButton)
            {
                var btn = Control as UIButton;
                btn.TouchUpInside -= OnTap;
            }
            else
            {
                if (_tapGestureRecognizer != null)
                    control.RemoveGestureRecognizer(_tapGestureRecognizer);

            }
            _tooltip?.Dismiss();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == TooltipEffect.BackgroundColorProperty.PropertyName)
            {
                _tooltip.BubbleColor = TooltipEffect.GetBackgroundColor(Element).ToUIColor();
            }
            else if (args.PropertyName == TooltipEffect.TextColorProperty.PropertyName)
            {
                _tooltip.ForegroundColor = TooltipEffect.GetTextColor(Element).ToUIColor();
            }
            else if (args.PropertyName == TooltipEffect.TextProperty.PropertyName)
            {
                _tooltip.Text = new Foundation.NSString(TooltipEffect.GetText(Element));
            }
            else if (args.PropertyName == TooltipEffect.PositionProperty.PropertyName)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            var position = TooltipEffect.GetPosition(Element);
            switch (position)
            {
                case TooltipPosition.Top:
                    _tooltip.ArrowPosition = EasyTipView.ArrowPosition.Bottom;
                    break;
                case TooltipPosition.Left:
                    _tooltip.ArrowPosition = EasyTipView.ArrowPosition.Right;
                    break;
                case TooltipPosition.Right:
                    _tooltip.ArrowPosition = EasyTipView.ArrowPosition.Left;
                    break;
                case TooltipPosition.Bottom:
                default:
                    _tooltip.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    break;
            }
        }
    }
}