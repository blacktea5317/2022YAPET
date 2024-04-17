using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace YAPET.Models
{
    public class GetPwd
    {
        public static string getHashPassword(string pw)
        {
            byte[] hashValue;
            string result = "";


            UnicodeEncoding ue = new UnicodeEncoding();

            byte[] pwBytes = ue.GetBytes(pw);

            SHA256 shHash = SHA256.Create();

            hashValue = shHash.ComputeHash(pwBytes);

            foreach (byte b in hashValue)
            {
                result += b.ToString();
            }

            return result;
        }
    }
}