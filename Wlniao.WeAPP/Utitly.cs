using System;
using System.Collections.Generic;
using System.Text;

namespace Wlniao.WeAPP
{
    /// <summary>
    /// 
    /// </summary>
    public class Utitly
    {
        /// <summary>
        /// 微信小程序 encryptedData 解密
        /// </summary>
        /// <param name="encryptedDataStr"></param>
        /// <param name="key">session_key</param>
        /// <param name="iv">iv</param>
        /// <returns></returns>
        public string AES_Decrypt(string encryptedDataStr, string key, string iv)
        {
            var rijalg = System.Security.Cryptography.Aes.Create();
            rijalg.KeySize = 128;

            rijalg.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijalg.Mode = System.Security.Cryptography.CipherMode.CBC;

            rijalg.Key = System.Convert.FromBase64String(key);
            rijalg.IV = System.Convert.FromBase64String(iv);


            var decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);
            var encryptedData = System.Convert.FromBase64String(encryptedDataStr);
            using (var msDecrypt = new System.IO.MemoryStream(encryptedData))
            {
                using (var csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
