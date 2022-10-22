using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bridge.Shared
{
    /// <summary>
    /// 주어진 오브젝트를 다른 타입의 오브젝트로 변환한다.
    /// </summary>
    public class ObjectConverter
    {
        public static ObjectConverter Default { get; } = new();

        /// <summary>
        /// 열거형으로 변환
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="enumValue"></param>
        /// <returns>변환 성공여부</returns>
        private static bool TryConvertToEnum(Type type, object value, out object enumValue)
        {
            // int 및 문자열 변환
            if (Enum.IsDefined(type, value) && Enum.TryParse(type, Convert.ToString(value), true, out object? result))
            {
                enumValue = result!;
                return true;
            }

            // DisplayAttrite의 Name속성 이용
            foreach (var field in type.GetFields())
            {
                var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                {
                    if (string.Equals(displayAttr.Name, Convert.ToString(value), StringComparison.OrdinalIgnoreCase))
                    {
                        enumValue = field.GetValue(null)!;
                        return true;
                    }
                }
            }

            // 변환할 수 없음
            enumValue = new();
            return false;
        }

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
                if (!TryConvertToEnum(underlyingType, value, out var enumValue))
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
