using ExpandedXml.Readers.CellReaders;

namespace ExpandedXml.Readers
{
    /// <summary>
    /// 커스텀 읽기를 제공한다
    /// </summary>
    public interface ICustomize
    {
        /// <summary>
        /// 주어진 타입의 셀에 텍스트 변환기를 추가한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cellTextConverter"></param>
        void AddCellTextConverter(Type type, ICellTextConverter cellTextConverter);

        /// <summary>
        /// 셀 읽기 객체를 추가한다
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cellReader"></param>
        void AddCellReader(Type type, ICellReader cellReader);
    }
}
