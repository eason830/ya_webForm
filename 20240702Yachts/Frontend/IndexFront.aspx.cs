using _20240702Yachts.Backend;
using NetVips;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class IndexFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadBanner();
                loadNews();
            }
        }

        //輪播圖 JSON 資料
        public class ImagePath
        {
            public string SavePath { get; set; }
        }



        private void loadBanner()
        {
            //用 List<T> 接收 Json 格式圖片資料
            List<ImagePath> savePathList = new List<ImagePath>();

            //連線資料庫取出圖片資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT * FROM [Yachts] ORDER BY id DESC";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            // html builder 
            StringBuilder bannerHtml = new StringBuilder();

            // 將所有內容放入
            while (rd.Read()) {

                string loadJson = rd["bannerImgPathJSON"].ToString();

                // 反序列化
                savePathList= JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);


                //每個型號只取出第一張圖
                string imgNameStr = "";

                if (savePathList?.Count > 0) {
                    // 指定選取 List<T> 的第一筆資料
                    imgNameStr = savePathList[0].SavePath;
                }


                //遊艇型號字串用空格切割區分文字及數字
                string[] modelArr = rd["yachtsModel"].ToString().Split(' ');

                // 依新設計或是新建造來切換顯示標籤
                string isNewDesignStr = rd["isNewDesign"].ToString();
                string isNewBuildingStr = rd["isNewBuilding"].ToString();

                string newTagStr = ""; //標籤檔名用 => 檔案路徑

                // value 預設為 0 不顯示標籤
                string displayNewStr = "0";


                //判斷是否顯示對應標籤
                if (isNewDesignStr.Equals("True"))
                {
                    displayNewStr = "1";
                    newTagStr = "./html/images/new02.png";
                }
                else if (isNewBuildingStr.Equals("True"))
                {
                    displayNewStr = "1";
                    newTagStr = "./html/images/new01.png";
                }


                //加入遊艇型號輪播圖 HTML
                //加入遊艇型號輪播圖 HTML
                bannerHtml.Append($"<li class='info' style='border-radius: 5px;height: 424px;width: 978px;'>" +
                                    $"<a href='' target='_blank'><img src='/Image/imageYachts/{imgNameStr}' style='width: 978px;height: 424px;border-radius: 5px;'/></a>" +
                                    $"<div class='wordtitle'>{modelArr[0]} <span>{modelArr[1]}</span><br /><p>SPECIFICATION SHEET</p></div>" +
                                    $"<div class='new' style='display: none;overflow: hidden;border-radius:10px;'><img src='{newTagStr}' alt='new' /></div>" +
                                    $"<input type='hidden' value='{displayNewStr}' /></li>");



            }



            db.CloseDB();

            //渲染畫面
            LitBanner.Text = bannerHtml.ToString();
            LitBannerNum.Text = bannerHtml.ToString(); //不顯示但影響輪播圖片數量計算


        }


        private void loadNews()
        {
            //設定首頁 3 筆新聞的時間範圍
            DateTime nowTime = DateTime.Now;
            string nowDate = nowTime.ToString("yyyy-MM-dd");
            int startDate = -1;
            DateTime limitTime = nowTime.AddMonths(startDate);
            string limitDate = limitTime.ToString("yyyy-MM-dd");

            //計算設定的時間範圍內新聞數量
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT COUNT(id) FROM [News] WHERE dateTitle >= @limitDate AND dateTitle <= @nowDate";
            Dictionary<string,object> parameters = new Dictionary<string,object>();

            parameters["@limitDate"] = limitDate;
            parameters["@nowDate"] = nowDate;

            // 計算數量 =>看有沒有取到三筆
            int newsNum = Convert.ToInt32(db.SearchDB_one(sqlCommand, parameters));

            //時間範圍設定持續往前 1 個月，直到取出新聞數量超過 3 筆
            while (newsNum < 3)
            {
                startDate--;
                limitTime = nowTime.AddDays(startDate);
                limitDate = limitTime.ToString("yyyy-MM-dd");
                parameters["@limitDate"] = limitDate;
                parameters["@nowDate"] = nowDate;

                newsNum = Convert.ToInt32(db.SearchDB_one(sqlCommand, parameters));
            }


            db.CloseDB();


            //取出時間範圍內首 3 筆新聞，且 TOP 會放在取出項的最前面
            DBHelper db2 = new DBHelper();

            string sqlCommand2 = $"SELECT TOP 3 * FROM [News] WHERE dateTitle >= @limitDate AND dateTitle <= @nowDate ORDER BY isTop DESC, dateTitle DESC";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();

            parameters2["@limitDate"] = limitDate;
            parameters2["@nowDate"] = nowDate;

            SqlDataReader rd2 = db2.SearchDB(sqlCommand2 , parameters2);

            int count = 1; // 第幾筆新聞

            while (rd2.Read())
            {
                string isTopStr = rd2["isTop"].ToString();
                string guidStr = rd2["guid"].ToString();
                if (count == 1)
                {
                    //渲染第1筆新聞圖卡
                    string newsImg = rd2["thumbnailPath"].ToString();
                    LiteralNewImg1.Text = $"<img id='thumbnail_Image1' src='{newsImg}' style='border-width: 0px;' />";
                    DateTime dateTimeTitle = DateTime.Parse(rd2["dateTitle"].ToString());
                    LabNewsDate1.Text = dateTimeTitle.ToString("yyyy/M/d");
                    HLinkNews1.Text = rd2["headline"].ToString();
                    HLinkNews1.NavigateUrl = $"NewFront2.aspx?id={guidStr}";
                    if (isTopStr.Equals("True"))
                    {
                        ImageIsTop1.Visible = true;
                    }
                }
                else if (count == 2)
                {
                    //渲染第2筆新聞圖卡
                    string newsImg = rd2["thumbnailPath"].ToString();
                    LiteralNewImg2.Text = $"<img id='thumbnail_Image2' src='{newsImg}' style='border-width: 0px;' />";
                    DateTime dateTimeTitle = DateTime.Parse(rd2["dateTitle"].ToString());
                    LabNewsDate2.Text = dateTimeTitle.ToString("yyyy/M/d");
                    HLinkNews2.Text = rd2["headline"].ToString();
                    HLinkNews2.NavigateUrl = $"NewFront2.aspx?id={guidStr}";
                    if (isTopStr.Equals("True"))
                    {
                        ImageIsTop2.Visible = true;
                    }
                }
                else if (count == 3)
                {
                    //渲染第3筆新聞圖卡
                    string newsImg = rd2["thumbnailPath"].ToString();
                    LiteralNewImg3.Text = $"<img id='thumbnail_Image3' src='{newsImg}' style='border-width: 0px;' />";
                    DateTime dateTimeTitle = DateTime.Parse(rd2["dateTitle"].ToString());
                    LabNewsDate3.Text = dateTimeTitle.ToString("yyyy/M/d");
                    HLinkNews3.Text = rd2["headline"].ToString();
                    HLinkNews3.NavigateUrl = $"NewFront2.aspx?id={guidStr}";
                    if (isTopStr.Equals("True"))
                    {
                        ImageIsTop3.Visible = true;
                    }
                }
                else break; //超過3筆後停止
                count++;
            }


            db2.CloseDB();



        }

    }
}