namespace web.Models
{
    public class WeatherStation
    {
        public int MeteoId { get; set; }
        public DateTime? Time { get; set; }
        public double? Temp { get; set; }
        public double? Hum { get; set; }
    }
}