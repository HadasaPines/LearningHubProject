using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BL.service
{
    /// <summary>
    /// מנהל תורים זמינים – כולל בדיקה האם תאריך נתון הוא שבת או חג בלוח השנה העברי.
    /// משתמש ב-API של hebcal.com
    /// </summary>
    public class AvailableQueueManager
    {
        private static AvailableQueueManager _instance;
        public static AvailableQueueManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AvailableQueueManager();
                return _instance;
            }
        }

        private static readonly string _baseUrl = "https://www.hebcal.com/hebcal";

        public AvailableQueueManager() { }

        /// <summary>
        /// בודק האם תאריך נתון הוא שבת או חג (לפי API של hebcal)
        /// </summary>
        /// <param name="date">תאריך לבדיקה</param>
        /// <returns>אמת אם מדובר בשבת או חג עברי</returns>
        public async Task<bool> IsHolidayOrShabbatAsync(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return true;

            string year = date.Year.ToString();
            string month = date.Month.ToString("D2");

            var url = $"{_baseUrl}?v=1&cfg=json&year={year}&month={month}&maj=on&min=off&mod=on&nx=off";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"API Error: {response.StatusCode}");

                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);
                var items = json["items"];

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var holidayDate = DateTime.Parse(item["date"]?.ToString() ?? "");

                        if (holidayDate.Date == date.Date)
                        {
                            var subcat = item["subcat"]?.ToString()?.ToLower();
                            var title = item["title"]?.ToString();

                            if (subcat == "modern" && title != "Yom HaAtzma'ut")
                                return false;

                            return true; 
                        }
                    }
                }

                return false; 
            }
        }
    }
}
