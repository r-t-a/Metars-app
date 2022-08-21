using Android.Content;
using Android.Graphics.Drawables;
using Metars.Views.Controls.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]
public class GradientButtonRenderer : ButtonRenderer
{
    GradientDrawable _gradient;
    public GradientButtonRenderer(Context context) : base(context) { }

    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
    {
        base.OnElementChanged(e);

        if (Control != null)
        {
            try
            {
                Control.StateListAnimator = new Android.Animation.StateListAnimator();
                Control.SetBackground(DrawGradient(e));
            }
            catch (Exception)
            {
            }
        }
    }

    private GradientDrawable DrawGradient(ElementChangedEventArgs<Button> e)
    {
        var button = e.NewElement as GradientButton;
        var orientation = button.GradientOrientation == GradientButton.GradientOrientationStates.Horizontal ?
                          GradientDrawable.Orientation.LeftRight : GradientDrawable.Orientation.TopBottom;

        _gradient = new GradientDrawable(orientation, new[]
            {
                button.StartColor.ToAndroid().ToArgb(),
                button.EndColor.ToAndroid().ToArgb(),
            });

        _gradient.SetCornerRadius(button.CornerRadius * 10);
        _gradient.SetStroke(0, button.StartColor.ToAndroid());

        return _gradient;
    }
}