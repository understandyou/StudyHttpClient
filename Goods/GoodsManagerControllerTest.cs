/*
 *==============================================================================================
 * 时间：2020/12/25 15:07:26
 * 作者：zhiyangshuang
 * 说明：
 *==============================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Duo.System.Common.Utilities;
using Duo.System.Model;
using Duo.System.Model.Dto.Goods;
using Duo.System.Model.EnumModel.Goods;
using Duo.System.Model.View.Goods;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Goods
{
    public class GoodsManagerControllerTest : BaseTest
    {
        /// <summary>
        /// 主商品
        /// </summary>
        public string zspid { get; set; }
        /// <summary>
        /// 组合商品
        /// </summary>
        public string zhspid { get; set; }

        public List<GoodsSpecificationVM> queryGuige { get; set; }
        private static Dictionary<string, decimal> sub
        {
            get
            {
                Dictionary<string, decimal> temp = new Dictionary<string, decimal>();
                temp.Add("6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2", 1);
                temp.Add("CEFECB96-3FD2-4E08-8AF1-836FE56F4707", 2);
                return temp;
            }
        }

        private static List<GoodsSpecificationAddDto> guige = new List<GoodsSpecificationAddDto>()
        {
            new GoodsSpecificationAddDto()
            {
                UnitContent="个",
                BarCode = "123",
                GoodsUnitID = "731C7E07-44D3-4381-8E48-8B9CEF0260DE",
                IsShop = EnumSpecificationIsShop.Yes,
                LineationPrice = 12,
                GoodsID = "",
                IsStore = EnumSpecificationIsStore.No,
                MaxPrice = 15,
                MinPrice = 5, Remark = "hhh", ShopPrice = 11, StorePrice = (decimal) 11.2
            }
        };

        //普通商品
        GoodsManagerAddDto ptdto = new GoodsManagerAddDto()
        {
            Detailed = "这是一个详情",
            EnglishName = "nameEnglish",
            GoodsBrandID = "",
            GoodsClassesID = "75B8D67C-A434-4CB4-A6EE-1254718077EF",
            GoodsImage = "https://www.baidu.com",
            SubImages = "https://www.baidu.com",
            GoodsName = "这是一个名字",
            GoodsNo = "123",
            GoodsType = EnumGoodsManagerType.Common,
            GoodsUnitID = "731C7E07-44D3-4381-8E48-8B9CEF0260DE",
            Remark = "",
            Status = EnumGoodsManagerStatus.Start,
            SpecificationAddDtos = guige
        };
        //组合
        GoodsGroupSubAddDto zhdto = new GoodsGroupSubAddDto()
        {
            Detailed = "这是一个详情",
            EnglishName = "EnglishName",
            GoodsBrandID = "2F146B49-5B43-4BAF-BBD3-8C15CA08CC15",
            GoodsClassesID = "75B8D67C-A434-4CB4-A6EE-1254718077EF",
            GoodsImage = "https://www.baidu.com",
            GoodsName = "组合名",
            GoodsNo = "111",
            SubImages = "www.baidu.com",
            GoodsSubs = sub,
            GoodsUnitID = "731C7E07-44D3-4381-8E48-8B9CEF0260DE",
            Remark = "备注"
        };
        [Test]
        [Order(1)]
        public void AddGoods()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsManager/AddGoods";
                StringContent content = new StringContent(JsonConvert.SerializeObject(ptdto));
                var apiResult = await PostAsync<Goods_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                zspid = apiResult.Data.ID;
            }).Wait();

        }

        [Test]
        [Order(2)]
        public void AddGroupSub()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsManager/AddGroupSub";
                var content = new StringContent(JsonConvert.SerializeObject(zhdto));
                var apiResult = await PostAsync<Goods_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                zhspid = apiResult.Data.ID;
            }).Wait();


        }

        [Test]
        [Order(3)]
        public void UpdateGroupSub()
        {
            Task.Run(async () =>
            {
                zhdto.GoodsName += "改";
                zhdto.Remark += "改";
                zhdto.GoodsNo += "666";
                var dto = zhdto.Adapt<GoodsGroupSubUpdateDto>();
                dto.ID = zhspid;
                //dto.ID = "7D5EBADC-C68A-4D5E-A434-B08098D53108";
                var content = new StringContent(JsonConvert.SerializeObject(dto));
                url = "/api/GoodsManager/UpdateGroupSub";

                var apiResult = await PostAsync<Goods_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);

            }).Wait();


        }
        [Test]
        [Order(4)]
        public void QueryGoods()
        {
            Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(zspid)) throw new Exception("ID空");
                url = "/api/GoodsManager/QueryGoods?goodsID=" + zspid;
                var apiResult = await GetAsync<GoodsManagerVM>(url);
                Assert.NotNull(apiResult.Data.GoodsSpecifications, apiResult.Message);
                queryGuige = apiResult.Data.GoodsSpecifications;

            }).Wait();

        }
        [Test]
        [Order(5)]
        public void UpdateGoods()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsManager/UpdateGoods";
                ptdto.GoodsName += "改";
                ptdto.Detailed += "改";
                var dto = ptdto.Adapt<GoodsManagerUpdateDto>();
                dto.ID = zspid;
                //dto.ID = "187B4A8C-00CD-49D6-9363-B156BED28A81";
                dto.SpecificationUpdateDtos = queryGuige.MapListTo<GoodsSpecificationVM, GoodsSpecificationUpdateDto>().ToList();

                //dto.SpecificationUpdateDtos.ForEach(o=>o.ID=);
                var content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Goods_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();

        }

        [Test]
        [Order(6)]
        public void GetPagesGroupSub()
        {
            Task.Run(async () =>
                {
                    url = "/api/GoodsManager/GetPagesGroupSub";
                    GoodsGroupSubPageDto dto = new GoodsGroupSubPageDto();
                    dto.PageSize = 10;
                    dto.PageIndex = 1;
                    var content = new StringContent(JsonConvert.SerializeObject(dto));
                    var apiResult = await PostAsync<PagedInfo<Goods_Manager>>(url, content);
                    Assert.Greater(apiResult.Data.DataSource.Count, 0, apiResult.Message);
                }).Wait();
        }

        [Test]
        [Order(7)]
        public void GetGroupSub()
        {
            Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(zhspid)) throw new Exception("ID空");
                url = "/api/GoodsManager/GetGroupSub?goodsId=" + zhspid;
                var apiResult = await GetAsync<GoodsGroupSubVM>(url);
                Assert.Greater(apiResult.Data.SubVms.Count, 0, apiResult.Message);
            }).Wait();
        }

        [Test]
        [Order(8)]
        public void QueryPages()
        {
            Task.Run(async () =>
                {
                    url = "/api/GoodsManager/QueryPages";
                    GoodsManagerPageDto pageDto = new GoodsManagerPageDto()
                    {
                        PageSize = 10,
                        PageIndex = 1
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(pageDto));
                    var apiResult = await PostAsync<PagedInfo<Goods_Manager>>(url, content);
                    Assert.AreEqual(200, apiResult.StatusCode, apiResult.Message);
                }).Wait();
        }
        /// <summary>
        /// 改价
        /// </summary>
        [Test]
        [Order(9)]
        public void UpdatePrice()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsUpdatePrice/AddUpdatePrice";
                GoodsUpdatePriceAddDto updatePrice = new GoodsUpdatePriceAddDto()
                {
                    CommoditySpecificationID = queryGuige[0].ID,
                    PriceType = EnumGoodsPriceType.ShopPrice,
                    Remark = "备注",
                    UpdateAfterPrice = 999
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(updatePrice));
                var apiResult = await PostAsync<Goods_UpdatePrice>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();
        }

        [Test]
        [Order(10)]
        public void DeleteGoods()
        {
            Task.Run(async () =>
            {
                url = "/api/GoodsManager/DeleteGoods?goodsId=" + zspid;
                var apiResult = await GetAsync<bool>(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }

    }
}
