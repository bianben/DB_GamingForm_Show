using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DB_GamingForm_Show;

namespace Gaming_Forum
{
    public class ClassUtility
    {   //===================紀錄會員ID
        public static int aid { get; set; }
        public static int FirmID { get; set; }
        public static int MemberID { get; set; }
        //====================格式測試
        public static bool FirmName { get; set; }
        public static bool Password { get; set; }
        public static bool Phone { get; set; }
        public static bool Email { get; set; }
        public static bool TaxID { get; set; }
        public static bool FirmAddress { get; set; }

        public static bool FirmScale { get; set; }
        //=======================加密方法
        public static string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
        //==========================測試方法
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public  string CheckName(string input, ref string result)
        {
            if (input == "")
            {
                result = "請輸入公司名稱";
                FirmName = false;
            }
            else if (db.Firms.Any(Firm => Firm.FirmName == input))
            {
                result = "公司名稱重複";
                FirmName = false;
            }
            else
            {
                result = "公司名稱可使用";
                FirmName = true;
            }
            return result;
        }
        public string CheckEmail(string input, ref string result)
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

        public string CheckPassword(string input, ref string result)
        {
            if (input == "")
            {
                result = "請輸入密碼";
                Password = false;
            }
            else if (Regex.IsMatch(input, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"))
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

        public string CheckPhone(string input, ref string result)
        {
            if (input == "")
            {
                result = "請輸入電話號碼";
                Phone = false;
            }
            else if (db.Firms.Any(p => p.Contact == input))
            {
                result = "此電話號碼已被註冊";
                Phone = false;
            }
            else if (Regex.IsMatch(input, @"^\(0[0-9]\)[0-9]{3,4}-[0-9]{4}$"))
            {
                result = "電話號碼格式正確";
                Phone = true;
            }
            else
            {
                result = "電話號碼格式錯誤";
                Phone = false;
            }
            return result;
        }

        public string CheckAddress(string input, ref string result)
        {
            if (input == "")
            {
                result = "請輸入公司地址";
                FirmAddress = false;
            }
            else if (db.Firms.Any(Firm => Firm.FirmAddress == input))
            {
                result = "地址重複";
                FirmAddress = false;
            }

            else
            {
                result = "格式正確";
                FirmAddress = true;
            }
            return result;
        }

        public string CheckFirmScale(string input, ref string result)
        {
            if (Int32.TryParse(input, out int Firmscale) == false)
            {
                result = "請輸入員工數";
                FirmScale = false;
            }

            else 
            {
                result = "格式正確";
                FirmScale = true;
            }
            return result;
        }

        public Firm GetFirm(ref Firm firm) 
        {
            var Q = from f in db.Firms.AsEnumerable()
                    where f.FirmID == FirmID
                    select f;
            firm = Q.FirstOrDefault();
            return firm;
        }


    }
}
