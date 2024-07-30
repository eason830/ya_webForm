using CKFinder;
using NetVips;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Backend
{
    public partial class OverviewManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ckfinderSetPath();  // ck editor init setting

                DListModel.DataBind(); //先綁定取得選取值
                loadContent(); //取得尺寸欄位區外的主要內容
                loadRowList(); //取得尺寸欄位內容
                renderRowList(); //渲染尺寸欄位內容
            }
        }


        //欄位表 JSON 資料
        public class RowData
        {
            public string SaveItem { get; set; }
            public string SaveValue { get; set; }
        }

        //宣告 List 方便用 Add 依序添加欄位資料
        private List<RowData> saveRowList = new List<RowData>();

        // ck editor setting start --------------------------------------------------------------------------

        private void ckfinderSetPath()
        {
            FileBrowser fileBrowser = new FileBrowser();
            fileBrowser.BasePath = "/Backend/ckfinder";
            fileBrowser.SetupCKEditor(CKEditorControl1);

            // 網頁後置程式碼 .aspx.cs 取得輸入內容編譯方法如下
            // Literal.Text = HttpUtility.HtmlEncode(CKEditorControl1.Text);

        }

        // ck editor setting end ----------------------------------------------------------------------------



        //建立取得尺寸欄位外的主要內容方法 loadContent();
        private void loadContent()
        {
            //清空畫面資料
            TBoxVideo.Text = "";
            TBoxDLTitle.Text = "";
            TBoxDimImg.Text = "";
            TBoxDLFile.Text = "";
            PDFpreview.Text = "";
            LiteralDimImg.Text = "";

            //依下拉選單選取型號取出資料
            string selectModel_id = DListModel.SelectedValue;

            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT * FROM [Yachts] WHERE id = @selectModel_id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@selectModel_id"] = selectModel_id;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);


            while (rd.Read()) {
                //渲染畫面
                CKEditorControl1.Text = HttpUtility.HtmlDecode(rd["overviewContentHtml"].ToString());

                string imgPathStr = rd["overviewDimensionsImgPath"].ToString();
                string filePathStr = rd["overviewDownloadsFilePath"].ToString();

                // 顯示路徑文字
                TBoxDimImg.Text = imgPathStr;
                // 顯示圖片
                LiteralDimImg.Text = $"<img alt='Dimensions Image' class='img-thumbnail rounded mx-auto d-block' src='/Image/imageYachts/{imgPathStr}' Width='250px'/>";

                // 顯示路徑文字
                TBoxDLFile.Text = filePathStr;
                
                // 顯示檔案
                // PDF 預覽用 <object> 標籤放入畫面。
                PDFpreview.Text = $"<object type='application/pdf' data='/Image/imageYachts/{filePathStr}' width='250px' height='385px' class='rounded mx-auto d-block' ></object>";

            }

            db.CloseDB();


        }


        //建立取得尺寸欄位內容方法 loadRowList(); 方法邏輯內容如下
        private void loadRowList()
        {
            //依選取型號取得欄位資料更新 List<T>
            string selectModel_id = DListModel.SelectedValue;


            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT * FROM [Yachts] WHERE id = @selectModel_id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@selectModel_id"] = selectModel_id;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                string loadJson = HttpUtility.HtmlDecode(rd["overviewDimensionsJSON"].ToString());
                saveRowList = JsonConvert.DeserializeObject<List<RowData>>(loadJson);
            }

            db.CloseDB();


            if (saveRowList?.Count == 0)
            {
                //增加新欄位
                saveRowList.Add(new RowData { SaveItem = "Video URL", SaveValue = "" });
                saveRowList.Add(new RowData { SaveItem = "Downloads Title", SaveValue = "" });
            }


        }


        //  建立渲染尺寸欄位畫面方法 renderRowList(); 方法邏輯內容如下

        private void renderRowList()
        {
            // 確認有儲存 item 跟 Value
            if (saveRowList?.Count > 0)
            {
                string addRowHtmlStr = "";
                int index = 0;

                //從 List<T> 載入資料 => JSON 資料前兩筆是 Video 連結及 PDF 顯示文字。
                foreach (var item in saveRowList)
                {
                    if (index == 0)
                    {
                        TBoxVideo.Text = item.SaveValue;
                    }
                    if (index == 1)
                    {
                        TBoxDLTitle.Text = item.SaveValue;
                    }
                    if (index > 1)
                    {
                        addRowHtmlStr += $"<tr class='table-info'>" +
                                            $"<th><input id='item{index}' name='item{index}' type='text' class='form-control font-weight-bold' value='{item.SaveItem}' /></th>" +
                                            $"<td><input id='value{index}' name='value{index}' type='text' class='form-control' value='{item.SaveValue}' /></td>" +
                                            $"</tr>";
                    }
                    index++;
                }
                //渲染畫面
                LitDimensionsHtml.Text = addRowHtmlStr;
            }
        }


        // 遊艇型號下拉選單選項改變時觸發內容
        protected void DListModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //隱藏上傳成功提示
            UploadOverviewContentLab.Visible = false;
            LabUpdateDimensionsList.Visible = false;
            //渲染畫面
            loadContent();
            loadRowList();
            renderRowList();
        }



        // 建立尺寸附圖上傳 Upload 按鈕 OnClick 功能如下
        protected void BtnUploadDimImg_Click(object sender, EventArgs e)
        {

            string savePath = Server.MapPath("~/Image/imageYachts/");
            string fileName = DimImgUpload.FileName;
            string selectModel_id = DListModel.SelectedValue;


            //有選檔案才可上傳，沒選檔案則執行清空
            if (DimImgUpload.HasFile)
            {
                DBHelper db = new DBHelper();

                string sqlCommand = $"SELECT overviewDimensionsImgPath FROM [Yachts] WHERE id = @selectModel_id";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@selectModel_id"] = selectModel_id;


                //刪除舊圖檔
                SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

                if (rd.Read())
                {
                    string delFileName = rd["overviewDimensionsImgPath"].ToString();
                    if (!String.IsNullOrEmpty(delFileName))
                    {
                        File.Delete(savePath + delFileName);
                    }
                }

                db.CloseDB();


                //儲存圖片檔案及圖片名稱
                //檢查專案資料夾內有無同名檔案，有同名就加流水號
                DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
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
                DimImgUpload.SaveAs(savePath + fileName);


                //更新資料
                DBHelper db2 = new DBHelper();

                string sqlCommand2 = $"UPDATE [Yachts] SET overviewDimensionsImgPath = @fileName WHERE id = @selectModel_id";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>();
                parameters2["@selectModel_id"] = selectModel_id;
                parameters2["@fileName"] = fileName;

                db2.UpdateDB(sqlCommand2, parameters2);

                db2.CloseDB();

                //渲染畫面
                loadContent();
                loadRowList();
                renderRowList();


            }
            else
            {
                //沒選檔案點上傳則清空
                DBHelper db3 = new DBHelper();

                string sqlCommand3 = $"UPDATE [Yachts] SET overviewDimensionsImgPath = @imgPath WHERE id = @selectModel_id";
                Dictionary<string, object> parameters3 = new Dictionary<string, object>();
                parameters3["@selectModel_id"] = selectModel_id;
                parameters3["@imgPath"] = "";

                db3.UpdateDB(sqlCommand3, parameters3);

                db3.CloseDB();


                //渲染畫面
                loadContent();
                loadRowList();
                renderRowList();
            }

        }






        //建立 PDF 檔上傳 Upload 按鈕 OnClick 功能如下
        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            string savePath = Server.MapPath("~/Image/imageYachts/");
            string fileName = FileUpload1.FileName;
            string selectModel_id = DListModel.SelectedValue;


            //有選檔案才可上傳，沒選檔案則執行清空
            if (FileUpload1.HasFile)
            {
                DBHelper db = new DBHelper();

                string sqlCommand = $"SELECT overviewDownloadsFilePath FROM [Yachts] WHERE id = @selectModel_id";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@selectModel_id"] = selectModel_id;


                //刪除舊圖檔
                SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

                if (rd.Read())
                {
                    string delFileName = rd["overviewDownloadsFilePath"].ToString();
                    if (!String.IsNullOrEmpty(delFileName))
                    {
                        File.Delete(savePath + delFileName);
                    }
                }

                db.CloseDB();


                //儲存圖片檔案及圖片名稱
                //檢查專案資料夾內有無同名檔案，有同名就加流水號
                DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
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
                FileUpload1.SaveAs(savePath + fileName);


                //更新資料
                DBHelper db2 = new DBHelper();

                string sqlCommand2 = $"UPDATE [Yachts] SET overviewDownloadsFilePath  = @fileName WHERE id = @selectModel_id";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>();
                parameters2["@selectModel_id"] = selectModel_id;
                parameters2["@fileName"] = fileName;

                db2.UpdateDB(sqlCommand2, parameters2);

                db2.CloseDB();

                //渲染畫面
                loadContent();
                loadRowList();
                renderRowList();


            }
            else
            {
                //沒選檔案點上傳則清空
                DBHelper db3 = new DBHelper();

                string sqlCommand3 = $"UPDATE [Yachts] SET overviewDownloadsFilePath  = @imgPath WHERE id = @selectModel_id";
                Dictionary<string, object> parameters3 = new Dictionary<string, object>();
                parameters3["@selectModel_id"] = selectModel_id;
                parameters3["@imgPath"] = "";

                db3.UpdateDB(sqlCommand3, parameters3);

                db3.CloseDB();


                //渲染畫面
                loadContent();
                loadRowList();
                renderRowList();
            }





        }




        // 動態新增欄位按鈕功能利用增加空白 List 資料，配合前面的讀取及渲染方法達成
        protected void AddRow_Click(object sender, EventArgs e)
        {
            //將 JSON 資料載入 List<T>
            loadRowList();






            saveRowList.Add(new RowData { SaveItem= "", SaveValue=""});

            // 更新資料庫資料
            uploadRowList();

            // 渲染畫面
            renderRowList();

            // 將畫面移至出現上傳按鈕處
            Page.SetFocus(BtnUpdateDimensionsList);


        }




        // 新增上方出現的更新尺寸欄位資料至資料庫的 uploadRowList(); 方法
        private void uploadRowList()
        {
            // 先更新 List<T> 前兩個資料 Video 及 Download File
            saveRowList[0].SaveValue = TBoxVideo.Text;
            saveRowList[1].SaveValue = TBoxDLTitle.Text;

            // 更新 List<T> ，欄位內容用 Request.Form
            for (int i = 2; i < saveRowList.Count; i++)
            {
                saveRowList[i].SaveItem = Request.Form[$"item{i}"];
                saveRowList[i].SaveValue = Request.Form[$"value{i}"];
            }

            // 依選取型號更新資料庫資料
            string selectModel_id =DListModel.SelectedValue;

            // 將 List<T> 資料轉為 JSON 格式字串
            string saveRowListJsonStr = JsonConvert.SerializeObject(saveRowList);

            // 更新資料庫
            DBHelper db = new DBHelper();

            string sqlCommand = $"UPDATE [Yachts] SET overviewDimensionsJSON = @overviewDimensionsJSON WHERE id = @id";

            Dictionary<string,object> parameters = new Dictionary<string, object>();
            parameters["@overviewDimensionsJSON"] = saveRowListJsonStr;
            parameters["@id"] = selectModel_id;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();


        }





        // 動態刪除欄位按鈕功能利用移除最末筆 List 資料，配合前面的讀取及渲染方法達成
        protected void DeleteRow_Click(object sender, EventArgs e)
        {
            // 將 JSON 資料載入 List <T>
            loadRowList();

            // 更新資料庫資料
            uploadRowList();

            // 刪除尺寸欄位最末欄 => 根據索引值刪除

            // saveRowList.Count 如果大於 2 才可以刪

            saveRowList.RemoveAt(saveRowList.Count - 1);

            // 更新資料庫資料
            uploadRowList();


            // 渲染表格畫面
            renderRowList();

            // 將畫面移至出現上傳按鈕處
            Page.SetFocus(BtnUpdateDimensionsList);


        }



        //建立更新尺寸欄位 Update Dimensions List 按鈕的 OnClick 功能
        protected void BtnUpdateDimensionsList_Click(object sender, EventArgs e)
        {
            // 將 JSON 資料載入 List<T>
            loadRowList();

            // 更新資料庫資料
            uploadRowList();


            // 渲染表格畫面
            renderRowList();

            // 渲染上傳成功提示
            DateTime nowtime = DateTime.Now;
            LabUpdateDimensionsList.Visible = true;
            LabUpdateDimensionsList.Text = "*Upload Success! - " + nowtime.ToString("G");


        }




        // 建立圖文編輯區上傳 Upload Overview Content 按鈕 OnClick 功能
        protected void UploadOverviewContentBtn_Click(object sender, EventArgs e)
        {
            // 將文字編輯器 HTML 內容轉為 HTML 字元編碼
            string overviewContentHtmlStr = HttpUtility.HtmlEncode(CKEditorControl1.Text);

            // 依下拉選單選取型號存入型號介紹主要圖文內容
            string selectModel_id = DListModel.SelectedValue;

            // 更新到資料庫
            DBHelper db = new DBHelper();

            string sqlCommand = $"UPDATE [Yachts] SET overviewContentHtml=@overviewContentHtml WHERE id =@id";

            Dictionary<string,object> parameters = new Dictionary<string,object>();
            parameters["@overviewContentHtml"] = overviewContentHtmlStr;
            parameters["@id"] = selectModel_id;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            // 渲染上傳成功顯示
            DateTime nowtime = DateTime.Now;
            UploadOverviewContentLab.Visible = true;
            UploadOverviewContentLab.Text = "*Upload Success! - " + nowtime.ToString("G");


        }














    }
}