using Bridge.Application.Places.Commands;
using Bridge.Domain.Places.Entities.Places;
using Bridge.WebApp.Api;
using Bridge.WebApp.Api.ApiClients.Admin;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class RestroomFormModel
    {
        private readonly List<OpeningTimeFormModel> _openingTimes = new()
        {
            new OpeningTimeFormModel(DayOfWeek.Monday),
            new OpeningTimeFormModel(DayOfWeek.Tuesday),
            new OpeningTimeFormModel(DayOfWeek.Wednesday),
            new OpeningTimeFormModel(DayOfWeek.Thursday),
            new OpeningTimeFormModel(DayOfWeek.Friday),
            new OpeningTimeFormModel(DayOfWeek.Saturday),
            new OpeningTimeFormModel(DayOfWeek.Sunday),
        };

        public AdminRestroomApiClient RestroomApiClient { get; set; } = null!;

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 기본주소
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        public bool IsUnisex { get; set; }

        /// <summary>
        /// 기저귀 교환대 여부
        /// </summary>
        public bool HasDiaperTable { get; set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        public DiaperTableLocation? DiaperTableLocation { get; set; }

        public int? MaleToilet { get; set; }

        public int? MaleUrinal { get; set; }

        public int? MaleDisabledToilet { get; set; }

        public int? MaleDisabledUrinal { get; set; }

        public int? MaleKidToilet { get; set; }

        public int? MaleKidUrinal { get; set; }

        public int? FemaleToilet { get; set; }

        public int? FemaleKidToilet { get; set; }

        public int? FemaleDisabledToilet { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTimeFormModel> OpeningTimes
        {
            get => _openingTimes;
            set
            {
                foreach (var openingTime in _openingTimes.ToArray())
                {
                    var entry = value.FirstOrDefault(x => x.Day == openingTime.Day);
                    if (entry != null)
                    {
                        _openingTimes.Remove(_openingTimes.First(x => x.Day == openingTime.Day));
                        _openingTimes.Add(entry);
                    }
                }
            }
        }

        /// <summary>
        /// 영업시간(월요일부터)
        /// </summary>
        public IEnumerable<OpeningTimeFormModel> OpeningTimesFromMonday
        {
            get
            {
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Monday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Tuesday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Wednesday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Thursday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Friday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Saturday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Sunday);
            }
        }

        public async Task<IApiResult> LoadRestroomAsync(long placeId)
        {
            var result = await RestroomApiClient.GetRestroomById(placeId);
            if (!result.Success)
                return result;

            var restroomDto = result.Data!;
            Id = restroomDto.Id;
            Name = restroomDto.Name;
            BaseAddress = restroomDto.Address.BaseAddress;
            DetailAddress = restroomDto.Address.DetailAddress;
            IsUnisex = restroomDto.IsUnisex;
            HasDiaperTable = restroomDto.HasDiaperTable;
            DiaperTableLocation = restroomDto.DiaperTableLocation;
            MaleToilet = restroomDto.MaleToilet;
            MaleUrinal = restroomDto.MaleUrinal;
            MaleDisabledToilet = restroomDto.MaleDisabledToilet;
            MaleDisabledUrinal = restroomDto.MaleDisabledUrinal;
            MaleKidToilet = restroomDto.MaleKidToilet;
            MaleKidUrinal = restroomDto.MaleKidUrinal;
            FemaleToilet = restroomDto.FemaleToilet;
            FemaleDisabledToilet = restroomDto.FemaleDisabledToilet;
            FemaleKidToilet = restroomDto.FemaleKidToilet;
            OpeningTimes = restroomDto.OpeningTimes.Select(x => OpeningTimeFormModel.Create(x));

            return result;
        }

        public async Task<IApiResult> CreateNewRestroomAsync()
        {
            var command = new CreateRestroomCommand()
            {
                Name = Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = BaseAddress,
                    DetailAddress = DetailAddress,
                },
                IsUnisex = IsUnisex,
                HasDiaperTable = HasDiaperTable,
                DiaperTableLocation = DiaperTableLocation,
                MaleToilet = MaleToilet,
                MaleUrinal = MaleUrinal,
                MaleDisabledToilet = MaleDisabledToilet,
                MaleDisabledUrinal = MaleDisabledUrinal,
                MaleKidToilet = MaleKidToilet,
                MaleKidUrinal = MaleKidUrinal,
                FemaleToilet = FemaleToilet,
                FemaleDisabledToilet = FemaleDisabledToilet,
                FemaleKidToilet = FemaleKidToilet,
                OpeningTimes = OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.Dayoff,
                    TwentyFourHours = t.TwentyFourHours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };
            var result = await RestroomApiClient.CreateRestroom(command);
            Id = result.Data;

            return result;
        }

        public async Task<IApiResult> UpdateRestroomAsync()
        {
            var command = new UpdateRestroomCommand()
            {
                Id = Id,
                Name = Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = BaseAddress,
                    DetailAddress = DetailAddress,
                },
                IsUnisex = IsUnisex,
                HasDiaperTable = HasDiaperTable,
                DiaperTableLocation = DiaperTableLocation,
                MaleToilet = MaleToilet,
                MaleUrinal = MaleUrinal,
                MaleDisabledToilet = MaleDisabledToilet,
                MaleDisabledUrinal = MaleDisabledUrinal,
                MaleKidToilet = MaleKidToilet,
                MaleKidUrinal = MaleKidUrinal,
                FemaleToilet = FemaleToilet,
                FemaleDisabledToilet = FemaleDisabledToilet,
                FemaleKidToilet = FemaleKidToilet,
                OpeningTimes = OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.Dayoff,
                    TwentyFourHours = t.TwentyFourHours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };

            return await RestroomApiClient.UpdateRestroom(command);
        }
        
    }
}
