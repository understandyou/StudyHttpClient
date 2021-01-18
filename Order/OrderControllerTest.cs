using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Order;
using Duo.System.Model.EnumModel.ExpenditureManager;
using Duo.System.Model.View.Order;
using Newtonsoft.Json;
using NUnit.Framework;
using ExpenditureLog = Duo.System.Model.Dto.Order.ExpenditureLog;

namespace TestApplication.Order
{
    public class OrderControllerTest : BaseTest
    {
        private Expenditure_Manager resultData = null;
        [Test]
        [Order(1)]
        public void AddOrder()
        {
            Task.Run(async () =>
            {

                ExpenditureManagerAddDto addDto = new ExpenditureManagerAddDto()
                {
                    Details = new List<ExpenditureDetail>()
                {
                    new ExpenditureDetail()
                    {
                        DiscountsPrice = (decimal)0.1,ExpenditureID = "",ExpenditurePrice = (decimal)9.9,GoodsID = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2"
                        ,GoodsName = "香蕉",GoodsNumber = 2,OriginalPrice = 10
                    }
                }
                    ,
                    DiscountsPrice = (decimal)200.2,
                    ExpenditureLogs = new List<ExpenditureLog>()
                {
                    new ExpenditureLog(){PaymentAmount = (decimal)19.8,PaymentMethod = EnumPaymentMethod.AliPay}
                },
                    ExpenditureType = EnumExpenditureType.Member,
                    MemberManagerID = "02BFC051-CD16-48C3-BF8E-190096EF3BA9",
                    OrderStatus = EnumOrderStatus.Finish,
                    OrderType = EnumOrderType.Shop
                    ,
                    Remark = "",
                    MemberCard = "123456",
                    MemberName = "张三",
                    Phone = "15456856452"
                };
                url = "/api/Order/AddOrder";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Expenditure_Manager>(url, content);
                resultData = apiResult.Data;
                Assert.NotNull(resultData);
            }).Wait();
        }
        [Test]
        [Order(2)]
        public void GetOrderPages()
        {
            Task.Run(async () =>
                {
                    url = "/api/Order/GetOrderPages";
                    OrderPagesDto pageDto = new OrderPagesDto()
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(pageDto));
                    var apiResult = await PostAsync<PagedInfo<Expenditure_Manager>>(url, content);
                    Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1, apiResult.Message);
                }).Wait();

        }

        [Test]
        [Order(3)]
        public void GetOrder()
        {
            Task.Run(async () =>
            {
                url = "/api/Order/GetOrder?orderId=" + resultData.ID;
                var apiResult = await GetAsync<ExpenditureManagerVM>(url);
                Assert.NotNull(apiResult.Data);
            }).Wait();
        }

    }
}