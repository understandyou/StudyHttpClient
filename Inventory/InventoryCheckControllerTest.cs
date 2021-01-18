/*
 *==============================================================================================
 * 时间：2020/12/29 15:52:34
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
using Duo.System.Model.Dto.Inventory;
using Duo.System.Model.EnumModel.Inventory;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Inventory
{
    public class InventoryCheckControllerTest : BaseTest
    {
        private Inventory_Check check;
        InventoryCheckAddDto addDto = new InventoryCheckAddDto()
        {
            Cause = EnumCause.DayCause,
            DifferencePrice = 2,
            InventoryCheckDetailDtos = new List<InventoryCheckDetailDto>()
            {
                new InventoryCheckDetailDto()
                {
                    CheckNumber = 2,DifferencePrice = (decimal) 1.2,DifferenceNumber = 2,GoodsID = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2"
                    ,GoodsUnit = "斤",InventoryAvg = 1,Remark = "",ShopName = "苹果",SysNumber =3,
                }
            },
            Remark = "",
            Status = EnumInventoryStatus.NoAudited
        };
        [Test]
        [Order(1)]
        public void AddInventoryCheck()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryCheck/AddInventoryCheck";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Inventory_Check>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message + apiResult.ReturnMessage);
                check = apiResult.Data;
            }).Wait();

        }

        [Test]
        [Order(2)]
        public void UpdateInventoryCheck()
        {
            Task.Run(async () =>
                {
                    url = "/api/InventoryCheck/UpdateInventoryCheck";
                    var updateDto = check.Adapt<InventoryCheckUpdateDto>();
                    updateDto.InventoryCheckDetailDtos = addDto.InventoryCheckDetailDtos;
                    updateDto.SysNumber = 2;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync<Inventory_Check>(url, content);
                    Assert.NotNull(apiResult.Data, apiResult.Message + apiResult.ReturnMessage);
                }).Wait();

        }

        [Test]
        [Order(3)]
        public void Audit()
        {
            Task.Run(async () =>
            {
                url = $"/api/InventoryCheck/Audit?checkId={check.ID}&status=1";
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }
        //成本变更
        [Test]
        [Order(4)]
        public void ChangeTheCost()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryChangeTheCost/ChangeTheCost";
                ChangeTheCostDto changeDto = new ChangeTheCostDto()
                {
                    AfterPrice = 10,
                    GoodsID = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2"
                    ,
                    GoodsName = "苹果",
                    GoodsUnit = "斤",
                    Reason = EnumChangeTheCostReason.DayChange
                    ,
                    Remark = "自动测试"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(changeDto));
                var apiResult = await PostAsync<Inventory_ChangeTheCost>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();

        }

    }
}
