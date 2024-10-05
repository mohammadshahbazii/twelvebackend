using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class SendSms
    {
        async public static Task<string> Send(string phonenumber)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "P26rmd5BX9rKZCJeK0oGQAyGXP1j0T2zZv1Ad4BAQhlTbwUkeTftjl0hL6dvT2TB");
            var payload = @"{" + "\n" +
            @"  ""lineNumber"": 300089930364," + "\n" +
            @"  ""messageTexts"": [" + "\n" +
            @"    ""با سلام و احترام\nلینک دانلود سوپر اپلیکیشن دوازده به شرح ذیل می باشد\nhttps://dl.12application.ir/twelve.apk""" + "\n" +
            @"  ]," + "\n" +
            @"  ""mobiles"": [" + "\n" +
            @$"    ""{phonenumber}""" + "\n" +
            @"  ]," + "\n" +
            @"  ""sendDateTime"": null" + "\n" +
            @"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/likeToLike", content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "کاربر گرامی ! پیامک حاوی لینک دانلود به شماره تلفن شما ارسال شد";
            }
            else { return "کاربر گرامی ! هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید"; }
        }
        async public static Task<string> SendPassword(string phonenumber, string password)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "P26rmd5BX9rKZCJeK0oGQAyGXP1j0T2zZv1Ad4BAQhlTbwUkeTftjl0hL6dvT2TB");
            var payload = @"{" + "\n" +
            @"  ""lineNumber"": 300089930364," + "\n" +
            @"  ""messageTexts"": [" + "\n" +
            @$"    ""با سلام و احترام\nرمز عبور شما به شرح ذیل می باشد\n{password}""" + "\n" +
            @"  ]," + "\n" +
            @"  ""mobiles"": [" + "\n" +
            @$"    ""{phonenumber}""" + "\n" +
            @"  ]," + "\n" +
            @"  ""sendDateTime"": null" + "\n" +
            @"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/likeToLike", content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "پیامک حاوی رمز عبور به شماره تلفن شما ارسال شد";
            }
            else { return "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید"; }
        }
    }
}
