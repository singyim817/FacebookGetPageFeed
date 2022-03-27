using BaoZiLinSCKGetFeedData.models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace BaoZiLinSCKGetFeedData
{
    class Program
    {
        Settings settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build().GetRequiredSection("Settings").Get<Settings>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string accessToken = new Program().GetAccessToken();
            if(accessToken != null)
            {
                new Program().GetFeeds(accessToken);
            }
        }

        public string GetAccessToken()
        {
            //client_id, client_secret, grant_type
            string url = settings.graphApiUrl + "/oauth/access_token", clientId = settings.clientId, clientSecret = settings.clientSecret, grantType = "client_credentials";

            using (HttpClient client = new HttpClient())
            {
                var query = new Dictionary<string, string>()
                {
                    ["client_id"] = clientId,
                    ["client_secret"] = clientSecret,
                    ["grant_type"] = grantType
                };
                var uri = QueryHelpers.AddQueryString(url, query);
                var response = client.GetAsync(uri).Result;
                if(response.IsSuccessStatusCode)
                {
                    string accessToken = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("access token: " + accessToken);
                    return accessToken;
                }
                else
                {
                    return null;
                }
            }
        }

        public void GetFeeds(string accessToken)
        {
            string jsonData = GetFacebookFeedData(accessToken);
            Console.WriteLine("jsonData: " + jsonData);
            if (jsonData != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
                try
                {
                    var dataListObject = jsonObject["data"];
                    List<Feed> dataList = JsonConvert.DeserializeObject<List<Feed>>(JsonConvert.SerializeObject(dataListObject));

                    List<News> newsList = new List<News>();
                    foreach (Feed feed in dataList)
                    {
                        newsList.Add(feed.ToNews());
                    }
                    string toJson = JsonConvert.SerializeObject(newsList, Formatting.Indented);
                    File.WriteAllText(settings.newsFileLocation, toJson, Encoding.UTF8);
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public string GetFacebookFeedData(string accessToken)
        {
            //client_id, client_secret, grant_type
            string url = settings.graphApiUrl + "/185480044864688/feed";

            using HttpClient client = new HttpClient();
            var query = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
            };
            var uri = QueryHelpers.AddQueryString(url, query);
            var response = client.GetAsync(uri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }


    }
}
