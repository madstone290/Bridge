using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static string AddQueryParam(this string uri, string name, object value)
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
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {    
                var paramString = "{" + property.Name + "}";
                var index = uri.IndexOf(paramString);
                if (0 < index)
                {
                    var value = Convert.ToString(property.GetValue(obj, null));
                    uri = uri.Replace(paramString, value);
                }
            }
            return uri;
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
