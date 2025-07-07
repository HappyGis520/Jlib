/*******************************************************************
 * * 功   能：  事件转命令
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
namespace Jlib
{
    /// <summary>
    /// 关联UI事件与ViewModel中的命令
    /// 事件参数以命令参数 CommandParameter 形式传递
    /// </summary>
    public class EventToCommandBehavior : Behavior<UIElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        #region 事件
        // 
        public string Event { get { return (string)GetValue(EventProperty); } set { SetValue(EventProperty, value); } }
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        #endregion

        #region 命令
        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));
        #endregion

        #region PassArguments
        // PassArguments (default: false)
        public bool PassArguments { 
            get { return (bool)GetValue(PassArgumentsProperty); } 
            set { SetValue(PassArgumentsProperty, value); }
        }
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior),
            new PropertyMetadata(false));

        #endregion

        #region Paramer
        /// <summary>
        /// 参数
        /// </summary>
        public object Paramer
        {
            get { return (object)GetValue(ParamerProperty); }
            set { SetValue(ParamerProperty, value); }
        }
        public static readonly DependencyProperty ParamerProperty = DependencyProperty.Register("Paramer", typeof(object), typeof(EventToCommandBehavior),
            new PropertyMetadata(null));

        #endregion

        protected override void OnAttached()
        {
            AttachHandler(this.Event); // initial set
        }


        /// <summary>
        /// 附加事件
        /// </summary>
        private void AttachHandler(string eventName)
        {
          
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(this.AssociatedObject, _handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = this.AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(this.AssociatedObject, _handler);
                    _oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'", eventName, this.AssociatedObject.GetType().Name));
            }
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        private void ExecuteCommand(object sender, EventArgs e)
        {

            object parameter = this.PassArguments ? e : Paramer;
            if (this.Command != null)
            {

                if (this.Command.GetType().BaseType.Name.Contains("ActionCommandEvent"))
                {
                    var info = this.Command.GetType().GetProperty("e");
                    info.SetValue(this.Command, e, null);
                }
                if (e.GetType() == typeof(MouseButtonEventArgs))
                {

                }
                //( this.Command)
                try {

                    if (this.Command.CanExecute(parameter))
                        this.Command.Execute(parameter);
                }
                catch {
                   
                }
            }
        }
    }

     

}
