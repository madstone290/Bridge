namespace ExpandedXml.Readers.CellReaders.CellTextConverters
{
    /// <summary>
    /// 문자열을 불리언 값으로 변환한다.
    /// </summary>
    public class BoolTextConverter : CellTextConverter<bool>
    {
        private readonly HashSet<string> _trueStrings = new()
        {
            "yes",
            "y",
            "1",
        };

        /// <summary>
        /// 참으로 판단할 문자열을 추가한다
        /// </summary>
        /// <param name="trueString"></param>
        public void AddTrueString(string trueString)
        {
            _trueStrings.Add(trueString);
        }

        public override bool TryConvert(string text, Type valueType, out object? value)
        {
            value = null;
            if (valueType != typeof(bool)) 
                return false;
            
            value = _trueStrings.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase));
            return true;
        }
    }
}
