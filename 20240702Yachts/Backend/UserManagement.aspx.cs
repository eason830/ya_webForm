using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Razor.Tokenizer;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace _20240702Yachts.Backend
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                showUserList();

            }

        }


        // Show User list
        public void showUserList()
        {

            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [User]";
            SqlDataReader rd = db.SearchDB(sqlCommand);
            GridViewUser.DataSource = rd;
            GridViewUser.DataBind();



            // 連結資料庫，使用 using 可以確認 connection 有關閉
            //using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString))
            //{

            //    // 建立 Command 物件
            //    SqlCommand sqlCommand = new SqlCommand();

            //    // 資料庫指令 => 純粹查詢，不用使用參數
            //    //sqlCommand.CommandText = @"SELECT * FROM ConnectCategory";
            //    sqlCommand.CommandText = @"SELECT * FROM [User]";

            //    sqlCommand.Connection = connection;

            //    connection.Open();

            //    SqlDataReader rd = sqlCommand.ExecuteReader(); // Command 物件的命令賦予SQLSentence字串

            //    if (rd != null)
            //    {
            //        GridViewUser.DataSource = rd; // 將 Repeater2 的來源連結到 SqlDataReader 上
            //        GridViewUser.DataBind(); // 將 Repeater2 與資料來源進行綁定

            //    }

            //}



        }


        // 使用者管理，最高權限者，不給刪除，避免誤刪 => GridView 拿掉 Admin 的刪除鍵
        protected void GridViewUser_DataBound(object sender, EventArgs e)
        {
            GridViewUser.Rows[0].Cells[7].Controls.Clear();
        }



        // 創建 User
        protected void ButtonCreateUser_Click(object sender, EventArgs e)
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

            SqlDataReader rd = db2.SearchDB(sqlCommand2,parameters2);


            if (rd.Read())
            {
                haveSameAccount = true;
                //LabelAdd.Visible = true; //帳號重複通知
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增失敗 => 新增的帳號重複！');window.location.href='UserManagement.aspx';", true);
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
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增 User 成功！');window.location.href='UserManagement.aspx';", true);
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


        // 編輯功能 => 將特定資料行轉換為編輯模式 => OnRowEditing
        protected void GridViewUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewUser.EditIndex = e.NewEditIndex; // 將資料行轉換為:編輯模式
            showUserList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }




        // 編輯功能 => 將特定 資料行 取消 編輯模式 => OnRowCancelingEdit
        protected void GridViewUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewUser.EditIndex = -1; // 將資料行取消編輯模式
            showUserList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }



        // 編輯功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的修改
        protected void GridViewUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            // 1. 找到特定表格行數 (Row) => ex: 第五行
            int IndexRow = e.RowIndex;


            // 2. 取得該行數的表格行數物件 (GridViewRow) => ex: 第五行的物件
            GridViewRow TargetRow = GridViewUser.Rows[IndexRow];


            // 3. 於該物件內部找到要修改的欄位 (Column) 物件 => ex:物件中的 link 物件
            TextBox UpdateAccountTextBox = TargetRow.FindControl("TextBoxAccount") as TextBox;
            TextBox UpdatePasswordTextBox = TargetRow.FindControl("TextBoxPassword") as TextBox;
            TextBox UpdateEmailTextBox = TargetRow.FindControl("TextBoxEmail") as TextBox;
            TextBox UpdateNameTextBox = TargetRow.FindControl("TextBoxName") as TextBox;

            DropDownList UpdateDropDownListPermission = TargetRow.FindControl("DropDownListPermission") as DropDownList;

            // 4. 找到該行數的 Key Value (ID) => ex: 第五行中的 ID 欄位值
            string IDkey = GridViewUser.DataKeys[IndexRow].Value.ToString();


            // 5. 透過 SQL 語法進行資料的修改 (開始撰寫 DBHelper 的四個流程)
            DBHelper db = new DBHelper();
            string sqlCommand = "UPDATE [User] SET account=@account,password=@password,email=@email,name=@name,permission=@permission WHERE id = @id";

            // 使用參數化查詢來避免 SQL 注入攻擊
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@account"] = UpdateAccountTextBox.Text;
            parameters["@password"] = UpdatePasswordTextBox.Text;
            parameters["@email"] = UpdateEmailTextBox.Text;
            parameters["@name"] = UpdateNameTextBox.Text;
            parameters["@permission"] = UpdateDropDownListPermission.SelectedValue;
            parameters["@id"] = IDkey;

            db.UpdateDB(sqlCommand, parameters);


            // 6.補充:不要忘記重新 把編輯模式 改回 閱讀模式 以及執行 showCategoryGV()
            db.CloseDB();
            GridViewUser.EditIndex = -1;
            showUserList();  //當然也可以Redirect，就不用showGV了



        }




        // 刪除功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的刪除 => OnRowDeleting
        protected void GridViewUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. 找到特定表格行數 (Row) ⇒ ex：第五行
            int IndexRow = e.RowIndex;

            // 2. 找到該行數的 Key Value (ID) ⇒ ex：第五行中的 ID 欄位值
            string IDkey = GridViewUser.DataKeys[IndexRow].Value.ToString();

            // 3. 透過SQL語法進行資料的修改
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [User] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = IDkey;
            db.UpdateDB(sqlCommand, parameters);


            // 4. 補充：不要忘記重新執行 showGV()
            db.CloseDB();
            showUserList();   //當然也可以Redirect，就不用showGV了
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
        private byte[] HashPassword (string password, byte[] salt)
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