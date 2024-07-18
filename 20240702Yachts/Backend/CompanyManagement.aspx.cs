using CKFinder;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace _20240702Yachts.Backend
{
    public partial class CompanyManagement : System.Web.UI.Page
    {


        // JSON 資料 Vertical Image
        public class ImageNameV
        {
            public string SaveName { get; set; }
        }


        // JSON 資料 Horizontal Image
        public class ImageNameH
        {
            public string SaveName { get; set; }
        }


        //宣告全域 List<T> 可用 Add 依序添加資料
        private List<ImageNameV> saveNameListV = new List<ImageNameV>();
        private List<ImageNameH> saveNameListH = new List<ImageNameH>();



        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ckfinderSetPath();
                loadCkeditorContent();
                loadCertificatContent();
                loadImageVList();
                loadImageHList();
            }





        }



        // ck editor setting start --------------------------------------------------------------------------

        private void ckfinderSetPath()
        {
            FileBrowser fileBrowser = new FileBrowser();
            fileBrowser.BasePath = "/Backend/ckfinder";
            fileBrowser.SetupCKEditor(CKEditorControl1);

            // 網頁後置程式碼 .aspx.cs 取得輸入內容編譯方法如下
            // Literal.Text = HttpUtility.HtmlEncode(CKEditorControl1.Text);

        }

        // ck editor setting end ----------------------------------------------------------------------------




        // about ck editor start -----------------------------------------------------------------------------

        // 載入資料庫顯示 Ck editor 的內容
        private void loadCkeditorContent()
        {
            //  取得 About Us 頁面的 HTML資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT aboutUsHtml FROM [Company] WHERE id=1";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                //渲染畫面
                CKEditorControl1.Text = HttpUtility.HtmlDecode(rd["aboutUsHtml"].ToString());
            }

            db.CloseDB();

        }




        // 更新 About Us 的內容
        protected void UploadAboutUsBtn_Click(object sender, EventArgs e)
        {
            // 取得 CKEditor 的 HTML 的內容 => 使用 .HtmlEncode() 來避免錯誤或惡意使用。
            string aboutUsHtmlStr = HttpUtility.HtmlEncode(CKEditorControl1.Text);

            // 更新 About Us 頁面 HTML 資料
            DBHelper db = new DBHelper();
            string sqlCommand = $"UPDATE [Company] SET aboutUsHtml = @aboutUsHtml WHERE id = 1";

            Dictionary<string,object> parameters = new Dictionary<string,object>();

            parameters["aboutUsHtml" ] = aboutUsHtmlStr;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();


            // 渲染畫面提示
            DateTime nowtime = DateTime.Now;
            UploadAboutUsLab.Visible = true;
            UploadAboutUsLab.Text = $"Upload Success! - " +nowtime.ToString("G");


        }

        // about ck editor end ---------------------------------------------------------------------------------


        // certificat text start -------------------------------------------------------------------------------

        // 載入 certificat 的文字內容
        private void loadCertificatContent()
        {
            // 取得 Certificat 頁文字說明內容
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT certificatContent FROM [Company] WHERE id=1";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                //渲染畫面
                certificatTbox.Text = rd["certificatContent"].ToString();
            }

            db.CloseDB();


        }




        // 更新 certificat 的文字內容
        protected void uploadCertificatBtn_Click(object sender, EventArgs e)
        {
            // 更新 Certificat 頁文字說明內容

            DBHelper db = new DBHelper();
            string sqlCommand = $"UPDATE [Company] SET certificatContent = @certificatContent WHERE id = 1";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["certificatContent"] = certificatTbox.Text;

            db.UpdateDB(sqlCommand, parameters);

            db.CloseDB();


            // 渲染畫面提示
            DateTime nowtime = DateTime.Now;
            uploadCertificatLab.Visible = true;
            uploadCertificatLab.Text = $"Upload Success! - " + nowtime.ToString("G");



        }


        // certificat text end ---------------------------------------------------------------------------------




        // vertical img start ----------------------------------------------------------------------------------


        // 建立 loadImageVList() => Vertical Img
        private void loadImageVList()
        {
            // 連線資料庫取出資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT certificatVerticalImgJSON FROM [Company] WHERE id=1";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read())
            {
                string loadJson = rd["certificatVerticalImgJSON"].ToString();

                // 反序列化 Json 格式 => 將一個 JSON 格式的字串 loadJson，轉換為一個 List<ImageNameH> 的物件列表
                saveNameListV = JsonConvert.DeserializeObject<List<ImageNameV>>(loadJson);
            }


            db.CloseDB();


            //可以改成用 ?.Count 來判斷不是 Null 後才執行 .Count 避免錯誤
            if (saveNameListV?.Count > 0)
            {
                // 逐一取出 JSON 的每筆資料
                foreach (var item in saveNameListV)
                {

                    // 使用 html 要轉義
                    // 將 RadioButtonList 選項內容改為圖片格式，值設為檔案名稱
                    ListItem listItem = new ListItem($"<img src='/Image/imageCertificat/{item.SaveName}' alt='thumbnail' class='img-thumbnail' width='230px' />", item.SaveName);

                    // 加入圖片選項
                    RadioButtonListV.Items.Add(listItem);

                }
            }


            DelVImageBtn.Visible = false; // 刪除紐有選擇圖片才顯示


        }

        // 新增 Vertical Img 
        protected void UploadVBtn_Click(object sender, EventArgs e)
        {
            // 有選擇檔案才執行
            if (imageUploadV.HasFile)
            {
                // 先讀取資料庫原有資料
                loadImageVList();
                string savePath = Server.MapPath("~/Image/imageCertificat/");

                //添加圖檔資料
                //逐一讀取選擇的圖片檔案
                foreach (HttpPostedFile postedFile in imageUploadV.PostedFiles)
                {
                    //儲存圖片檔案及圖片名稱
                    //檢查專案資料夾內有無同名檔案，有同名就加流水號
                    DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
                    string fileName = postedFile.FileName;
                    string[] fileNameArr = fileName.Split('.');
                    int count = 0;
                    foreach (var fileItem in directoryInfo.GetFiles())
                    {
                        if (fileItem.Name.Contains(fileNameArr[0]))
                        {
                            count++;
                        }
                    }
                    fileName = fileNameArr[0] + $"({count + 1})." + fileNameArr[1];
                    //在圖片名稱前加入 temp 標示並儲存圖片檔案
                    //postedFile.SaveAs(savePath + "temp" + fileName);
                    postedFile.SaveAs(savePath + fileName);
                    //新增 JSON 資料
                    saveNameListV.Add(new ImageNameV { SaveName = fileName });


                    // 不壓縮了 => 因為 File.Delete 會有問題
                    //使用 NetVips 套件進行壓縮圖檔
                    //判斷儲存的原始圖片寬度是否大於設定寬度的 2 倍
                    //var img = NetVips.Image.NewFromFile(savePath + "temp" + fileName);
                    //if (img.Width > 214 * 2)
                    //{
                    //    //產生原使圖片一半大小的新圖片
                    //    var newImg = img.Resize(0.5);
                    //    //如果新圖片寬度還是大於原始圖片設定寬度的 2 倍就持續縮減
                    //    while (newImg.Width > 214 * 2)
                    //    {
                    //        newImg = newImg.Resize(0.5);
                    //    }
                    //    //儲存正式名稱的新圖片
                    //    newImg.WriteToFile(savePath + fileName);
                    //}
                    //else
                    //{
                    //    postedFile.SaveAs(savePath + fileName);
                    //}


                    //刪除原始圖片  => 這個會有 bug > 會刪不了 => 就先不刪除了
                    //File.Delete(savePath + "temp" + fileName);


                }


                // 更新新增後的圖片名稱 JSON 存入資料庫
                string fileNameJsonStr = JsonConvert.SerializeObject(saveNameListV);

                DBHelper db = new DBHelper();
                string sqlCommand = $"UPDATE [Company] SET certificatVerticalImgJSON=@fileNameJsonStr WHERE id=1";

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters["fileNameJsonStr"] = fileNameJsonStr;

                // 要清掉資料庫圖片的資料，就將下列移除
                //parameters["fileNameJsonStr"] = "[]";

                db.UpdateDB(sqlCommand, parameters);

                db.CloseDB();

                // 渲染畫面
                RadioButtonListV.Items.Clear();
                loadImageVList();

            }


        }


        // 當有選擇 Vertical RadioButtonList 的選項，要跳出 Delete 的 button
        protected void RadioButtonListV_SelectedIndexChanged(object sender, EventArgs e)
        {
            //顯示刪除按鈕
            DelVImageBtn.Visible = true;
        }


        // 刪除 vertical 的圖片
        protected void DelVImageBtn_Click(object sender, EventArgs e)
        {
            //先讀取資料庫原有資料
            loadImageVList();


            //  取得選取項目的值
            string selVImageStr = RadioButtonListV.SelectedValue;

            // 刪除圖片檔案
            string savePath = Server.MapPath("~/Image/imageCertificat/");

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{savePath+selHImageStr}');window.location.href='CompanyManagement.aspx';", true);

            File.Delete(savePath + selVImageStr);

            // 逐一比對原始資料 List <saveNameListH> 中的 檔案名稱
            for (int i = 0; i < saveNameListV.Count; i++)
            {
                // 與刪除的選項相同名稱
                if (saveNameListV[i].SaveName.Equals(selVImageStr))
                {
                    // 移除 List 中同名的資料
                    saveNameListV.RemoveAt(i);
                }
            }

            // 更新刪除後的圖片名稱 JSON 存入資料庫
            DBHelper dB = new DBHelper();

            string saveNameJsonStr = JsonConvert.SerializeObject(saveNameListV);

            string sqlCommand = $"UPDATE [Company] SET certificatVerticalImgJSON=@certificatVerticalImgJSON WHERE id=1";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["certificatVerticalImgJSON"] = saveNameJsonStr;


            dB.SearchDB(sqlCommand, parameters);

            dB.CloseDB();

            // 渲染畫面
            RadioButtonListV.Items.Clear();
            loadImageVList();
        }




        // vertical img end ------------------------------------------------------------------------------------



        // Horizontal img  start -------------------------------------------------------------------------------

        // 建立 loadImageHList() => Horizontal Img
        private void loadImageHList()
        {
            // 連線資料庫取出資料
            DBHelper db = new DBHelper();

            string sqlCommand = $"SELECT certificatHorizontalImgJSON FROM [Company] WHERE id=1";

            SqlDataReader rd = db.SearchDB(sqlCommand);

            if (rd.Read()) {
                string loadJson = rd["certificatHorizontalImgJSON"].ToString();

                // 反序列化 Json 格式 => 將一個 JSON 格式的字串 loadJson，轉換為一個 List<ImageNameH> 的物件列表
                saveNameListH = JsonConvert.DeserializeObject<List<ImageNameH>>(loadJson);
            }


            db.CloseDB();


            //可以改成用 ?.Count 來判斷不是 Null 後才執行 .Count 避免錯誤
            if(saveNameListH?.Count > 0)
            {
                // 逐一取出 JSON 的每筆資料
                foreach (var item in saveNameListH) {

                    // 使用 html 要轉義
                    // 將 RadioButtonList 選項內容改為圖片格式，值設為檔案名稱
                    ListItem listItem = new ListItem($"<img src='/Image/imageCertificat/{item.SaveName}' alt='thumbnail' class='img-thumbnail' width='230px' />",item.SaveName);

                    // 加入圖片選項
                    RadioButtonListH.Items.Add(listItem);

                }
            }


            DelHImageBtn.Visible= false; // 刪除紐有選擇圖片才顯示


        }


        // 新增 Horizontal Img 
        protected void UploadHBtn_Click(object sender, EventArgs e)
        {
            // 有選擇檔案才執行
            if (imageUploadH.HasFile)
            {
                // 先讀取資料庫原有資料
                loadImageHList();
                string savePath = Server.MapPath("~/Image/imageCertificat/");

                //添加圖檔資料
                //逐一讀取選擇的圖片檔案
                foreach (HttpPostedFile postedFile in imageUploadH.PostedFiles)
                {
                    //儲存圖片檔案及圖片名稱
                    //檢查專案資料夾內有無同名檔案，有同名就加流水號
                    DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
                    string fileName = postedFile.FileName;
                    string[] fileNameArr = fileName.Split('.');
                    int count = 0;
                    foreach (var fileItem in directoryInfo.GetFiles())
                    {
                        if (fileItem.Name.Contains(fileNameArr[0]))
                        {
                            count++;
                        }
                    }
                    fileName = fileNameArr[0] + $"({count + 1})." + fileNameArr[1];
                    //在圖片名稱前加入 temp 標示並儲存圖片檔案
                    //postedFile.SaveAs(savePath + "temp" + fileName);
                    postedFile.SaveAs(savePath  + fileName);
                    //新增 JSON 資料
                    saveNameListH.Add(new ImageNameH { SaveName = fileName });


                    // 不壓縮了 => 因為 File.Delete 會有問題
                    //使用 NetVips 套件進行壓縮圖檔
                    //判斷儲存的原始圖片寬度是否大於設定寬度的 2 倍
                    //var img = NetVips.Image.NewFromFile(savePath + "temp" + fileName);
                    //if (img.Width > 214 * 2)
                    //{
                    //    //產生原使圖片一半大小的新圖片
                    //    var newImg = img.Resize(0.5);
                    //    //如果新圖片寬度還是大於原始圖片設定寬度的 2 倍就持續縮減
                    //    while (newImg.Width > 214 * 2)
                    //    {
                    //        newImg = newImg.Resize(0.5);
                    //    }
                    //    //儲存正式名稱的新圖片
                    //    newImg.WriteToFile(savePath + fileName);
                    //}
                    //else
                    //{
                    //    postedFile.SaveAs(savePath + fileName);
                    //}


                    //刪除原始圖片  => 這個會有 bug > 會刪不了 => 就先不刪除了
                    //File.Delete(savePath + "temp" + fileName);


                }


                // 更新新增後的圖片名稱 JSON 存入資料庫
                string fileNameJsonStr = JsonConvert.SerializeObject(saveNameListH);

                DBHelper db = new DBHelper();
                string sqlCommand = $"UPDATE [Company] SET certificatHorizontalImgJSON=@fileNameJsonStr WHERE id=1";

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters["fileNameJsonStr"] = fileNameJsonStr;

                // 要清掉資料庫圖片的資料，就將下列移除
                //parameters["fileNameJsonStr"] = "[]";

                db.UpdateDB(sqlCommand, parameters);

                db.CloseDB();

                // 渲染畫面
                RadioButtonListH.Items.Clear();
                loadImageHList();

            }



        }


        // 當有選擇 Horizontal RadioButtonList 的選項，要跳出 Delete 的 button
        protected void RadioButtonListH_SelectedIndexChanged(object sender, EventArgs e)
        {
            //顯示刪除按鈕
            DelHImageBtn.Visible = true;
        }



        // 刪除 horizontal 的圖片
        protected void DelHImageBtn_Click(object sender, EventArgs e)
        {
            //先讀取資料庫原有資料
            loadImageHList();


            //  取得選取項目的值
            string selHImageStr = RadioButtonListH.SelectedValue;

            // 刪除圖片檔案
            string savePath = Server.MapPath("~/Image/imageCertificat/");

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{savePath+selHImageStr}');window.location.href='CompanyManagement.aspx';", true);

            File.Delete(savePath + selHImageStr);

            // 逐一比對原始資料 List <saveNameListH> 中的 檔案名稱
            for (int i = 0; i < saveNameListH.Count; i++)
            {
                // 與刪除的選項相同名稱
                if (saveNameListH[i].SaveName.Equals(selHImageStr))
                {
                    // 移除 List 中同名的資料
                    saveNameListH.RemoveAt(i);
                }
            }

            // 更新刪除後的圖片名稱 JSON 存入資料庫
            DBHelper dB = new DBHelper();

            string saveNameJsonStr = JsonConvert.SerializeObject(saveNameListH);

            string sqlCommand = $"UPDATE [Company] SET certificatHorizontalImgJSON=@certificatHorizontalImgJSON WHERE id=1";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["certificatHorizontalImgJSON"] = saveNameJsonStr;


            dB.SearchDB(sqlCommand, parameters);

            dB.CloseDB();

            // 渲染畫面
            RadioButtonListH.Items.Clear();
            loadImageHList();

        }







        // Horizontal img  end -------------------------------------------------------------------------------



    }
}