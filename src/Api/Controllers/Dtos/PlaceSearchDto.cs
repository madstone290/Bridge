using Bridge.Domain.Places.Entities;

namespace Bridge.Api.Controllers.Dtos
{
    public class PlaceSearchDto
    {
        /// <summary>
        /// 장소 유형
        /// </summary>
        public PlaceType PlaceType { get; set; }
    }
}
