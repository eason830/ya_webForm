using MimeKit;
using MailKit.Net.Smtp;
// 下方預設的會撞名，要使用 MailKit.Net.Smtp
//using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace _20240702Yachts.Frontend
{
    public partial class ContactFront : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadModelList();
            }
        }

        // 送出 Contact 表單後要做的事情
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            // 檢查機器人驗證有沒有過關
            if (String.IsNullOrEmpty(Recaptcha1.Response))
            {
                // 沒有勾選機器人驗證的狀況
                lblMessage.Visible = true;
                lblMessage.Text = "Captcha cannot be empty.";

                //跳出 alert => 顯示沒有勾選機器人驗證
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Captcha cannot be empty！');", true);

            }
            else
            {
                // 有勾選機器人驗證
                var result = Recaptcha1.Verify();

                // 通過驗證
                if (result.Success)
                {
                    //此處可加入"我不是機器人驗證"成功後要做的事

                    //驗證成功則寄出信件並送出警告提醒

                    sendMail();

                    Response.Write("<script>alert('Thank you for contacting us!');location.href='ContactFront.aspx';</script>");

                }
                else
                {
                    // 沒通過驗證
                    lblMessage.Text = "Error(s): ";

                    foreach (var err in result.ErrorCodes)
                    {
                        lblMessage.Text = lblMessage.Text + err;
                    }

                    // 拿到錯誤訊息
                    //string errorMessage = "Error(s): ";

                    //foreach (var err in result.ErrorCodes)
                    //{
                    //    errorMessage = errorMessage + err;
                    //}

                    //跳出 alert，顯示驗證失敗的錯誤訊息
                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{errorMessage}');", true);

                }

            }

        }



        // 取得下拉選單遊艇型號 loadModelList()

        private void loadModelList()
        {
            //1.連線資料庫
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString))
            {

                // 2. sql 語法
                string sql = "SELECT * FROM Yachts";

                // 3.創建 Command 物件
                SqlCommand command = new SqlCommand();
                command.CommandText = sql;
                command.Connection = connection;


                //取得遊艇型號分類
                connection.Open();
                SqlDataReader rd = command.ExecuteReader();

                while (rd.Read())
                {
                    string typeStr = rd["yachtsModel"].ToString();
                    string isNewDesign = rd["isNewDesign"].ToString();
                    string isNewBuilding = rd["isNewBuilding"].ToString();


                    //加入遊艇型號下拉選單選項
                    ListItem listItem = new ListItem();

                    if (isNewDesign.Equals("True"))
                    {
                        listItem.Text = $"{typeStr} (New Design)";
                        listItem.Value = $"{typeStr} (New Design)";

                        DropDownListYachts.Items.Add(listItem);
                    }
                    else if (isNewBuilding.Equals("True"))
                    {
                        listItem.Text = $"{typeStr} (New Building)";
                        listItem.Value = $"{typeStr} (New Building)";
                        DropDownListYachts.Items.Add(listItem);
                    }
                    else
                    {
                        listItem.Text = typeStr;
                        listItem.Value = typeStr;
                        DropDownListYachts.Items.Add(listItem);
                    }

                    // 因為使用 using 所以不用特別 close 


                }



            }


        }



        // 寄信
        public void sendMail()
        {
            // 宣告使用 MimeMessage
            var message = new MimeMessage();

            // 設定發信地址 ("發信人","發信 email")
            message.From.Add(new MailboxAddress("TayanaYacht", "Yachts20240702@gmail.com"));

            // 設定收信地址 ("收信人","收信 email")
            message.To.Add(new MailboxAddress(TextBoxName.Text.Trim(),TextBoxEmail.Text.Trim()));

            // 寄件副本 email
            message.Cc.Add(new MailboxAddress("CC的人，可以CC 公司自己的帳號", "Yachts20240702@gmail.com"));

            // 設定優先權
            //message.Priority = MessagePriority.Normal;

            // 信件標題
            message.Subject = "TayanaYacht Auto Email";

            // 建立 html 郵件格式
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody =
            "<h1>Thank you for contacting us!</h1>" +
            $"<h3>Name : {TextBoxName.Text.Trim()}</h3>" +
            $"<h3>Email : {TextBoxEmail.Text.Trim()}</h3>" +
            $"<h3>Phone : {TextBoxPhone.Text.Trim()}</h3>" +
            $"<h3>Country : {DropDownListCountry.SelectedValue}</h3>" +
            $"<h3>Type : {DropDownListYachts.SelectedValue}</h3>" +
            $"<h3>Comments : </h3>" +
            $"<p>{TextBoxComments.Text.Trim()}</p>";

            // 設定郵件內容
            message.Body = bodyBuilder.ToMessageBody(); // 轉成郵件內容格式


            // smtp 寄信

            using (var client = new SmtpClient())
            {
                // 有開防毒時需設定 false 關閉檢查
                client.CheckCertificateRevocation = false;

                // 設定連線 gmail ("smtp Server", Port, SSL加密) 

                client.Connect("smtp.gmail.com",587,false); // localhost 測試使用加密需先關閉


                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("Yachts20240702@gmail.com", "dsko gfpg ptda mfnu");

                // 發信 
                client.Send(message);

                // 結束連線
                client.Disconnect(true);


            }




        }




    }
}