using System;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using VoicePay.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(FramedEntryRenderer))]
namespace VoicePay.Droid.Renderers
{
    public class FramedEntryRenderer : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = ContextCompat.GetDrawable(Forms.Context, Resource.Drawable.rounded_corners);
                Control.Gravity = GravityFlags.CenterVertical;

                int pixels = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 10, Context.Resources.DisplayMetrics);
                Control.SetPadding(pixels, 0, 0, 0);
            }
        }

    }
}
