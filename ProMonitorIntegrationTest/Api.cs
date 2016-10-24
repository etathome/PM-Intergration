using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ProMonitorIntegrationTest
{
    public class Api
    {
        protected const string AuthType = "Bearer";
        public string AccessToken { get; set; }
        public string ClientKey { get; set; }
        public string Thumbprint { get; set; }
        public string ApiUsername { get; set; }
        public string ApiPassword { get; set; }
        public Uri ApiUrl { get; set; }
        public bool DisableSslCertValidation { get; set; }

        public Api() : this("") { }

        public Api(string accessToken)
        {
            AccessToken = accessToken;
            ApiUsername = ConfigurationManager.AppSettings["Api.Username"];
            ClientKey = ConfigurationManager.AppSettings["Api.ClientKey"];
            ApiPassword = ConfigurationManager.AppSettings["Api.Password"];
            ApiUrl = new Uri(ConfigurationManager.AppSettings["Api.Url"]);
            Thumbprint = ConfigurationManager.AppSettings["Api.Thumbprint"];
            DisableSslCertValidation = false;
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("Api.DisableSslCertValidation")) return;
            bool disableValidation;
            if (bool.TryParse(ConfigurationManager.AppSettings["Api.DisableSslCertValidation"], out disableValidation) && disableValidation)
            {
                DisableSslCertValidation = true;
            }
        }

        public T CallApiGet<T>(string actionUri)
        {
            T result;
            ManageSslCertificationValidation();
            using (var request = CreateHttpRequestMessage(actionUri, HttpMethod.Get)) {
                AddRequiredHeaders(request);
                using (var httpClient = new HttpClient()) {
                    using (var response = httpClient.SendAsync(request).Result) { 
                        if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception("Http status code: Not Found");
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsAsync<T>().Result;
                    }
                }
            }
            return result;
        }

        public T CallApiPost<T>(string actionUri, object content)
        {
            T result;
            ManageSslCertificationValidation();
            using (var request = CreateHttpRequestMessage(actionUri, HttpMethod.Post)) { 
                AddRequiredHeaders(request);
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient()) {
                    using (var response = httpClient.SendAsync(request).Result) {
                        Debug.WriteLine(JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result));
                        response.EnsureSuccessStatusCode();
                        result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            return result;
        }

        public string AquireToken()
        {
            ManageSslCertificationValidation();
            string authToken;
            using (var handler = new WebRequestHandler())
            {
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", ApiUsername),
                    new KeyValuePair<string, string>("password", ApiPassword)
                };
                using (var content = new FormUrlEncodedContent(postData)) {
                    using (var httpClient = new HttpClient(handler)) {
                        using (var response = httpClient.PostAsync(GetUrl("Token"), content).Result) {
                            Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
                            response.EnsureSuccessStatusCode();
                            dynamic responseContent = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                            authToken = Convert.ToString(responseContent.access_token);
                        }
                    }
                }
            }
            return authToken;
        }

        private void AddRequiredHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", string.Format("{0} {1}", AuthType, AccessToken));
            request.Headers.Add("ClientKey", ClientKey);
        }

        private void ManageSslCertificationValidation()
        {
            if (DisableSslCertValidation)
            {
                ServicePointManager
                    .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            }
        }

        protected HttpRequestMessage CreateHttpRequestMessage(string actionUri, HttpMethod method)
        {
            var address = new Uri(GetUrl(actionUri));
            var request = new HttpRequestMessage(method, address);
            var certs = new List<string> { Thumbprint };
            request.Headers.Add("Thumbprint", certs);
            return request;
        }

        protected string GetUrl(string actionUri)
        {
            return ApiUrl + actionUri;
        }
    }
}