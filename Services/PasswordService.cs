
using Microsoft.AspNetCore.Identity;

namespace USER_DEPS.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(new object(), password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                var result = _hasher.VerifyHashedPassword(
                    new object(),
                    passwordHash,
                    password);

                return result != PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
