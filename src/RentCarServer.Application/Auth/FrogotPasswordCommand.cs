using FluentValidation;
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
internal sealed class FrogotPasswordCommandHandler(IUserRepository userRepository) :
    IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p => p.Email.value.Equals(request.Email), cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        // Şifre sıfırlama maili gönder
        return "Şifre sıfırlama mailiniz gönderilmiştir. Litfen mail adresinizi kontrol ediniz.";
    }
}
