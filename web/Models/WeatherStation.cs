namespace web.Models
{
    public class WeatherStation
    {
        public string? MeteoId { get; set; }
        public DateTime? Time { get; set; }
        public float? Temp { get; set; }
        public float? Hum { get; set; }

        public WeatherStation(){
            MeteoId = null;
        }
    }
}