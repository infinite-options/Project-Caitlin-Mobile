using System;
using Xamarin.Forms;

namespace ProjectCaitlin.Extensions
{
    public class ExtendedButton : Xamarin.Forms.Button
    {
        [Obsolete]
        public static BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create<ExtendedButton, Xamarin.Forms.TextAlignment>(x => x.HorizontalTextAlignment, Xamarin.Forms.TextAlignment.Center);

        [Obsolete]
        public Xamarin.Forms.TextAlignment HorizontalTextAlignment
        {
            get
            {
                return (Xamarin.Forms.TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            }
            set
            {
                SetValue(HorizontalTextAlignmentProperty, value);
            }
        }
    }
}
