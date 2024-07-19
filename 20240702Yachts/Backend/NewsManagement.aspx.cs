using CKFinder;
using NetVips;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static _20240702Yachts.Backend.CompanyManagement;

namespace _20240702Yachts.Backend
{
    public partial class NewsManagement : System.Web.UI.Page
    {

        // JSON 資料 Image
        public class ImagePath
        {
            public string SaveName { get; set; }
        }


        //宣告 List 方便用 Add 依序添加資料
        private List<ImagePath> savePathList = new List<ImagePath>();



        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ckfinderSetPath();  // ck editor init setting
                Calendar1.SelectedDate = Calendar1.TodaysDate; // 預設選取當日日期
                loadDayNewsHeadLine();


                //如果選取當日有新聞
                if(headlineRadioBtnList.Items.Count > 0)
                {
                    PanelCoverContent.Visible = true; // 顯示右上區塊
                    PanelMainContent.Visible = true; // 顯示左下區塊
                    PanelGroupImage.Visible = true; // 顯示右下區塊

                    loadThumbnail();  // 讀取新聞列表縮圖
                    loadSummary(); // 讀取新聞簡介
                    loadNewsContent(); // 讀取-新聞上方主要內容
                    loadImageList(); // 讀取新聞下方組圖

                }


            }
        }





        // 新增新聞
        protected void AddHeadlineBtn_Click(object sender, EventArgs e)
        {
            // 產生 GUID 隨機碼 + 時間2位秒數 (加強避免重複)
            DateTime nowTime = DateTime.Now;

            string nowSec = nowTime.ToString("FF");
            string guid = Guid.NewGuid().ToString().Trim() + nowSec;

            // 取得日歷選取日期
            string selNewsDate =Calendar1.SelectedDate.ToString("yyyy-M-dd");

            // 取得是否勾選
            string isTop = CBoxIsTop.Checked.ToString(); // 得到 True 或是 False

            // 將資料存入資料庫
            DBHelper dB = new DBHelper();
            string sqlCommand = $"INSERT INTO [News] (dateTitle,headline,guid,isTop) Values(@dateTitle, @headline, @guid, @isTop)";
            Dictionary<string,object> parameters = new Dictionary<string, object>();

            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] =headlineTbox.Text;
            parameters["guid"] = guid;
            parameters["isTop"] = isTop; // 存入資料庫會轉為 0 或 1

            dB.UpdateDB(sqlCommand,parameters);

            dB.CloseDB();


            // 渲染畫面

            // 當添加新的新聞標題，將目前的設定值還原到預設狀態
            deleteNewsBtn.Visible = false; //隱藏:日曆區刪除新聞標題按鈕
            DelImageBtn.Visible = false; //隱藏:新聞下方組圖刪除圖片按鈕
            CBoxIsTop.Checked = false; //將焦點新聞選項設為不勾選
            headlineRadioBtnList.Items.Clear(); //清空日曆區的新聞標題選項
            RadioButtonListU.Items.Clear(); //清空新聞下方組圖的圖片選項
            loadDayNewsHeadLine(); //讀取新聞標題

            //如果選取當日有新聞，顯示其它3個區塊並載入內容
            if (headlineRadioBtnList.Items.Count > 0)
            {
                PanelCoverContent.Visible = true; // 顯示右上區塊
                PanelMainContent.Visible = true; // 顯示左下區塊
                PanelGroupImage.Visible = true; // 顯示右下區塊
                loadThumbnail();
                loadSummary();
                loadNewsContent();
                loadImageList();
            }
            else
            {
                //選取當日無新聞，隱藏其它3個區塊，只留日曆區
                PanelCoverContent.Visible = false; // 隱藏右上區塊
                PanelMainContent.Visible = false; // 隱藏左下區塊
                PanelGroupImage.Visible = false; // 隱藏右下區塊
            }


            // 清空輸入欄位
            headlineTbox.Text = "";

        }


        // 載入選擇日期的 News
        private void loadDayNewsHeadLine()
        {
            //依選取日期取得資料庫新聞內容
            DBHelper db =  new DBHelper();
            string sqlCommand = $"SELECT * FROM [News] WHERE dateTitle=@dateTitle ORDER BY id ASC";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = Calendar1.SelectedDate.ToString("yyyy-M-dd");
            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);


            while (rd.Read())
            {
                string headlineStr = rd["headline"].ToString();
                string isTopStr = rd["isTop"].ToString();

                // 渲染畫面
                LabIsTop.Visible = false;

                if (isTopStr.Equals("True"))
                {
                    LabIsTop.Visible=true;
                }

                ListItem listItem = new ListItem();
                listItem.Text = headlineStr;
                listItem.Value = headlineStr;
                headlineRadioBtnList.Items.Add(listItem);

            }

            db.CloseDB();


            // 預設選取新增新聞項目
            int RadioBtnCount = headlineRadioBtnList.Items.Count;
            if (RadioBtnCount > 0) {
                headlineRadioBtnList.Items[RadioBtnCount - 1].Selected = true;
                deleteNewsBtn.Visible = true;
            }


        }




        // 刪除新聞 Delete News 按鈕的 OnClick 事件功能 
        // => 後續製作需加入刪除列表頁縮圖圖檔及刪除新聞內容組圖等功能。
        protected void deleteNewsBtn_Click(object sender, EventArgs e)
        {
            // 圖片檔案資料夾 => 只需要到 image 層就好
            string savePath = Server.MapPath("~/");
            string savePath2 = Server.MapPath("~/Image/imageNews/");

            // 隱藏刪除按鈕
            deleteNewsBtn.Visible = false;

            //取得選取項目內容
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;

            //取得日曆選取日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");


            // 刪除資料庫前，先 => 刪除圖檔(縮圖) thumbnail
            DBHelper db2 = new DBHelper();
            string sqlCommand2 = $"SELECT thumbnailPath FROM [News] WHERE dateTitle = @dateTitle AND headline = @headline";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2["dateTitle"] = selNewsDate;
            parameters2["headline"] = selHeadlineStr;
            SqlDataReader rd2= db2.SearchDB(sqlCommand2, parameters2);

            if (rd2.Read())
            {
                string thumbnailPath = rd2["thumbnailPath"].ToString();
                if (!String.IsNullOrEmpty(thumbnailPath))
                {
                    File.Delete(savePath + thumbnailPath);
                }
            }

            db2.CloseDB();



            // 刪除資料庫前，先 => 刪除圖檔(組圖) group image
            DBHelper db3 = new DBHelper();
            string sqlCommand3 = $"SELECT newsImageJson FROM [News] WHERE dateTitle = @dateTitle AND headline = @headline";
            Dictionary<string, object> parameters3 = new Dictionary<string, object>();
            parameters3["dateTitle"] = selNewsDate;
            parameters3["headline"] = selHeadlineStr;
            SqlDataReader rd3 = db3.SearchDB(sqlCommand3, parameters3);

            if (rd3.Read())
            {

               
                string loadJson = HttpUtility.HtmlDecode(rd3["newsImageJson"].ToString());
                //反序列化JSON格式
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);
            }

            db3.CloseDB();

            if (savePathList?.Count > 0)
            {
                foreach (var item in savePathList)
                {
                    File.Delete(savePath2 + item.SaveName);
                }
            }


            //連線資料庫 => 刪除資料庫該筆資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [News] WHERE dateTitle = @dateTitle and headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;
            db.SearchDB(sqlCommand, parameters);

            db.CloseDB();


            //渲染畫面
            // 當添加新的新聞標題，將目前的設定值還原到預設狀態
            deleteNewsBtn.Visible = false; //隱藏:日曆區刪除新聞標題按鈕
            DelImageBtn.Visible = false; //隱藏:新聞下方組圖刪除圖片按鈕
            CBoxIsTop.Checked = false; //將焦點新聞選項設為不勾選
            headlineRadioBtnList.Items.Clear(); //清空日曆區的新聞標題選項
            RadioButtonListU.Items.Clear(); //清空新聞下方組圖的圖片選項
            loadDayNewsHeadLine(); //讀取新聞標題

            //如果選取當日有新聞，顯示其它3個區塊並載入內容
            if (headlineRadioBtnList.Items.Count > 0)
            {
                PanelCoverContent.Visible = true; // 顯示右上區塊
                PanelMainContent.Visible = true; // 顯示左下區塊
                PanelGroupImage.Visible = true; // 顯示右下區塊
                loadThumbnail();
                loadSummary();
                loadNewsContent();
                loadImageList();
            }
            else
            {
                //選取當日無新聞，隱藏其它3個區塊，只留日曆區
                PanelCoverContent.Visible = false; // 隱藏右上區塊
                PanelMainContent.Visible = false; // 隱藏左下區塊
                PanelGroupImage.Visible = false; // 隱藏右下區塊
            }


        }



        // 建立新聞標題選取項目按鈕選取改變時的 OnSelectedIndexChanged 事件功能
        protected void headlineRadioBtnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 依日期的新聞標題選取項目判斷是不是焦點新聞
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT isTop FROM [News] WHERE dateTitle=@dateTitle AND headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = Calendar1.SelectedDate.ToString("yyyy-M-dd");
            parameters["headline"] = headlineRadioBtnList.SelectedValue;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                string isTopStr = rd["isTop"].ToString();
                //渲染畫面
                LabIsTop.Visible = false;
                if (isTopStr.Equals("True"))
                {
                    LabIsTop.Visible = true;
                }
            }

            db.CloseDB();


            //底部附加渲染畫面
            RadioButtonListU.Items.Clear(); //清除新聞下方組圖圖片選項
            loadThumbnail();
            loadSummary();
            loadNewsContent();
            loadImageList();


        }



        //建立日曆日期選取改變時的 OnSelectionChanged 事件功能
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            //渲染畫面
            // 當添加新的新聞標題，將目前的設定值還原到預設狀態
            deleteNewsBtn.Visible = false; //隱藏:日曆區刪除新聞標題按鈕
            DelImageBtn.Visible = false; //隱藏:新聞下方組圖刪除圖片按鈕
            LabIsTop.Visible = false; // 是不是 top news 還原 false
            headlineRadioBtnList.Items.Clear(); //清空日曆區的新聞標題選項
            RadioButtonListU.Items.Clear(); //清空新聞下方組圖的圖片選項

            //CBoxIsTop.Checked = false; //將焦點新聞選項設為不勾選  => 這個就不還原勾選了

            loadDayNewsHeadLine(); //讀取新聞標題

            //如果選取當日有新聞，顯示其它3個區塊並載入內容
            if (headlineRadioBtnList.Items.Count > 0)
            {
                PanelCoverContent.Visible = true; // 顯示右上區塊
                PanelMainContent.Visible = true; // 顯示左下區塊
                PanelGroupImage.Visible = true; // 顯示右下區塊
                loadThumbnail();
                loadSummary();
                loadNewsContent();
                loadImageList();
            }
            else
            {
                //選取當日無新聞，隱藏其它3個區塊，只留日曆區
                PanelCoverContent.Visible = false; // 隱藏右上區塊
                PanelMainContent.Visible = false; // 隱藏左下區塊
                PanelGroupImage.Visible = false; // 隱藏右下區塊
            }

        }




        // 建立日曆日期渲染畫面時的 OnDayRender 事件功能
        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT dateTitle FROM [News]";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            while (rd.Read())
            {
                //轉換為 DateTime 型別
                DateTime newsTime = DateTime.Parse(rd["dateTitle"].ToString());

                //有新聞的日期 且 此日期不是選中的日期時 就修改日期外觀
                if (e.Day.Date.Date == newsTime && e.Day.Date.Date != Calendar1.SelectedDate)
                {
                    //渲染畫面
                    //e.Cell.BorderWidth = Unit.Pixel(1); //外框線粗細
                    //e.Cell.BorderColor = Color.BlueViolet; //外框線顏色
                    e.Cell.Font.Underline = true; //有無下地線
                    e.Cell.Font.Bold = true; //是否為粗體
                    e.Cell.ForeColor = Color.DodgerBlue; //外觀色彩
                }

            }

            db.CloseDB();


        }





        // loadThumbnail();  start -----------------------------------------------------------------
        // 載入 此 news 的縮圖

        private void loadThumbnail()
        {


            //取得選取項目內容
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;

            //取得日曆選取日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");



            // 連接資料庫，拿到此 news 的縮圖，  顯示在 img 控制項上面
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT thumbnailPath FROM [News] WHERE dateTitle = @dateTitle and headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;
            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                ImageThumbnail.ImageUrl = rd["thumbnailPath"].ToString();
                ImageThumbnail.Visible = true;


                // 從沒有上傳圖片，就直接隱藏
                if (rd["thumbnailPath"].ToString() == "null" || rd["thumbnailPath"].ToString() == "")
                {
                    ImageThumbnail.Visible = false;

                }


            }



            db.CloseDB();


        }




        // 新增 news 縮圖
        protected void ButtonThumbnail_Click(object sender, EventArgs e)
        {

            // 先上傳檔案成功，取得圖片路徑，再去新增到資料庫

            // 1.取得 Server 資料夾的路徑 (記得要先去建立資料夾)
            string ServerFolderPath = Server.MapPath("~/Image/imageNews");

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;

                // 在使用之前打開連接
                connection.Open();

                // 2.確認 FileUpload 控制項，是否有檔案
                if (FileUploadThumbnail.HasFiles) // Note:控制項目可以開啟 "AllowMultiple" 功能
                {
                    // 3.將 FileUpload 控制項裡面的檔案跑迴圈
                    foreach (var postfile in FileUploadThumbnail.PostedFiles)
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

                            //取得選取項目內容
                            string selHeadlineStr = headlineRadioBtnList.SelectedValue;

                            //取得日曆選取日期
                            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");


                            // 5. 進行資料庫寫入
                            string pathStore = "/Image/imageNews/" + FileName;

                            string sql = $"UPDATE [News] SET thumbnailPath=@thumbnailPath WHERE dateTitle = @dateTitle and headline = @headline";


                            sqlCommand.CommandText = sql;
                            sqlCommand.Parameters.Clear(); // 確保每次執行前清除先前的參數
                            sqlCommand.Parameters.AddWithValue("@thumbnailPath", pathStore);
                            sqlCommand.Parameters.AddWithValue("@dateTitle", selNewsDate);
                            sqlCommand.Parameters.AddWithValue("@headline", selHeadlineStr);

                            int result = sqlCommand.ExecuteNonQuery();

                            if (result == 1)
                            {
                                postfile.SaveAs(FilePath); // 成功資料庫寫入，將檔案存入指定資料夾路徑
                                Response.Write($"<script>alert('已新增 thumbnail 成功')</script>");

                                loadThumbnail();

                            }


                        }




                    }

                }



            }


        }

        // loadThumbnail();  end   -----------------------------------------------------------------

        //     Summary start -----------------------------------------------------------------------

        // loadSummary
        private void loadSummary()
        {
            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            //取得新聞簡介內容
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT summary FROM [News] WHERE dateTitle = @dateTitle AND headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;
            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                summaryTbox.Text = rd["summary"].ToString();
            }
            else
            {
                summaryTbox.Text = "";
            }



            db.CloseDB();

            // 渲染畫面
            LabUploadSummary.Visible = false;



        }




        // uploadSummary
        protected void ButtonSummary_Click(object sender, EventArgs e)
        {
            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            DBHelper db = new DBHelper();
            string sqlCommand = $"UPDATE [News] SET summary=@summary  WHERE dateTitle = @dateTitle AND headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["summary"] = summaryTbox.Text;
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;
            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();

            // 渲染畫面
            DateTime nowtime = DateTime.Now;
            LabUploadSummary.Visible = true;
            LabUploadSummary.Text = "*Upload Success! - " + nowtime.ToString("G");

        }



        //     Summary end   -----------------------------------------------------------------------



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



        // Main Content  start ------------------------------------------------------------------------------

        // 載入資料庫顯示 Ck editor 的內容
        private void loadNewsContent()
        {

            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");


            //  取得 About Us 頁面的 HTML資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT newsContentHtml FROM [News] WHERE dateTitle = @dateTitle AND headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;


            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                //渲染畫面
                CKEditorControl1.Text = HttpUtility.HtmlDecode(rd["newsContentHtml"].ToString());
            }

            db.CloseDB();

        }




        // 更新 About Us 的內容
        protected void UploadMainContentBtn_Click(object sender, EventArgs e)
        {

            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            
            // 取得 CKEditor 的 HTML 的內容 => 使用 .HtmlEncode() 來避免錯誤或惡意使用。
            string mainContentHtmlStr = HttpUtility.HtmlEncode(CKEditorControl1.Text);

            // 更新 About Us 頁面 HTML 資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"UPDATE [News] SET newsContentHtml = @newsContentHtml WHERE dateTitle = @dateTitle AND headline = @headline";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["newsContentHtml"] = mainContentHtmlStr;
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();


            // 渲染畫面提示
            DateTime nowtime = DateTime.Now;
            UploadMainContentLab.Visible = true;
            UploadMainContentLab.Text = $"Upload Success! - " + nowtime.ToString("G");


        }

        // Main Content  end   ------------------------------------------------------------------------------


        // Group Image start ---------------------------------------------------------------------------------

        //loadImageList
        // 建立 loadImageList() 
        private void loadImageList()
        {


            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            // 連線資料庫取出資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT newsImageJson FROM [News] WHERE dateTitle = @dateTitle AND headline = @headline";


            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;



            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            if (rd.Read())
            {
                string loadJson = rd["newsImageJson"].ToString();

                // 反序列化 Json 格式 => 將一個 JSON 格式的字串 loadJson，轉換為一個 List<ImagePath> 的物件列表
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);
            }


            db.CloseDB();


            //可以改成用 ?.Count 來判斷不是 Null 後才執行 .Count 避免錯誤
            if (savePathList?.Count > 0)
            {
                // 逐一取出 JSON 的每筆資料
                foreach (var item in savePathList)
                {

                    // 使用 html 要轉義
                    // 將 RadioButtonList 選項內容改為圖片格式，值設為檔案名稱
                    ListItem listItem = new ListItem($"<img src='/Image/imageNews/{item.SaveName}' alt='thumbnail' class='img-thumbnail' width='230px' />", item.SaveName);

                    // 加入圖片選項
                    RadioButtonListU.Items.Add(listItem);

                }
            }


            DelImageBtn.Visible = false; // 刪除紐有選擇圖片才顯示


        }

        // 新增 Horizontal Img 
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
                    string savePath = Server.MapPath("~/Image/imageNews/");

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
                        savePathList.Add(new ImagePath { SaveName = fileName });


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


                    //取得新聞標題
                    string selHeadlineStr = headlineRadioBtnList.SelectedValue;
                    //取得新聞日期
                    string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

                    DBHelper db = new DBHelper();
                    string sqlCommand = $"UPDATE [News] SET newsImageJson=@fileNameJsonStr WHERE dateTitle = @dateTitle AND headline = @headline";

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters["fileNameJsonStr"] = fileNameJsonStr;
                    parameters["dateTitle"] = selNewsDate;
                    parameters["headline"] = selHeadlineStr;

                    // 要清掉資料庫圖片的資料，就將下列移除
                    //parameters["fileNameJsonStr"] = "[]";

                    db.UpdateDB(sqlCommand, parameters);

                    db.CloseDB();

                    // 渲染畫面
                    RadioButtonListU.Items.Clear();
                    loadImageList();

                }
                else
                {
                    Response.Write("<script>alert('*The maximum upload size is 10MB!');</script>");
                }



               
            }



        }


        // 當有選擇 Horizontal RadioButtonList 的選項，要跳出 Delete 的 button
        protected void RadioButtonListU_SelectedIndexChanged(object sender, EventArgs e)
        {
            //顯示刪除按鈕
            DelImageBtn.Visible = true;
        }



        // 刪除 horizontal 的圖片
        protected void DelImageBtn_Click(object sender, EventArgs e)
        {
            //先讀取資料庫原有資料
            loadImageList();


            //  取得選取項目的值
            string selHImageStr = RadioButtonListU.SelectedValue;

            // 刪除圖片檔案
            string savePath = Server.MapPath("~/Image/imageNews/");

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{savePath+selHImageStr}');window.location.href='CompanyManagement.aspx';", true);

            File.Delete(savePath + selHImageStr);

            // 逐一比對原始資料 List <saveNameListH> 中的 檔案名稱
            for (int i = 0; i < savePathList.Count; i++)
            {
                // 與刪除的選項相同名稱
                if (savePathList[i].SaveName.Equals(selHImageStr))
                {
                    // 移除 List 中同名的資料
                    savePathList.RemoveAt(i);
                }
            }

            // 更新刪除後的圖片名稱 JSON 存入資料庫
            DBHelper dB = new DBHelper();
            string saveNameJsonStr = JsonConvert.SerializeObject(savePathList);

            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            string sqlCommand = $"UPDATE [News] SET newsImageJson=@newsImageJson WHERE dateTitle = @dateTitle AND headline = @headline";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["newsImageJson"] = saveNameJsonStr;
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;


            dB.SearchDB(sqlCommand, parameters);

            dB.CloseDB();

            // 渲染畫面
            RadioButtonListU.Items.Clear();
            loadImageList();

        }



        // Group Image end   ---------------------------------------------------------------------------------

    }
}