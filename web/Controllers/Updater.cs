using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using System.Xml;
using System.Xml.XPath;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;
namespace web.Controllers;

public class Updater : Controller
{

    public static String[] PridobiVseIzbrane(string userID)
    {
        try{
            using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
            {

                SqlDataReader x;

                string query = "SELECT meteosiID FROM [dbo].[izbor] WHERE userID='"+userID+"';";

                using(SqlCommand command = new SqlCommand(query, cn))
                {
                    cn.Open();
                    x=command.ExecuteReader();
                    x.Read();
                    cn.Close();
                }

                List<String> list = new List<String>();

                while(x.Read())
                {
                    list.Add((String)x.GetSqlString(45));
                }
                return list.ToArray();
        }
        }
        catch(Exception e){
            Console.WriteLine(e.StackTrace);
            return null;
        }
    }
    public static void UpdateIzbira(string userID, string[] meteosiID)
    {
        try{
            using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
            {
                foreach(string x in meteosiID){

                    string query = "INSERT INTO [dbo].[izbor] VALUES('"+x+"','"+userID+"');";

                    using(SqlCommand command = new SqlCommand(query, cn))
                    {
                        cn.Open();
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
        }
        catch(Exception e){
            Console.WriteLine(e.StackTrace);
        }
    }
}