using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal static class Tools
    {
        private static SqlConnection _Connection;
        internal static SqlConnection Connection()
        {
            if (_Connection == null)
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                Debug.Print(baseDir);
                int index = baseDir.IndexOf("WebApi");
                if (index == -1)
                      index = baseDir.IndexOf("UnitTestProject1");
                string dataDir = baseDir.Substring(0, index) + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
                String connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\SunHotels.mdf;Integrated Security=True";
                _Connection = new SqlConnection(connString);
            }
            return _Connection;
        }

        internal static void OpenConnection()
        {
            Connection().Open();
        }
        internal static void CloseConnection()
        {
            Connection().Close();
        }

        internal static string GetPagedExpression(string tableName ,string columnsName ,int requestedPage, int pageSize, string orderByFieldName, bool orderByDescending)
        {
            var sqlResult = new System.Text.StringBuilder();
            sqlResult.AppendLine("; WITH Results_Paged AS");
            sqlResult.AppendLine("      (SELECT ");
            sqlResult.AppendLine(columnsName);
            if (!string.IsNullOrEmpty(orderByFieldName))
                sqlResult.AppendLine(",ROW_NUMBER() OVER(ORDER BY  "+ orderByFieldName + " " + ((orderByDescending)? "DESC":"ASC") + ") AS RowNum");
            else
                sqlResult.AppendLine(",ROW_NUMBER() OVER(ORDER BY  "+ columnsName + " " + ((orderByDescending) ? "DESC" : "ASC") + ") AS RowNum");
            sqlResult.AppendLine("  FROM " + tableName+")");
            sqlResult.AppendLine(" SELECT * FROM Results_Paged ");
            sqlResult.AppendFormat(" WHERE RowNum > {0} AND RowNum <= {1} ", (requestedPage - 1) * pageSize, (requestedPage - 1)*pageSize+pageSize);

            return sqlResult.ToString();

            //ORDER BY EXPRESSION FROM PAGED QUERY
            //var order = "";
            //if (!string.IsNullOrEmpty(orderByFieldName))
            //    order = " ORDER BY " + orderByFieldName + " " + (orderByDescending ? " DESC" : " ASC");

            ////OFFSET EXPRESSION FROM PAGED QUERY
            //var offSet = " OFFSET (" + (requestedPage - 1) + " * " + pageSize + ")";

            ////FETCH EXPRESSION FROM PAGED QUERY
            //var fetch = " ROWS FETCH NEXT " + pageSize + " ROWS ONLY;";

            //return order + offSet + fetch;

            //  ; WITH Results_Paged AS
            // (
            //     SELECT

            //         Id, Name, Description,
            //         ROW_NUMBER() OVER(ORDER BY  Name, Description) AS RowNum

            //     FROM Product
            // )
            //SELECT * FROM Results_Paged
            //WHERE RowNum >= 4 AND RowNum < 4 + 3
        }
    }
}
