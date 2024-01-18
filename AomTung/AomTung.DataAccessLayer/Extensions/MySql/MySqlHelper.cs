using System.Security.Cryptography;
using System.Text;

namespace AomTung.DataAccessLayer.Extensions.MySql
{
    internal class MySqlHelper : IMySqlHelper
    {
        public string aes_encrypt(string input, string saltKey)
        {
            if (string.IsNullOrEmpty(saltKey))
            {
                throw new ArgumentNullException(nameof(saltKey), "Salt key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            using var aes = Aes.Create(); // Use default provider
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = mkey(saltKey);
            aes.IV = new byte[16]; // Initialize with zeros

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return ToHexString(xBuff);
        }

        public string aes_decrypt(string input, string saltKey)
        {
            if (string.IsNullOrEmpty(saltKey))
            {
                throw new ArgumentNullException(nameof(saltKey), "Salt key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            using var aes = Aes.Create(); // Use default provider
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = mkey(saltKey);
            aes.IV = new byte[16]; // Initialize with zeros

            var decrypt = aes.CreateDecryptor();
            byte[] encryptedStr = FromHex2ByteArray(input);

            using (var ms = new MemoryStream(encryptedStr))
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs))
            {
                string plainText = reader.ReadToEnd();
                return plainText;
            }
        }

        private byte[] mkey(string skey)
        {
            byte[] key = Encoding.UTF8.GetBytes(skey);
            byte[] k = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < key.Length; i++)
            {
                k[i % 16] = (byte)(k[i % 16] ^ key[i]);
            }
            return k;
        }

        private string ToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        private static byte[] FromHex2ByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");
            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
            }
            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }
    }
}
