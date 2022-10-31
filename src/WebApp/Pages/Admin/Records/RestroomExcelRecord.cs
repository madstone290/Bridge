using Bridge.Domain.Places.Entities.Places;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bridge.WebApp.Pages.Admin.Records
{
    /// <summary>
    /// 대구광역시 공중화장실 엑셀 데이터 모델
    /// </summary>
    public class DaeguRestroomExcelRecord
    {
        /// <summary>
        /// 화장실 이름
        /// </summary>
        [Display(Name = "화장실명")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 지번주소
        /// </summary>
        [Display(Name = "소재지지번주소")]
        public string JibunAddress { get; set; } = string.Empty;

        /// <summary>
        /// 도로명주소
        /// </summary>
        [Display(Name = "소재지도로명주소")]
        public string RoadAddress { get; set; } = string.Empty;

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        [Display(Name = "남녀공용화장실여부")]
        public bool IsUnisex { get; set; }

        [Display(Name = "기저귀교환대장소")]
        public string? DiaperTableLocationText { get; set; }

        [Display(Name = "개방시간")]
        public string? OpeningTimeText { get; set; }

        [Display(Name = "남성용-대변기수")]
        public int? MaleToilet { get; set; }

        [Display(Name = "남성용-소변기수")]
        public int? MaleUrinal { get; set; }

        [Display(Name = "남성용-장애인용대변기수")]
        public int? MaleDisabledToilet { get; set; }

        [Display(Name = "남성용-장애인용소변기수")]
        public int? MaleDisabledUrinal { get; set; }

        [Display(Name = "남성용-어린이용대변기수")]
        public int? MaleKidToilet { get; set; }

        [Display(Name = "남성용-어린이용소변기수")]
        public int? MaleKidUrinal { get; set; }

        [Display(Name = "여성용-대변기수")]
        public int? FemaleToilet { get; set; }

        [Display(Name = "여성용-장애인용대변기수")]
        public int? FemaleKidToilet { get; set; }

        [Display(Name = "여성용-어린이용대변기수")]
        public int? FemaleDisabledToilet { get; set; }

        [Display(Name = "데이터기준일자")]
        public DateTime? LastUpdateDateTime { get; set; }

        public bool Is24Hours { get; set; }

        /// <summary>
        /// 개방 시작시간
        /// </summary>
        public TimeSpan? OpenTime { get; set; }

        /// <summary>
        /// 개방 종료시간
        /// </summary>
        public TimeSpan? CloseTime { get; set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        public DiaperTableLocation? DiaperTableLocation { get; set; }

        /// <summary>
        /// 기본주소
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 텍스트에서 속성값으로 변환한다.
        /// </summary>
        public void ReadFromText()
        {
            BaseAddress = string.IsNullOrWhiteSpace(JibunAddress) ? RoadAddress : JibunAddress;
            DiaperTableLocation = (DiaperTableLocationText?.Trim()) switch
            {
                "남자화장실" => (DiaperTableLocation?)Domain.Places.Entities.Places.DiaperTableLocation.MaleToilet,
                "여자화장실" => (DiaperTableLocation?)Domain.Places.Entities.Places.DiaperTableLocation.FemaleToilet,
                "남자화장실+여자화장실" or "여자화장실+남자화장실" => (DiaperTableLocation?)Domain.Places.Entities.Places.DiaperTableLocation.Both,
                _ => (DiaperTableLocation?)Domain.Places.Entities.Places.DiaperTableLocation.None,
            };

            var trimmedOpeningTimeText = OpeningTimeText?.Trim();
            if(trimmedOpeningTimeText != null)
            {
                switch (trimmedOpeningTimeText)
                {
                    case "연중무휴":
                    case "24시간":
                    case "00:00~24:00":
                        Is24Hours = true;
                        break;

                    default:
                        var regex = new Regex("^([0-9]{1,2}):([0-9]{0,2})~([0-9]{1,2}):([0-9]{0,2})");
                        var match = regex.Match(trimmedOpeningTimeText);
                        if (match.Success)
                        {
                            var parts = trimmedOpeningTimeText.Split("~");
                            
                            var start = parts[0];
                            var startParts = start.Split(":");
                            _ = int.TryParse(startParts[0], out int startHour);
                            _ = int.TryParse(startParts[1], out int startMin);
                            OpenTime = new TimeSpan(startHour, startMin, 0);

                            var end = parts[1];
                            var endParts = end.Split(":");
                            _ = int.TryParse(endParts[0], out int endHour);
                            _ = int.TryParse(endParts[1], out int endMin);
                            CloseTime = new TimeSpan(endHour, endMin, 0);
                        }
                        break;
                }
            }
           
        }

        /// <summary>
        /// 속성값을 텍스트 표현으로 변경한다.
        /// </summary>
        public void WriteToText()
        {

        }
    }
}
