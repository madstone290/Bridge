namespace Bridge.WebApp.Pages
{
    /// <summary>
    /// Form 및 유효성 검증
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IFormValidation<TModel> : IForm, IValidation<TModel>
    {
    }
}
