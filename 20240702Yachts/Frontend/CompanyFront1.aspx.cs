using _20240702Yachts.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Frontend
{
    public partial class CompanyFront1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadContent();
            }
        }


        private void loadContent() { 
            
            DBHelper db = new DBHelper();
            string sqlCommand = $"SELECT TOP 1 aboutUsHtml FROM [Company]";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read()) { 
                Literal1.Text = HttpUtility.HtmlDecode(rd["aboutUsHtml"].ToString());
            }

            db.CloseDB();


        }







    }
}