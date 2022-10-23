using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bridge.Shared.Converters
{
    /// <summary>
    /// 열거형 변환기. Nullable 타입은 다루지 않는다.
    /// </summary>
    public class EnumConverter
    {
        /// <summary>
        /// 열거형으로 변환
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public bool TryConvert<TEnum>(object? value, out TEnum enumValue)
        {
            bool canConvert = TryConvert(typeof(TEnum), value, out var obj);
            enumValue = (TEnum)obj;
            return canConvert;
        }

        /// <summary>
        /// 열거형으로 변환
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="enumValue"></param>
        /// <returns>변환 성공여부</returns>
        public bool TryConvert(Type type, object? value, out object enumValue)
        {
            if (value == null)
            {
                enumValue = new();
                return false;
            }

            // int 및 문자열 변환
            if (Enum.IsDefined(type, value) && Enum.TryParse(type, Convert.ToString(value), true, out object? result))
            {
                enumValue = result!;
                return true;
            }

            // DisplayAttrite의 Name속성 이용
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr == null)
                    continue;

                if (string.Equals(displayAttr.Name, Convert.ToString(value), StringComparison.OrdinalIgnoreCase))
                {
                    enumValue = field.GetValue(null)!;
                    return true;
                }
            }

            // 변환할 수 없음
            enumValue = new();
            return false;
        }
    }
}
