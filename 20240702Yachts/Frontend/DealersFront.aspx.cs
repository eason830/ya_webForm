using _20240702Yachts.Backend;
using MimeKit;
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
    public partial class DealersFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getCountryId(); // 取得國家 id
                loadLeftMenu();
                loadDealerList();
            }
        }


        // 建立取得國家 id 的 function
        private void getCountryId()
        {
            // 取得網址傳值的 id 內容
            string urlIdStr = Request.QueryString["id"];


            // 如果沒有網址傳值則設為第一筆國家 id
            if (string.IsNullOrEmpty(urlIdStr))
            {
                DBHelper db = new DBHelper();

                string sqlCommand = $"SELECT TOP 1 id FROM [Country]";

                // 使用參數化查詢來避免 SQL 注入攻擊
                //Dictionary<string, object> parameters = new Dictionary<string, object>();
                //parameters["id"] = urlIdStr;

                SqlDataReader rd = db.SearchDB(sqlCommand);

                if (rd.Read())
                {
                    urlIdStr = rd["id"].ToString();
                }


                db.CloseDB();



            }


            // 將 id 存入 session 使用
            Session["id"] = urlIdStr;

        }




        // 建立讀取側邊欄 loadLeftMenu(); 方法
        private void loadLeftMenu()
        {
            //反覆變更字串的值建議用 StringBuilder 效能較好
            StringBuilder leftMenuHtml = new StringBuilder();

            // 取得國家分類
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT * FROM [Country]";

            // 使用參數化查詢來避免 SQL 注入攻擊
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters["id"] = urlIdStr;

            SqlDataReader rd = db.SearchDB(sqlCommand);


            while (rd.Read()) {
            
                string idStr=rd["id"].ToString();
                string countryStr = rd["countryName"].ToString();

                // StringBuilder 用 Append 加入字串內容
                leftMenuHtml.Append($"<li><a href='dealersFront.aspx?id={idStr}'>{countryStr}</a></li>");

            }


            db.CloseDB();

            //渲染畫面
            LiteralCountry.Text = leftMenuHtml.ToString();



        }




        //建立讀取主要內容 loadDealerList(); 方法
        private void loadDealerList()
        {

            //取得 Session 儲存 id，Session 物件需轉回字串
            string countryIdStr = Session["id"].ToString();

            DBHelper dB = new DBHelper();

            string sqlCommand = $"SELECT countryName FROM [Country] WHERE id=@countryIdStr";

            Dictionary<string ,object> parameters = new Dictionary<string ,object>();

            parameters["countryIdStr"]=countryIdStr;

            SqlDataReader rd = dB.SearchDB(sqlCommand,parameters);


            if (rd.Read())
            {

                string countryStr = rd["countryName"].ToString();
                LabLink.InnerText = countryStr;
                LitTitle.InnerText = countryStr;
            }



            //依 country id 取得代理商資料
            StringBuilder dealerListHtml = new StringBuilder();

            DBHelper db2 = new DBHelper();
            string sqlCommand2 = $"SELECT * FROM [Dealers] WHERE countryId=@countryIdStr";


            Dictionary<string, object> parameters2 = new Dictionary<string, object>();

            parameters2["countryIdStr"] = countryIdStr;

            SqlDataReader rd2 = db2.SearchDB(sqlCommand2, parameters2);

            while (rd2.Read()) {

                string idStr = rd2["id"].ToString();
                string areaStr = rd2["area"].ToString();
                string imgPathStr = rd2["dealerImgPath"].ToString();
                string nameStr = rd2["name"].ToString();
                string contactStr = rd2["contact"].ToString();
                string addressStr = rd2["address"].ToString();
                string telStr = rd2["tel"].ToString();
                string faxStr = rd2["fax"].ToString();
                string emailStr = rd2["email"].ToString();
                string linkStr = rd2["link"].ToString();


                dealerListHtml.Append("<li>" +
                    "<div class='list02'>" +
                        "<ul>" +
                            "<li class='list02li'>" +
                                "<div>" +
                                   $"<p><img id='Image{idStr}' src='{imgPathStr}' style='border-width:0px;' Width='209px' /> </p>" +
                                   $"</div>" +
                             "</li>" +
                             $"<li class='list02li02'>" +
                                $" <span>{areaStr}</span><br />");



                if (!string.IsNullOrEmpty(nameStr))
                {
                    dealerListHtml.Append($"{nameStr}<br />");
                }
                if (!string.IsNullOrEmpty(contactStr))
                {
                    dealerListHtml.Append($"Contact：{contactStr}<br />");
                }
                if (!string.IsNullOrEmpty(addressStr))
                {
                    dealerListHtml.Append($"Address：{addressStr}<br />");
                }
                if (!string.IsNullOrEmpty(telStr))
                {
                    dealerListHtml.Append($"TEL：{telStr}<br />");
                }
                if (!string.IsNullOrEmpty(faxStr))
                {
                    dealerListHtml.Append($"FAX：{faxStr}<br />");
                }
                if (!string.IsNullOrEmpty(emailStr))
                {
                    dealerListHtml.Append($"E-Mail：{emailStr}<br />");
                }
                if (!string.IsNullOrEmpty(linkStr))
                {
                    dealerListHtml.Append($"<a href='{linkStr}' target='_blank'>{linkStr}</a>");
                }

                dealerListHtml.Append("</li></ul></div></li>");




            }

            //渲染畫面

            LiteralDealerList.Text = dealerListHtml.ToString();


        }




    }









}