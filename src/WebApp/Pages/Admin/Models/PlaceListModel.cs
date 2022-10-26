using Bridge.WebApp.Api;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.DataModels;
using Bridge.WebApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class PlaceListModel
    {
        private readonly IExcelService _excelService;
        private readonly AdminRestroomApiClient _restroomApiClient;

        public bool UploadSuccessful { get; private set; }
        public string? UploadError { get; private set; }

        public int BatchTotalCount { get; private set; }
        public int BatchFailCount { get; private set; }
        public IEnumerable<string> BatchErrors { get; private set; } = Enumerable.Empty<string>();

        private ExcelOptions ExcelOptions { get; } = new ()
        {
            Columns = typeof(RestroomExcelDataModel).GetProperties()
            .Select(x => new ExcelOptions.Column(x.Name, x.GetCustomAttribute<DisplayAttribute>()?.Name ?? x.Name))
        };

        public PlaceListModel(IExcelService excelService, AdminRestroomApiClient restroomApiClient)
        {
            _excelService = excelService;
            _restroomApiClient = restroomApiClient;
        }

        public async Task DownloadExcel()
        {
            await _excelService.DownloadAsync<RestroomExcelDataModel>("화장실 폼.xlsx", new List<RestroomExcelDataModel>(), ExcelOptions);
        }

        public async Task UploadExcel()
        {
            var restrooms = await _excelService.UploadAsync<RestroomExcelDataModel>(ExcelOptions);
            if (!restrooms.Any())
                return;

            var command = new Application.Places.Commands.CreateRestroomBatchCommand()
            {
                Commands = restrooms.Select(x => new Application.Places.Commands.CreateRestroomCommand()
                {
                    Name = x.Name,
                    Address = new Application.Places.Dtos.AddressDto()
                    {
                        BaseAddress = x.BaseAddress,
                        DetailAddress = x.DetailAddress
                    },
                    IsUnisex = x.IsUnisex,
                    HasDiaperTable = x.HasDiaperTable,
                    DiaperTableLocation = x.DiaperTableLocation,
                    MaleToilet = x.MaleToilet,
                    MaleUrinal = x.MaleUrinal,
                    MaleDisabledToilet = x.MaleDisabledToilet,
                    MaleDisabledUrinal = x.MaleDisabledUrinal,
                    MaleKidToilet = x.MaleKidToilet,
                    MaleKidUrinal = x.MaleKidUrinal,
                    FemaleToilet = x.FemaleToilet,
                    FemaleDisabledToilet = x.FemaleDisabledToilet,
                    FemaleKidToilet = x.FemaleKidToilet,
                    LastUpdateDateTimeLocal = x.LastUpdateDateTime,
                    OpeningTimes = new List<Application.Places.Dtos.OpeningTimeDto>()
                    {
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Monday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Tuesday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Wednesday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Thursday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Friday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Saturday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Sunday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                    }
                })
            };

            var result = await _restroomApiClient.CreateRestroomBatch(command);

            UploadSuccessful = result.Success;
            UploadError = result.Error;

            if (UploadSuccessful)
            {
                BatchTotalCount = restrooms.Count();
                BatchFailCount = result.Data!.Count();
                BatchErrors = result.Data!;
            }
        }

    }
}
