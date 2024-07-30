using _20240702Yachts.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class YachtsLayoutFront : System.Web.UI.Page
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
            //取得 Session 共用 Guid，Session 物件需轉回字串
            string guidStr = Session["guid"].ToString();

            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT layoutDeckPlanImgPathJSON FROM [Yachts] WHERE guid = @guidStr";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["guidStr"] = guidStr;

            SqlDataReader rd = db.SearchDB(sqlCommand, parameters);


            StringBuilder layoutHtmlStr = new StringBuilder();
            List<LayoutPath> saveImagPathList = new List<LayoutPath>();

            if (rd.Read())
            {
                string loadImgJson = HttpUtility.HtmlDecode(rd["layoutDeckPlanImgPathJSON"].ToString());
                //加入頁面組圖 HTML
                saveImagPathList = JsonConvert.DeserializeObject<List<LayoutPath>>(loadImgJson);

                foreach (var item in saveImagPathList)
                {
                    //加入每張圖片
                    layoutHtmlStr.Append($"<li><img src='/Image/imageYachts/{item.SavePath}' alt='layout' Width='670px' /></li>");
                }

                //渲染畫面
                ContentHtml.Text = layoutHtmlStr.ToString();
            }
            db.CloseDB();

        }

        //頁面組圖 JSON 資料
        public class LayoutPath
        {
            public string SavePath { get; set; }
        }


    }
}