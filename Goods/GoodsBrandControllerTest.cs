/*
 *==============================================================================================
 * 时间：2020/12/24
 * 作者：zhiyangshuang
 * 说明：
 *==============================================================================================
 */
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Goods;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Goods
{
    public class GoodsBrandControllerTest:BaseTest
    {
        private string id;
        GoodsBrandAddDto dto = new GoodsBrandAddDto()
        {
            BrandName = "testAdd",
            BrandNo = "testNo",
            Remark = "备注"
        };
        [Test]
        [Order(1)]
        public void AddBrand()//添加品牌
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsBrand/AddBrand";
                var content = new StringContent(JsonConvert.SerializeObject(dto));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                var postAsync = await client.PostAsync(url, content);
                var stringAsync = await postAsync.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<Goods_Brand>>(stringAsync);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();

        }
        [Test]
        [Order(2)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsBrand/QueryPages";
                GoodsBrandPageDto dto = new GoodsBrandPageDto()
                {
                    PageSize = 10,
                    PageIndex = 1
                };
                var content = new StringContent(JsonConvert.SerializeObject(dto));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                var postAsync = await client.PostAsync(url, content);
                var stringAsync = await postAsync.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<PagedInfo<Goods_Brand>>>(stringAsync);
                Assert.Greater(apiResult.Data.DataSource.Count, 0);
                id = apiResult.Data.DataSource[0].ID;
            }).Wait();

        }
        [Test]
        [Order(3)]
        public void UpdateBrand()
        {
            Task.Run(async() =>
            {
                url = "/api/GoodsBrand/UpdateBrand";
                dto.Remark = "";
                dto.BrandName += "改";
                dto.BrandNo += "gai";
                var adapt = dto.Adapt<GoodsBrandUpdateDto>();
                if (string.IsNullOrWhiteSpace(id)) throw new Exception("ID为空");
                adapt.ID = id;
                var content = new StringContent(JsonConvert.SerializeObject(adapt));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                var postAsync = await client.PostAsync(url, content);
                var stringAsync = await postAsync.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<Goods_Brand>>(stringAsync);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();

        }

        [Test]
        [Order(4)]
        public void Query()
        {
            Task.Run(async () =>
            {
                //id = "345E8EA1-9BD1-4586-A972-303311FD5A86";
                url = "/api/GoodsBrand/Query?brandId=" + id;
                var content = new StringContent("{\"brandId\":\"" + id + "\"}");
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                var getAsync = await client.GetAsync(url);
                var stringAsync = await getAsync.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<Goods_Brand>>(stringAsync);
                Assert.AreEqual(apiResult.Data.ID, id);
            }).Wait();

        }

        [Test]
        [Order(5)]
        public void DeleteBrand()
        {
            Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(id)) throw new Exception("ID为空");
                url = "/api/GoodsBrand/DeleteBrand?brandId=" + id;
                var getAsync = await client.GetAsync(url);
                var stringAsync = await getAsync.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult>(stringAsync);
                Assert.AreEqual(apiResult.StatusCode, 200);
            }).Wait();

        }
    }
}
