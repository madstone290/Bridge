using FluentValidation;

namespace Bridge.WebApp.Pages.Identity.Models
{
    public class RegisterModel
    {
        public class Validator : BaseValidator<RegisterModel>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .EmailAddress()
                    .WithMessage("유효한 이메일 형식이 아닙니다");

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(8, 100)
                    .WithMessage("8자 이상의 비밀번호가 필요합니다");


                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("비밀번호가 일치하지 않습니다");

                RuleFor(x => x.UserName)
                    .NotEmpty()
                    .WithMessage("이름이 비었습니다");
            }
        }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
