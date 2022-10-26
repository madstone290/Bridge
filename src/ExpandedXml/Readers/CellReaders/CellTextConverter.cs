namespace ExpandedXml.Readers.CellReaders
{
    public abstract class CellTextConverter<TValueType> : ICellTextConverter, ICellTextConverter<TValueType>
    {
        public abstract bool TryConvert(string text, Type valueType, out object? value);

        bool ICellTextConverter<TValueType>.TryConvert(string text, out TValueType value)
        {
            var result = ((ICellTextConverter)this).TryConvert(text, typeof(TValueType), out object? objValue);
            if (objValue != null)
                value = (TValueType)objValue;
            else
                value = default!;
            return result;
        }
    }
}