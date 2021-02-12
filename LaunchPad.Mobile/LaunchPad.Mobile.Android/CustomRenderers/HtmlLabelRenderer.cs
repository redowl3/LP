using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Widget;
using LaunchPad.Mobile.CustomControls;
using LaunchPad.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace LaunchPad.Mobile.Droid.CustomRenderers
{
    public class HtmlLabelRenderer : LabelRenderer
    {

        public HtmlLabelRenderer(Context context) : base(context)
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (!string.IsNullOrEmpty(Element.Text))
                Control?.SetText(GetText(), TextView.BufferType.Spannable);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                if (!string.IsNullOrEmpty(Element.Text))
                    Control?.SetText(GetText(), TextView.BufferType.Spannable);

            }
        }

        [Obsolete]
        private ISpanned GetText()
        {
            //renderimg.GetDrawable(Element.Text);
            return (int)Build.VERSION.SdkInt >= 24
                ? Html.FromHtml(Element.Text, FromHtmlOptions.ModeLegacy, new renderimg(), null)
                : Html.FromHtml(Element.Text, new renderimg(), null);
        }
    }
}