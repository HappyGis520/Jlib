using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Jlib.Controls
{
    /// <summary>
    /// 提示消息控件  
    /// </summary>
    public class NotificationControl : Control
    {
        static NotificationControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationControl),
                new FrameworkPropertyMetadata(typeof(NotificationControl)));
            AudioPlayer.Initialize();
        }

        #region Notification 依赖属性
        public static readonly DependencyProperty NotificationProperty =DependencyProperty.Register("Notification", typeof(Notification), typeof(NotificationControl),
            new PropertyMetadata(null, OnMessageNotificationChanged));
        public Notification Notification
        {
            get => (Notification)GetValue(NotificationProperty);
            set => SetValue(NotificationProperty, value);
        }
        private static void OnMessageNotificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NotificationControl)d;
            var notification = e.NewValue as Notification;

            if (notification != null)
            {
                control.Visibility = Visibility.Visible;
                control.Message = notification.Message;
                control.NotificationType = notification.Type;

                var result = control.PlaySound(out var msg);

                if (notification.Type == EnumNotificationType.Error)
                {
                    control.StartShakeAnimation();
                }
            }

        }

        #endregion

        #region Message 依赖属性
        // 这些可以保留原来的属性作为支持模板使用
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(NotificationControl), new PropertyMetadata(string.Empty));
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            private set => SetValue(MessageProperty, value);
        }
        #endregion

        #region Message 依赖属性

        public static readonly DependencyProperty NotificationTypeProperty =
            DependencyProperty.Register("NotificationType", typeof(EnumNotificationType), typeof(NotificationControl), new PropertyMetadata(EnumNotificationType.None));

        public EnumNotificationType NotificationType
        {
            get => (EnumNotificationType)GetValue(NotificationTypeProperty);
            private set => SetValue(NotificationTypeProperty, value);
        }

        #endregion



        private static void OnMessageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 可扩展逻辑：切换样式或颜色
        }

        private bool PlaySound(out string message)
        {
            message = string.Empty;
            var soundFile = string.Empty;
            switch (NotificationType)
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
                    message = "未知消息类型，无法播放声音";
                    break;
            }

            if (!string.IsNullOrEmpty(soundFile))
            {
                try
                {
                    //var player = new System.Media.SoundPlayer(soundFile);
                    //player.Play();
                    AudioPlayer.PlayMp3(soundFile);
                    return true;
                }
                catch {
                    message = "播放异常";
                    return false;
                }
            }
            return false;
        }

        private void StartShakeAnimation()
        {
            if (this.Template.FindName("PART_Content", this) is FrameworkElement content)
            {
                var animation = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromPercent(0.1)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromPercent(0.2)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromPercent(0.3)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromPercent(0.4)));
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));

                Storyboard.SetTarget(animation, content);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

                var sb = new Storyboard();
                sb.Children.Add(animation);
                content.RenderTransform = new TranslateTransform();
                sb.Begin();
            }
        }
    }

}
