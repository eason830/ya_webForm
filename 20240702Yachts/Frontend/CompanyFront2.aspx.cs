using _20240702Yachts.Backend;
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
    public partial class CompanyFront2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadContentText();
                loadContentImgV();
                loadContentImgH();

            }

        }


        private void loadContentText()
        {
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT TOP 1 certificatContent FROM [Company]";
            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                //渲染畫面
                ContentText.Text = rd["certificatContent"].ToString();
            }


            db.CloseDB();

        }

        // JSON 資料 V 直式
        public class ImagePathV
        {
            public string SaveName { get; set; }
        }


        private void loadContentImgV()
        {
            // 會重複添加內容所以用 StringBuilder 效能比較好
            StringBuilder imgVHtml = new StringBuilder();

            // 用 List<> 來存取 JSON 格式圖片檔名
            List<ImagePathV> savePathListV = new List<ImagePathV>();

            //從資料庫取出直式圖檔檔名
            DBHelper db =new DBHelper();
            string sqlCommand = $"SELECT TOP 1 certificatVerticalImgJSON FROM [Company]";
            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read()) {
                string loadJson = HttpUtility.HtmlDecode(rd["certificatVerticalImgJSON"].ToString());
                // 反序列化 Json 格式
                savePathListV = JsonConvert.DeserializeObject<List<ImagePathV>>(loadJson);
            }

            db.CloseDB();


            // ?. 可用來判斷不是 Null 才執行 Count
            if (savePathListV?.Count > 0) {
                foreach (var item in savePathListV) {
                    imgVHtml.Append($"<li><p><img src='/Image/imageCertificat/{item.SaveName}' alt='Tayana ' width='300px' /></p></li>");
                }


                //渲染畫面
                ContentImgV.Text= imgVHtml.ToString();

            }




        }


        // JSON 資料 H 直式
        public class ImagePathH
        {
            public string SaveName { get; set; }
        }

        private void loadContentImgH()
        {
            //會重複添加內容所以用 StringBuilder 效能較好
            StringBuilder ImgHHtml = new StringBuilder();

            //用 List<T> 來存取 JSON 格式圖片檔名
            List<ImagePathH> savePathListH = new List<ImagePathH>();

            //從資料庫取出直式圖檔檔名
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT TOP 1 certificatHorizontalImgJSON FROM [Company]";
            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                string loadJson = HttpUtility.HtmlDecode(rd["certificatHorizontalImgJSON"].ToString());
                //反序列化JSON格式
                savePathListH = JsonConvert.DeserializeObject<List<ImagePathH>>(loadJson);
            }


            db.CloseDB();


            // ?. 可用來判斷不是 Null 才執行 Count
            if (savePathListH?.Count > 0)
            {
                foreach (var item in savePathListH)
                {
                    ImgHHtml.Append($"<li><p><img src='/Image/imageCertificat/{item.SaveName}' alt='Tayana ' width='319px' height='234px' /></p></li>");
                }
            }
            ContentImgH.Text = ImgHHtml.ToString();
        }

    }
}