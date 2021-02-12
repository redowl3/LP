using Android.Content;
using LaunchPad.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly:ExportRenderer(typeof(Label),typeof(FixedFontLabelRenderer))]
namespace LaunchPad.Mobile.Droid.CustomRenderers
{
    public class FixedFontLabelRenderer : LabelRenderer
    {
        public FixedFontLabelRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.SetTextSize(Android.Util.ComplexUnitType.Dip, (float)e.NewElement.FontSize);
        }
    }
}