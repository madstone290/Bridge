
using Bridge.Shared.Converters;

namespace Bridge.Shared
{
    /// <summary>
    /// 주어진 오브젝트를 다른 타입의 오브젝트로 변환한다.
    /// </summary>
    public class ObjectConverter
    {
        private readonly EnumConverter _enumConverter = new();

        public static ObjectConverter Default { get; } = new();
        
        public Dictionary<Type, Func<object?, object?>> CustomConverters { get; } = new()
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

        /// <summary>
        /// 주어진 Type으로 값을 변환한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object? Execute(Type type, object? value)
        {
            if (value?.GetType() == type)
                return value;

            // 1. 커스텀 변환기
            if (CustomConverters.ContainsKey(type))
                return CustomConverters[type].Invoke(value);

            // 2. 입력이 null인 경우 default값 반환
            if (value == null)
                return type.IsValueType ? Activator.CreateInstance(type) : null;

            // Nullable type 확인
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            // 3. 열거형 변환
            if (underlyingType.IsEnum)
            {
                // 변환이 성공한 경우 변환된 값 반환. 실패한 경우 기본값 반환.
                if (!_enumConverter.TryConvert(underlyingType, value, out var enumValue))
                    enumValue = Activator.CreateInstance(type);
                return enumValue;
            }

            // 4. 그 외 변환
            object? convertedValue;
            try
            { 
                convertedValue = Convert.ChangeType(value, underlyingType);
            }
            catch 
            { 
                convertedValue = underlyingType.IsValueType ? Activator.CreateInstance(type) : null; 
            }
            return convertedValue;
        }
    }
}
