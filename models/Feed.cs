using System;

namespace BaoZiLinSCKGetFeedData.models
{
    class Feed
    {
        public string id { get; set; }

        public string created_time { get; set; }

        public string message { get; set; }

        public DateTime getCreateTime()
        {
            DateTime.TryParse(this.created_time, out DateTime result);
            return result;
        }

        public News ToNews()
        {
            return new News
            {
                id = id,
                content = message,
                sourceType = "facebook",
                sourceUrl = $"https://facebook.com/{id}"
            };
        }
    }
}
