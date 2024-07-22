using _20240702Yachts.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class NewFront2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadNews();
            }
        }

        //用來接收組圖 JSON 資料
        public class ImagePath
        {
            public string SaveName { get; set; }
        }


        private void loadNews()
        {
            List<ImagePath> savePathList = new List<ImagePath>();

            string guidStr = Request.QueryString["id"];

            //如果沒有網址傳值就導回新聞列表頁
            if (String.IsNullOrEmpty(guidStr))
            {
                Response.Redirect("NewsFront.aspx");
            }

            //依取得 guid 連線資料庫取得新聞資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT * FROM [News] WHERE guid = @guid";
            Dictionary<string,object> parameters = new Dictionary<string, object>();

            parameters["guid"] = guidStr.Trim();

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);


            if (rd.Read())
            {
                //渲染新聞標題
                newsTitle.InnerText = rd["headline"].ToString();

                //渲染新聞主文
                newsContent.Text = HttpUtility.HtmlDecode(rd["newsContentHtml"].ToString());

                string loadJson = HttpUtility.HtmlDecode(rd["newsImageJson"].ToString());
                //反序列化 JSON 格式
                savePathList = JsonConvert.DeserializeObject<List<ImagePath>>(loadJson);
            }


            db.CloseDB();


            //渲染新聞組圖
            if (savePathList?.Count > 0)
            {
                string imgHtmlStr = "";
                foreach (var item in savePathList)
                {
                    imgHtmlStr += $"<p><img alt='Image' src='/Image/imageNews/{item.SaveName}' style='width: 700px;' /></p>";
                }
                groupImg.Text = HttpUtility.HtmlDecode(imgHtmlStr);
            }



        }






    }
}