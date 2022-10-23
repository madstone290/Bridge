using Bridge.Domain.Places.Entities.Places;
using System.ComponentModel.DataAnnotations;

namespace Bridge.WebApp.Pages.Admin.DataModels
{
    public class RestroomExcelDataModel
    {
        /// <summary>
        /// 화장실 이름
        /// </summary>
        [Display(Name = "이름")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 기본주소
        /// </summary>
        [Display(Name = "기본주소")]
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        [Display(Name = "상세주소")]
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        [Display(Name = "남녀공용 여부")]
        public bool IsUnisex { get; set; }

        /// <summary>
        /// 기저귀 교환대 여부
        /// </summary>
        [Display(Name = "기저귀 교환대 여부")]
        public bool HasDiaperTable { get; set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        [Display(Name = "기저귀 교환대 위치")]
        public DiaperTableLocation? DiaperTableLocation { get; set; }

        [Display(Name = "남-대변기")]
        public int? MaleToilet { get; set; }

        [Display(Name = "남-소변기")]
        public int? MaleUrinal { get; set; }

        [Display(Name = "남-장애인 대변기")]
        public int? MaleDisabledToilet { get; set; }

        [Display(Name = "남-장애인 소변기")]
        public int? MaleDisabledUrinal { get; set; }

        [Display(Name = "남-아이 대변기")]
        public int? MaleKidToilet { get; set; }

        [Display(Name = "남-아이 소변기")]
        public int? MaleKidUrinal { get; set; }

        [Display(Name = "여-대변기")]
        public int? FemaleToilet { get; set; }

        [Display(Name = "여-장애인 대변기")]
        public int? FemaleKidToilet { get; set; }

        [Display(Name = "여-아이 대변기")]
        public int? FemaleDisabledToilet { get; set; }

        [Display(Name = "24시간 개방 여부")]
        public bool Is24Hours { get; set; }

        [Display(Name = "개방 시작시간")]
        public TimeSpan? OpenTime { get; set; }

        [Display(Name = "개방 종료시간")]
        public TimeSpan? CloseTime { get; set; }

        [Display(Name = "최근 업데이트 일시")]
        public DateTime? LastUpdateDateTime { get; set; }
    }
}
