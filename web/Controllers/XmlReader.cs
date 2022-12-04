using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using System.Xml;

using System;
using System.Data;
using System.Data.SqlClient;


namespace web.Controllers;

public class XMLtoEntity : Controller
{
    public static void Read(string url)
    {

        XmlReader xmlFile;
        string sql;

        xmlFile = XmlReader.Create(@url, new XmlReaderSettings());
        string meteoId;
        string time;
        string temp;
        string hum;
        while(xmlFile.Read())
        {
            switch(xmlFile.Name){
                case "domain_meteosiId":
                case "tsValid_issued":
                case "t":
                case "rh": Console.WriteLine(xmlFile.ReadElementContentAsString());break;
            }
        }
        Console.ReadLine();
    }
}