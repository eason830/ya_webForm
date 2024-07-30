using _20240702Yachts.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class YachtsVideoFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //會先跑 Content 頁的 Page_Load 才跑 Master 頁的 Page_Load
            if (!IsPostBack)
            {
                loadContent();
            }
        }


        private void loadContent()
        {
            List<RowData> saveRowList = new List<RowData>();
            //取得 Session 共用 Guid，Session 物件需轉回字串
            string guidStr = Session["guid"].ToString();

            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT overviewDimensionsJSON FROM [Yachts] WHERE guid = @guid";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["@guid"] = guidStr;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);


            if (rd.Read())
            {
                string loadJson = HttpUtility.HtmlDecode(rd["overviewDimensionsJSON"].ToString());
                saveRowList = JsonConvert.DeserializeObject<List<RowData>>(loadJson);
                // List<T> 第一筆資料是放影片連結
                string youtubeUrlStr = saveRowList[0].SaveValue;
                //如果沒有影片連結就導回 OverView 分頁
                if (String.IsNullOrEmpty(youtubeUrlStr))
                {
                    Response.Redirect($"YachtsOverviewFront.aspx?id={guidStr}");
                }
                else
                {
                    //將取出的 YouTube 連結字串分離出 "影片 ID 字串"
                    //使用者如果是用分享功能複製連結時處理方式
                    string[] youtubeUrlArr = youtubeUrlStr.Split('/');
                    //使用者如果是直接從網址複製連結時處理方式
                    string[] vedioIDArr = youtubeUrlArr[youtubeUrlArr.Length - 1].Split('=');
                    //將 "影片 ID 字串" 組合成嵌入狀態的 YouTube 連結
                    //https://www.youtube.com/watch?v=11iZcYbq_is&list=RDMM11iZcYbq_is&start_radio=1
                    //https://www.youtube.com/watch?v=nL_VoX0ZR0E&list=RDnL_VoX0ZR0E&start_radio=1

                    //string strNewUrl = "https:/" + "/youtube.com/" + "embed/" + vedioIDArr[vedioIDArr.Length - 1];
                    string strNewUrl = "https:/" + "/youtube.com/" + "embed/" + youtubeUrlArr[youtubeUrlArr.Length - 1];
                    //更新 <iframe> src 連結
                    video.Attributes.Add("src", strNewUrl);

                }
            }
            db.CloseDB();
        }

        // JSON 資料
        public class RowData
        {
            public string SaveItem { get; set; }
            public string SaveValue { get; set; }
        }

    }
}