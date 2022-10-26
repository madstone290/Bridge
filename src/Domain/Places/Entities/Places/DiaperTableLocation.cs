using System.ComponentModel.DataAnnotations;

namespace Bridge.Domain.Places.Entities.Places
{
    /// <summary>
    /// 기저귀 테이블 위치
    /// </summary>
    public enum DiaperTableLocation
    {
        [Display(Name = "없음")]
        None,

        /// <summary>
        /// 남자화장실
        /// </summary>
        [Display(Name = "남자화장실")]
        MaleToilet,

        /// <summary>
        /// 여자화장실
        /// </summary>
        [Display(Name = "여자화장실")]
        FemaleToilet,

        /// <summary>
        /// 모두 다 있음
        /// </summary>
        Both
    }
}
