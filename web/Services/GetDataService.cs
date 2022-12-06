using System.Data.SqlClient;
using web.Models;

namespace web.Services.GetDataService;

public class GetDataService{

    public static int NumOfStations = 1000;
    public static WeatherStation[] ws = new WeatherStation[NumOfStations];
    public static WeatherStation[] getWeatherStationData(){
        using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
        {
        string query = "select m.name, m.date, m.temp, m.hum from [dbo].[meritev] m inner join (select name, max(date) as MaxDate from [dbo].[meritev] group by name) tm on m.name = tm.name and m.date = tm.MaxDate order by name asc;";
        using(SqlCommand command = new SqlCommand(query, cn))
        {
            cn.Open();
            //moj dodatek :hehe:
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                int st = 0;
                while (reader.Read())
                {
                    
                    ws[st] = new WeatherStation();
                    ws[st].MeteoId = reader.GetString(0);
                    ws[st].Time = reader.GetDateTime(1);
                    ws[st].Temp = reader.GetFloat(2);
                    ws[st].Hum = reader.GetFloat(3);
                    //Console.WriteLine("{0}\t{1}\t{2}", ws[st].MeteoId, ws[st].Temp, ws[st].Hum);
                    st++;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

        }
        cn.Close();
        }
    return ws;
    }
}