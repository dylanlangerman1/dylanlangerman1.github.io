using System;
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
            string APIresponse = APICall(3,url,text);
            JObject json = JObject.Parse(APIresponse);
            string twitter = ParseTwitter(json.GetValue("summary").ToString());
            Responses.Add(twitter);
            System.Threading.Thread.Sleep(3000);
            APIresponse = APICall(5,url, text);
            json = JObject.Parse(APIresponse);
            string chatbot = ParseChatbot(json.GetValue("summary").ToString());
            Responses.Add(chatbot);

            //Sleep(3000) makes application wait 3 second to avoid a time out for API calls
            System.Threading.Thread.Sleep(3000);
            APIresponse = APICall(4, url, text);
            json = JObject.Parse(APIresponse);
            string facebook = ParseFacebook(json.GetValue("summary").ToString());
            Responses.Add(facebook);
            APIresponse = APICall(3, url, text);
            json = JObject.Parse(APIresponse);
            string abbreviated = ParseAbbreviated(json.GetValue("summary").ToString());
            Responses.Add(abbreviated);
            return Responses;

        }
        [WebMethod]
        public string hash()
        {
            var client = new RestClient("https://api.ritekit.com/v1/stats/multiple-hashtags");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "client_id=0559d8e4ffcecd87c6e9098e2cae536b896471dfbd92&post=Is%20artificial%20intelligence%20the%20future%20of%20customer%20service%3F&maxHashtags=2&hashtagPosition=auto", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;

        }
        public string QuickParse(string text)
        {
            return text.Replace("[...]", "");
        }
       

        public string ParseTwitter(string text)
        {
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
            return response.Content;
            
        }



	}
}
