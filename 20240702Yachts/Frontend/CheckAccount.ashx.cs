using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace _20240702Yachts.Frontend
{
    /// <summary>
    /// CheckAccount 的摘要描述
    /// </summary>
    public class CheckAccount : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // ashx 裡的 Request/Response 都要加上 context
            string ticketUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
            string[] ticketUserDataArr = ticketUserData.Split(';');
            bool haveRight = HttpContext.Current.User.Identity.IsAuthenticated;


            // 依照管理權限導頁
            if (haveRight) 
            {
                
                if (ticketUserDataArr[0].Equals("admin")) // 最高權限者
                {
                    //以驗證票夾帶資料作為限制
                    context.Response.Redirect("../Backend/UserManagement.aspx"); //最高管理員-跳至管理員審核頁面 // 跳到人員管理頁面
                }
                else // 等於 user => 一般使用者 => 導入到 前台
                {
                    context.Response.Redirect("ContactFront.aspx");
                }
            }
            else // 沒有任何權限
            {
                context.Response.Redirect("LoginFront.aspx"); //導回登入頁
            }



            // 測試用
            //context.Response.Write(haveRight);
            //context.Response.Write(ticketUserDataArr[0].Equals("True"));
            //context.Response.Write(ticketUserDataArr[0].Equals("admin"));
            //context.Response.Write(ticketUserDataArr[0].Equals("user"));



            // default 設定的
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}