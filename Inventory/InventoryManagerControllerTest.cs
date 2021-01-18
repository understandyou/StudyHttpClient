/*
 *==============================================================================================
 * 时间：2020/12/26 17:10:21
 * 作者：zhiyangshuang
 * 说明：
 *==============================================================================================
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Inventory;
using Duo.System.Model.EnumModel.Inventory;
using Duo.System.Model.View.Inventory;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Inventory
{
    public class InventoryManagerControllerTest : BaseTest
    {
        /// <summary>
        /// 库存的ID
        /// </summary>
        public string idInvent { get; set; }
        static string goodsId = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2";
        public string bsId { get; set; }
        InventoryManagerAddDto inventoryAdd = new InventoryManagerAddDto()
        {
            Details = new List<InventoryDetail>()
            {
                new InventoryDetail()
                {
                    GoodsID = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2",
                    GoodsUnitID = "731C7E07-44D3-4381-8E48-8B9CEF0260DE",
                    InventoryType = EnumInventoryType.InLibrary,
                    PutNumber = 10,
                    PutPrice = (decimal) 9.9,
                    Remark = "入库操作",
                    GoodsName = "香蕉",
                    UnitName = "斤"
                },
                new InventoryDetail()
                {
                    GoodsID = "CEFECB96-3FD2-4E08-8AF1-836FE56F4707",
                    GoodsUnitID = "EB577D2A-1B17-44AB-B005-EBABC9DCF217",
                    InventoryType = EnumInventoryType.InLibrary,
                    PutNumber = 10,
                    PutPrice = (decimal) 8.9,
                    Remark = "入库操作",
                    GoodsName = "苹果",
                    UnitName = "斤"
                }
            },
            InventoryStatus = EnumInventoryStatus.NoAudited,
            InventoryType = EnumInventoryType.InLibrary,
            Reason = EnumReason.PersonInLibrary,
            Remark = "入库",
            Source = EnumSource.SourceManager
        };
        LossReportedAddDto lossAdd = new LossReportedAddDto()
        {
            InventoryStatus = EnumInventoryStatus.NoAudited,
            InventoryType = EnumInventoryType.Damage,
            LossReporteds = new List<LossReportedDto>()
            {
                new LossReportedDto()
                {
                    GoodsID = goodsId,
                    GoodsName = "水果名",
                    GoodsUnit = "斤",
                    InventoryNumber = 2,
                    Price = (decimal) 9.9,
                    Remark = "报损"
                },
                new LossReportedDto()
                {
                    GoodsID = "CEFECB96-3FD2-4E08-8AF1-836FE56F4707",
                    GoodsName = "水果名2",
                    GoodsUnit = "斤",
                    InventoryNumber = 2,
                    Price = (decimal) 9.9,
                    Remark = "报损"
                }
            }
            ,
            Source = EnumSource.SourcePc
            ,
            Reason = EnumReason.Sell
        };
        /// <summary>
        /// 出入库操作
        /// </summary>
        [Test]
        [Order(1)]
        public void OperationInventory()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryManager/OperationInventory";
                StringContent content = new StringContent(JsonConvert.SerializeObject(inventoryAdd));
                var apiResult = await PostAsync<Inventory_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                idInvent = apiResult.Data.ID;
            }).Wait();

        }
        /// <summary>
        /// 获得商品库存详情
        /// </summary>
        [Test]
        [Order(2)]
        public void GetInventoryInfo()
        {
            Task.Run(async () =>
                {
                    url = "/api/InventoryManager/GetInventoryInfo?goodsId=" + goodsId;
                    var apiResult = await GetAsync<Inventory_Quantity>(url);
                    Assert.NotNull(apiResult.Data, apiResult.Message);
                }).Wait();
        }
        [Test]
        [Order(3)]
        public void LossAddReported()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryManager/LossAddReported";
                StringContent content = new StringContent(JsonConvert.SerializeObject(lossAdd));
                var apiResult = await PostAsync<Inventory_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
                bsId = apiResult.Data.ID;
            }).Wait();
        }

        [Test]
        [Order(4)]
        public void LossUpdateReported()
        {
            Task.Run(async () =>
                {
                    url = "/api/InventoryManager/LossUpdateReported";
                    var updateDto = lossAdd.Adapt<LossReportedUpdateDto>();
                    updateDto.ID = bsId;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync(url, content);
                    Assert.AreEqual(200, apiResult.StatusCode);
                }).Wait();
        }

        [Test]
        [Order(5)]
        public void InQueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryManager/InQueryPages";
                InventoryManagerPageDto page = new InventoryManagerPageDto()
                {
                    PageIndex = 1,
                    PageSize = 10
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(page));
                var apiResult = await PostAsync<PagedInfo<InventoryManagerVM>>(url, content);
                Assert.Greater(apiResult.Data.DataSource.Count, 0, apiResult.Message);
            }).Wait();
        }
        [Test]
        [Order(6)]
        public void UpdateInInventory()
        {
            Task.Run(async () =>
            {
                url = "/api/InventoryManager/UpdateInInventory";
                var inventoryDto = inventoryAdd.Adapt<InventoryManagerUpdateDto>();
                inventoryDto.ID = idInvent;
                StringContent content = new StringContent(JsonConvert.SerializeObject(inventoryDto));
                var apiResult = await PostAsync<Inventory_Manager>(url, content);
                Assert.NotNull(apiResult.Data, apiResult.Message);
            }).Wait();
        }

        //public void Audit()
        //{
        //    url = $"/api/InventoryManager/Audit?inventoryId={}&status=1,isInInventory=1";

        //}

    }
}
