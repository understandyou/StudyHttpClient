
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Duo.System.Hostd;
using Duo.System.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication
{
    public class BaseTest
    {
        public WebApplicationFactory<Duo.System.Hostd.Startup> app { get; set; }
        public HttpClient client { get; set; }
        public string Token { get; set; }
        public string url { get; set; }
        public BaseTest()
        {
            app = new WebApplicationFactory<Startup>();
            client = app.CreateClient();
        }
        [OneTimeSetUp]
        public async Task setToken()//获取权限
        {
            var stringContent = new StringContent("{\"userName\":\"9999\",\"passWord\":\"123456\"}");
            //参数格式
            stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");

            var postAsync = await client.PostAsync("/api/Auth/LoginMiniProgram", stringContent);
            string read = await postAsync.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult<string>>(read);
            Assert.AreEqual(apiResult.StatusCode, 200);//非200，error
            Token = apiResult.Data;
            //token
            client.DefaultRequestHeaders.Add("SYSTOKEN", Token);
        }
        [TearDown]
        public void ClearUrl()//清理url
        {
            url = string.Empty;
        }
        [Obsolete("使用异步方法")]
        public ApiResult<T> Get<T>(string url)
        {
            var result = client.GetAsync(url).Result;
            var s = result.Content.ReadAsStringAsync().Result;
            var apiResult = JsonConvert.DeserializeObject<ApiResult<T>>(s);
            return apiResult;
        }
        [Obsolete("使用异步方法")]
        public ApiResult Get(string url)
        {
            var result = client.GetAsync(url).Result;
            var s = result.Content.ReadAsStringAsync().Result;
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(s);
            return apiResult;
        }

        public async Task<ApiResult<T>> GetAsync<T>(string url)
        {
            var result = await client.GetAsync(url);
            var s = await result.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult<T>>(s);
            return apiResult;
        }
        public async Task<ApiResult> GetAsync(string url)
        {
            var result = await client.GetAsync(url);
            var s =await result.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(s);
            return apiResult;
        }
        [Obsolete("使用异步方法")]
        public ApiResult<T> Post<T>(string url,StringContent content)
        {
            content.Headers.ContentType= MediaTypeHeaderValue.Parse("application/json-patch+json");
            var result = client.PostAsync(url, content).Result;
            var s = result.Content.ReadAsStringAsync().Result;
            var apiResult = JsonConvert.DeserializeObject<ApiResult<T>>(s);
            return apiResult;
        }
        [Obsolete("使用异步方法")]
        public ApiResult Post(string url, StringContent content)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
            var result = client.PostAsync(url, content).Result;
            var s = result.Content.ReadAsStringAsync().Result;
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(s);
            return apiResult;
        }
        public async Task<ApiResult<T>> PostAsync<T>(string url, StringContent content)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
            var result = await client.PostAsync(url, content);
            var s = result.Content.ReadAsStringAsync().Result;
            var apiResult = JsonConvert.DeserializeObject<ApiResult<T>>(s);
            return apiResult;
        }
        public async Task<ApiResult> PostAsync(string url, StringContent content)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
            var result = await client.PostAsync(url, content);
            var s = await result.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(s);
            return apiResult;
        }

    }
}
