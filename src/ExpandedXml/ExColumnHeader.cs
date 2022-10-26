namespace ExpandedXml
{
    /// <summary>
    /// 컬럼 헤더.
    /// </summary>
    public class ExColumnHeader
    {
        public ExColumnHeader(string name, string caption, Type type)
        {
            Name = name;
            Caption = caption;
            Type = type;
        }


        /// <summary>
        /// 컬럼의 데이터 타입
        /// </summary>
        public Type Type { get; init; }

        /// <summary>
        /// 컬럼명. 컬럼을 식별하기 위한 이름.
        /// </summary>
        public string Name { get; init;} = string.Empty;
         
        /// <summary>
        /// 컬럼 머릿말. 컬럼헤더셀의 텍스트.
        /// </summary>
        public string Caption { get; init; } = string.Empty;
    }
}
