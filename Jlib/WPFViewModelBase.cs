/*******************************************************************
 * * 功   能：  视图模型基类
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Jlib
{
    /// <summary>
    /// 立镖MVVM视图模型基类
    /// </summary>
    public class WPFViewModelBase : GalaSoft.MvvmLight.ViewModelBase,IDisposable
    {
        public virtual void Dispose()
        {
        }

        public void Register<T>(Action<T> action)
        {
            Messenger.Default.Register<T>(this, action);
        }
        public void Register<T>(string token, Action<T> action)
        {
            Messenger.Default.Register<T>(this, token, action);
        }
        public void SendMsg<T>(T msg)
        {
            try {
                Messenger.Default.Send(msg);
            }
            catch (Exception) {
            }
        }
        public void SendMsg<T>(T msg, string token)
        {
            try {

                Messenger.Default.Send(msg, token);
            }
            catch (Exception ) {
            }
        }
    }
    /// <summary>
    /// 立镖MVVM视图模型基类
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class WPFViewModelBase<TSource> : WPFViewModelBase
    {
        protected TSource _Source;
        public virtual TSource Source {
            get {
                return _Source;
            }
            protected set {
                _Source = value;
            }
        }
        public WPFViewModelBase(TSource  source)
        {
            this._Source = source;
        }
    }
    /// <summary>
    /// 立镖MVVM视图模型基类
    /// </summary>
    /// <typeparam name="TParent">父级对象</typeparam>
    /// <typeparam name="TSource">数据源</typeparam>
    public class WPFViewModelBase<TParent, TSource> : WPFViewModelBase
    {
        protected TSource _Source;
        protected TParent _Parent;
        public virtual TSource Source { get { return _Source; }}
        
        public virtual TParent Parent { get { return _Parent; } }
        public WPFViewModelBase(TParent parent, TSource source)
        {
            this._Parent = parent;
            this._Source = source;
        }

    }
}
