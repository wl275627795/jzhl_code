using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace MH.CMN
{
    /// <summary>
    /// 系统公用的加密解密安全类，提供：
    /// 1、RSA 算法，用于验证Server License、SN中的数字签名和硬件Hash值是否有效。
    /// 2、TripleDES 加密、解密方法，用于敏感信息的传输。
    /// 3、MD5 散列值计算方法，用于用户密码的保存。
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// MD5 散列值计算方法，获取指定文本的MD5散列值。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            try
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
            }
            catch { return string.Empty; }
        }

        #region 3DES算法的KEY+IV，RSA算法的公钥

        /// <summary>
        /// 3DES算法：KEY密钥
        /// </summary>
        private static string KEY = "HHHCISRHISCISRHISCISRHIS";

        /// <summary>
        /// 3DES算法：IV向量
        /// </summary>
        private static string IV = "HCISRHIHCISRHIHCISRHISSS";

        /// <summary>
        /// 公钥 (Public Key)
        /// </summary>
        private const string RSA_Public_Key = "<RSAKeyValue><Modulus>1Bb6sfn2kB+/A8VeJr0zCSX86v93CVHdTT85vU7lgHnHexeZb6cc/+/F2tSwLQzWzi4nQkiV67dLEZlESAJ4Izlg71/QUb3ALzXBP+Jk3V4kT1whNnnNOTsC7Y6a4I4174jbk0DP380dIm+30uliwGBAOpq1iAuhpz3OxeYNA/s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        #endregion

        /// <summary>
        /// 使用DES对称加密算法对输入内容（明文）进行加密，输出密文。
        /// </summary>
        /// <param name="originalText">明文</param>
        /// <returns></returns>
        public static string Encrypt(string originalText)
        {
            try
            {
                byte[] _Key = System.Text.ASCIIEncoding.UTF8.GetBytes(KEY);
                byte[] _IV = System.Text.ASCIIEncoding.UTF8.GetBytes(IV);

                var des_csp = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des_csp.CreateEncryptor(_Key, _IV), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);

                sw.Write(originalText);
                sw.Flush();
                cs.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 使用DES对称加密算法对输入内容（密文）进行解密，输出明文，如果解密失败，返回null。
        /// </summary>
        /// <param name="cryptograph">密文</param>
        /// <returns></returns>
        public static string Decrypt(string cryptograph)
        {
            try
            {
                byte[] _Key = System.Text.ASCIIEncoding.UTF8.GetBytes(KEY);
                byte[] _IV = System.Text.ASCIIEncoding.UTF8.GetBytes(IV);

                var des_csp = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(cryptograph));
                CryptoStream cs = new CryptoStream(ms, des_csp.CreateDecryptor(_Key, _IV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 验证Server License中的数字签名是否有效（签名来源，以及和服务器硬件信息的匹配关系）。
        /// </summary>
        /// <param name="serverInfo">服务器硬件摘要信息</param>
        /// <param name="license">License授权文件内容(全文)</param>
        /// <returns></returns>
        public static bool VerifyServerLicense(string serverInfo, string license)
        {
            try
            {
                // 获得需要验证的内容
                Match match = Regex.Match(license, @"\[Security\](.*?)\[/Security\]");
                string verifyCode = match.Value.Replace("[Security]", string.Empty).Replace("[/Security]", string.Empty);

                return VerifySignature(serverInfo, verifyCode);
            }
            catch { return false; }
        }

        /// <summary>
        /// 验证序列号和验证码是否有效。
        /// </summary>
        /// <param name="sn">序列号</param>
        /// <param name="hardwareInfo">服务器硬件摘要信息</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        public static bool VerifySN(string sn, string serverInfo, string verifyCode)
        {
            return VerifySignature(sn + serverInfo, verifyCode);
        }

        /// <summary>
        /// 验证服务器关键参数的配置值和数字签名。
        /// </summary>
        /// <param name="value">关键参数的配置值</param>
        /// <param name="serverInfo">服务器硬件摘要信息</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        public static bool VerifyConfigValue(string value, string serverInfo, string verifyCode)
        {
            return VerifySignature(value + serverInfo, verifyCode);
        }

        private static bool VerifySignature(string value, string verifyCode)
        {
            try
            {
                // 加载RSA公钥，验证数字签名和内容Hash值。
                RSACryptoServiceProvider myrsa = new RSACryptoServiceProvider();
                myrsa.FromXmlString(RSA_Public_Key);
                RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(myrsa);
                f.SetHashAlgorithm("SHA1");
                byte[] key = Convert.FromBase64String(verifyCode);
                SHA1Managed sha = new SHA1Managed();
                byte[] name = sha.ComputeHash(Encoding.Unicode.GetBytes(value));
                return f.VerifySignature(name, key);
            }
            catch { return false; }
        }
    }
}
