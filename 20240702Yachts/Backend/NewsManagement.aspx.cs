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
                //ckfinderSetPath();  // ck editor init setting
                Calendar1.SelectedDate = Calendar1.TodaysDate; // 預設選取當日日期
                loadDayNewsHeadLine();


                //如果選取當日有新聞
                if(headlineRadioBtnList.Items.Count > 0)
                {
                    PanelCoverContent.Visible = true; // 顯示右上區塊
                    PanelMainContent.Visible = true; // 顯示左下區塊
                    PanelGroupImage.Visible = true; // 顯示右下區塊

                    loadThumbnail();  // 讀取新聞列表縮圖
                    // loadSummary(); // 讀取新聞簡介
                    // loadNewsContent(); // 讀取-新聞上方主要內容
                    // loadImageList(); // 讀取新聞下方組圖

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
            headlineRadioBtnList.Items.Clear();
            loadDayNewsHeadLine();

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
                deleteNewsBtn.Enabled = true;
            }


        }




        // 刪除新聞 Delete News 按鈕的 OnClick 事件功能 => 後續製作需加入刪除列表頁縮圖圖檔及刪除新聞內容組圖等功能。
        protected void deleteNewsBtn_Click(object sender, EventArgs e)
        {
            // 隱藏刪除按鈕
            deleteNewsBtn.Visible = false;

            //取得選取項目內容
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;

            //取得日曆選取日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            //連線資料庫 => 刪除資料庫該筆資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"DELETE FROM [News] WHERE dateTitle = @dateTitle and headline = @headline";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["dateTitle"] = selNewsDate;
            parameters["headline"] = selHeadlineStr;
            db.SearchDB(sqlCommand, parameters);

            db.CloseDB();


            //渲染畫面
            headlineRadioBtnList.Items.Clear();
            loadDayNewsHeadLine();

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


        }



        //建立日曆日期選取改變時的 OnSelectionChanged 事件功能
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            deleteNewsBtn.Visible = false;
            LabIsTop.Visible=false;
            headlineRadioBtnList.Items.Clear();
            loadDayNewsHeadLine();

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

        // loadSummary start -----------------------------------------------------------------------

        private void loadSummary()
        {
            //取得新聞標題
            string selHeadlineStr = headlineRadioBtnList.SelectedValue;
            //取得新聞日期
            string selNewsDate = Calendar1.SelectedDate.ToString("yyyy-M-dd");

            //取得新聞簡介內容
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT summary FROM [News] WHERE dateTitle = @dateTitle and headline = @headline";
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


        // loadSummary end   -----------------------------------------------------------------------

    }
}