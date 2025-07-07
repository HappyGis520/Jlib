/*******************************************************************
 * * 功   能：  命令扩展
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.Windows.Input;

namespace Jlib
{

    public class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
            }
            remove {
                CommandManager.RequerySuggested -= value;
            }
        }
        private Action _action;
        private Func<bool> _canExecute;
        public ActionCommand(Action action)
        {
            _action = action;
        }
        public ActionCommand(Action action, Func<bool> canExecute) : this(action)
        {
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute();
        }

        public void Execute(object parameter)
        {

            if (_action != null) {
                _action();
            }
        }

    }
    public class ActionCommand<T> : ICommand
    {
        #region ICommand
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            Type underlyingType = Nullable.GetUnderlyingType(typeof(T));
            var arg = (T)Convert.ChangeType(parameter, underlyingType ?? typeof(T));
            return _canExecute(arg);
        }
        public void Execute(object parameter)
        {
            if (_action != null) {
                //Console.WriteLine(_action.Target.ToString() + "--------" + _action.Method.Name);
                if (parameter == null) {
                    _action(default(T));
                }
                else {
                    Type underlyingType = Nullable.GetUnderlyingType(typeof(T));
                    var arg = (T)Convert.ChangeType(parameter, underlyingType ?? typeof(T));
                    _action(arg);
                }
            }
        }
        #endregion

        private Action<T> _action;
        private Func<T, bool> _canExecute;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public ActionCommand(Action<T> action)
        {
            _action = action;
        }
        public ActionCommand(Action<T> action, Func<T, bool> canExecute) : this(action)
        {
            _canExecute = canExecute;
        }


    }
    public class ActionCommandEvent<T>
    {
        public T e { get; set; }
    }
    /// <summary>
    /// 带两个参数的命令
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class ActionCommandE<T1, T2> : ActionCommandEvent<T2>, ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<T1, T2> _action;
        private Func<T1, T2, bool> _canExecute;
        public ActionCommandE(Action<T1, T2> action)
        {
            _action = action;
        }
        public ActionCommandE(Action<T1, T2> action, Func<T1, T2, bool> canExecute) : this(action)
        {
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            Type underlyingType = Nullable.GetUnderlyingType(typeof(T1));
            var arg = (T1)Convert.ChangeType(parameter, underlyingType ?? typeof(T1));
            return _canExecute(arg, e);
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
               
                if (parameter == null || typeof(T1).Name == "Object")
                {
                    _action(default(T1), e);
                }
                else
                {
                    //Type underlyingType = Nullable.GetUnderlyingType(typeof(T));
                    //var arg = (T)Convert.ChangeType(parameter, underlyingType ?? typeof(T));
                    //_action(arg);  
                    var arg = ChangeType<T1>(parameter);
                    _action(arg, e);
                    //if ()
                    //{
                    //    Type underlyingType = Nullable.GetUnderlyingType(typeof(T1));
                    //    var arg = (T1)Convert.ChangeType(parameter, underlyingType ?? typeof(T1));
                    //    _action(arg, e);
                    //}
                    //else
                    //{

                    //    _action((T1)parameter, e);
                    //}
                }
            }
        }
        public static T ChangeType<T>(object obj)
        {
            Type type = typeof(T);
            if ((type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object))
            {
                return (T)Convert.ChangeType(obj, type);
            }
            #region Nullable
            Type nullableType = Nullable.GetUnderlyingType(typeof(T));
            if (nullableType != null)
            {
                if (obj == null)
                {
                    return default(T);
                }
                return (T)Convert.ChangeType(obj, nullableType);
            }

            #endregion
            if (typeof(System.Enum).IsAssignableFrom(type))
            {
                return (T)Enum.Parse(type, obj.ToString());
            }
            return (T)obj;
        }
    }



}
