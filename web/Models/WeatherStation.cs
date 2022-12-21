namespace web.Models
{
    public class WeatherStation
    {
        public string MeteoId { get; set; }
        public DateTime Time { get; set; }
        public float? Temp { get; set; }
        public float? Hum { get; set; }
        public string? Name { get; set; }
        public string? Img { get; set; }
        public float? Long { get; set; }
        public float? Lat { get; set; }
        public float? Alt { get; set; }
    }
}