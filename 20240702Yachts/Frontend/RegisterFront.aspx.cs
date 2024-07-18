using _20240702Yachts.Backend;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class RegisterFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // 註冊使用者
        protected void ButtonSignUp_Click(object sender, EventArgs e)
        {
            // 獲取使用者輸入的值
            string name = TextBoxCreateName.Text;
            string account = TextBoxCreateAccount.Text;
            string password = TextBoxCreatePassword.Text;
            string email = TextBoxCreateEmail.Text;


            // 需要檢查 新增的帳號是否有跟資料庫的帳號重複
            bool haveSameAccount = false;

            DBHelper db2 = new DBHelper();

            string sqlCommand2 = @"SELECT * FROM [User] WHERE account = @account";

            Dictionary<string, object> parameters2 = new Dictionary<string, object>();

            parameters2["@account"] = account;

            SqlDataReader rd = db2.SearchDB(sqlCommand2, parameters2);


            if (rd.Read())
            {
                haveSameAccount = true;
                //LabelAdd.Visible = true; //帳號重複通知
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增失敗 => 新增的帳號重複！');", true);
            }


            db2.CloseDB();


            //無重複帳號才執行加入
            if (!haveSameAccount)
            {

                //Hash 加鹽加密
                var salt = CreateSalt();
                string saltStr = Convert.ToBase64String(salt); // 將 byte 改回字串存回資料表 ，這一個是用來存在資料庫
                var hash = HashPassword(password, salt);
                string hashPassword = Convert.ToBase64String(hash);



                // 更改使用 DBHelper
                DBHelper db = new DBHelper();

                string sqlCommand = @"INSERT INTO [User](name,account,password,email,salt) VALUES(@name,@account,@password,@email,@salt)";


                //// 使用參數化查詢來避免 SQL 注入攻擊
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@name"] = name;
                parameters["@account"] = account;
                parameters["@password"] = hashPassword;
                parameters["@email"] = email;
                parameters["@salt"] = saltStr;

                int totalNumberAffected = db.UpdateDB(sqlCommand, parameters);


                // 顯示成功跟失敗
                if (totalNumberAffected > 0)
                {
                    // 插入成功，顯示 alert
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增 User 成功！');window.location.href='LoginFront.aspx';", true);
                }
                else
                {
                    // 插入失敗，顯示 alert
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增 User 失敗！');", true);
                }


                // 記得關閉資料庫連線
                db.CloseDB();


                // 跳轉回後臺系統 => 使用 js 去跳轉，避免 alert 沒有顯示
                //Response.Redirect("UserManagement.aspx");
            }
        }




        // Argon2 加密
        // 產生 Salt 功能
        private byte[] CreateSalt()
        {
            var buffer = new byte[16];

            // 更高等級的生成隨機數
            var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(buffer);

            return buffer;

        }


        // Hash 處理加鹽的密碼功能
        private byte[] HashPassword(string password, byte[] salt)
        {

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            //底下這些數字會影響運算時間，而且驗證時要用一樣的值
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 1; // 四核心設置 8
            argon2.Iterations = 4; // 迭代次數
            argon2.MemorySize = 1024 * 1024; // 1GB

            return argon2.GetBytes(16);

        }






    }
}