using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bridge.Shared.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// 문자열에 쿼리 파라미터를 추가한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddQueryParam(this string uri, string name, object? value)
        {
            bool hasQuery = uri.Contains('?');

            if (hasQuery)
            {
                return uri + "&" + name + "=" + value;
            }
            else
            {
                return uri + "?" + name + "=" + value;
            }
        }

        /// <summary>
        /// 문자열에 라우트 파라미터를 추가한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddRouteParam(this string uri, string name, object value)
        {
            var nameWithoutBracket = name.Replace("{", string.Empty).Replace("}", string.Empty);
            var regex = new Regex($"{{{nameWithoutBracket}:?.*}}", RegexOptions.IgnoreCase);
            return regex.Replace(uri, value.ToString()!);
        }


        /// <summary>
        /// 쿼리 파라미터를 추가한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string AddQueryParam(this string uri, object obj)
        {
            var queryString = obj.ToQueryString();
            return uri + queryString;
        }

        /// <summary>
        /// 라우트 파라미터를 추가한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string AddRouteParam(this string uri, object obj)
        {
            var uriWithParam = uri;
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(obj);
                if (propertyValue == null)
                    continue;
                uriWithParam = uriWithParam.AddRouteParam(property.Name, propertyValue);
            }
            return uriWithParam;
        }

        /// <summary>
        /// 오브젝트를 쿼리문자열로 변경한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties();

            var query = string.Empty;
            foreach (var property in properties)
            {
                var value = Convert.ToString(property.GetValue(obj, null));
                if (value != null)
                {
                    query = query.AddQueryParam(property.Name, value);
                }
            }
            return query;
        }


    }
}
