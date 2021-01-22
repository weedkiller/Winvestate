using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Winvestate_Offer_Management_MVC.Classes
{
    public static class Cipher
    {

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        /// 

        public static string Encrypt(string plainText, string password)
        {
            if (plainText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static string Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string DecryptString(string pCipherString, string pName = "")
        {
            byte[] toDecryptArray = Convert.FromBase64String(pCipherString);
            var loResult = DecryptData(toDecryptArray, pName);
            return Encoding.UTF8.GetString(loResult);
        }

        public static string EncryptString(string pToEncrypt, string pName = "")
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(pToEncrypt);
            var loResultArray = EncryptData(toEncryptArray, pName);
            return Convert.ToBase64String(loResultArray, 0, loResultArray.Length);
        }

        public static byte[] EncryptData(byte[] pToEncryptArray, string pName)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            var loEncryptionKey = new byte[] { 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45, 0x20, 0xCB, 0xA5, 0x12, 0x4A, 0xAF, 0xF8, 0x54, 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45 };

            if (!string.IsNullOrEmpty(pName))
            {
                var loNameMd5 = Encoding.UTF8.GetBytes(Md5OfString(pName));
                var loMax = loNameMd5.Length > loEncryptionKey.Length ? loEncryptionKey.Length : loNameMd5.Length;

                for (var i = 0; i < loMax; i++)
                {
                    loEncryptionKey[i] = loNameMd5[i];

                }
            }

            //set the secret key for the tripleDES algorithm
            tdes.Key = loEncryptionKey;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                (pToEncryptArray, 0, pToEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return resultArray;
        }

        public static byte[] DecryptData(byte[] pToDecryptArray, string pName)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            var loEncryptionKey = new byte[] { 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45, 0x20, 0xCB, 0xA5, 0x12, 0x4A, 0xAF, 0xF8, 0x54, 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45 };

            if (!string.IsNullOrEmpty(pName))
            {
                var loNameMd5 = Encoding.UTF8.GetBytes(Md5OfString(pName));
                var loMax = loNameMd5.Length > loEncryptionKey.Length ? loEncryptionKey.Length : loNameMd5.Length;

                for (var i = 0; i < loMax; i++)
                {
                    loEncryptionKey[i] = loNameMd5[i];

                }
            }

            //set the secret key for the tripleDES algorithm
            tdes.Key = loEncryptionKey;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                (pToDecryptArray, 0, pToDecryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return resultArray;
        }

        public static string Md5OfString(string pPlainText)
        {
            var md5 = new MD5CryptoServiceProvider();

            var btr = Encoding.UTF8.GetBytes(pPlainText);
            btr = md5.ComputeHash(btr);

            var sb = new StringBuilder();

            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            return sb.ToString().ToUpper();
        }
    }
}
