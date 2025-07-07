using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
    ///     <MyNamespace:NotificationBox/>
    ///
    /// </summary>

    public class NotificationLabel : Control
    {
        
        static NotificationLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationLabel), new FrameworkPropertyMetadata(typeof(NotificationLabel)));
            AudioPlayer.Initialize();
        }

        #region NotificationMessage
        public static readonly DependencyProperty NotificationMessageProperty = DependencyProperty.Register(
            "NotificationMessage", typeof(Notice),  typeof(NotificationLabel),  new PropertyMetadata(null, OnNotificationChanged));
        public Notice NotificationMessage
        {
            get => (Notice)GetValue(NotificationMessageProperty);
            set => SetValue(NotificationMessageProperty, value);
        }
        private static void OnNotificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NotificationLabel control && e.NewValue is Notification notification)
            {
                control.Message = notification.Message;
                control.Type = notification.Type;
                if(control.NotificationMessage.Speak && !string.IsNullOrEmpty(control.Message))
                    control.Speak();
                control.ApplyVisualState();
                control.StartShake();
                if(control.NotificationMessage.Beep)
                    control.PlaySound(control.NotificationMessage.Type,out var msg);

            }
        }
        #endregion

        #region Message
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            nameof(Message),
            typeof(string),
            typeof(NotificationLabel),
            new PropertyMetadata(string.Empty));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            private set => SetValue(MessageProperty, value);
        }
        #endregion

        #region Type
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type),
            typeof(EnumNotificationType),
            typeof(NotificationLabel),
            new PropertyMetadata(EnumNotificationType.None));

        public EnumNotificationType Type
        {
            get => (EnumNotificationType)GetValue(TypeProperty);
            private set => SetValue(TypeProperty, value);
        }
        #endregion

        private void ApplyVisualState()
        {
            VisualStateManager.GoToState(this, Type.ToString(), true);
        }
        private bool PlaySound(EnumNotificationType type,out string errorMsg)
        {
            errorMsg = string.Empty;
            var soundFile = NotificationMessage?.SoundFile;
            if (string.IsNullOrEmpty(soundFile))
            {
                switch (type)
                {
                    case EnumNotificationType.None:
                        soundFile = "";
                        break;
                    case EnumNotificationType.Info:
                        soundFile = "Sounds/info.wav";
                        break;
                    case EnumNotificationType.Warning:
                        soundFile = "Sounds/warning.wav";
                        break;
                    case EnumNotificationType.Error:
                        soundFile = "Sounds/error.mp3";
                        break;
                    default:
                        soundFile = "";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(soundFile))
            {
                try
                {
                    AudioPlayer.PlayMp3(soundFile);
                    return true;
                }
                catch
                {
                    errorMsg = "播放异常";
                    return false;
                }
            }
            return false;
        }
        private void Speak()
        {
            try
            {
                //Task.Run(() => {
                    var  _Synthesizer = new SpeechSynthesizer()
                    {
                        Rate = 3,
                        Volume = 100
                    };
                    if (string.IsNullOrEmpty(Message)) return;
                    _Synthesizer.SpeakAsync(Message);
                //});
            }
            catch { /* ignore speech failures */ }
        }
        private void StartShake()
        {
            if (GetTemplateChild("PART_Shake") is FrameworkElement element)
            {
                var animation = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(600)
                };

                animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(-4, KeyTime.FromPercent(0.1)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(4, KeyTime.FromPercent(0.2)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(-4, KeyTime.FromPercent(0.3)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(4, KeyTime.FromPercent(0.4)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));

                element.RenderTransform = new TranslateTransform();
                Storyboard.SetTarget(animation, element);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

                var storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }
    }
}
