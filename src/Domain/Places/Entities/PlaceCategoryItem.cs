using Bridge.Domain.Common;

namespace Bridge.Domain.Places.Entities
{
    /// <summary>
    /// 장소 카테고리
    /// </summary>
    public class PlaceCategoryItem : Entity
    {
        private PlaceCategoryItem() { }
        public PlaceCategoryItem(PlaceCategory category)
        {
            Category = category;
        }

        /// <summary>
        /// 카테고리
        /// </summary>
        public PlaceCategory Category { get; set; }

        public override int GetHashCode()
        {
            return Category.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is PlaceCategoryItem item)
                return Category == item.Category;
            return false;
        }
    }
}