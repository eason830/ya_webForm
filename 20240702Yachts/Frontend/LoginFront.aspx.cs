using _20240702Yachts.Backend;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace _20240702Yachts.Frontend
{
    public partial class LoginFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        // Argon2 驗證加密密碼
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


        //驗證
        private bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash); // LineQ => 是 LINQ 提供的一個方法，用來比較兩個序列是否相等。
        }



        //設定驗證票
        private void SetAuthenTicket(string userData,string userId)
        {
            //宣告一個驗證票 //需額外引入 using System.Web.Security;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3),false,userData);

            // 加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // 建立 Cookie
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            //將 Cookie 寫入回應
            Response.Cookies.Add(authenticationCookie);

        }


        // 登入鍵
        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            // 輸入的帳密
            string account = TextBoxAccount.Text;
            string password = TextBoxPassword.Text;

            // 之後再替換成使用 DBHelper
            //// 連線資料庫
            //DBHelper db = new DBHelper();
            //string sqlCommand = @"SELECT * FROM [User] WHERE account = @account";
            //// 使用參數化查詢來避免 SQL 注入攻擊
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters["@account"] = account;
            //SqlDataReader rd = db.SearchDB(sqlCommand, parameters);
            //// 建立一個空的 DataTable
            //DataTable dataTable = new DataTable();

            //// 將 SqlDataReader 的資料加載到 DataTable 中
            //dataTable.Load(rd);

            //// 關閉 SqlDataReader
            //rd.Close();

            //// 如果需要，可以在這裡處理 DataTable，例如顯示資料
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    foreach (var item in row.ItemArray)
            //    {
            //        Console.Write(item + "\t");
            //    }
            //    Console.WriteLine();
            //}


            // 使用 Adapter 的做法
            // 1.連線資料庫
            SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString);

            // 2.sql語法 (@參數化避免隱碼攻擊)
            string sql = "SELECT * FROM [User] WHERE account = @account";

            // 3.創建 command 物件
            SqlCommand command = new SqlCommand(sql, connection);

            // 4.放入參數化資料
            command.Parameters.AddWithValue("@account", account);

            // 5.資料庫用 Adapter 執行指令
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

            // 6.建立一個空的 Table
            DataTable dataTable = new DataTable();

            // 7.將資料放入 Table
            dataAdapter.Fill(dataTable);


            // 登入流程管理 (Cookie)
            if (dataTable.Rows.Count > 0)  // SQL 有找到資料時執行
            {
                // 將字串轉為 byte
                byte[] hash = Convert.FromBase64String(dataTable.Rows[0]["password"].ToString());
                byte[] salt = Convert.FromBase64String(dataTable.Rows[0]["salt"].ToString());

                // 驗證密碼
                bool success = VerifyHash(password, salt,hash);


                if (success) { 
                    // 宣告驗證票要夾帶的資料 (用;區隔)
                    string userData = dataTable.Rows[0]["permission"].ToString() + ";" + dataTable.Rows[0]["account"].ToString() + ";" + dataTable.Rows[0]["name"].ToString() + ";" + dataTable.Rows[0]["email"].ToString();

                    //設定驗證票(夾帶資料，cookie 命名) // 需額外引入using System.Web.Configuration;
                    SetAuthenTicket(userData, account);

                    //導頁至權限分流頁
                    Response.Redirect("CheckAccount.ashx");

                }
                else
                {
                    //資料庫裡找不到相同資料時，表示密碼有誤!
                    //Label4.Text = "password error, login failed!";
                    //Label4.Visible = true;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('password error, login failed！');", true);

                    connection.Close();
                    return;
                }


            }
            else
            {
                //資料庫裡找不到相同資料時，表示帳號有誤!
                //Label4.Text = "Account error, login failed!";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Account error, login failed!');", true);

                //Label4.Visible = true;
                //終止程式
                //Response.End(); //會清空頁面
                //return;
            }

            connection.Close();

        }
    }
}