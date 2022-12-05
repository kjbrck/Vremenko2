using System.Data.SqlClient;
using web.Models;

namespace web.Services.GetDataService;

public class GetDataService{

    public static int NumOfStations = 105;
    public static WeatherStation[] ws = new WeatherStation[NumOfStations];
    public static WeatherStation[] getWeatherStationData(){
        using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
        {
        string query = "SELECT * FROM [dbo].[postaja];";
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
                    ws[st].Temp = reader.GetFloat(1);
                    ws[st].Hum = reader.GetFloat(2);
                    Console.WriteLine("{0}\t{1}\t{2}", ws[st].MeteoId, ws[st].Temp, ws[st].Hum);
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