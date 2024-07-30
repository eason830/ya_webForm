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
    public partial class YachtsOverviewFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 會先跑 Content 頁的 Page_Load 才跑 Master 頁 的 Page_Load
            if (!IsPostBack)
            {
                loadContent();
            }
        }


        // 表格欄位 JSON 資料
        public class RowData
        {
            public string SaveItem { get; set; }
            public string SaveValue { get; set; }
        }


        // 建立取得 OverView 分頁內容的 loadContent()
        private void loadContent()
        {
            // 取得 Session 共用 GUID ， Session 物件需轉回字串
            string guidStr = Session["guid"].ToString();

            // 依 GUID 取得遊艇資料
            DBHelper dB = new DBHelper();
            string sqlCommand = $"SELECT * FROM [Yachts] WHERE guid = @guid";
            Dictionary<string,object> parameters = new Dictionary<string,object>();
            parameters["@guid"] = guidStr;
            SqlDataReader rd = dB.SearchDB(sqlCommand, parameters);

            StringBuilder dimensionsTableHtmlStr = new StringBuilder();
            List<RowData> saveRowList = new List<RowData>();

            if (rd.Read())
            {
                string yachtModelStr = rd["yachtsModel"].ToString();

                string contentHtmlStr = HttpUtility.HtmlDecode(rd["overviewContentHtml"].ToString());
                string loadJson = HttpUtility.HtmlDecode(rd["overviewDimensionsJSON"].ToString());
                string dimensionImgPathStr = rd["overviewDimensionsImgPath"].ToString();
                string downloadsFilePathStr = rd["overviewDownloadsFilePath"].ToString();

                saveRowList = JsonConvert.DeserializeObject<List<RowData>>(loadJson);

                // 渲染型號主要內容
                ContentHtml.Text = contentHtmlStr;

                // 渲染 Dimensions 尺寸資料區塊 (第 3 筆開始才是尺寸資料) 
                if (saveRowList?.Count > 2)
                {
                    // 渲染尺寸表型號標題
                    string[] yachtModelArr = yachtModelStr.Split(' ');
                    dimensionTitle.InnerText = yachtModelArr[1] + " DIMENSIONS";

                    // 加入渲染 DIMENSIONS 尺寸資料
                    int count = 1;
                    foreach (var item in saveRowList)
                    {
                        // 第一筆是 Video 網址，第二筆是 Download 檔名，從第三筆開始取
                        if (count > 2)
                        {
                            dimensionsTableHtmlStr.Append($"<tr><th>{item.SaveItem}</th><td>{item.SaveValue}</td></tr>");
                        }
                        count++;
                    }

                    // 渲染尺寸表格文字內容
                    DimensionTableHtml.Text = dimensionsTableHtmlStr.ToString();

                    // 渲染尺寸表格圖片內容，無圖片不執行
                    if (!String.IsNullOrEmpty(dimensionImgPathStr))
                    {
                        DimensionsImgHtml.Text = $"<td><img alt='{yachtModelStr}' src='../Image/imageYachts/{dimensionImgPathStr}' Width='278px' /></td>";
                    }

                }
                else
                {
                    // 無尺寸資料則隱藏整個區塊
                    DimensionTableHtml.Visible = false;
                }


                // 渲染下方 Downloads 區塊
                if (!String.IsNullOrEmpty(downloadsFilePathStr))
                {
                    string downloadsTitle = saveRowList[1].SaveValue;
                    if (!String.IsNullOrEmpty(downloadsTitle))
                    {
                        // 如果沒設定 PDF 標題文字，則顯示文字改為 PDF 檔名
                        downloadsTitle = downloadsFilePathStr;
                    }

                    // 渲染下載連結
                    DownloadsHtml.Text = $"<a id='HyperLink1' href='../Image/imageYachts/{downloadsFilePathStr}' >{downloadsTitle}</a>";

                }
                else
                {
                    // 無下載連結則隱藏整個區塊
                    divDownload.Visible = false;
                }

            }


            dB.CloseDB();


        }






    }
}