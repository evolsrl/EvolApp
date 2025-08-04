using System.Security.Cryptography;
using System.Text;

namespace EvolApp.Utils
{
    public static class CryptoUtils
    {
        public static string EncriptarHash(string texto)
        {
            using SHA1 sha1 = SHA1.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            byte[] hashBytes = sha1.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
        public static string EncriptarTexto(string texto)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(texto);
            return (Convert.ToBase64String(bytes));
        }

        public static string DesencriptarTexto(string texto)
        {
            byte[] bytes = Convert.FromBase64String(texto);
            return (System.Text.Encoding.UTF8.GetString(bytes));
        }
    }
}