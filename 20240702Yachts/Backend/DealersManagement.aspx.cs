using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Backend
{
    public partial class DealersManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                showCountryList();

                // 先進行綁定
                DropDownListCountrySelect.DataBind();

                // 傳入 CountryId
                showDealsersList(DropDownListCountrySelect.SelectedValue);
            }
        }



        // Show Country list
        public void showCountryList()
        {

            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [Country]";
            SqlDataReader rd = db.SearchDB(sqlCommand);
            GridViewCountry.DataSource = rd;
            GridViewCountry.DataBind();

        }




        // 創建 Country
        protected void ButtonCreateCountry_Click(object sender, EventArgs e)
        {


            // 獲取使用者輸入的值
            string countryName = TextBoxCreateName.Text;


            // 需要檢查 新增的帳號是否有跟資料庫的帳號重複
            bool haveSameAccount = false;

            DBHelper db2 = new DBHelper();

            string sqlCommand2 = @"SELECT * FROM [Country] WHERE countryName = @countryName";

            Dictionary<string, object> parameters2 = new Dictionary<string, object>();

            parameters2["@countryName"] = countryName;

            SqlDataReader rd = db2.SearchDB(sqlCommand2, parameters2);


            if (rd.Read())
            {
                haveSameAccount = true;
                //LabelAdd.Visible = true; //帳號重複通知
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增失敗 => 新增的 Country 重複！');window.location.href='DealersManagement.aspx';", true);
            }


            db2.CloseDB();


            //無重複帳號才執行加入
            if (!haveSameAccount)
            {


                // 更改使用 DBHelper
                DBHelper db = new DBHelper();

                string sqlCommand = @"INSERT INTO [Country](countryName) VALUES(@countryName)";


                //// 使用參數化查詢來避免 SQL 注入攻擊
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@countryName"] = countryName;

                int totalNumberAffected = db.UpdateDB(sqlCommand, parameters);


                // 顯示成功跟失敗
                if (totalNumberAffected > 0)
                {
                    // 插入成功，顯示 alert
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增 Country 成功！');window.location.href='DealersManagement.aspx';", true);
                }
                else
                {
                    // 插入失敗，顯示 alert
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('新增 Country 失敗！');", true);
                }


                // 記得關閉資料庫連線
                db.CloseDB();


                // 跳轉回後臺系統 => 使用 js 去跳轉，避免 alert 沒有顯示
                //Response.Redirect("UserManagement.aspx");
            }

        }




        // 編輯功能 => 將特定資料行轉換為編輯模式 => OnRowEditing
        protected void GridViewCountry_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCountry.EditIndex = e.NewEditIndex; // 將資料行轉換為:編輯模式
            showCountryList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }




        // 編輯功能 => 將特定 資料行 取消 編輯模式 => OnRowCancelingEdit
        protected void GridViewCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCountry.EditIndex = -1; // 將資料行取消編輯模式
            showCountryList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }



        // 編輯功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的修改
        protected void GridViewCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            // 1. 找到特定表格行數 (Row) => ex: 第五行
            int IndexRow = e.RowIndex;


            // 2. 取得該行數的表格行數物件 (GridViewRow) => ex: 第五行的物件
            GridViewRow TargetRow = GridViewCountry.Rows[IndexRow];


            // 3. 於該物件內部找到要修改的欄位 (Column) 物件 => ex:物件中的 link 物件
            TextBox UpdateCountryNameTextBox = TargetRow.FindControl("TextBoxCountryName") as TextBox;

            // 4. 找到該行數的 Key Value (ID) => ex: 第五行中的 ID 欄位值
            string IDkey = GridViewCountry.DataKeys[IndexRow].Value.ToString();


            // 5. 透過 SQL 語法進行資料的修改 (開始撰寫 DBHelper 的四個流程)
            DBHelper db = new DBHelper();
            string sqlCommand = "UPDATE [Country] SET countryName=@countryName WHERE id = @id";

            // 使用參數化查詢來避免 SQL 注入攻擊
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@countryName"] = UpdateCountryNameTextBox.Text;
            parameters["@id"] = IDkey;

            db.UpdateDB(sqlCommand, parameters);


            // 6.補充:不要忘記重新 把編輯模式 改回 閱讀模式 以及執行 showCategoryGV()
            db.CloseDB();
            GridViewCountry.EditIndex = -1;
            showCountryList();  //當然也可以Redirect，就不用showGV了



        }




        // 刪除功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的刪除 => OnRowDeleting
        protected void GridViewCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. 找到特定表格行數 (Row) ⇒ ex：第五行
            int IndexRow = e.RowIndex;

            // 2. 找到該行數的 Key Value (ID) ⇒ ex：第五行中的 ID 欄位值
            string IDkey = GridViewCountry.DataKeys[IndexRow].Value.ToString();

            // 3. 透過SQL語法進行資料的修改
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [Country] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = IDkey;
            db.UpdateDB(sqlCommand, parameters);


            // 4. 補充：不要忘記重新執行 showGV()
            db.CloseDB();
            showCountryList();   //當然也可以Redirect，就不用showGV了


            // 可能還需要刪除附表 => 看看 設定 關聯 / 重疊顯示 => 會不會自己 將子項目刪除

            // 需要更新下拉選單
            Response.Write($"<script>alert('刪除 Country 成功'); window.location.href='DealersManagement.aspx' ;</script>");


        }




        // Dealers 功能 start ---------------------------------------------------------------------------------


        // Show Dealers list => 參數為 Country 傳入

        // 當 Country 下拉改變 => 有設置會觸發 postback
        protected void DropDownListCountrySelect_SelectedIndexChanged(object sender, EventArgs e)
        {

            // 新增 modal 的 Country 預設變成跟外部選擇一樣
            DropDownListCountrySelect2.SelectedIndex = DropDownListCountrySelect.SelectedIndex;

            // POST BACK 觸發
            showDealsersList(DropDownListCountrySelect.SelectedValue);

        }


        public void showDealsersList(string countryId)
        {
            if (!string.IsNullOrEmpty(countryId))
            {

                DBHelper db = new DBHelper();
                string sqlCommand = $"SELECT * FROM [Dealers] WHERE countryId=@countryId";

                // 使用參數化查詢來避免 SQL 注入攻擊
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@countryId"] = countryId;

                SqlDataReader rd = db.SearchDB(sqlCommand, parameters);
                GridViewDealers.DataSource = rd;
                GridViewDealers.DataBind();
            }
            else
            {
                DBHelper db = new DBHelper();
                string sqlCommand = $"SELECT * FROM [Dealers]";
                SqlDataReader rd = db.SearchDB(sqlCommand);
                GridViewDealers.DataSource = rd;
                GridViewDealers.DataBind();
            }



        }




        // 創建 Dealers
        protected void ButtonCreateDealers_Click(object sender, EventArgs e)
        {
            // 取得創建 Dealers 所有要使用的值
            // id 也需要



            //  countryId *  下拉選單
            string countryId = DropDownListCountrySelect2.SelectedValue;

            //  area *
            string area = TextBoxCreateDealersArea.Text;

            // dealerImgPath
            // 等上傳成功，才拿得到圖片檔案的路徑

            // name
            string name = TextBoxCreateDealersName.Text;

            // contact
            string contact = TextBoxCreateDealersContact.Text;

            // address
            string address = TextBoxCreateDealersAddress.Text;

            // tel
            string tel = TextBoxCreateDealersTel.Text;

            // fax
            string fax = TextBoxCreateDealersFax.Text;

            // email
            string email = TextBoxCreateDealersEmail.Text;

            // link
            string link = TextBoxCreateDealersLink.Text;


            // 先上傳檔案成功，取得圖片路徑，再去新增到資料庫

            // 1.取得 Server 資料夾的路徑 (記得要先去建立資料夾)
            string ServerFolderPath = Server.MapPath("~/Image/imageDealers");

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;

                // 在使用之前打開連接
                connection.Open();

                // 2.確認 FileUpload 控制項，是否有檔案
                if (FileUploadCreateDealersImg.HasFiles) // Note:控制項目可以開啟 "AllowMultiple" 功能
                {
                    // 3.將 FileUpload 控制項裡面的檔案跑迴圈
                    foreach (var postfile in FileUploadCreateDealersImg.PostedFiles)
                    {
                        // 4. 建立 單一檔案 篩選邏輯
                        int FileMemory = postfile.ContentLength; // 取得 單一檔案 容量變數
                        string FileName = Path.GetFileName(postfile.FileName); // 取得單一檔案 名稱變數
                        string FileExtension = Path.GetExtension(postfile.FileName).ToLower(); // 取得 單一檔案 檔名變數 ，並轉成小寫
                        string FilePath = Path.Combine(ServerFolderPath, FileName); // 取得 單一檔案 儲存路徑

                        if (FileMemory > 10000000)  // 4-1. 如果 單一檔案 大於 1M 跳過不存
                        {
                            continue;
                        }
                        else if (FileExtension != ".jpg") // 4-2. 如果 單一檔案 不是 ".jpg"檔名，跳過不存
                        {
                            continue;
                        }
                        else // 4-3. 如果單一檔案，吻合格式
                        {
                            // 5. 進行資料庫寫入
                            string pathStore = "/Image/imageDealers/" + FileName;

                            string sql = $"INSERT INTO [Dealers] (countryId,area,dealerImgPath,name,contact,address,tel,fax,email,link) VALUES (@countryId,@area,@dealerImgPath,@name,@contact,@address,@tel,@fax,@email,@link)";


                            sqlCommand.CommandText = sql;
                            sqlCommand.Parameters.Clear(); // 確保每次執行前清除先前的參數
                            sqlCommand.Parameters.AddWithValue("@countryId", countryId);
                            sqlCommand.Parameters.AddWithValue("@area", area);
                            sqlCommand.Parameters.AddWithValue("@dealerImgPath", pathStore);
                            sqlCommand.Parameters.AddWithValue("@name", name);
                            sqlCommand.Parameters.AddWithValue("@contact", contact);
                            sqlCommand.Parameters.AddWithValue("@address", address);
                            sqlCommand.Parameters.AddWithValue("@tel", tel);
                            sqlCommand.Parameters.AddWithValue("@fax", fax);
                            sqlCommand.Parameters.AddWithValue("@email", email);
                            sqlCommand.Parameters.AddWithValue("@link", link);

                            int result = sqlCommand.ExecuteNonQuery();

                            if (result == 1)
                            {
                                postfile.SaveAs(FilePath); // 成功資料庫寫入，將檔案存入指定資料夾路徑
                                Response.Write($"<script>alert('已新增 Dealers 成功')</script>");
                            }


                        }





                    }

                }
                else
                {
                    //Response.Write("<script>alert('沒有上傳任何圖片')</script>");

                    // 沒有上傳圖片的情況 => 只寫入文字資料

                    // 5. 進行資料庫寫入

                    string sql = $"INSERT INTO [Dealers] (countryId,area,name,contact,address,tel,fax,email,link) VALUES (@countryId,@area,@name,@contact,@address,@tel,@fax,@email,@link)";


                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.Clear(); // 確保每次執行前清除先前的參數
                    sqlCommand.Parameters.AddWithValue("@countryId", countryId);
                    sqlCommand.Parameters.AddWithValue("@area", area);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@contact", contact);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@tel", tel);
                    sqlCommand.Parameters.AddWithValue("@fax", fax);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.AddWithValue("@link", link);

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result == 1)
                    {
                        Response.Write($"<script>alert('已新增 Dealers 成功')</script>");
                    }

                }

            }


            // 多做一次，避免有進入到編輯模式
            GridViewDealers.EditIndex = -1; // 將資料行取消編輯模式 => 不要讓他進入編輯模式

            showDealsersList(DropDownListCountrySelect.SelectedValue);


            // 清空表單
            //clearAddDealersForm
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClearAddDealersForm", "clearAddDealersForm();", true);


        }


        // 編輯功能 => 將特定資料行轉換為編輯模式 => OnRowEditing => 更改成叫出 PanelEdit 顯示
        protected void GridViewDealers_RowEditing(object sender, GridViewEditEventArgs e)
        {


            GridViewDealers.EditIndex = -1; // 將資料行取消編輯模式 => 不要讓他進入編輯模式

            // 更新資料
            showDealsersList(DropDownListCountrySelect.SelectedValue);


            // 當 post back 先卷軸下拉，再去打開 modal
            // 註冊並執行捲動頁面到底部的 JS 函數
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ScrollToBottom", "scrollToBottom();", true);

            // 使用 js 去 call bootstrap 寫好的 modal 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);



            // 這邊將資料放入 modal 裡面的資料欄位

            // 獲取被編輯資料行的 ID 值
            int id = Convert.ToInt32(GridViewDealers.DataKeys[e.NewEditIndex].Value);

            // 5. 透過 SQL 語法進行資料的修改 (開始撰寫 DBHelper 的四個流程)
            DBHelper db = new DBHelper();
            string sqlCommand = "SELECT * FROM [Dealers] WHERE id = @id";

            // 使用參數化查詢來避免 SQL 注入攻擊
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = id;


            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            //要取得 rd 的 name 的值
            if (rd.HasRows)
            {
                while (rd.Read())
                {

                    // 或者直接使用欄位名稱獲取欄位值
                    string countryId = rd["countryId"].ToString();
                    string area = rd["area"].ToString();
                    string dealerImgPath = rd["dealerImgPath"].ToString();
                    string name = rd["name"].ToString();
                    string contact = rd["contact"].ToString();
                    string address = rd["address"].ToString();
                    string tel = rd["tel"].ToString();
                    string fax = rd["fax"].ToString();
                    string email = rd["email"].ToString();
                    string link = rd["link"].ToString();


                    // 看拿到的值 => 用 alert 看
                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{name}');", true);


                    TextBoxEditDealersId.Text = id.ToString();

                    DropDownListCountrySelect3.SelectedValue = countryId;

                    TextBoxEditDealersArea.Text = area;

                    // img => 這個要用 img 去接 =>  dealerImgPath
                    ImageEditDealersImg.ImageUrl = dealerImgPath;

                    TextBoxEditDealersName.Text= name;

                    TextBoxEditDealersContact.Text = contact;

                    TextBoxEditDealersAddress.Text = address;

                    TextBoxEditDealersTel.Text = tel;

                    TextBoxEditDealersFax.Text = fax;

                    TextBoxEditDealersEmail.Text = email;

                    TextBoxEditDealersLink.Text = link;



                }
            }



            // 6.補充:不要忘記重新 把編輯模式 改回 閱讀模式 以及執行 showCategoryGV()
            db.CloseDB();

            //GridViewCountry.EditIndex = -1;
            //showDealsersList(DropDownListCountrySelect.SelectedValue);  //當然也可以Redirect，就不用showGV了



        }



        // 送出編輯內容
        protected void ButtonEditDealers_Click(object sender, EventArgs e)
        {


            // 取得 Edit Dealers 所有要使用的值

            // id
            string id = TextBoxEditDealersId.Text;

            //  countryId *  下拉選單
            string countryId = DropDownListCountrySelect3.SelectedValue;

            //  area *
            string area = TextBoxEditDealersArea.Text;

            // dealerImgPath
            // 等上傳成功，才拿得到圖片檔案的路徑

            // name
            string name = TextBoxEditDealersName.Text;

            // contact
            string contact = TextBoxEditDealersContact.Text;

            // address
            string address = TextBoxEditDealersAddress.Text;

            // tel
            string tel = TextBoxEditDealersTel.Text;

            // fax
            string fax = TextBoxEditDealersFax.Text;

            // email
            string email = TextBoxEditDealersEmail.Text;

            // link
            string link = TextBoxEditDealersLink.Text;



            // 先上傳檔案成功，取得圖片路徑，再去新增到資料庫

            // 1.取得 Server 資料夾的路徑 (記得要先去建立資料夾)
            string ServerFolderPath = Server.MapPath("~/Image/imageDealers");

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;

                // 在使用之前打開連接
                connection.Open();

                // 2.確認 FileUpload 控制項，是否有檔案
                if (FileUploadEditDealersImg.HasFiles) // Note:控制項目可以開啟 "AllowMultiple" 功能
                {
                    // 3.將 FileUpload 控制項裡面的檔案跑迴圈
                    foreach (var postfile in FileUploadEditDealersImg.PostedFiles)
                    {
                        // 4. 建立 單一檔案 篩選邏輯
                        int FileMemory = postfile.ContentLength; // 取得 單一檔案 容量變數
                        string FileName = Path.GetFileName(postfile.FileName); // 取得單一檔案 名稱變數
                        string FileExtension = Path.GetExtension(postfile.FileName).ToLower(); // 取得 單一檔案 檔名變數 ，並轉成小寫
                        string FilePath = Path.Combine(ServerFolderPath, FileName); // 取得 單一檔案 儲存路徑

                        if (FileMemory > 10000000)  // 4-1. 如果 單一檔案 大於 1M 跳過不存
                        {
                            continue;
                        }
                        else if (FileExtension != ".jpg") // 4-2. 如果 單一檔案 不是 ".jpg"檔名，跳過不存
                        {
                            continue;
                        }
                        else // 4-3. 如果單一檔案，吻合格式
                        {
                            // 5. 進行資料庫寫入
                            string pathStore = "/Image/imageDealers/" + FileName;


                            string sql = $"UPDATE [Dealers] SET countryId=@countryId,area=@area,dealerImgPath=@dealerImgPath,name=@name,contact=@contact,address=@address,tel=@tel,fax=@fax,email=@email,link=@link WHERE id = @id";


                            sqlCommand.CommandText = sql;
                            sqlCommand.Parameters.Clear(); // 確保每次執行前清除先前的參數
                            sqlCommand.Parameters.AddWithValue("@id", id);
                            sqlCommand.Parameters.AddWithValue("@countryId", countryId);
                            sqlCommand.Parameters.AddWithValue("@area", area);
                            sqlCommand.Parameters.AddWithValue("@dealerImgPath", pathStore);
                            sqlCommand.Parameters.AddWithValue("@name", name);
                            sqlCommand.Parameters.AddWithValue("@contact", contact);
                            sqlCommand.Parameters.AddWithValue("@address", address);
                            sqlCommand.Parameters.AddWithValue("@tel", tel);
                            sqlCommand.Parameters.AddWithValue("@fax", fax);
                            sqlCommand.Parameters.AddWithValue("@email", email);
                            sqlCommand.Parameters.AddWithValue("@link", link);

                            int result = sqlCommand.ExecuteNonQuery();

                            if (result == 1)
                            {
                                postfile.SaveAs(FilePath); // 成功資料庫寫入，將檔案存入指定資料夾路徑
                                Response.Write($"<script>alert('已編輯 Dealers 成功')</script>");
                            }


                        }





                    }

                }
                else
                {
                    //Response.Write("<script>alert('沒有上傳任何圖片')</script>");

                    // 沒有上傳圖片的情況 => 只寫入文字資料

                    // 5. 進行資料庫寫入


                    string sql = $"UPDATE [Dealers] SET countryId=@countryId,area=@area,name=@name,contact=@contact,address=@address,tel=@tel,fax=@fax,email=@email,link=@link WHERE id = @id";



                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.Clear(); // 確保每次執行前清除先前的參數
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@countryId", countryId);
                    sqlCommand.Parameters.AddWithValue("@area", area);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@contact", contact);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@tel", tel);
                    sqlCommand.Parameters.AddWithValue("@fax", fax);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.AddWithValue("@link", link);

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result == 1)
                    {
                        Response.Write($"<script>alert('已編輯 Dealers 成功')</script>");
                    }

                }

            }


            // 多做一次，避免有進入到編輯模式
            GridViewDealers.EditIndex = -1; // 將資料行取消編輯模式 => 不要讓他進入編輯模式

            showDealsersList(DropDownListCountrySelect.SelectedValue);




            // 清理 modal 裡面的資料 => postback 應該就會清除了
            // id
            TextBoxEditDealersId.Text="";

            //  countryId *  下拉選單
           DropDownListCountrySelect3.SelectedIndex = 0;

            //  area *
            TextBoxEditDealersArea.Text = "";

            // dealerImgPath
            // 等上傳成功，才拿得到圖片檔案的路徑
            //FileUploadEditDealersImg.

            // thumbnail 清空
            ImageEditDealersImg.ImageUrl = "";


            // name
            TextBoxEditDealersName.Text = "";

            // contact
            TextBoxEditDealersContact.Text = "";

            // address
            TextBoxEditDealersAddress.Text = "";

            // tel
            TextBoxEditDealersTel.Text = "";

            // fax
            TextBoxEditDealersFax.Text = "";

            // email
            TextBoxEditDealersEmail.Text = "";

            // link
            TextBoxEditDealersLink.Text = "";


        }





        // 刪除功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的刪除 => OnRowDeleting

        protected void GridViewDealers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. 找到特定表格行數 (Row) ⇒ ex：第五行
            int IndexRow = e.RowIndex;

            // 2. 找到該行數的 Key Value (ID) ⇒ ex：第五行中的 ID 欄位值
            string IDkey = GridViewDealers.DataKeys[IndexRow].Value.ToString();

            // 3. 透過SQL語法進行資料的修改
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [Dealers] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = IDkey;
            db.UpdateDB(sqlCommand, parameters);


            // 4. 補充：不要忘記重新執行 showGV()
            db.CloseDB();


            // 更新資料
            showDealsersList(DropDownListCountrySelect.SelectedValue); //當然也可以Redirect，就不用showGV了

        }


    }
}