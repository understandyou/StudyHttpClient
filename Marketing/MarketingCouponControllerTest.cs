using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Marketing;
using Duo.System.Model.EnumModel.Marketing;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Marketing
{
    public class MarketingCouponControllerTest : BaseTest
    {
        private Marketing_Coupon coupon = null;
        List<CouponAddRangeDto> listRanges = new List<CouponAddRangeDto>()
        {
            new CouponAddRangeDto()
            {
                TargetID = "6F118505-1D7F-49AC-8F38-C5F5DAFB7AC2"
            }
        };
        [Test]
        [Order(1)]
        public void AddCoupon()
        {
            Task.Run(async () =>
            {
                url = "/api/MarketingCoupon/AddCoupon";
                CouponAddDto dto = new CouponAddDto()
                {
                    CouponDetail = "这是一个超值优惠券",
                    CouponName = "满10减5",
                    CouponType = EnumCouponType.Derate,
                    Discount = null
                    ,
                    GetCouponType = EnumGetCouponType.Initiative,
                    IsOverlay = false,
                    MeetMoney = 10,
                    Range = EnumCouponRange.AssignGoods,
                    Ranges = listRanges,
                    SubMoney = 2,
                    ValidType = EnumValidType.AssignDateAfter,
                    ValidDay = 3,
                    UseTargetType = EnumUseTargetType.All,
                    CouponNumber = 10
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Marketing_Coupon>(url, content);
                coupon = apiResult.Data;
                Assert.NotNull(coupon, apiResult.Message);
            }).Wait(1000 * 5);
        }

        [Test]
        [Order(2)]
        public void UpdateCoupon()
        {
            Task.Run(async () =>
                {
                    url = "/api/MarketingCoupon/UpdateCoupon";
                    var updateDto = coupon.Adapt<CouponUpdateDto>();
                    updateDto.CouponName = "满1减1";
                    updateDto.Ranges = listRanges;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync<Marketing_Coupon>(url, content);
                    Assert.NotNull(apiResult.Data);
                }).Wait(1000 * 5);
        }
        [Test]
        [Order(3)]
        public void UpdateStatus()
        {
            Task.Run(async () =>
            {
                url = $"/api/MarketingCoupon/UpdateStatus?id={coupon.ID}&status=false";
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait(1000 * 5);
        }

        [Test]
        [Order(4)]
        public void QueryPages()
        {
            Task.Run(async () =>
                {
                    CouponPageDto dto = new CouponPageDto()
                    {
                        CouponName = "满1减1",
                        Status = false
                    };

                    url = "/api/MarketingCoupon/QueryPages";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                    var apiResult = await PostAsync<PagedInfo<Marketing_Coupon>>(url, content);
                    Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1);
                }).Wait(1000 * 5);
        }
        [Test]
        [Order(5)]
        public void Query()
        {
            Task.Run(async () =>
            {
                url = "/api/MarketingCoupon/Query?id=" + coupon.ID;
                var apiResult = await GetAsync<Marketing_Coupon>(url);
                Assert.NotNull(apiResult.Data);
            }).Wait(1000 * 5);
        }

        [Test]
        [Order(6)]
        public void DeleteCoupon()
        {
            Task.Run(async () =>
            {
                url = "/api/MarketingCoupon/DeleteCoupon?id=" + coupon.ID;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait(1000 * 5);
        }
    }
}