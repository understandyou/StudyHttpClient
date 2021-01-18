/*
 *==============================================================================================
 * 时间：2020/12/26 14:40:10
 * 作者：zhiyangshuang
 * 说明：
 *==============================================================================================
 */
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Goods;
using Duo.System.Model.EnumModel.Goods;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Goods
{
    public class GoodsUnitControllerTest : BaseTest
    {
        public string id { get; set; }
        GoodsUnitAddDto addDto = new GoodsUnitAddDto()
        {
            Remark = "备注",
            Status = EnumGoodsUnit.Normal,
            UnitName = "这是一个单位名称"
        };

        [Test]
        [Order(1)]
        public void AddUnit()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsUnit/AddUnit";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Goods_Unit>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                id = apiResult.Data.ID;
            }).Wait();

        }
        [Test]
        [Order(2)]
        public void UpdateUnit()
        {
            Task.Run(async () =>
                {
                    url = "/api/GoodsUnit/UpdateUnit";
                    addDto.Remark += "改";
                    addDto.Sort = 1;
                    var updateDto = addDto.Adapt<GoodsUnitUpdateDto>();
                    updateDto.ID = id;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync<Goods_Unit>(url, content);
                    Assert.NotNull(apiResult.Data, apiResult.Message);
                }).Wait();
        }
        [Test]
        [Order(3)]
        public void Query()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsUnit/Query?unitId=" + id;
                var apiResult = await GetAsync<Goods_Unit>(url);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                Assert.NotNull(apiResult.Data.AddDateTime, apiResult.Message);
                Assert.NotNull(apiResult.Data.AddUserID, apiResult.Message);
            }).Wait();

        }
        [Test]
        [Order(4)]
        public void QueryPages()
        {
            Task.Run(async () =>
                {
                    url = "/api/GoodsUnit/QueryPages";
                    GoodsUnitPageDto pageDto = new GoodsUnitPageDto()
                    {
                        PageSize = 10,
                        PageIndex = 1
                    };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(pageDto));
                    var apiResult = await PostAsync<PagedInfo<Goods_Unit>>(url, content);
                    Assert.Greater(apiResult.Data.DataSource.Count, 0, apiResult.Message);
                }).Wait();
        }

        [Test]
        [Order(5)]
        public void DeleteUnit()
        {
            Task.Run(async () =>
            {
                //id = "F00DD7D0-0F9E-40C8-A6C2-C72D8E0494A8";
                url = "/api/GoodsUnit/DeleteUnit?unitId=" + id;
                var apiResult = await GetAsync<bool>(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }

    }
}
