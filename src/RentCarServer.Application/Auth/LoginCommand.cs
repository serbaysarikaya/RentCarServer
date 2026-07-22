using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.LoginTokens.ValueObjects;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;
namespace RentCarServer.Application.Auth;
public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<LoginCommandResponse>>;
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUserName).NotEmpty().WithMessage("Geçerli bir mail ya da kullanıcı adı girin");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre giriniz");
    }
}
public sealed record LoginCommandResponse
{
    public string? Token { get; set; }
    public string? TFACode { get; set; }
}
public sealed class LoginCommandHandler(
    IUserRepository userRepository, IJwtProvider jwtProvider, IMailService mailService, IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.Email.Value == request.EmailOrUserName
        || p.UserName.Value == request.EmailOrUserName);
        if (user == null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı yada şifre yanlış");
        }
        var checkPassword = user.VerifyPasswordHash(request.Password);
        if (!checkPassword)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı yada şifre yanlış");
        }
        if (!user.TFAStatus.Value)
        {
            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
            var res = new LoginCommandResponse
            {
                Token = token,
            };
            return res;
        }
        else
        {
            user.CreateTFACode();
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            string to = user.Email.Value;
            string subject = "Giriş Onayı.";
            string body = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
<meta charset='UTF-8'>
</head>
<body style='margin:0; padding:0; background-color:#f4f5f7; font-family: Arial, Helvetica, sans-serif;'>
    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f5f7; padding:40px 0;'>
        <tr>
            <td align='center'>
                <table role='presentation' width='480' cellpadding='0' cellspacing='0' style='background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
                    <tr>
                        <td style='background-color:#1a73e8; padding:24px 32px;'>
                            <h1 style='color:#ffffff; font-size:20px; margin:0;'>Giriş Onayı</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:32px;'>
                            <p style='color:#333333; font-size:15px; line-height:1.6; margin:0 0 16px 0;'>
                                Merhaba <strong>{user.UserName.Value}</strong>,
                            </p>
                            <p style='color:#333333; font-size:15px; line-height:1.6; margin:0 0 24px 0;'>
                                Hesabınıza giriş yapabilmek için aşağıdaki doğrulama kodunu kullanınız. Bu kod kısa bir süre için geçerlidir.
                            </p>
                            <div style='text-align:center; margin:32px 0;'>
                                <span style='display:inline-block; background-color:#f0f4ff; color:#1a73e8; font-size:28px; font-weight:bold; letter-spacing:6px; padding:16px 32px; border-radius:6px;'>
                                    {user.TFAConfirmCode!.Value}
                                </span>
                            </div>
                            <p style='color:#888888; font-size:13px; line-height:1.5; margin:0;'>
                                Bu işlemi siz gerçekleştirmediyseniz, bu e-postayı dikkate almayınız ve hesabınızın güvenliğini kontrol ediniz.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style='background-color:#f4f5f7; padding:20px 32px; text-align:center;'>
                            <p style='color:#aaaaaa; font-size:12px; margin:0;'>
                                &copy; {DateTime.UtcNow.Year} RentCarServer. Tüm hakları saklıdır.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
            await mailService.SendAsync(to, subject, body);
            var res = new LoginCommandResponse
            {
                TFACode = user.TFACode!.Value,
            };
            return res;
        }
    }
}