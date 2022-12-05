using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using System.Xml;
using System.Xml.XPath;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using web.Services.GetDataService;

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

            XPathExpression metadata, lat, lon, alt,time,temp,hum; 
            metadata = nav.Compile("/data/metData");
            XPathNodeIterator iterator = nav.Select(metadata);
            //listBox1.Items.Clear();
            try
            {
                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    Console.WriteLine(nav2.Value);
                    //listBox1.Items.Add("price: " + nav2.Value);
                }
                GetDataService.getWeatherStationData();
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
                       /* string query = "SELECT CASE WHEN meteosiID = '@meteoId' THEN 1 ELSE 0 END FROM [dbo].[postaja];";
                        using(SqlCommand command = new SqlCommand(query, cn))
                        {
                            command.Parameters.AddWithValue("@meteoId",meteoId);
                            cn.Open();
                            isMeteoIDTrue=(Boolean)command.ExecuteScalar();
                            
                        }
                        Console.WriteLine(meteoId+":"+isMeteoIDTrue);
                        /*if(!isMeteoIDTrue)//Če n obstaja postaja dodaj postajo
                        {
                            

                        }
                        //insert v postajo                   break;
                }
            }*/
        }
    }
}
