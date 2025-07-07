/*******************************************************************
 * * 功   能：  扩展Json标签属性
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jlib
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public class JasonNameAttribute : Attribute
    {
        public string EnglishDescribe { get; set; }
        public string ChineseDescribe { get; set; }
    }

    public class JasonContractResolver : DefaultContractResolver
    {
        // 修正后的初始化
        private static readonly JsonSerializerSettings _baseSettings = new JsonSerializerSettings();

        public string Language { get; set; } = "en";

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var attr = member.GetCustomAttribute<JasonNameAttribute>();

            if (attr != null)
            {
                property.PropertyName = Language.Equals("zh") ?
                    attr.ChineseDescribe ?? property.PropertyName : attr.EnglishDescribe ?? property.PropertyName;
            }

            return property;
        }
    }

}
