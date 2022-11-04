using System.Text.RegularExpressions;

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
            var valueString = Convert.ToString(value);
            if (valueString == null) //null인 경우 uri 반환
                return uri;

            var queryIndex = uri.IndexOf('?');
            var hasQuery = queryIndex != -1;

            if(!hasQuery) // 쿼리가 없는 경우 파라미터 추가
                return uri + "?" + name + "=" + valueString;

            
            var nameIndex = uri.IndexOf(name + "=", queryIndex);
            var parameterExist = nameIndex != -1;
            if (!parameterExist) // 동일한 파라미터가 없는 경우 새 파라미터 추가
                return uri + "&" + name + "=" + valueString;

            // 파라미터가 중복되는 경우 덮어쓰기
            var valueIndex = nameIndex + name.Length + 1;
            var ampersandIndex = uri.IndexOf("&", valueIndex);
            if (ampersandIndex == -1)
                ampersandIndex = uri.Length;

            var oldValueLength = ampersandIndex - valueIndex;
            uri = uri.Remove(valueIndex, oldValueLength);
            uri = uri.Insert(valueIndex, valueString);
            return uri;
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
