namespace web.Models
{
    public class WeatherStation
    {
        public string MeteoId { get; set; }
        public DateTime? Time { get; set; }
        public double? Temp { get; set; }
        public double? Hum { get; set; }
    }
}