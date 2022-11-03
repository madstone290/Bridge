﻿using System.Linq.Expressions;

namespace Bridge.WebApp.Pages
{
    /// <summary>
    /// Form기능을 제공하는 뷰모델
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IFormViewModel<TModel>
    {
        FormMode FormMode { get; set; } 

        /// <summary>
        /// 모델 유효성 검사 함수를 가져온다
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(Expression<Func<TModel, TProperty>> expression);
    }
}
