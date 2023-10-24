using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DB_GamingForm_Show;

namespace Gaming_Forum
{
    public class Member_Firm
    {
        public static class ClassUtility
        {
            public static string HashPassword(string password)
            {
                using (var sha256 = new SHA256Managed())
                {
                    var bytes = Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);

                    return Convert.ToBase64String(hash);
                }
            }            
            public static int MemberID { get; set; }
        }
        
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        public bool ResgistedName { get; set; }
        public bool Email { get; set; }
        public bool Phone { get; set; }
        public bool Password { get; set; }


        public string CheckName(string input,ref string result)
        {
            if (input == "")
            {
                result = "請輸入暱稱";
                ResgistedName = false;
            }
            else if (db.Members.Any(n => n.Name == input))
            {
                result = "暱稱重複";
                ResgistedName = false;
            }
            else
            {
                result = "暱稱可使用";
                ResgistedName = true;
            }
            return result;
        }
        public string CheckEmail(string input,ref string result)
        {
            if (input == "")
            {
                result = "請輸入信箱";
                Email = false;
            }
            else if (db.Members.Any(em => em.Email == input))
            {
                result = "此信箱已被註冊";
                Email = false;
            }
            else if (Regex.IsMatch(input, @"^[a-zA-Z0-9_.+\-!@#$%^&*]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                result = "信箱可使用";
                Email = true;
            }
            else
            {
                result = "信箱格式錯誤";
                Email = false;
            }
            return result;
        }
        public string CheckPassword(string input,ref string result)
        {
            if (input == "")
            {
                result = "請輸入密碼";
                Password = false;
            }
            else if (Regex.IsMatch(input, "^(?=.*\\d)(?=.*[a-zA-Z]).{8,}$"))
            {
                result = "密碼格式正確";
                Password = true;
            }
            else
            {
                result = "密碼格式錯誤";
                Password = false;
            }
            return result;
        }
        public string CheckPhone(string input,ref string result)
        {
            if (input == "")
            {
                result = "請輸入手機號碼";
                Phone = false;
            }
            else if (db.Members.Any(p => p.Phone == input))
            {
                result = "此手機號碼已被註冊";
                Phone = false;
            }
            else if (Regex.IsMatch(input, "^09\\d{8}$"))
            {
                result = "手機號碼格式正確";
                Phone = true;
            }
            else
            {
                result = "手機號碼格式錯誤";
                Phone = false;
            }
            return result;
        }
    }
}
