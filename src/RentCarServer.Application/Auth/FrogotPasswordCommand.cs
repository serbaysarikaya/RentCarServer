using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result<string>>;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Geçerli bir Email adresi girin")
            .EmailAddress().WithMessage("Geçerli bir Email adresi girin");
    }
}
internal sealed class FrogotPasswordCommandHandler(
    IUserRepository userRepository,
     IUnitOfWork unitOfWork,
    IMailService mailService) :    IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p => p.Email.Value.Equals(request.Email), cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        user.CreateForgotPasswordId();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string to = user.Email.Value;
        string subject = "Şifre Sıfırla";

        string templatePth = Path.Combine(AppContext.BaseDirectory, "Templates", "ForgotPasswordEmail.html");
        

        string body = await File.ReadAllTextAsync(templatePth,cancellationToken);

        string resetPasswordUrl = $"https://localhost:4200/reset-password?code={user.ForgotPasswordId!.Value}";

        body = body
              .Replace("{UserName}", user.FirstName.Value)
              .Replace("{ResetPasswordUrl}", resetPasswordUrl);
        await mailService.SendAsync(to, subject, body, cancellationToken);
        return "Şifre sıfırlama mailiniz gönderilmiştir. Lütfen mail adresinizi kontrol edin";
    }
}
