namespace ExpandedXml.Readers.CellReaders
{
    /// <summary>
    /// 셀 문자열을 다른 타입으로 변환한다.
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public interface ICellTextConverter<TValueType>
    {
        bool TryConvert(string text, out TValueType value);
    }

    /// <summary>
    /// 셀 문자열을 다른 타입으로 변환한다.
    /// </summary>
    public interface ICellTextConverter
    {
        bool TryConvert(string text, Type valueType, out object? value);
    }
}