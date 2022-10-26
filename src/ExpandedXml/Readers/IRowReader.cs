using ClosedXML.Excel;

namespace ExpandedXml.Readers
{
    public interface IRowReader : ICustomize
    {
        /// <summary>
        /// 시트의 행을 읽는다.
        /// </summary>
        /// <param name="exRowNumber">읽기 완료 후 생성된 행의 번호</param>
        /// <param name="xlRow">읽은 xl행</param>
        /// <param name="exColumns">행 읽기에 사용할 컬럼 정보</param>
        /// <returns></returns>
        ExRow Read(int exRowNumber, IXLRow xlRow, IEnumerable<ExColumn> exColumns);
    }
}
