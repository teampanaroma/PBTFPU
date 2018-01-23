using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace PBTFrontOfficeProject.Resources
{
    public class DinamikImageButton : ButtonBase
    {
        static DinamikImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DinamikImageButton), new FrameworkPropertyMetadata(typeof(DinamikImageButton)));
        }

        public string IconImageUri
        {
            get { return (string)GetValue(IconImageUriProperty); }
            set { SetValue(IconImageUriProperty, value); }
        }
        public static readonly DependencyProperty IconImageUriProperty =
            DependencyProperty.Register("IconImageUri", typeof(string), typeof(DinamikImageButton), new UIPropertyMetadata(string.Empty,
              (o, e) =>
              {
                  try
                  {
                      Uri uriSource = new Uri((string)e.NewValue, UriKind.RelativeOrAbsolute);
                      if (uriSource != null)
                      {
                          DinamikImageButton button = o as DinamikImageButton;
                          BitmapImage img = new BitmapImage(uriSource);
                          button.SetValue(IconImageProperty, img);
                      }
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              }));

        public BitmapImage IconImage
        {
            get { return (BitmapImage)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }
        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register("IconImage", typeof(BitmapImage), typeof(DinamikImageButton), new UIPropertyMetadata(null));


    }
}
