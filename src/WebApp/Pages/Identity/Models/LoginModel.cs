using FluentValidation;

namespace Bridge.WebApp.Pages.Identity.Models
{
    public class LoginModel
    {
        public class Validator : BaseValidator<LoginModel>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("유효한 이메일 형식이 아닙니다");

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("비밀번호가 비었습니다");

            }
        }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }


    }
}
