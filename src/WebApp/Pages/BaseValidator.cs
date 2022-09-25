using FluentValidation;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Bridge.WebApp.Pages
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
    {
        /// <summary>
        /// 모델의 전체 속성에 대해 유효성 검사를 실행한다.
        /// </summary>
        /// <returns></returns>
        public Func<object, string, Task<IEnumerable<string>>> ModelValidation =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<TModel>
                    .CreateWithOptions((TModel)model, x => x.IncludeProperties(propertyName)));

                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };


        /// <summary>
        /// 하나의 모델 속성에 대해 유효성 검사를 실행한다.
        /// </summary>
        /// <typeparam name="TProperty">속성 타입</typeparam>
        /// <param name="expression">속성 선택자</param>
        /// <returns></returns>
        public Func<TProperty, Task<IEnumerable<string>>> PropertyValidation<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var propertyName = string.Empty;
            if (expression.Body is MemberExpression m)
                propertyName = m.Member.Name;
            else if (expression.Body is UnaryExpression u && u.Operand is MemberExpression mm)
                propertyName = mm.Member.Name;

            return PropertyValidation<TProperty>(propertyName);
        }

        /// <summary>
        /// 하나의 모델 속성에 대해 유효성 검사를 실행한다.
        /// </summary>
        /// <typeparam name="TProperty">속성 타입</typeparam>
        /// <param name="propertyName">속성 이름</param>
        /// <returns></returns>
        public Func<TProperty, Task<IEnumerable<string>>> PropertyValidation<TProperty>(string propertyName)
            => async (propertyValue) =>
            {
                var model = Activator.CreateInstance<TModel>();
                typeof(TModel).GetProperty(propertyName)?.SetValue(model, propertyValue);

                var result = await ValidateAsync(ValidationContext<TModel>
                    .CreateWithOptions(model, x => x.IncludeProperties(propertyName)));

                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };

    }
   
}
