using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomePizzaBLL.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Convert the value of Enum to name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static string? ToValueName<T>(this T enumValue, bool toUpperCase = false) where T : struct, IConvertible
        {
            string? valueName = Enum.GetName(typeof(T), enumValue);

            return toUpperCase ? valueName?.ToUpper() : valueName;
        }

        /// <summary>
        /// Convert the value of the Enum to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static string ToValueName<T>(this T enumValue, string regex) where T : struct, IConvertible
        {
            var enumName = Enum.GetName(typeof(T), enumValue);
            if (string.IsNullOrEmpty(regex))
                enumName = Regex.Replace(enumName, regex, string.Empty);
            return enumName;
        }


        public static TAttribute? GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        /// <summary>
        /// Convert the name of Enum to corresponding Enum value (Case Sensitive)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string name)
        {
            string[] names = Enum.GetNames(typeof(T));
            if (((IList)names).Contains(name))
            {
                return (T)Enum.Parse(typeof(T), name);
            }
            return default;
        }


        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static string? GetDisplayName(this Enum enumValue)
        {
            try
            {
                if (enumValue == null)
                {
                    return null;
                }

                return enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?
                    .GetName();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string? GetDisplayShortName(this Enum enumValue)
        {
            try
            {
                if (enumValue == null)
                {
                    return null;
                }

                return enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?
                    .GetShortName();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string? GetDisplayDescription(this Enum enumValue)
        {
            try
            {
                if (enumValue == null)
                {
                    return null;
                }

                return enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?
                    .GetDescription();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static DisplayAttribute? GetDisplayAttr(this Enum enumValue)
        {
            try
            {
                if (enumValue == null)
                {
                    return null;
                }

                return enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
