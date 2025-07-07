using System.Windows;
using System.Windows.Media;

namespace Jlib.Controls
{
    /// <summary>
    /// 标签扩展类
    /// </summary>
    public static class ControlHelper
    {
        #region IconSource
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.RegisterAttached(  "IconSource",
                typeof(ImageSource),  typeof(ControlHelper),  new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIconSource(UIElement element, ImageSource value)
        {
            element.SetValue(IconSourceProperty, value);
        }

        public static ImageSource GetIconSource(UIElement element)
        {
            return (ImageSource)element.GetValue(IconSourceProperty);
        }

        #endregion

        #region TitleText
        public static readonly DependencyProperty TitleTextProperty = DependencyProperty.RegisterAttached( "TitleText",
            typeof(string), typeof(ControlHelper), new FrameworkPropertyMetadata("工位", FrameworkPropertyMetadataOptions.Inherits));

        public static void SetTitleText(UIElement element, string value)
        {
            element.SetValue(TitleTextProperty, value);
        }

        public static string GetTitleText(UIElement element)
        {
            return (string)element.GetValue(TitleTextProperty);
        }
        #endregion


        #region 父级容器元素

        public static FrameworkElement GetContainElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ContainElementProperty);
        }
        public static void SetContainElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(ContainElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for ContainElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainElementProperty =
            DependencyProperty.RegisterAttached("ContainElement", typeof(FrameworkElement), typeof(ControlHelper), new PropertyMetadata(null));
        #endregion


    }
}
