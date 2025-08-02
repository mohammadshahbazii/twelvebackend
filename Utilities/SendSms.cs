using System.Text;
using System.Text.Json;
using System.Resources;
using System.Threading.Tasks;

namespace Utilities
{
    public static class SendSms
    {
        private static readonly ResourceManager _resources =
            new("Utilities.Resources.SendSms", typeof(SendSms).Assembly);

        public static async Task<string> Send(string phonenumber)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key",
                "P26rmd5BX9rKZCJeK0oGQAyGXP1j0T2zZv1Ad4BAQhlTbwUkeTftjl0hL6dvT2TB");

            var message = _resources.GetString("DownloadSmsText") ??
                          "با سلام و احترام\nلینک دانلود سوپر اپلیکیشن دوازده به شرح ذیل می باشد\nhttps://dl.12application.ir/twelve.apk";

            var body = new
            {
                lineNumber = 300089930364,
                messageTexts = new[] { message },
                mobiles = new[] { phonenumber },
                sendDateTime = (string?)null
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(body),
                Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/likeToLike", content);

            return response.IsSuccessStatusCode
                ? _resources.GetString("DownloadSmsSent") ??
                  "کاربر گرامی ! پیامک حاوی لینک دانلود به شماره تلفن شما ارسال شد"
                : _resources.GetString("SmsError") ??
                  "کاربر گرامی ! هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید";
        }

        public static async Task<string> SendPassword(string phonenumber, string password)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key",
                "P26rmd5BX9rKZCJeK0oGQAyGXP1j0T2zZv1Ad4BAQhlTbwUkeTftjl0hL6dvT2TB");

            var template = _resources.GetString("PasswordSmsText") ??
                           "با سلام و احترام\nرمز عبور شما به شرح ذیل می باشد\n{0}";
            var message = string.Format(template, password);

            var body = new
            {
                lineNumber = 300089930364,
                messageTexts = new[] { message },
                mobiles = new[] { phonenumber },
                sendDateTime = (string?)null
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(body),
                Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/likeToLike", content);

            return response.IsSuccessStatusCode
                ? _resources.GetString("PasswordSmsSent") ??
                  "پیامک حاوی رمز عبور به شماره تلفن شما ارسال شد"
                : _resources.GetString("SmsError") ??
                  "کاربر گرامی ! هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید";
        }
    }
}
