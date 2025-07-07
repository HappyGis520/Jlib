/*******************************************************************
 * * 功   能：  标记扩展
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.ComponentModel;

namespace Jlib
{

    public class CaptionAttribute : Attribute
    {
        public string Caption { get; set; }
        public String Value { get; set; }

        public CaptionAttribute(string aCaption)
        {
            this.Caption = aCaption;
        }
        public CaptionAttribute(string aCaption, String aValue)
        {
            this.Caption = aCaption;
            this.Value = aValue;
        }
    }

    public static class JAttribute
    {
        /// <summary>
        /// 获取枚举说明信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetCaption(this Enum obj)
        {
            var val = obj.GetAttribute<CaptionAttribute>();
            return val == null ? string.Empty : val.Caption;
        }
        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetCaptionValue(this Enum obj)
        {
            var val = obj.GetAttribute<CaptionAttribute>();
            return val == null ? string.Empty : val.Value;
        }

        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum val) where T : Attribute
        {
            T t = default(T);
            Type type = val.GetType();
            var fd = type.GetField(val.ToString());
            if (fd == null)
                return default(T);
            object[] attrs = fd.GetCustomAttributes(typeof(T), false);
            if (attrs.Length > 0)
            {
                t = (attrs[0] as T);
            }
            return t;
        }
        ///// <summary>
        ///// 获取枚举的备注信息
        ///// </summary>
        ///// <param name="val"></param>
        ///// <returns></returns>
        //public static T GetAttribute<T>(this Enum val) where T : EnumerableButtonAttribute
        //{

        //    T t = default(T);
        //    Type type = val.GetType();
        //    var fd = type.GetField(val.ToString());
        //    if (fd == null)
        //        return default(T);
        //    object[] attrs = fd.GetCustomAttributes(typeof(T), false);
        //    if (attrs.Length > 0)
        //    {
        //        t = (attrs[0] as T);
        //    }
        //    return t;
        //}

        /// <summary>
        /// 获取类的DescribeAttribut标签内容
        /// </summary>
        /// <param name="t"></param>
        /// <param name="cName"></param>
        /// <returns></returns>
        public static string GetClassDescribe(this Type t)
        {
            string str = string.Empty;
            var desc = Attribute.GetCustomAttribute(t, typeof(DescriptionAttribute));
            if (desc != null) {
                str = ((DescriptionAttribute)desc).Description;
            }

            return str;
        }


    }
}
