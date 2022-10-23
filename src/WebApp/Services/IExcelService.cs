using ClosedXML.Excel;
using MudBlazor;
using System.Reflection;
using Bridge.WebApp.Shared;
using Bridge.Shared;

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
        /// <param name="stream">엑셀 데이터</param>
        /// <returns></returns>
        IEnumerable<T> ReadTable<T>(Stream stream, ExcelOptions? options = null);

        /// <summary>
        /// 엑셀 테이블을 생성한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="list">인스턴스 리스트</param>
        /// <returns></returns>
         Stream WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null);

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

        public ExcelService(IDialogService dialogService, IFileService fileService)
        {
            _dialogService = dialogService;
            _fileService = fileService;
        }

        public IEnumerable<T> ReadTable<T>(Stream stream, ExcelOptions? options = null)
        {
            var list = new List<T>();
            var workBook = new XLWorkbook(stream);
            var worksheet = workBook.Worksheet(1);

            var sheetRows = worksheet.RowsUsed();
            var sheetColumns = worksheet.ColumnsUsed();
            if (!sheetRows.Any() || !sheetColumns.Any())
                return list;

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            var optionsColumns = options?.Columns ?? properties.Select(prop => new ExcelOptions.Column(prop.Name, prop.Name)).ToList();

            // 컬럼 캡션(헤더텍스트), 컬럼번호(데이터 읽기), 컬럼명(인스턴스 속성)
            // 컬럼번호/인스턴스속성 사전
            var columnNumberProperties = new Dictionary<int, PropertyInfo>();

            // read header row
            var headerRow = sheetRows.First();
            foreach (var column in sheetColumns)
            {
                var columnNumber = column.ColumnNumber();
                var headerCell = headerRow.Cell(columnNumber);
                var columnCaption = headerCell.GetString();
                var optionsColumn = optionsColumns.FirstOrDefault(x => string.Equals(x.Caption, columnCaption, StringComparison.OrdinalIgnoreCase));
                if (optionsColumn == null)
                    continue;

                var property = properties.FirstOrDefault(x => string.Equals(x.Name, optionsColumn.Name, StringComparison.OrdinalIgnoreCase));
                if (property == null)
                    continue;

                columnNumberProperties.Add(columnNumber, property);
            }

            // read data rows
            foreach(var dataRow in sheetRows.Skip(1))
            {
                var instance = (T)Activator.CreateInstance(typeof(T))!;
                foreach(var column in sheetColumns)
                {
                    var columnNumber = column.ColumnNumber();
                    if (!columnNumberProperties.ContainsKey(columnNumber))
                        continue;
                    
                    try
                    {
                        var property = columnNumberProperties[columnNumber];
                        var cellValue = dataRow.Cell(columnNumber).Value;
                        var propertyValue = ObjectConverter.Default.Execute(property.PropertyType, cellValue);
                        
                        property.SetValue(instance, propertyValue);
                    }
                    catch { } // Failed to change value type 
                }
                list.Add(instance);
            }

            return list;
        }

        public Stream WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null)
        {
            var wbook = new XLWorkbook();
            var worksheet = wbook.AddWorksheet();

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            var optionsColumns = options?.Columns ?? properties.Select(prop => new ExcelOptions.Column(prop.Name, prop.Name)).ToList();
            var columnNumberProperties = new Dictionary<int, PropertyInfo>();

            var rowNumber = 1;
            var columnNumber = 1;
            // write header row
            foreach (var optionsColumn in optionsColumns)
            {
                worksheet.Cell(rowNumber, columnNumber).Value = optionsColumn.Caption;
                
                var property = properties.FirstOrDefault(x => string.Equals(x.Name, optionsColumn.Name, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                    columnNumberProperties.Add(columnNumber, property);

                columnNumber++;
            }
            rowNumber++;

            // write data rows
            foreach (var instance in list)
            {
                columnNumber = 1;
                foreach (var column in optionsColumns)
                {
                    if (columnNumberProperties.ContainsKey(columnNumber))
                    {
                        var property = columnNumberProperties[columnNumber];
                        var cell = worksheet.Cell(rowNumber, columnNumber);
                        var value = property.GetValue(instance);
                        
                        cell.SetValue(value);
                    }
                    columnNumber++;
                }
                rowNumber++;
            }
            var stream = new MemoryStream();
            wbook.SaveAs(stream);

            return stream;
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
                using var stream = (Stream)result.Data;
                return ReadTable<T>(stream, options);
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

        public async Task DownloadAsync<T>(string fileName, IEnumerable<T> list, ExcelOptions? options = null)
        {
            using var stream = WriteTable(list, options);
            await _fileService.DownloadFileAsync(fileName, stream);
        }
    }

}
