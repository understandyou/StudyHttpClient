using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Shop;
using Duo.System.Model.EnumModel.Goods;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Shop
{
    public class ShopManagerControllerTest : BaseTest
    {
        private Shop_Manager manager = null;
        [Test]
        [Order(1)]
        public void AddShop()
        {
            Task.Run(async () =>
            {

                url = "/api/ShopManager/AddShop";
                ShopManagerAddDto addDto = new ShopManagerAddDto()
                {
                    City = "成都",
                    DetailedAddress = "华府大道",
                    District = "高新区",
                    HelpCode = "aa",
                    Linkman = "张三",
                    ParentID = ""
                    ,
                    PersonNumber = 10,
                    Phone = "15845623545",
                    ProvincialCapital = "四川",
                    Remark = "",
                    UsableArea = 12,
                    ShopClasses = EnumShopClasses.DirectSales
                    ,
                    ShopName = "生疏",
                    ShopCode = "bm",
                    ShopPhone = "36525456985",
                    Status = EnumStatus.Disable
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Shop_Manager>(url, content);
                manager = apiResult.Data;
                Assert.NotNull(manager);
            }).Wait();

        }
        [Test]
        [Order(2)]
        public void UpdateShop()
        {
            Task.Run(async () =>
            {
                url = "/api/ShopManager/UpdateShop";
                var updateDto = manager.Adapt<ShopManagerUpdateDto>();
                updateDto.DetailedAddress = "华府一段";
                StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                var apiResult = await PostAsync<Shop_Manager>(url, content);
                Assert.NotNull(apiResult.Data);

            }).Wait();
        }
        [Test]
        [Order(3)]
        public void UpdateStatus()
        {
            Task.Run(async () =>
                {
                    url = $"/api/ShopManager/UpdateStatus?shopId={manager.ID}&status=1";
                    var apiResult = await GetAsync(url);
                    Assert.AreEqual(200, apiResult.StatusCode);
                }).Wait();
        }
        [Test]
        [Order(4)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/ShopManager/QueryPages";
                ShopManagerPageDto page = new ShopManagerPageDto()
                {
                    ShopName = "生疏"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(page));
                var apiResult = await PostAsync<PagedInfo<Shop_Manager>>(url, content);
                Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1, apiResult.Message);
            }).Wait();
        }
        [Test]
        [Order(5)]
        public void Query()
        {
            Task.Run(async () =>
                {
                    url = "/api/ShopManager/Query?shopId=" + manager.ID;
                    var apiResult = await GetAsync<Shop_Manager>(url);
                    Assert.NotNull(apiResult.Data);
                });

        }
        [Test]
        [Order(6)]
        public void DeleteShop()
        {
            Task.Run(async () =>
            {
                url = "/api/ShopManager/DeleteShop?shopId=" + manager.ID;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }

    }
}