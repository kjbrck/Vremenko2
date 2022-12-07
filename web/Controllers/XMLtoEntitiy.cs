using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using System.Xml;
using System.Xml.XPath;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;
namespace web.Controllers;

public class XMLtoEntity : Controller
{
    public static void Read(string url)
    {
        using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
        {

            XPathDocument doc = new XPathDocument(url);
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression metadata; 
            metadata = nav.Compile("/data/metData");

            XPathNodeIterator iterator = nav.Select(metadata);

            ZapisVBazo zvp=new ZapisVBazo();

            try
            {
                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();

                    nav2.MoveToChild("domain_meteosiId", "");
                    zvp.meteoID=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("domain_shortTitle", "");
                    zvp.name=nav2.Value.ToUpper();
                    //zvp.name=zvp.name;
                    nav2.MoveToParent();

                    nav2.MoveToChild("domain_lat", "");
                    zvp.lat=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("domain_lon", "");
                    zvp.lon=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("domain_altitude", "");
                    zvp.alt=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("t", "");
                    zvp.temp=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("tsValid_issued_UTC", "");
                    zvp.time=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("rh", "");
                    zvp.hum=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("nn_icon-wwsyn_icon", "");
                    zvp.oblaki=nav2.Value;
                    nav2.MoveToParent();

                    nav2.MoveToChild("wwsyn_icon", "");
                    zvp.vPojav=nav2.Value;
                    nav2.MoveToParent();

                    
                    int isMeteoIDTrue;
                    string query = "SELECT COUNT(*) FROM [dbo].[postaja] WHERE meteosiID = '"+zvp.meteoID+"';";
                    
                    SqlDataReader x;

                    using(SqlCommand command = new SqlCommand(query, cn))
                    {
                        cn.Open();

                        x=command.ExecuteReader();
                        x.Read();
                        isMeteoIDTrue=x.GetInt32(0);
                        cn.Close();
                        
                    }
                    if(isMeteoIDTrue==0)//Če ne obstaja postaja -> dodaj postajo
                    {
                        query = "INSERT INTO [dbo].[postaja] VALUES ('"+zvp.meteoID+"',"+zvp.lon+","+zvp.lat+","+zvp.alt+",'"+zvp.name+"');";
                        Console.WriteLine(query);
                        using(SqlCommand command = new SqlCommand(query, cn))
                        {
                            cn.Open();
                            command.ExecuteReader();
                            cn.Close();
                        } 
                    }
                    
                    int isZapisVMeritvi;
                    string[] tempS= zvp.time.Split('.',' ',':');
                   
                    query = "SELECT COUNT(*) FROM [dbo].[meritev] WHERE name = '"+zvp.meteoID+"' AND date = CONVERT(DATETIME, '"+tempS[2]+"-"+tempS[1]+"-"+tempS[0]+" "+tempS[3]+":"+tempS[4]+":00', 120);";

                    using(SqlCommand command = new SqlCommand(query, cn))
                    {
                        cn.Open();
                        x=command.ExecuteReader();
                        x.Read();
                        isZapisVMeritvi=x.GetInt32(0);
                        cn.Close();
                        
                    }
                    //Console.WriteLine(zvp.meteoID+" "+isZapisVMeritvi);
                    if(isZapisVMeritvi==0)
                    {
                        /*
                        foreach(string tempX in tempS)
                        {
                            Console.WriteLine(tempX);
                        }
                        Console.WriteLine(zvp.meteoID);*/
                        if(zvp.hum==""){
                            zvp.hum="null";    
                        }
                        if(zvp.temp==""){
                            zvp.temp="null";    
                        }

                        query = "INSERT INTO [dbo].[meritev] VALUES(CONVERT(DATETIME, '"+tempS[2]+"-"+tempS[1]+"-"+tempS[0]+" "+tempS[3]+":"+tempS[4]+":00', 120),'"+zvp.meteoID+"',"+zvp.temp+","+zvp.hum+",'"+zvp.oblaki+"','"+zvp.vPojav+"');";
                        //Console.WriteLine(query);
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
}
