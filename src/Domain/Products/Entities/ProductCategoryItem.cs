using Bridge.Domain.Common;

namespace Bridge.Domain.Products.Entities
{
    /// <summary>
    /// 제품 카테고리
    /// </summary>
    public class ProductCategoryItem : Entity
    {
        private ProductCategoryItem() { }
        public ProductCategoryItem(ProductCategory category)
        {
            Category = category;
        }

        /// <summary>
        /// 카테고리
        /// </summary>
        public ProductCategory Category { get; set; }

        public override int GetHashCode()
        {
            return Category.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj is ProductCategoryItem item)
            {
                return item.Category == Category;
            }
            return false;
        }
    }
}
