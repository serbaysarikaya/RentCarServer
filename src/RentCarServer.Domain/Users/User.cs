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
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            FullName = new($"{FirstName.value} {LastName.value} ({Email.value})");
            UserName = userName;
            Password = password;
        }

        public User() { }

        public FirstName FirstName { get; private set; } = default!;
        public LastName LastName { get; private set; } = default!;
        public FullName FullName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public UserName UserName { get; private set; } = default!;
        public Password Password { get; private set; } = default!;

        public bool VerifyPasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(Password.PasswordHash);
        }
    }
}
