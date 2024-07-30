using _20240702Yachts.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace _20240702Yachts.Frontend
{
    public partial class SiteYachts : System.Web.UI.MasterPage
    {


        // 執行順序 : Content 頁 Init > Master 頁 Init > Content 頁 Load > Master 頁 Load

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 將型號對應 guid 存入 Session 與子頁共用
                getGuid(); // 要放到 Init 不然 Content 會去先去抓 Session 而抓不到
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadGallery();  // 讀取並渲染上方相簿輪播
                loadLeftMenu(); // 讀取並渲染左側型號邊欄
                loadTopMenu();  // 讀取並渲染型號內容上方標題及分頁列
            }
        }



        // 建立取得遊艇型號對應 GUID 的 getGuid(); 方法邏輯如下
        private void getGuid()
        {
            // 取得網址傳值的型號對應 GUID
            string guidStr = Request.QueryString["id"];

            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT TOP 1 guid FROM [Yachts]";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                // 如果無網址傳值就用第一筆遊艇型號的 GUID
                if (String.IsNullOrEmpty(guidStr))
                {
                    guidStr = rd["guid"].ToString().Trim();
                }

            }

            db.CloseDB();

            // 將 GUID 存入 Session 供上方列表使用
            Session["guid"] = guidStr;


        }



        //型號輪播圖片 JSON 資料
        public class ImagePath
        {
            public string SavePath { get; set; }
        }


        // 建立渲染上方相簿輪播的 loadGallery(); 方法程式邏輯如下
        private void loadGallery()
        {
            // 建立資料表存資料
            DataTable dataTable = new DataTable();

            // 新增表格欄位，預設從 1 開始，設定欄位名稱
            dataTable.Columns.AddRange(new DataColumn[1] { new DataColumn("imageUrl") });

            // 取得 Session 共用 GUID ， Session 物件需轉為字串
            string guidStr = Session["guid"].ToString();

            // 依 GUID 取得遊艇輪播圖片資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT bannerImgPathJSON FROM [Yachts] WHERE guid = @guid";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@guid"] = guidStr;
            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            List<ImagePath> savePathList = new List<ImagePath>();

            if (rd.Read())
            {
                string loadJson = HttpUtility.HtmlDecode(rd["bannerImgPathJSON"].ToString());
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);

                foreach (var item in savePathList)
                {
                    // 逐一填入圖片路徑欄位值
                    dataTable.Rows.Add($"/Image/imageYachts/{item.SavePath}");
                }
            }

            db.CloseDB();

            // 輪播圖片必須使用 Repeater 送 ，不然 JavaScript 抓不到 HTML 標籤會失敗
            // 設定用 Eval 綁定的輪播圖片路徑資料
            RepeaterImg.DataSource = dataTable; // 設定資料來源
            RepeaterImg.DataBind(); // 刷新圖片資料


        }






        // 建立渲染左側型號邊欄的 loadLeftMenu(); 方法程式邏輯如下
        private void loadLeftMenu()
        {
            string urlPathStr = System.IO.Path.GetFileName(Request.PhysicalPath);

            // 取得遊艇型號資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [Yachts]";
            SqlDataReader rd = db.SearchDB(sqlCommand);

            StringBuilder leftMenuHtml = new StringBuilder();

            while (rd.Read())
            {
                string yachtModelStr = rd["yachtsModel"].ToString();
                string isNewDesignStr = rd["isNewDesign"].ToString();
                string isNewBuildingStr = rd["isNewBuilding"].ToString();
                string guidStr = rd["guid"].ToString();
                string isNewStr = "";

                // 依是否為新建或是新設計加入標註
                if (isNewDesignStr.Equals("True"))
                {
                    isNewStr = "(New Design)";
                }
                else if (isNewBuildingStr.Equals("True")) 
                {
                    isNewStr = "(New Building)";
                }


                leftMenuHtml.Append($"<li><a href='{urlPathStr}?id={guidStr}'>{yachtModelStr} {isNewStr}</a></li>");



            }

            db.CloseDB();

            // 渲染左側遊艇型號選單
            LeftMenuHtml.Text = leftMenuHtml.ToString();


        }




        // 建立渲染型號內容上方標題及分頁列的 loadTopMenu(); 方法程式邏輯如下

        // 表格欄位 JSON 資料
        public class RowData
        {
            public string SaveItem { get; set; }
            public string SaveValue { get; set; }

        }



        private void loadTopMenu()
        {
            // 取得 Session 共用 GUID ， Session 物件需轉為字串
            string guidStr = Session["guid"].ToString();

            // 依 GUID 取得遊艇資料
            List<RowData> saveRowList = new List<RowData>();

            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [Yachts] WHERE guid=@guid";

            Dictionary<string,object> parameters = new Dictionary<string,object>();
            parameters["@guid"] = guidStr;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);

            StringBuilder topMenuHtmlStr = new StringBuilder();
            StringBuilder dimensionsTableHtmlStr = new StringBuilder();

            if (rd.Read())
            {
                string yachtModelStr = rd["yachtsModel"].ToString();
                string contentHtmlStr = HttpUtility.HtmlDecode(rd["overviewContentHtml"].ToString());
                string loadJson = HttpUtility.HtmlDecode(rd["overviewDimensionsJSON"].ToString()) ;

                string dimensionsImgPathStr = rd["overviewDimensionsImgPath"].ToString() ;
                string downloadsFilePathStr = rd["overviewDownloadsFilePath"].ToString() ;

                // 加入渲染型號內容上方分類連結列表
                topMenuHtmlStr.Append($"<li><a class='menu_yli01' href='YachtsOverviewFront.aspx?id={guidStr}'>Overview</a></li>");
                
                topMenuHtmlStr.Append($"<li><a class='menu_yli02' href='YachtsLayoutFront.aspx?id={guidStr}'>Layout & deck plan</a></li>");
                
                topMenuHtmlStr.Append($"<li><a class='menu_yli03' href='YachtsSpecificationFront.aspx?id={guidStr}'>Specification</a></li>");

                // 加入渲染型號內容上方分類連結列表 Video 分頁標籤，有存影片連結網址才渲染
                saveRowList=JsonConvert.DeserializeObject<List<RowData>>(loadJson);

                if (!String.IsNullOrEmpty(saveRowList[0].SaveValue))
                {
                    topMenuHtmlStr.Append($"<li><a class='menu_yli04' href='YachtsVideoFront.aspx?id={guidStr}'>Video</a></li>");
                }

                // 渲染畫面
                // 渲染上方小連結
                LabLink.InnerText = yachtModelStr;

                // 渲染標題
                LabTitle.InnerText = yachtModelStr;

                // 渲染型號內容上方分類連結列表
                TopMenuLinkHtml.Text = topMenuHtmlStr.ToString();

            }

            db.CloseDB();


        }








    }
}