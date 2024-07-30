using _20240702Yachts.Backend;
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
    public partial class NewsFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadNewsList();
            }
        }


        private void loadNewsList()
        {
            //取得目前的時間，只顯示日期前的新聞
            DateTime nowTime = DateTime.Now;
            string nowDate = nowTime.ToString("yyyy-MM-dd");


            //1.連線資料庫
            DBHelper db = new DBHelper();

            //2.建立判斷網址是否有傳值邏輯 (網址傳值功能已於製作控制項時已完成)
            int page = 1; //預設為第1頁
                          //判斷網址後有無參數
                          //也可用String.IsNullOrWhiteSpace
            if (!String.IsNullOrEmpty(Request.QueryString["page"]))
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }


            //3.設定頁面參數屬性
            //設定控制項參數: 一頁幾筆資料
            Pagination.limit = 5;
            //設定控制項參數: 作用頁面完整網頁名稱
            Pagination.targetPage = "NewsFront.aspx";

            //4.建立計算分頁資料顯示邏輯 (每一頁是從第幾筆開始到第幾筆結束)
            //計算每個分頁的第幾筆到第幾筆
            var floor = (page - 1) * Pagination.limit + 1; //每頁的第一筆
            var ceiling = page * Pagination.limit; //每頁的最末筆

            //5.建立計算資料筆數的 SQL 語法
            //算出我們要秀的資料數
            string sqlCommand = $"SELECT COUNT(id) FROM [News] WHERE dateTitle <= @nowDate";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["nowDate"] = nowDate;

            //6.將取得的資料數設定給參數 count

            //用 ExecuteScalar() 來算數量
            int count = Convert.ToInt32(db.SearchDB_one(sqlCommand, parameters));


            db.CloseDB();


            //7.將取得的資料筆數設定給頁面參數屬性
            //設定控制項參數: 總共幾筆資料
            Pagination.totalItems = count;

            //8.使用 showPageControls() 渲染至網頁 (方法於製作控制項時已完成)
            //渲染分頁控制項
            Pagination.showPageControls();

            //9.將原始資料表的 SQL 語法使用 CTE 暫存表改寫，並使用 ROW_NUMBER() 函式製作資料項流水號 rowindex
            // SQL 用 CTE 暫存表 + ROW_NUMBER 去生出我的流水號 rowindex 後以流水號為條件來查詢暫存表
            // 排序先用 isTop 後用 dateTitle 產生 TOP News 置頂效果
            DBHelper db2 = new DBHelper();
            string sqlCommand2 = $"WITH temp AS (SELECT ROW_NUMBER() OVER (ORDER BY isTop DESC, dateTitle DESC) AS rowindex, * FROM [News] WHERE dateTitle <= @nowDate) SELECT * FROM temp WHERE rowindex >= {floor} AND rowindex <= {ceiling}";

            Dictionary<string, object> parameters2 = new Dictionary<string, object>();

            parameters2["nowDate"] = nowDate;


            //10.取得每頁的新聞列表資料製作成 HTML 內容
            StringBuilder newListHtml = new StringBuilder();
            SqlDataReader rd = db2.SearchDB(sqlCommand2 , parameters2);

            while (rd.Read())
            {
                string idStr = rd["id"].ToString();
                DateTime dateTimeTitle = DateTime.Parse(rd["dateTitle"].ToString());
                string dateTitleStr = dateTimeTitle.ToString("yyyy/M/d");
                string headlineStr = rd["headline"].ToString();
                string summaryStr = rd["summary"].ToString();
                string thumbnailPathStr = rd["thumbnailPath"].ToString();
                string guidStr = rd["guid"].ToString();
                string isTopStr = rd["isTop"].ToString();
                string displayStr = "none";
                if (isTopStr.Equals("True"))
                {
                    displayStr = "inline-block";
                }
                newListHtml.Append($"<li><div class='list01'><ul><li><div class='news01'>" +
                    $"<img src='/Frontend/html/images/new_top01.png' alt='&quot;&quot;' style='display: {displayStr};position: absolute;z-index: 5;'/>" +
                    $"<div class='news02p1' style='margin: 0px;border-width: 0px;padding: 0px;' ><p>" +
                    $"<img id='thumbnail_Image{idStr}' src='{thumbnailPathStr}' style='border-width: 0px;position: absolute;z-index: 1;' width='161px' height='121px' />" +
                    $"</p></div></li><li><span>{dateTitleStr}</span><br />" +
                    $"<a href='NewFront2.aspx?id={guidStr}'>{headlineStr} </a></li><br />" +
                    $"<li>{summaryStr} </li></ul></div></li>");
            }


            db2.CloseDB();

            //渲染新聞列表
            newList.Text = newListHtml.ToString();

        }








    }
}