using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _20240702Yachts.Backend
{
    public partial class SiteBackend : System.Web.UI.MasterPage
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            //清除Cache，避免登出後按上一頁還會顯示Cache頁面
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
        }



        // 將權限關門判斷放在 Master Page 就不用每一頁都寫。
        protected void Page_Load(object sender, EventArgs e)
        {
            //權限關門判斷 (Cookie)
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Response.Redirect("../Frontend/LoginFront.aspx"); // 導回登入頁面
            }
            else
            {
                //取得驗證票夾帶資訊
                string ticketUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                string[] ticketUserDataArr = ticketUserData.Split(';');
                bool haveRight = HttpContext.Current.User.Identity.IsAuthenticated;

                // 依管理權限導頁面
                if (haveRight)
                {
                    // 如果是 admin 最高權限
                    if (ticketUserDataArr[0].Equals("admin"))
                    {
                        //以驗證票夾帶資料作為限制，最高權限者使用時顯示使用者管理頁並切換圖示

                        // 可以設置 某一個權限才會顯示的 content placeholder
                        //ManagerMenuContentPlaceHolder.Visible = true;
                        //ManagerMainContentPlaceHolder.Visible = true;
                        // 可以設置最高權限者使用的圖示
                        //ImageHead.ImageUrl = "assets/images/avatar-4.png";
                        //ImageMenu.ImageUrl = "assets/images/avatar-4.png";


                    }
                    else
                    {
                        // 一般 user 使用者 => 顯示一般區塊即可
                        //ManagerMenuContentPlaceHolder.Visible = false;
                        //ManagerMainContentPlaceHolder.Visible = false;
                    }

                    //載入使用者個人基本資料(渲染畫面)
                    // 顯示在登入資訊
                    //LabMenuAccount.Text = ticketUserDataArr[1];
                    //LabMenuEmail.Text = ticketUserDataArr[3];
                    //LabHeadUserName.Text = ticketUserDataArr[2];

                    LabelName.Text = ticketUserDataArr[2];
                    LabelEmail.Text = ticketUserDataArr[3];
                }


            }

        }
    }
}