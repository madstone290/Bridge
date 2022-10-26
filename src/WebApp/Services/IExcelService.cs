using MudBlazor;
using System.Reflection;
using Bridge.WebApp.Shared;
using ExpandedXml.Readers;
using ExpandedXml.Readers.CellReaders.CellTextConverters;
using ExpandedXml;
using ExpandedXml.Writers;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// 엑셀 기능을 제공한다.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// 엑셀 테이블을 읽고 T타입 인스턴스로 변환한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="data">엑셀 데이터</param>
        /// <returns></returns>
        IEnumerable<T> ReadTable<T>(byte[] data, ExcelOptions? options = null);

        /// <summary>
        /// 엑셀 테이블을 생성한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="list">인스턴스 리스트</param>
        /// <returns></returns>
        byte[] WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null);

        /// <summary>
        /// 엑셀파일을 업로드한다.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> UploadAsync<T>(ExcelOptions? options = null);

        /// <summary>
        /// 엑셀파일을 다운로드한다.
        /// </summary>
        /// <returns></returns>
        Task DownloadAsync<T>(string fileName, IEnumerable<T> list, ExcelOptions? options = null);

    }

    /// <summary>
    /// 엑셀 옵션
    /// </summary>
    /// <param name="Columns"></param>
    public class ExcelOptions
    {
        /// <summary>
        /// 엑셀 컬럼
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Caption"></param>
        public record Column
        {
            public Column(string name, string caption)
            {
                Name = name;
                Caption = caption;
            }
            public string Name { get; }
            public string Caption { get; }
        }

        /// <summary>
        /// 엑셀 테이블 컬럼
        /// </summary>
        public IEnumerable<Column> Columns { get; set; } = Enumerable.Empty<Column>();
    }

    public class ExcelService : IExcelService
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly IExReader _exReader = new ExReader();
        private readonly IExWriter _exWriter = new ExWriter();

        public ExcelService(IDialogService dialogService, IFileService fileService)
        {
            _dialogService = dialogService;
            _fileService = fileService;

            var boolTextConvertet = new BoolTextConverter();
            boolTextConvertet.AddTrueString("y");
            boolTextConvertet.AddTrueString("Y");
            boolTextConvertet.AddTrueString("1");
            _exReader.AddCellTextConverter(typeof(bool), boolTextConvertet);
        }

        private static IEnumerable<ExColumnHeader> CreateHeaders(IEnumerable<PropertyInfo> propertyInfos, IEnumerable<ExcelOptions.Column>? optionsColumns)
        {
            return propertyInfos.Select(pInfo =>
            {
                var caption = optionsColumns?.FirstOrDefault(c => string.Equals(c.Name, pInfo.Name))?.Caption ?? pInfo.Name;
                var propertyType = Nullable.GetUnderlyingType(pInfo.PropertyType) ?? pInfo.PropertyType;
                return new ExColumnHeader(pInfo.Name, caption, propertyType);
            });
        }

        public IEnumerable<T> ReadTable<T>(byte[] data, ExcelOptions? options = null)
        {
            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            return _exReader.Read<T>(data, CreateHeaders(properties, options?.Columns));
        }

        public byte[] WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null)
        {
            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            var data = _exWriter.Write<T>(list, CreateHeaders(properties, options?.Columns));
            return data;
        }

        public async Task<IEnumerable<T>> UploadAsync<T>(ExcelOptions? options = null)
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
            };

            var dialog = _dialogService.Show<ExcelUploadDialog>(null, options: dialogOptions, parameters: dialogParameters);

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                return ReadTable<T>((byte[])result.Data, options);
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

        public async Task DownloadAsync<T>(string fileName, IEnumerable<T> list, ExcelOptions? options = null)
        {
            var data = WriteTable(list, options);
            using var memoryStream = new MemoryStream(data);
            await _fileService.DownloadFileAsync(fileName, memoryStream);
        }
    }

}
