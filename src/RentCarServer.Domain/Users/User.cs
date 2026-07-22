using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Users
{
    public sealed class User : Entity
    {
        public User(
            FirstName firstName,
            LastName lastName,
            Email email,
            UserName userName,
            Password password)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetFullName();
            SetUserName(userName);
            SetPassword(password);
            SetIsForgotPasswordComplated(new(true));
            SetTFAStatus(new(false));

        }

        public User() { }

        public FirstName FirstName { get; private set; } = default!;
        public LastName LastName { get; private set; } = default!;
        public FullName FullName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public UserName UserName { get; private set; } = default!;
        public Password Password { get; private set; } = default!;
        public ForgotPasswordCode? ForgotPasswordCode { get; private set; }
        public ForgotPasswordDate? ForgotPasswordDate { get; private set; }
        public IsForgotPasswordComplated IsForgotPasswordComplated { get; private set; } = default!;

        //2FA
        public TFAStatus TFAStatus { get; private set; } = default!;
        public TFACode? TFACode { get; private set; } 
        public TFAConfirmCode? TFAConfirmCode { get; private set; } 
        public TFAExpiresDate? TFAExpiresDate { get; private set; } 
        public TFAIsCompleted? TFAIsCompleted { get; private set; } 


        #region Behaviors
        public bool VerifyPasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(Password.PasswordHash);
        }

        public void CreateForgotPasswordId()
        {
            ForgotPasswordCode = new(Guid.CreateVersion7());
            ForgotPasswordDate = new(DateTimeOffset.Now);
            IsForgotPasswordComplated = new(false);
        }

        public void SetFirstName(FirstName firstName)
        {
            FirstName = firstName;
        }
        public void SetLastName(LastName lastName)
        {
            LastName = lastName;
        }
        public void SetEmail(Email email)
        {
            Email = email;
        }
        public void SetUserName(UserName userName)
        {
            UserName = userName;
        }
        public void SetFullName()
        {
            FullName = new($"{FirstName.Value} {LastName.Value} ({Email.Value})");
        }
        public void SetPassword(Password password)
        {
            Password = password;
        }
        public void SetIsForgotPasswordComplated(IsForgotPasswordComplated isForgotPasswordComplated)
        {
            IsForgotPasswordComplated = isForgotPasswordComplated;
        }

        public void SetTFAStatus(TFAStatus tfaStatus)
        {
            TFAStatus = tfaStatus;
        }

        public void CreateTFACode()
        {
            var code = Guid.CreateVersion7().ToString();
            var confirmCode = Guid.CreateVersion7().ToString();
            var expires = DateTimeOffset.Now.AddMinutes(5);
            TFACode = new(code);
            TFAConfirmCode = new(confirmCode);
            TFAExpiresDate = new(expires);
            TFAIsCompleted = new(false);
        }

        public void SetTFACompleted()
        {
            TFAIsCompleted = new(true);
        }


        #endregion
    }


}
