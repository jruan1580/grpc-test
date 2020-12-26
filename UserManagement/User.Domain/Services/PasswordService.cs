using System.Security.Cryptography;
using System.Text;

namespace User.Domain.Services
{
    public interface IPasswordService
    {
        byte[] CreatePasswordHash(string password);

        bool VerifyPasswordHash(string password, byte[] passwordHash);
    }
    public class PasswordService : IPasswordService
    {
        private const string _saltString = "";
        private readonly byte[] _salt;

        public PasswordService()
        {
            _salt = Encoding.UTF8.GetBytes(_saltString);
        }

        public byte[] CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512(_salt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(_salt))
            {
                byte[] computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computerHash.Length; i++)
                {
                    if (computerHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
