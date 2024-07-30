using NetVips;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Backend
{
    public partial class YachtManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList1.DataBind(); //先綁定，圖片才能取到型號
                loadImageList();
                showYachtModelList();
            }

        }


        //宣告 List 方便用 Add 依序添加圖檔資料
        private List<ImagePath> savePathList = new List<ImagePath>();


        //輪播圖 JSON 資料
        public class ImagePath
        {
            public string SavePath { get; set; }
        }



        // 顯示 Banner 的圖片
        private void loadImageList()
        {
            // 取得下拉選單選取值
            string selModel_id = DropDownList1.SelectedValue;

            //連線資料庫取得首頁輪播圖資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT bannerImgPathJSON FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = selModel_id;
            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                string loadJson = rd["bannerImgPathJSON"].ToString();

                //反序列化JSON格式
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);

            }


            db.CloseDB();


            if(savePathList?.Count > 0)
            {
                //預設第一張上傳的圖片為該遊艇型號首頁圖片
                bool firstCheck = true;

                foreach(var item in savePathList)
                {
                    if (firstCheck)
                    {
                        // 替首張加上醒目色彩框 => ListItem listItem = new ListItem(text, value);
                        ListItem listItem = new ListItem($"<img src='/Image/imageYachts/{item.SavePath}' alt='thumbnail' class='img-thumbnail bg-success' width='200px />'",item.SavePath);

                        RadioButtonList1.Items.Add(listItem);
                        firstCheck = false;
                    }
                    else
                    {
                        ListItem listItem = new ListItem($"<img src='/Image/imageYachts/{item.SavePath}' alt='thumbnail' class='img-thumbnail' width='200px />'", item.SavePath);
                        RadioButtonList1.Items.Add(listItem);
                    }


                }


            }

            DelImageBtn.Visible = false; //刪除鈕有選擇圖片時才顯示

        }



        // 上傳圖片
        protected void UploadBtn_Click(object sender, EventArgs e)
        {


            // 有選擇檔案才執行
            if (imageUpload.HasFile)
            {

                //取得上傳檔案大小 (限制 10MB)
                int fileSize = imageUpload.PostedFile.ContentLength; //Byte
                if (fileSize < 1024 * 1000 * 10)
                {
                    // 先讀取資料庫原有資料
                    loadImageList();
                    string savePath = Server.MapPath("~/Image/imageYachts/");

                    //添加圖檔資料
                    //逐一讀取選擇的圖片檔案
                    foreach (HttpPostedFile postedFile in imageUpload.PostedFiles)
                    {
                        //儲存圖片檔案及圖片名稱
                        //檢查專案資料夾內有無同名檔案，有同名就加流水號
                        DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
                        string fileName = postedFile.FileName;
                        string[] fileNameArr = fileName.Split('.');
                        int count = 0;
                        foreach (var fileItem in directoryInfo.GetFiles())
                        {
                            if (fileItem.Name.Contains(fileNameArr[0]))
                            {
                                count++;
                            }
                        }
                        fileName = fileNameArr[0] + $"({count + 1})." + fileNameArr[1];
                        //在圖片名稱前加入 temp 標示並儲存圖片檔案
                        //postedFile.SaveAs(savePath + "temp" + fileName);
                        postedFile.SaveAs(savePath + fileName);

                        //新增 JSON 資料  => 有 error
                        savePathList.Add(new ImagePath { SavePath = fileName });


                        // 不壓縮了 => 因為 File.Delete 會有問題
                        //使用 NetVips 套件進行壓縮圖檔
                        //判斷儲存的原始圖片寬度是否大於設定寬度的 2 倍
                        //var img = NetVips.Image.NewFromFile(savePath + "temp" + fileName);
                        //if (img.Width > 214 * 2)
                        //{
                        //    //產生原使圖片一半大小的新圖片
                        //    var newImg = img.Resize(0.5);
                        //    //如果新圖片寬度還是大於原始圖片設定寬度的 2 倍就持續縮減
                        //    while (newImg.Width > 214 * 2)
                        //    {
                        //        newImg = newImg.Resize(0.5);
                        //    }
                        //    //儲存正式名稱的新圖片
                        //    newImg.WriteToFile(savePath + fileName);
                        //}
                        //else
                        //{
                        //    postedFile.SaveAs(savePath + fileName);
                        //}


                        //刪除原始圖片  => 這個會有 bug > 會刪不了 => 就先不刪除了
                        //File.Delete(savePath + "temp" + fileName);


                    }


                    // 更新新增後的圖片名稱 JSON 存入資料庫
                    string fileNameJsonStr = JsonConvert.SerializeObject(savePathList);

                    // 取得下拉選單選取值
                    string selModel_id = DropDownList1.SelectedValue;


                    DBHelper db = new DBHelper();
                    string sqlCommand = $"UPDATE [Yachts] SET bannerImgPathJSON=@bannerImgPathJSON WHERE id = @id";

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters["@bannerImgPathJSON"] = fileNameJsonStr;
                    parameters["@id"] = selModel_id;

                    // 要清掉資料庫圖片的資料，就將下列移除
                    //parameters["bannerImgPathJSON"] = "[]";

                    db.UpdateDB(sqlCommand, parameters);

                    db.CloseDB();

                    // 渲染畫面
                    RadioButtonList1.Items.Clear();
                    loadImageList();

                }
                else
                {
                    Response.Write("<script>alert('*The maximum upload size is 10MB!');</script>");
                }




            }




        }


        // 當 有選擇圖片 => 要讓刪除按鈕顯示出來
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DelImageBtn.Visible = true;
        }

        // 刪除圖片
        protected void DelImageBtn_Click(object sender, EventArgs e)
        {
            //先讀取資料庫原有資料
            loadImageList();


            // 取得下拉選單選取值
            string selModel_id = DropDownList1.SelectedValue;

            //  取得選取項目的值
            string selImageStr = RadioButtonList1.SelectedValue;

            // 刪除圖片檔案
            string savePath = Server.MapPath("~/Image/imageYachts/");

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{savePath+selHImageStr}');window.location.href='CompanyManagement.aspx';", true);

            File.Delete(savePath + selImageStr);

            // 逐一比對原始資料 List <saveNameListH> 中的 檔案名稱
            for (int i = 0; i < savePathList.Count; i++)
            {
                // 與刪除的選項相同名稱
                if (savePathList[i].SavePath.Equals(selImageStr))
                {
                    // 移除 List 中同名的資料
                    savePathList.RemoveAt(i);
                }
            }

            // 更新刪除後的圖片名稱 JSON 存入資料庫
            DBHelper dB = new DBHelper();
            string saveNameJsonStr = JsonConvert.SerializeObject(savePathList);


            string sqlCommand = $"UPDATE [Yachts] SET bannerImgPathJSON=@bannerImgPathJSON WHERE id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["@bannerImgPathJSON"] = saveNameJsonStr;
            parameters["@id"] = selModel_id;


            dB.SearchDB(sqlCommand, parameters);

            dB.CloseDB();

            // 渲染畫面
            RadioButtonList1.Items.Clear();
            loadImageList();
        }


        // 新增 Yacht 型號
        protected void BtnAddYacht_Click(object sender, EventArgs e)
        {
            //插入空格區隔文字跟數字 (頁面細項標題會用到)
            string yachtModelStr = TBoxAddYachtModel.Text + " " +TBoxAddYachtLength.Text;


            //產生 GUID 隨機碼 + 時間2位秒數 (加強避免重複)
            DateTime nowTime = DateTime.Now;
            string nowSec = nowTime.ToString("ff");
            string guidStr = Guid.NewGuid().ToString().Trim() + nowSec;

            // 取得勾選項目
            string isNewDesign = CBoxNewDesign.Checked.ToString();
            string isNewBuilding =CBoxNewBuilding.Checked.ToString();

            // 插入遊艇基本型號 => 新增此遊艇型號
            DBHelper db = new DBHelper();
            string sqlCommand = $"INSERT INTO [Yachts] (yachtsModel,isNewDesign,isNewBuilding,guid) VALUES (@yachtsModel,@isNewDesign,@isNewBuilding,@guid)";

            Dictionary<string,object> parameters = new Dictionary<string,object>();

            parameters["yachtsModel"] = yachtModelStr;
            parameters["isNewDesign"] = isNewDesign;
            parameters["isNewBuilding"] = isNewBuilding;
            parameters["guid"] = guidStr;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            //畫面渲染

            // 更新下拉選項
            // 手動刷新 SqlDataSource：在代碼後台強制刷新 SqlDataSource 並重新綁定
            DropDownList1.DataBind();

            // 這邊因為還沒 post back  => drop list 沒有拿到新選項，所以選擇手動添加 => 後續更改看看



            // 更改 GridView 重新渲染 Call 的東西
            //GridView1.DataBind();
            showYachtModelList();

            TBoxAddYachtModel.Text = "";
            TBoxAddYachtLength.Text = "";
            CBoxNewDesign.Checked = false;
            CBoxNewBuilding.Checked = false;
            //DropDownList1.SelectedValue = yachtModelStr; //設定下拉選單選取項為新增項 => 還沒 postback 會沒有拿到更新後的 DropList => 所以先註解後續再進行更改
            RadioButtonList1.Items.Clear(); //新添加型號還沒有任何圖片，記得要清空畫面


        }




        // 顯示 Yacht Model List

        private void showYachtModelList()
        {
            // 插入遊艇基本型號 => 新增此遊艇型號
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [Yachts]";

            SqlDataReader rd = db.SearchDB(sqlCommand);


            GridView1.DataSource = rd;
            GridView1.DataBind();

            db.CloseDB();


        }


        // 編輯功能 => 將特定資料行轉換為編輯模式 => OnRowEditing
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex; // 將資料行轉換為:編輯模式
            showYachtModelList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }




        // 編輯功能 => 將特定 資料行 取消 編輯模式 => OnRowCancelingEdit
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1; // 將資料行取消編輯模式
            showYachtModelList(); // 記得呼叫讀取 GridView 的 function， 進行重新 Binding，不然會出錯
        }



        // 編輯功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的修改
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            // 1. 找到特定表格行數 (Row) => ex: 第五行
            int IndexRow = e.RowIndex;


            // 2. 取得該行數的表格行數物件 (GridViewRow) => ex: 第五行的物件
            GridViewRow TargetRow = GridView1.Rows[IndexRow];


            // 3. 於該物件內部找到要修改的欄位 (Column) 物件 => ex:物件中的 link 物件
            TextBox UpdateYachtsModelTextBox = TargetRow.FindControl("TextBoxYachtsModel") as TextBox;
            CheckBox UpdateIsNewDesignCBox = TargetRow.FindControl("CheckBoxIsNewDesignEdit") as CheckBox;
            CheckBox UpdateIsNewBuildingCBox = TargetRow.FindControl("CheckBoxIsNewBuildingEdit") as CheckBox;


            // 4. 找到該行數的 Key Value (ID) => ex: 第五行中的 ID 欄位值
            string IDkey = GridView1.DataKeys[IndexRow].Value.ToString();


            // 5. 透過 SQL 語法進行資料的修改 (開始撰寫 DBHelper 的四個流程)
            DBHelper db = new DBHelper();
            string sqlCommand = "UPDATE [Yachts] SET yachtsModel=@yachtsModel,isNewDesign=@isNewDesign,isNewBuilding=@isNewBuilding WHERE id = @id";

            // 使用參數化查詢來避免 SQL 注入攻擊
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@yachtsModel"] = UpdateYachtsModelTextBox.Text;
            parameters["@isNewDesign"] = UpdateIsNewDesignCBox.Checked;
            parameters["@isNewBuilding"] = UpdateIsNewBuildingCBox.Checked;
            parameters["@id"] = IDkey;

            db.UpdateDB(sqlCommand, parameters);


            // 6.補充:不要忘記重新 把編輯模式 改回 閱讀模式 以及執行 showCategoryGV()
            db.CloseDB();
            GridView1.EditIndex = -1;
            showYachtModelList();  //當然也可以Redirect，就不用showGV了

        }



        // 6. 加入遊艇型號列表的 Delete 按鈕 OnRowDeleting 事件程式碼如下
        // 刪除功能 => 目的：將特定 資料行，透過SQL語法進行資料庫的刪除 => OnRowDeleting
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            // 1. 找到特定表格行數 (Row) ⇒ ex：第五行
            int IndexRow = e.RowIndex;

            // 2. 找到該行數的 Key Value (ID) ⇒ ex：第五行中的 ID 欄位值
            string IDkey = GridView1.DataKeys[IndexRow].Value.ToString();


            //取出刪除的遊艇型號的組圖資料
            DBHelper db2 = new DBHelper();
            string sqlCommand2 = $"SELECT bannerImgPathJSON FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2["@id"] = IDkey;
            SqlDataReader rd2 = db2.SearchDB(sqlCommand2,parameters2);

            if (rd2.Read())
            {
                string loadJson = rd2["bannerImgPathJSON"].ToString();

                //反序列化JSON格式
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);

            }


            db2.CloseDB();

            // 圖檔路徑
            string savePath = Server.MapPath("~/Image/imageYachts/");

            //刪除組圖實際圖檔
            for (int i = 0; i < savePathList.Count; i++)
            {
                File.Delete(savePath + savePathList[i].SavePath);
            }



            //取出刪除的遊艇型號的 Layout 組圖資料
            DBHelper db3 = new DBHelper();
            string sqlCommand3 = $"SELECT layoutDeckPlanImgPathJSON FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters3 = new Dictionary<string, object>();
            parameters3["@id"] = IDkey;

            SqlDataReader rd3 = db3.SearchDB(sqlCommand3,parameters3);

            if (rd3.Read())
            {
                string loadJson = HttpUtility.HtmlDecode(rd3["layoutDeckPlanImgPathJSON"].ToString());
                //反序列化JSON格式
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);
            }

            db3.CloseDB();

            //刪除組圖實際圖檔
            for (int i = 0; i < savePathList.Count; i++)
            {
                File.Delete(savePath + savePathList[i].SavePath);
            }



            //取出刪除的遊艇型號的 overview 規格圖片資料 => 後續添加
            DBHelper db4 = new DBHelper();
            string sqlCommand4 = $"SELECT overviewDimensionsImgPath FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters4 = new Dictionary<string, object>();
            parameters4["@id"] = IDkey;

            SqlDataReader rd4 = db4.SearchDB(sqlCommand4, parameters4);

            if (rd4.Read())
            {
                string imgPath = rd4["overviewDimensionsImgPath"].ToString();
                //刪除實際圖檔
                if (!String.IsNullOrWhiteSpace(imgPath))
                {
                    File.Delete(savePath + imgPath);
                }
            }

            db4.CloseDB();




            //取出刪除的遊艇型號的 overview 的 PDF 檔案資料  => 後續添加
            DBHelper db5 = new DBHelper();
            string sqlCommand5 = $"SELECT overviewDownloadsFilePath FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters5 = new Dictionary<string, object>();
            parameters5["@id"] = IDkey;

            SqlDataReader rd5 = db5.SearchDB(sqlCommand5, parameters5);

            if (rd5.Read())
            {
                string imgPath = rd5["overviewDownloadsFilePath"].ToString();
                //刪除實際圖檔
                if (!String.IsNullOrWhiteSpace(imgPath))
                {
                    File.Delete(savePath + imgPath);
                }
            }

            db5.CloseDB();
              


            // 3. 透過SQL語法進行資料的修改
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [Yachts] WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@id"] = IDkey;
            db.UpdateDB(sqlCommand, parameters);


            // 4. 補充：不要忘記重新執行 showGV()
            db.CloseDB();
            showYachtModelList();   //當然也可以Redirect，就不用showGV了


            // 下拉選單有需要再次更新 => 不然下拉選單會有刪除的型號  
            // DropDownList 需要更新


        }

        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            RadioButtonList1.Items.Clear(); //清空圖片選項
            DropDownList1.DataBind(); //刷新下拉選單
            loadImageList(); //取得圖片選項
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            DropDownList1.DataBind(); //刷新下拉選單
        }



        // 更改下拉選單，選擇的 Yachts 型號
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList1.Items.Clear(); //清空圖片選項
            loadImageList(); //取得圖片選項
        }

    }
}