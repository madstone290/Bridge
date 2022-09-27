using Bridge.Domain.Common;
using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Exception;
using Bridge.Domain.Users.Entities;
using Bridge.Domain.Users.Exceptions;
using System.Collections.Generic;

namespace Bridge.Domain.Products.Entities
{
    /**
     * 추가항목
     * -제품 이미지
     * */


    /// <summary>
    /// 제품
    /// </summary>
    public class Product : AggregateRoot
    {
        /// <summary>
        /// 제품 카테고리
        /// </summary>
        private ISet<ProductCategoryItem> _categoryItems = new HashSet<ProductCategoryItem>();

        private Product() { }
        private Product(string name, Place place)
        {
            SetName(name);
            PlaceId = place.Id;
        }

        public static Product Create(User user, string name, Place place)
        {
            if (!user.IsAdmin)
                throw new NoPermissionException();

            return new Product(name, place);
        }

        /// <summary>
        /// 제품 유형
        /// </summary>
        public ProductType Type { get; private set; }

        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 제품이 판매되는 장소.
        /// ReadModel에서만 사용한다. AggregateRoot에서는 항상 null이다.
        /// </summary>
        public Place Place { get; private set; } = null!;

        /// <summary>
        /// 제품이 판매되는 장소
        /// </summary>
        public long PlaceId { get; private set; }

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; private set; }

        /// <summary>
        /// 제품 카테고리
        /// </summary>
        public IEnumerable<ProductCategoryItem> CategoryItems => _categoryItems;

        /// <summary>
        /// 제품명을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidProductNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidProductNameException();

            if (Name == name)
                return;

            Name = name;
        }

        /// <summary>
        /// 가격을 변경한다
        /// </summary>
        /// <param name="price"></param>
        /// <exception cref="InvalidPriceException"></exception>
        public void SetPrice(decimal? price)
        {
            if (Price == price)
                return;

            if (price.HasValue)
                SetPrice(price.Value);
            else
                Price = null;
                    
        }

        /// <summary>
        /// 가격을 변경한다
        /// </summary>
        /// <param name="price"></param>
        /// <exception cref="InvalidPriceException"></exception>
        public void SetPrice(decimal price)
        {
            if(price < 0)
                throw new InvalidPriceException();

            Price = price;
        }

        /// <summary>
        /// 카테고리를 추가한다
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(ProductCategory category)
        {
            _categoryItems.Add(new ProductCategoryItem(category));
        }

        /// <summary>
        /// 카테고리를 제거한다
        /// </summary>
        /// <param name="category"></param>
        public void RemoveCategory(ProductCategory category)
        {
            var categoryItem = _categoryItems.FirstOrDefault(x => x.Category == category);
            if(categoryItem != null)
                _categoryItems.Remove(categoryItem);
        }

        /// <summary>
        /// 카테고리를 추가한다
        /// </summary>
        /// <param name="categories"></param>
        public void UpdateCategories(IEnumerable<ProductCategory> categories)
        {
            _categoryItems.Clear();
            foreach (var category in categories)
                AddCategory(category);
        }

    }
}
