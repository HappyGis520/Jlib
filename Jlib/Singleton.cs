/*******************************************************************
 * * 功   能：  单一实例模板
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;

namespace Jlib
{
        /// <summary>
        /// 单一实例模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Singleton<T> where T : new()
        {
            protected Singleton()
            {
                if (Instance != null)
                {
                    throw (new Exception("单例模式，请用class.Instance方式\""));
                }
            }
            public static T Instance
            {
                get
                {
                    return SingletonCreator.instance;
                }
            }
            class SingletonCreator
            {
                static SingletonCreator()
                {
                }
                internal static readonly T instance = new T();
            }
        }
}
