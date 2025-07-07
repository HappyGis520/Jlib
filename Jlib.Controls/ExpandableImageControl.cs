using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Jlib.Controls
{
    public class ExpandableImageControl : Control
    {
        static ExpandableImageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ExpandableImageControl),
                new FrameworkPropertyMetadata(typeof(ExpandableImageControl)));
        }

        #region ImageSource 依赖属性

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource),
                typeof(ExpandableImageControl), new PropertyMetadata(null));

        #endregion

        #region IsExpanded 依赖属性

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool),
                typeof(ExpandableImageControl),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var toggleButton = GetTemplateChild("ToggleButton") as Button;
            if (toggleButton != null)
            {
                toggleButton.Click += (s, e) =>
                {
                    IsExpanded = !IsExpanded;
                };
            }
        }
    }
}
