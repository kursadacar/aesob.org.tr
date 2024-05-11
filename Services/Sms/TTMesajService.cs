using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aesob.org.tr.Services.Sms
{
    public static class TTMesajService
    {
        private static string _tokenUsername;
        private static string _tokenPassword;
        private static string _username;
        private static string _password;
        private static string _alias;
        
        public static void Initialize(string tokenUsername, string tokenPassword, string username, string password, string alias)
        {
            _tokenUsername = tokenUsername;
            _tokenPassword = tokenPassword;
            _username = username;
            _password = password;
            _alias = alias;
        }

        private struct AccessTokenInfo
        {
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public int ExpiresIn { get; set; }
        }

        public static async Task<ServiceActionResult> SendSms(SMSObject sms)
        {
            var client = new RestClient("https://restapi.ttmesaj.com");

            try
            {
                var token = await GetAccessToken(client);

                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    return ServiceActionResult.CreateFail("Failed authentication");
                }

                var response = await SendOneToN(client, token, sms);

                client.Dispose();

                if (response.IsSuccessStatusCode)
                {
                    return ServiceActionResult.CreateSuccess(response.Content);
                }

                return ServiceActionResult.CreateFail(response.Content);
            }
            catch (Exception e)
            {
                return ServiceActionResult.CreateFail(e.ToString());
            }
        }

        private static async Task<AccessTokenInfo> GetAccessToken(RestClient client)
        {
            var request = new RestRequest("/api/Login/TokenJson");

            var postData = new
            {
                grant_type = "password",
                username = _tokenUsername,
                password = _tokenPassword,
            };

            request.AddJsonBody(postData);

            RestResponse response = await client.ExecutePostAsync(request);

            var deserializedMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);

            int expriesIn = 0;
            int.TryParse(deserializedMessage["expires_in"], out expriesIn);

            return new AccessTokenInfo()
            {
                AccessToken = deserializedMessage["access_token"],
                TokenType = deserializedMessage["token_type"],
                ExpiresIn = expriesIn
            };
        }

        private static async Task<RestResponse> SendOneToN(RestClient client, AccessTokenInfo token, SMSObject sms)
        {
            var request = new RestRequest("/api/SendSms/SendOneToN");

            var postData = new
            {
                username = _username,
                password = _password,
                XML = GetOneToNXml(sms),
                isNotification = (bool?)null,
                recipentType = "",
                brandCode = "",
            };

            request.AddHeader("Authorization", string.Format("Bearer {0}", token.AccessToken));
            request.AddJsonBody(postData);

            var response = await client.ExecutePostAsync(request);

            return response;
        }

        private static string GetOneToNXml(SMSObject sms)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<SINGLE_SMS>");
            sb.AppendLine($"<ORIGIN>{_alias}</ORIGIN>");
            sb.AppendLine($"<SEND_DATE>{sms.BeginDate.ToString("yyyyMMddHHmm")}</SEND_DATE>");
            sb.AppendLine($"<END_DATE>0</END_DATE>");
            sb.AppendLine($"<MESSAGE>{sms.Body}</MESSAGE>");

            var formattedNumbers = SMSHelper.FormatAllNumbersForSMS(sms.Numbers);

            for(int i = 0; i < formattedNumbers.Count; i++)
            {
                sb.AppendLine("<SMS>");
                    sb.AppendLine($"<GSMNO>{formattedNumbers[i]}</GSMNO>");
                sb.AppendLine("</SMS>");
            }
            sb.AppendLine("</SINGLE_SMS>");

            //$"<SINGLE_SMS>" +
            //    $"<ORIGIN>Mesaj başlık bilgisi</ORIGIN>" +
            //    $"SEND_DATE> Mesaj gönderim zamanı. Hemen gönderilmek istenen mesajlar için boş(“ ”) veya" +
            //    $"sıfır(0) olmalıdır. İleri tarihli gönderim yapmak istiyorsanız sonlanma zamanı olarak minimum 1 saat" +
            //    $"sonrası olarak belirlenmeli yyyyMMddHHmm formatında olmalıdır." +
            //    $"</SEND_DATE>" +
            //    $"<END_DATE> Mesajın son teslim zamanı. Herhangi bir zaman belirtilmek istenmediğinde default" +
            //    $"olarak 72 saat sonrası olarak belirlenir. Zaman belirtilmek istenmediğinde boş(“ ”) veya sıfır(0)" +
            //    $"olmalıdır." +
            //    $"</END_DATE>" +
            //    $"<MESSAGE> Gönderilmek istenen mesaj metni </MESSAGE>" +
            //    $"<SMS>" +
            //    $"<GSMNO> Gönderim yapmak istenen numara. Yurtdışı numaraları 00 ile başlamalı ve en" +
            //    $"az 12 karakter olmalıdır. Türkiye için 90 ile (0090 kabul edilmez) başlamalı ve minimum 12" +
            //    $"hane olmalıdır." +
            //    $"</GSMNO>" +
            //    $"</SMS>" +
            //    $"<SMS>" +
            //    $"<GSMNO> Gönderim yapmak istenen numara. Yurtdışı numaraları 00 ile başlamalı ve en" +
            //    $"az 12 karakter olmalıdır. Türkiye için 90 ile (0090 kabul edilmez) başlamalı ve minimum 12" +
            //    $"hane olmalıdır." +
            //    $"</GSMNO>" +
            //    $"</SMS>" +
            //$"</SINGLE_SMS>";

            return sb.ToString();
        }
    }
}
