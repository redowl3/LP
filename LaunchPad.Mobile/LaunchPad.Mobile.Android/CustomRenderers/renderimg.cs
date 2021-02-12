using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;

namespace LaunchPad.Mobile.Droid.CustomRenderers
{
    internal class renderimg : Java.Lang.Object, Html.IImageGetter
    {
        [Obsolete]
        public Drawable GetDrawable(string source)
        {
            Drawable drawable;
            Bitmap bitMap;
            BitmapFactory.Options bitMapOption;
            try
            {
                bitMapOption = new BitmapFactory.Options();
                bitMapOption.InJustDecodeBounds = false;
                bitMapOption.InPreferredConfig = Bitmap.Config.Argb4444;
                bitMapOption.InPurgeable = true;
                bitMapOption.InInputShareable = true;
                var url = new Java.Net.URL(source);

                bitMap = BitmapFactory.DecodeStream(url.OpenStream(), null, bitMapOption);
                drawable = new BitmapDrawable(bitMap);
                //var url = new Java.Net.URL(source);
                //drawable =  Drawable.CreateFromStream(url.OpenStream(), null);
            }
            catch (Exception ex)
            {
                return null;
            }

            drawable.SetBounds(0, 0, bitMapOption.OutWidth, bitMapOption.OutHeight);
            return drawable;
        }

        public new IntPtr Handle
        {
            get { return base.Handle; }
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}