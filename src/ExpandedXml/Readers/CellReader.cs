using ClosedXML.Excel;
using ExpandedXml.Readers.CellReaders;

namespace ExpandedXml.Readers
{
    public abstract class CellReader<TValueType> : ICellReader<TValueType>
    {
        public ICellTextConverter? CellTextConverter { get; set; }

        public abstract bool TryRead(IXLCell cell, out TValueType value);

        bool ICellReader.TryRead(IXLCell cell, out object? value)
        {
            var result = ((ICellReader<TValueType>)this).TryRead(cell, out TValueType specificValue);
            value = specificValue;

            return result;
        }
    }
}
