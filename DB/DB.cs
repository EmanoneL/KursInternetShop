using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Data;
using System.Linq;

namespace DB
{
    public class DB
    {
        public static string dataBaseName { get; } = @"handmade shop system.db";

        SqliteConnection connection;

        public DB()
        {
            connectDB();
        }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
        private void connectDB()
        {
            Batteries.Init();
            string connectionString = "Data Source=" + dataBaseName;
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        string generateSELECTcommand(string values = "*", string FROM = "Bank", string WHERE = "", string ORDERBY = "")
        {
            string sqlExpression = $"SELECT {values} FROM {FROM}";
            if (WHERE != "") sqlExpression += " WHERE " + WHERE;
            if (ORDERBY != "") sqlExpression += " ORDER BY " + ORDERBY;
            sqlExpression += ";";
            return sqlExpression;
        }

        public SqliteDataReader SELECT(string values = "*", string FROM = "Bank", string WHERE = "", string ORDERBY = "")
        {

            //connection.Open();
            string sqlExpression = generateSELECTcommand(values, FROM, WHERE, ORDERBY);
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            SqliteDataReader reader = command.ExecuteReader();
            //connection.Close();
            return reader;
        }

        public SqliteDataReader SELECT(string sqlExpression)
        {
            //connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            SqliteDataReader reader = command.ExecuteReader();
            //connection.Close();
            return reader;
        }

        private string generteINSERTcommand(string INTO, string[] COLOMNS, string[] VALUES)
        {
            string columns = string.Join(", ", COLOMNS);
            string parameters = string.Join(", ", VALUES.Select((value, index) => $"@param{index}"));

            return $"INSERT INTO {INTO} ({columns}) VALUES ({parameters})";
        }


        public void INSERT(string INTO, string[] COLOMNS, string[] VALUES)
        {
            //connection.Open();
            if (COLOMNS.Length == VALUES.Length)
            {
                string sqlExpression = generteINSERTcommand(INTO, COLOMNS, VALUES);
                SqliteCommand insertSQL = new SqliteCommand(sqlExpression, connection);

                for (int i = 0; i < VALUES.Length; i++)
                {
                    insertSQL.Parameters.AddWithValue($"@param{i}", VALUES[i]);
                }
                insertSQL.ExecuteNonQuery();

            }
            //connection.Close();
        }

        string generateDELETEcommand(string FROM, string WHERE)
        {
            string exp = "DELETE FROM " + FROM;
            if (WHERE != "") exp += " WHERE " + WHERE;
            exp += ";";
            return exp;

        }
        public void DELETE(string FROM, string WHERE = "")
        {
            //connection.Open();
            string sqlExpression = generateDELETEcommand(FROM, WHERE);
            SqliteCommand deleteSQL = new SqliteCommand(sqlExpression, connection);
            deleteSQL.ExecuteNonQuery();
            //connection.Close();
        }

        string generateUPDATEcommand(string FROM, string SET, string WHERE)
        {
            string exp = $"UPDATE {FROM} SET {SET} WHERE {WHERE};";
            return exp;

        }

        public void UPDATE(string FROM, string SET, string WHERE)
        {
            //connection.Open();
            string sqlExpression = generateUPDATEcommand(FROM, SET, WHERE);
            SqliteCommand updateSQL = new SqliteCommand(sqlExpression, connection);
            updateSQL.ExecuteNonQuery();
            //connection.Close();
        }
        public void Close()
        {
            connection.Close();
        }

        ~DB()
        {
            //connection.Close();
        }
    }
}
