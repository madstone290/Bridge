namespace ExpandedXml
{
    /// <summary>
    /// 시트 컬럼. 컬럼 번호 및 컬럼 데이터 타입을 가진다.
    /// </summary>
    public class ExColumn
    {
        public ExColumn(int number, Type type)
        {
            Number = number;
            Type = type;
        }

        /// <summary>
        /// 엑셀 컬럼 번호. 1부터 시작한다.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// 컬럼의 데이터 타입
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 컬럼 헤더
        /// </summary>
        public ExColumnHeader? Header { get; private set; }

        public void SetHeader(ExColumnHeader header)
        {
            Header = header;
        }

        public override string ToString()
        {
            return $"{typeof(ExColumn).Name}, {{Number: {Number}, Type: {Type.Name}}}";
        }

    }
}
