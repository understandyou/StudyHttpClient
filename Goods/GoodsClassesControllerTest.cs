/*
 *==============================================================================================
 * 时间：2020/12/25 11:55:40
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
using Duo.System.Model.View.Goods;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Goods
{
    public class GoodsClassesControllerTest:BaseTest
    {
        public string  Id { get; set; }
        GoodsClassesAddDto dto = new GoodsClassesAddDto()
        {
            ClassesName = "这事一个分类",
            ParentID = ""
        };
        
        [Test]
        [Order(1)]
        public void AddGoodsClasses()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsClasses/AddGoodsClasses";
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
                var result = await client.PostAsync(url, content);
                var s = await result.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<Goods_Classes>>(s);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                Id = apiResult.Data.ID;
            }).Wait();

        }
        //[Test]
        //[Order(2)]
        //public void QueryRelation()
        //{
        //    url = "/api/GoodsClasses/QueryRelation";
        //    var apiResult = Get<List<GoodsClassesVM>>(url);
        //    Assert.NotZero(apiResult.Data.Count);
        //}
        //public void Querys()
        //{
        //}
        [Test]
        [Order(2)]
        public void UpdateGoodsClasses()
        {
            Task.Run(async () =>
            {
                var updateDto = dto.Adapt<GoodsClassesUpdateDto>();
                updateDto.ID = Id;
                updateDto.ClassesName += "改";
                StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                url = "/api/GoodsClasses/UpdateGoodsClasses";
                var apiResult = await PostAsync<Goods_Classes>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();

        }
        [Test]
        [Order(3)]
        public void DeleteGoodsClasses()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsClasses/DeleteGoodsClasses?id=" + Id;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();

        }

    }
}
