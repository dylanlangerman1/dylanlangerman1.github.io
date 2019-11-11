﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace accountmanager
{
	
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class AccountServices : System.Web.Services.WebService
	{
        [WebMethod]
        public List<String> HelperAPI(string url = "",string text = "")
        {
            List<String> Responses = new List<string>();
            
            string twitter = ParseTwitter(APICall(3, url, text));
            Responses.Add(twitter);
            System.Threading.Thread.Sleep(3000);

            string chatbot = ParseChatbot(APICall(4, url, text));
            Responses.Add(chatbot);

            //Sleep(3000) makes application wait 3 second to avoid a time out for API calls
            System.Threading.Thread.Sleep(3000);
            
            string facebook = ParseFacebook(APICall(5, url, text));
            Responses.Add(facebook);
            
            string abbreviated = ParseAbbreviated(APICall(6, url, text));
            Responses.Add(abbreviated);
            return Responses;

        }
        [WebMethod]
        public string hash(string text)
        {
            var client = new RestClient("https://private-ceeab-ritekit.apiary-proxy.com/v1/stats/auto-hashtag?client_id=60f74b84a32620ad29eb0bcef4580617bc91a1560fb2&post="+text+"&maxHashtags=3&hashtagPosition=end");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "post=Is%20artificial%20intelligence%20the%20future%20of%20customer%20service%", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;

        }
        public string QuickParse(string text)
        {
            return text.Replace("[...]", "");
        }
       

        public string ParseTwitter(string text)
        {
            text = hash(text);
            JObject json = JObject.Parse(text);
            text = json.GetValue("post").ToString();
            
            string[] greetings = { "Hello, Twitter follower.","Hi,","Real talk,","Greetings,"
            ,"What's good!"};
            Random rnd = new Random();
            string greeting = greetings[rnd.Next(0, greetings.Count())];
            text = QuickParse(text);
            text = greeting + text;
            return text;
        }
        public string ParseChatbot(string text)
        {
            text = QuickParse(text);
            return text;
        }
        public string ParseFacebook(string text)
        {
            text = QuickParse(text);
            return text;
        }
        public string ParseAbbreviated(string text)
        {
            text = QuickParse(text);
            return text;
        }
        public string APICall(int sentenceNum, string url = "",string text = "")
        {
            var client = new RestClient("https://api.meaningcloud.com/summarization-1.0");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "key=980a54fdf7f02c37902b078872ce6eb0&sentences=" + sentenceNum.ToString()+"&txt="+text+"&url="+url+"&doc=", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);
            return json.GetValue("summary").ToString();

        }
	}
}
