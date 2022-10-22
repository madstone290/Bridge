namespace Bridge.Shared
{
    /// <summary>
    /// 주어진 오브젝트를 다른 타입의 오브젝트로 변환한다.
    /// </summary>
    public class ObjectConverter
    {
        private static readonly Dictionary<Type, Func<object?, object?>> typeSpecificConverters = new()
        {
            {   typeof(bool), new Func<object?, object?>((value) =>
                {
                    var valueText = value?.ToString()?.ToLower();
                    if(valueText == "y" || valueText == "true" || valueText == "1")
                        return true;
                    return false;
                })
            }
        };

        public static object? Execute(object? value, Type type)
        {
            Type t = Nullable.GetUnderlyingType(type) ?? type;

            object? convertedValue;
            if (typeSpecificConverters.ContainsKey(t))
            {
                convertedValue = typeSpecificConverters[t].Invoke(value);
            }
            else
            {
                convertedValue = (value == null) ? null : Convert.ChangeType(value, t);
            }

            return convertedValue;
        }
    }
}
