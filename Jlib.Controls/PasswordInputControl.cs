using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;

namespace Jlib.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlib.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlib.Controls;assembly=Jlib.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:PasswordInputControl/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(PasswordBox))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Placeholder", Type = typeof(TextBlock))]
    public class PasswordInputControl : Control
    {
        private PasswordBox _PasswordBox;
        private TextBox _TextBox;
        private ToggleButton _ToggleButton;
        private TextBlock _Placeholder;

        static PasswordInputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordInputControl),new FrameworkPropertyMetadata(typeof(PasswordInputControl)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _PasswordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;
            _TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            _ToggleButton = GetTemplateChild("PART_ToggleButton") as ToggleButton;
            _Placeholder = GetTemplateChild("PART_Placeholder") as TextBlock;

            if (_PasswordBox != null)
            {
                _PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
                _PasswordBox.GotFocus += InputBox_GotFocus;
                _PasswordBox.LostFocus += InputBox_LostFocus;
            }

            if (_TextBox != null)
            {
                _TextBox.TextChanged += TextBox_TextChanged;
                _TextBox.GotFocus += InputBox_GotFocus;
                _TextBox.LostFocus += InputBox_LostFocus;
            }

            if (_ToggleButton != null)
            {
                _ToggleButton.Checked += ToggleButton_Checked;
                _ToggleButton.Unchecked += ToggleButton_Unchecked;
                _ToggleButton.Click += ToggleButton_Click;
            }

            UpdatePlaceholderVisibility();
            UpdatePasswordVisibility();
        }

        #region  Password 依赖属性
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordInputControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordInputControl control)
            {
                control.UpdatePasswordBoxes();
                control.UpdatePlaceholderVisibility();
            }
        }
        #endregion

        #region 请输入密码
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string),
            typeof(PasswordInputControl),  new PropertyMetadata("密码"));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        #endregion

        #region 是否显示密码
        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register("IsPasswordVisible", typeof(bool), typeof(PasswordInputControl),
                new PropertyMetadata(false, OnPasswordVisibilityChanged));

        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        private static void OnPasswordVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordInputControl control)
            {
                control.UpdatePasswordVisibility();
            }
        }
        #endregion

        #region  IconForeground
        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(PasswordInputControl),
                new PropertyMetadata(Brushes.Gray));

        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }
        #endregion

        #region CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PasswordInputControl),
                new PropertyMetadata(new CornerRadius(4)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region Event Handlers

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _PasswordBox.Password;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password = _TextBox.Text;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = true;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = false;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsPasswordVisible)
            {
                _TextBox?.Focus();
                //_TextBox?.SelectAll();
            }
            else
            {
                _PasswordBox?.Focus();
                _PasswordBox?.GetType().GetMethod("Select", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                    .Invoke(_PasswordBox, new object[] {_PasswordBox.Password.Length,0});
            }
        }
        private bool _IsFocused = false;
        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
            _IsFocused = true;
            UpdatePlaceholderVisibility();
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            _IsFocused = false;
            UpdatePlaceholderVisibility();
        }

        #endregion

        #region Helper Methods

        private void UpdatePasswordBoxes()
        {
            if (_PasswordBox != null && _PasswordBox.Password != Password)
            {
                _PasswordBox.Password = Password;
            }

            if (_TextBox != null && _TextBox.Text != Password)
            {
                _TextBox.Text = Password;
            }
        }

        private void UpdatePlaceholderVisibility()
        {
            if (_Placeholder != null)
            {
                _Placeholder.Visibility = string.IsNullOrEmpty(Password) && !_IsFocused
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void UpdatePasswordVisibility()
        {
            if (_PasswordBox == null || _TextBox == null) return;

            if (IsPasswordVisible)
            {
                _TextBox.Visibility = Visibility.Visible;
                _PasswordBox.Visibility = Visibility.Collapsed;
                _TextBox.Text = Password;
            }
            else
            {
                _PasswordBox.Visibility = Visibility.Visible;
                _TextBox.Visibility = Visibility.Collapsed;
                _PasswordBox.Password = Password;
            }

            VisualStateManager.GoToState(this, IsPasswordVisible ? "PasswordVisible" : "PasswordHidden", true);
        }

        #endregion
    }
}
