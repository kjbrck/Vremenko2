using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using System.Xml;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace web.Controllers;

public class XMLtoEntity : Controller
{
    public static void Read(string url)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        //Object precision = configuration.GetValue<Object>("SchoolContext");
        //tring precision = configuration.GetValue<IList<ConnectionSettings>>("Connections");

        Console.WriteLine("++++");
        Console.WriteLine(configuration.ToString());

       // SqlConnection cn=new SqlConnection(precision.ToString());

        XmlReader xmlFile;
        string sql;

        xmlFile = XmlReader.Create(@url, new XmlReaderSettings());
        string meteoId;
        string time;
        string temp;
        string hum;
        //enum stanje(sončno, oblačno, dežuva, sneguva)
        while(xmlFile.Read())
        {
            switch(xmlFile.Name){
                case "domain_meteosiId":meteoId=xmlFile.ReadElementContentAsString();break;//v string meteoID flikn vse kar je tle + break;
                case "tsValid_issued":time=xmlFile.ReadElementContentAsString();break;
                case "t":temp=xmlFile.ReadElementContentAsString();break;
                case "rh":hum=xmlFile.ReadElementContentAsString();
                   /*if()//Če n obstaja postaja dodaj postajo
                    {


                    }
                    //insert v postajo
                */
                break;
            }
        }
        Console.ReadLine();
    }
}