using System.Windows;
using System.Windows.Controls;

namespace Jlib.Controls
{
    public class CameraStatusControl : Control
    {
        static CameraStatusControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CameraStatusControl),
                new FrameworkPropertyMetadata(typeof(CameraStatusControl)));
        }

        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register(nameof(IsConnected), typeof(bool), typeof(CameraStatusControl),
                new PropertyMetadata(true));

        public bool IsConnected
        {
            get => (bool)GetValue(IsConnectedProperty);
            set => SetValue(IsConnectedProperty, value);
        }
    }

}
