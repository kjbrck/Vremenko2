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
            /*
            XmlReader xmlFile;
            var settings = new XmlReaderSettings();
            settings.Async = true;
            xmlFile = XmlReader.Create(@url, settings);
            */

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
                    //Console.WriteLine(nav2.Value);
                    //listBox1.Items.Add("price: " + nav2.Value);

                    nav2.MoveToChild("domain_meteosiId", "");
                    zvp.meteoID=nav2.Value;
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
                    //Console.WriteLine(zvp.time.Substring(0,16));
                    if(isMeteoIDTrue==0)//Če ne obstaja postaja -> dodaj postajo
                    {
                        query = "INSERT INTO [dbo].[postaja] VALUES ('"+zvp.meteoID+"',"+zvp.lon+","+zvp.lat+","+zvp.alt+");";
                        using(SqlCommand command = new SqlCommand(query, cn))
                        {
                            cn.Open();
                            command.ExecuteReader();
                            cn.Close();
                        } 
                    }
                    /*
                    int isZapisVMeritvi;
                    query = "SELECT COUNT(*) FROM [dbo].[meritev] WHERE meteosiID = '"+zvp.meteoID+"';";
                    
                    SqlDataReader x;

                    using(SqlCommand command = new SqlCommand(query, cn))
                    {
                        cn.Open();
                        //Console.WriteLine(query);
                        x=command.ExecuteReader();
                        x.Read();
                        isMeteoIDTrue=x.GetInt32(0);
                        cn.Close();
                        
                    }*/


                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            /*
            string meteoId="";
            double lat, lon, alt;
            string time;
            double temp;
            double hum;
            Boolean isMeteoIDTrue;
            //enum stanje(sončno, oblačno, dežuva, sneguva)
            while(xmlFile.Read())
            {
                //Console.WriteLine("-"+xmlFile.Value+"-");
                Console.WriteLine(xmlFile.Name+"-"+xmlFile.Value+"-");
                switch(xmlFile.Name){
                    case "domain_meteosiId":meteoId=xmlFile.Value+"";break;//v string meteoID flikn vse kar je tle + break;
                    case "domain_lat":Console.WriteLine("-"+xmlFile.Value+"-");break;
                        //lat=Double.Parse(xmlFile.Value+"");break;
                    case "domain_lon":Console.WriteLine("-"+xmlFile.Value+"-");break;
                    case "domain_altitude":Console.WriteLine("-"+xmlFile.Value+"-");break;
                    case "tsValid_issued":Console.WriteLine("-"+xmlFile.Value+"-");break;
                    case "t":Console.WriteLine("-"+xmlFile.Value+"-");break;
                    case "rh":Console.WriteLine("-"+xmlFile.Value+"-");break;
                       /* 
                        //insert v postajo                   break;
                }
            }*/
        }
    }
}
