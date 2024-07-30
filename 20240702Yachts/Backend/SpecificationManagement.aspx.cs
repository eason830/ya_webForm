using NetVips;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Backend
{
    public partial class SpecificationManagement : System.Web.UI.Page
    {

        // Layout 圖片 JSON 資料
        public class ImagePath
        {
            public string SavePath { get; set; }
        }

        //宣告 List<T> 方便用 Add 依序添加資料
        private List<ImagePath> savePathList = new List<ImagePath>();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DListModel.DataBind(); //先取得型號預設選取值
                DListDetailTitle.DataBind(); //先取得細節標題預設選取值
                showDetailTitleList(); // 取得 detail title 根據其 id ，此型號的 Spec 有哪幾個 detail title
                loadImageList(); //取得 Layout 組圖
                loadDetailList(); //取得標題細節
            }
        }


        // 下拉選單變換 start ------------------------------------------------------------------------------------------

        //  建立部位細項細節選取改變 SelectedIndexChanged 事件邏輯如下
        protected void RadioButtonListDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //顯示細節項目刪除按鈕
            BtnDelDetail.Visible = true;
        }




        //建立部位標題下拉選單選取改變 SelectedIndexChanged 事件邏輯如下
        protected void DListDetailTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //刷新細節選項
            RadioButtonListDetail.Items.Clear();
            loadDetailList();
        }


        // 建立遊艇型號下拉選單選取改變 SelectedIndexChanged 事件邏輯如下
        protected void DListModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //刷新全部選項
            RadioButtonListImg.Items.Clear();
            RadioButtonListDetail.Items.Clear();
            loadImageList();
            loadDetailList();
        }


        // 下拉選單變換 end   ------------------------------------------------------------------------------------------






        // GridView detail title List  start ---------------------------------------------------------------------------

        // 取得型號共有的 Spec detail list
        private void showDetailTitleList()
        {
            //依選取日期取得資料庫新聞內容
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [DetailTitleSort]";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters["dateTitle"] = Calendar1.SelectedDate.ToString("yyyy-M-dd");
            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd !=null)
            {
                GridView1.DataSource= rd;
                GridView1.DataBind();
            }

            db.CloseDB();
        }


        // 新增 detail  title 
        protected void BtnAddNewTitle_Click(object sender, EventArgs e)
        {
            //取得輸入標題字串
            string newTitleStr = TBoxAddNewTitle.Text;


            //依選取型號更新圖檔資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"INSERT INTO [DetailTitleSort] (detailTitleSort) VALUES(@newTitleStr)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@newTitleStr"] = newTitleStr;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            //畫面渲染
            //GridView1.DataBind();
            showDetailTitleList();

            //畫面渲染
            DListDetailTitle.DataBind();

            //下拉選單改為選取最新項 => 有可能會報錯 => 因為下拉選單沒有更新的情況
            DListDetailTitle.SelectedIndex = DListDetailTitle.Items.Count - 1;

            //清空輸入欄位
            TBoxAddNewTitle.Text = "";


        }


        // 編輯功能 => 將特定資料行轉換為編輯模式 => OnRowEditing
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex; // 將資料行轉換為:編輯模式
            showDetailTitleList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }




        // 編輯功能 => 將特定 資料行 取消 編輯模式 => OnRowCancelingEdit
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1; // 將資料行取消編輯模式
            showDetailTitleList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }



        // 編輯功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的修改
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            // 1. 找到特定表格行數 (Row) => ex: 第五行
            int IndexRow = e.RowIndex;


            // 2. 取得該行數的表格行數物件 (GridViewRow) => ex: 第五行的物件
            GridViewRow TargetRow = GridView1.Rows[IndexRow];


            // 3. 於該物件內部找到要修改的欄位 (Column) 物件 => ex:物件中的 link 物件
            TextBox UpdateDetailTitleSortTextBox = TargetRow.FindControl("TextBoxDetailTitleSort") as TextBox;

            // 4. 找到該行數的 Key Value (ID) => ex: 第五行中的 ID 欄位值
            string IDkey = GridView1.DataKeys[IndexRow].Value.ToString();


            // 5. 透過 SQL 語法進行資料的修改 (開始撰寫 DBHelper 的四個流程)
            DBHelper db = new DBHelper();
            string sqlCommand = "UPDATE [DetailTitleSort] SET detailTitleSort=@detailTitleSort WHERE id = @id";

            // 使用參數化查詢來避免 SQL 注入攻擊
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@detailTitleSort"] = UpdateDetailTitleSortTextBox.Text;
            parameters["@id"] = IDkey;

            db.UpdateDB(sqlCommand, parameters);


            // 6.補充:不要忘記重新 把編輯模式 改回 閱讀模式 以及執行 showCategoryGV()
            db.CloseDB();
            GridView1.EditIndex = -1;
            showDetailTitleList();  //當然也可以Redirect，就不用showGV了



        }




        // 刪除功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的刪除 => OnRowDeleting
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. 找到特定表格行數 (Row) ⇒ ex：第五行
            int IndexRow = e.RowIndex;

            // 2. 找到該行數的 Key Value (ID) ⇒ ex：第五行中的 ID 欄位值
            string IDkey = GridView1.DataKeys[IndexRow].Value.ToString();

            // 3. 透過SQL語法進行資料的修改
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [DetailTitleSort] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = IDkey;
            db.UpdateDB(sqlCommand, parameters);


            // 4. 補充：不要忘記重新執行 showGV()
            db.CloseDB();
            showDetailTitleList();   //當然也可以Redirect，就不用showGV了



            //畫面渲染
            DListDetailTitle.DataBind();
        }





        // GridView detail title 建立共用的部位標題名稱列表 GridView 刪除按鈕 OnRowDeleted 事件刷新如下
        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            //刷新下拉選單 => 可能改成使用 ado.net 去做下拉選單更新
            DListDetailTitle.DataBind();
            //刷新細節項目
            RadioButtonListDetail.Items.Clear();
            RadioButtonListDetail.DataBind();
            loadDetailList();
        }

        // GridView detail title 建立共用的部位標題名稱列表 GridView 更新按鈕 OnRowUpdated 事件刷新如下
        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //刷新下拉選單
            DListDetailTitle.DataBind();
            //刷新細節項目
            RadioButtonListDetail.Items.Clear();
            RadioButtonListDetail.DataBind();
            loadDetailList();
        }



        // GridView detail title List  end ---------------------------------------------------------------------------



        //  Layout & Deck Plan Image start ---------------------------------------------------------------------------

        #region Group Image List
        // 建立 Layout & Deck Plan Image 組圖上傳管理 loadImageList(); 方法邏輯內容如下
        private void loadImageList()
        {
            //依型號取得組圖圖片資料
            string selectModel_id = DListModel.SelectedValue;

            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT layoutDeckPlanImgPathJSON FROM [Yachts] WHERE id =@id";
            Dictionary<string,object> parameters = new Dictionary<string,object>();
            parameters["id"]=selectModel_id;
            SqlDataReader rd =db.SearchDB(sqlCommand, parameters);

            if (rd.Read()) {
                //將特殊符號解碼 => 如果只是相片路徑的 json 的話，這樣應該不用特別解碼使用，因為上傳的時候，也不會編碼
                string loadJson = HttpUtility.HtmlDecode(rd["layoutDeckPlanImgPathJSON"].ToString());

                // 反序列化
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);

            }

            db.CloseDB();


            //渲染圖片選項
            if (savePathList?.Count > 0)
            {
                foreach (var item in savePathList)
                {
                    ListItem listItem = new ListItem($"<img src='/Image/imageYachts/{item.SavePath}' alt='thumbnail' class='img-thumbnail' width='250px'/>", item.SavePath);
                    RadioButtonListImg.Items.Add(listItem);
                }
            }

            DelImageBtn.Visible = false; //刪除鈕有選擇圖片時才顯示


        }




        // 上傳  Layout & Deck Plan Image 組圖上傳圖片
        protected void UploadImgBtn_Click(object sender, EventArgs e)
        {
            //有選擇檔案才執行
            if (imageUpload.HasFile)
            {
                //取得上傳檔案大小 (限制 10MB)
                int fileSize = imageUpload.PostedFile.ContentLength;
                if (fileSize < 1024 * 1024 * 10)
                {
                    // 先讀取資料庫原有資料
                    loadImageList();

                    // 設置要存取的位置
                    string savePath = Server.MapPath("~/Image/imageYachts/");

                    // 添加圖檔資料
                    foreach (HttpPostedFile postedFile in imageUpload.PostedFiles) {
                        // 儲存圖片檔案及圖片名稱
                        // 檢查專案資料夾內有無同名檔案，有同名就加流水號
                        DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
                        string fileName = postedFile.FileName;
                        string[] fileNameArr = fileName.Split('.');

                        int count = 0;
                        foreach (var fileItem in directoryInfo.GetFiles()) {
                            if (fileItem.Name.Contains(fileNameArr[0]))
                            {
                                count++;
                            }
                        }

                        fileName = fileNameArr[0] + $"({count + 1})." + fileNameArr[1];

                        postedFile.SaveAs(savePath + fileName);
                        savePathList.Add(new ImagePath { SavePath = fileName });



                    }

                    //依遊艇型號更新資料
                    string selectModel_id = DListModel.SelectedValue;

                    //將 List<T> 資料轉為 JSON 格式字串
                    string savePathJsonStr = JsonConvert.SerializeObject(savePathList);


                    DBHelper db = new DBHelper();
                    string sqlCommand = $"UPDATE [Yachts] SET layoutDeckPlanImgPathJSON = @layoutDeckPlanImgPathJSON WHERE id = @id";
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters["@layoutDeckPlanImgPathJSON"] = savePathJsonStr;
                    parameters["@id"] = selectModel_id;

                    db.UpdateDB(sqlCommand, parameters);

                    db.CloseDB();


                    //渲染畫面
                    RadioButtonListImg.Items.Clear();
                    loadImageList();


                }
                else
                {
                    Response.Write("<script>alert('*The maximum upload size is 10MB!');</script>");
                }


            }

        }




        // 當選擇其中一張圖片要顯示 DelImgBtn 顯示出來
        protected void RadioButtonListImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            DelImageBtn.Visible = true;
        }



        // 刪除  Layout & Deck Plan Image 圖片
        protected void DelImageBtn_Click(object sender, EventArgs e)
        {
            //依選取項目刪除 List<T> 資料
            loadImageList(); //先取得 List<T> 資料

            // 取得選擇的相片
            string selImageStr = RadioButtonListImg.SelectedValue;

            // 圖片存在的資料夾
            string savePath = Server.MapPath("~/Image/imageYachts/");

            // 檔案直接刪除
            File.Delete(savePath + selImageStr);

            // 將 json 裡面的圖片名稱移除，再去更新到資料庫上
            for (int i = 0; i < savePathList.Count; i++)
            {
                if (savePathList[i].SavePath.Equals(selImageStr))
                {
                    savePathList.RemoveAt(i);
                }
            }


            //將 List<T> 資料轉為 JSON 格式字串
            string savePathJsonStr = JsonConvert.SerializeObject(savePathList);
            string selectModel_id = DListModel.SelectedValue;

            //依選取型號更新圖檔資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"UPDATE [Yachts] SET layoutDeckPlanImgPathJSON = @layoutDeckPlanImgPathJSON WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@layoutDeckPlanImgPathJSON"] = savePathJsonStr;
            parameters["@id"] = selectModel_id;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();


            //渲染畫面
            RadioButtonListImg.Items.Clear();
            loadImageList();


        }

        #endregion


        //  Layout & Deck Plan Image end   ---------------------------------------------------------------------------



        // 細項細節 detail  start  -----------------------------------------------------------------------------------

        // 建立取得標題細節內容方法 loadDetailList(); 方法邏輯內容如下
        private void loadDetailList()
        {

            //取得 Model 代表 id
            string selectModel_id = DListModel.SelectedValue;
            //取得 Title 代表 id
            string selectTitle_id = DListDetailTitle.SelectedValue;

            //依選取型號更新圖檔資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT detail FROM [Specification] WHERE yachtModel_ID = @selectModel_id AND detailTitleSort_ID = @selectTitle_id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@selectModel_id"] = selectModel_id;
            parameters["@selectTitle_id"] = selectTitle_id;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            while (rd.Read())
            {
                string detail = rd["detail"].ToString();
                //將轉成字元實體的編碼轉回 HTML 標籤語法渲染 ， new ListItem(text,value)
                ListItem listItem = new ListItem(HttpUtility.HtmlDecode(detail), detail);
                RadioButtonListDetail.Items.Add(listItem);
            }

            db.CloseDB();


            BtnDelDetail.Visible = false; //刪除鈕有選擇項目時才顯示


        }

        // 建立部位細項細節新增按鈕 Add Detail 的 OnClick 事件邏輯如下
        protected void BtnAddDetail_Click(object sender, EventArgs e)
        {
            //取得新增 Detail
            string newDetailStr = TboxDetail.Text;

            //將換行跳脫字元改成 HTML 換行標籤
            newDetailStr = newDetailStr.Replace("\r\n", "<br>");

            //依取得下拉選項的值 (id) 存入 Detail 資料
            string selectModel_id = DListModel.SelectedValue;
            string selectTitle_id = DListDetailTitle.SelectedValue;


            DBHelper db = new DBHelper();
            string sqlCommand = $"INSERT INTO [Specification] (yachtModel_ID, detailTitleSort_ID, detail) VALUES (@selectModel_id, @selectTitle_id, @detail)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@selectModel_id"] = selectModel_id;
            parameters["@selectTitle_id"] = selectTitle_id;

            //特殊符號要轉成字元實體才能正常存進資料庫
            parameters["@detail"] = HttpUtility.HtmlEncode(newDetailStr);

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            //將改成 HTML 換行標籤資料加入選項渲染畫面
            ListItem listItem = new ListItem(newDetailStr, newDetailStr);
            RadioButtonListDetail.Items.Add(listItem);
            TboxDetail.Text = "";


        }


        // 建立部位細項細節刪除按鈕 Delete Detail 的 OnClick 事件邏輯如下
        protected void BtnDelDetail_Click(object sender, EventArgs e)
        {
            //依選取資料刪除 Detail 資料
            string selectModel_id = DListModel.SelectedValue;
            string selectTitle_id = DListDetailTitle.SelectedValue;
            string selectDetailStr = RadioButtonListDetail.SelectedValue;

            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [Specification] WHERE yachtModel_ID = @selectModel_id AND detailTitleSort_ID = @selectTitle_id AND detail = @selectDetailStr";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@selectModel_id"] = selectModel_id;
            parameters["@selectTitle_id"] = selectTitle_id;

            //特殊符號要轉成字元實體才能正常存進資料庫
            parameters["@selectDetailStr"] = selectDetailStr;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            //渲染畫面
            RadioButtonListDetail.Items.Clear();
            loadDetailList();


        }




        // 細項細節 detail  end    -----------------------------------------------------------------------------------
    }
}