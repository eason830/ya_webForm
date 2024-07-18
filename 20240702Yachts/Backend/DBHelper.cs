using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace _20240702Yachts.Backend
{

    // 將資料庫的操作，封裝到 DBHelper ，避免重複的 Code 一直寫

    public class DBHelper
    {

        // 建立 Sql 連線物件
        SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["20240702YachtsConnectionString"].ConnectionString);


        //  建立 Sql 命令物件
        SqlCommand sqlCommand = new SqlCommand();


        // 連線資料庫，讓別人不能連線，設成私人
        private void ConnectDB()
        {
            // 判斷有沒有連線了
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }



        // 關閉資料庫
        public void CloseDB()
        {
            connection.Close();
        }


        // 查詢資料庫 => ExecuteReader() : 回傳多筆資料時
        public SqlDataReader SearchDB(string sql, Dictionary<string, object> Dictionary = null) // Dictionary=null 起始值
        {
            // 連接 DB
            ConnectDB();

            // 判斷有沒有使用參數
            if (Dictionary != null)
            {
                foreach (var item in Dictionary)
                {
                    sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            // 發送 SQL 語法，取得結果，第一句前面有宣告過
            sqlCommand.Connection = connection;


            // 查詢方法
            // string sql = $"SELECT * FROM Student";

            // 將準備的 SQL 指令給操作物件
            sqlCommand.CommandText = sql;

            // 執行該 SQL 查詢， 用 reader 接資料
            SqlDataReader reader = sqlCommand.ExecuteReader();

            // 不能放在 return 下面
            // CloseDB();

            return reader;



            //// 使用方式


            //public void showGV()
            //{
            //    DbHelper db = new DbHelper();
            //    string sqlCommand = $"Select * From Video";
            //    SqlDataReader rd = db.SearchDB(sqlCommand);
            //    GridView1.DataSource = rd;
            //    GridView1.DataBind();
            //}

            //protected void ShowRepeater()  //記得要把 ShowRepeater() 放到 Page_Load
            //{
            //    DbHelper db = new DbHelper();
            //    string sql = "SELECT * FROM Video";
            //    SqlDataReader rd = db.SearchDB(sql);
            //    if (rd.HasRows)
            //    {
            //        Repeater1.DataSource = rd;
            //        Repeater1.DataBind();
            //    }
            //    db.CloseDB();
            //}


            //// 5. 透過SQL語法進行資料的修改 (開始撰寫dbhelper 4 個流程)
            //DbHelper db = new DbHelper();
            //string sqlCommand = "UPDATE video SET link = @link WHERE ID = @ID";

            //// 使用參數化查詢來避免 SQL 注入攻擊
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters["@link"] = UpdateLinkTextBox.Text;
            //parameters["@ID"] = IDkey;
            //db.SearchDB(sqlCommand, parameters);

            //// 6. 補充：不要忘記重新 把 編輯模式 改回 閱讀模式 以及執行 showGV()
            //db.CloseDB();

        }


        // 查詢資料庫 => ExecuteScalar() : 只回傳單筆資料
        public object SearchDB_one(string sql, Dictionary<string, object> Dictionary = null) // Dictionary=null 起始值
        {
            // 連接 DB
            ConnectDB();

            // 清空之前的參數，以免重複添加
            sqlCommand.Parameters.Clear();

            // 判斷有沒有使用參數
            if (Dictionary != null)
            {
                foreach (var item in Dictionary)
                {
                    sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            // 發送 SQL 語法，取得結果，第一句前面有宣告過
            sqlCommand.Connection = connection;

            // 將準備的 SQL 指令給操作物件
            sqlCommand.CommandText = sql;

            // 執行查詢，返回結果的第一行第一列的值
            object result = sqlCommand.ExecuteScalar();

            return result;

        }




        // 變更資料庫 => ExecuteNonQuery() : 只執行Insert, Update, Delete等行為
        public int UpdateDB(string sql, Dictionary<string, object> Dictionary = null) // Dictionary=null 起始值
        {
            ConnectDB();

            // 清空之前的參數，以免重複添加
            sqlCommand.Parameters.Clear();

            // 判斷有沒有使用參數
            if (Dictionary != null)
            {
                foreach (var item in Dictionary)
                {
                    sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            // 發送 SQL 語法，取得結果，第一句前面有宣告過
            sqlCommand.Connection = connection;

            // 設置 SQL 命令
            sqlCommand.CommandText = sql;

            // 執行非查詢類型的 SQL 操作，返回影響的行數
            int rowsAffected = sqlCommand.ExecuteNonQuery();

            return rowsAffected;
        }





        // 使用方式


        //// 使用 DbHelper 查詢資料並將結果綁定到 GridView
        //DbHelper dbHelper = new DbHelper();
        //string selectSql = "SELECT * FROM Video";
        //SqlDataReader reader = dbHelper.SearchDB(selectSql);
        //GridView1.DataSource = reader;
        //GridView1.DataBind();
        //dbHelper.CloseDB(); // 記得關閉資料庫連線

        //// 使用 DbHelper 執行計算總數的 SQL 查詢
        //string countSql = "SELECT COUNT(id) FROM Video";
        //int totalCount = (int)dbHelper.ExecuteScalar(countSql);
        //Response.Write($"總資料列數是 {totalCount}");

        //// 使用 DbHelper 執行插入操作的 SQL 語句
        //string insertSql = @"INSERT INTO Video (link, image, title, videoCategoryId)
        //             VALUES ('link value', 'image value', 'title name', 21)";
        //int rowsAffected = dbHelper.ExecuteNonQuery(insertSql);
        //Response.Write($"總影響資料列數是 {rowsAffected}");

        //dbHelper.CloseDB(); // 記得關閉資料庫連線







    }
}