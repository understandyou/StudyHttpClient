using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Marketing;
using Duo.System.Model.EnumModel.Marketing;
using Duo.System.Model.View.Marketing;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Marketing
{
    public class MarketingSpecialControllerTest : BaseTest
    {
        private Marketing_Special activity = null;
        List<ActivitySpecialRangeDto> activityRanges = new List<ActivitySpecialRangeDto>()
        {
            new ActivitySpecialRangeDto()
            {
                Target = "id1"
            },
            new ActivitySpecialRangeDto()
            {
                Target = "id2"
            }
        };
        [Test]
        [Order(1)]
        public void AddSpecial()
        {
            Task.Run(async () =>
            {
                url = "/api/MarketingSpecial/AddSpecial";
                SpecialAddDto dto = new SpecialAddDto()
                {
                    ActivityEndDate = DateTime.Now.AddDays(1),
                    ActivityEndTime = DateTime.Now.AddDays(1).AddHours(1)
                ,
                    ActivityName = "折扣活动",
                    ActivityRanges = activityRanges,
                    ActivityStartDate = DateTime.Now,
                    ActivityStartTime = DateTime.Now,
                    DiscountType = EnumDiscountType.Special,
                    PurposeType = EnumPurposeType.Shop,
                    Range = EnumDisCountRange.TargetGoods
                ,
                    UseType = EnumUseType.Day,
                    UseTypeValue = "1,2,3"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Marketing_Special>(url, content);
                activity = apiResult.Data;
                Assert.NotNull(activity, apiResult.Message);
            }).Wait();
        }
        [Test]
        [Order(2)]
        public void UpdateSpecial()
        {
            Task.Run(async () =>
            {
                var dto = activity.Adapt<SpecialUpdateDto>();
                dto.ActivityName = "改名拉";
                dto.ActivityRanges = activityRanges;
                url = "/api/MarketingSpecial/UpdateSpecial";
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Marketing_Special>(url, content);
                activity = apiResult.Data;
                Assert.NotNull(apiResult.Data);
            }).Wait();
        }
        [Test]
        [Order(3)]
        public void UpdateStatus()
        {
            Task.Run(async () =>
            {
                url = $"/api/MarketingSpecial/UpdateStatus?id={activity.ID}&status=false";
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }
        [Test]
        [Order(4)]
        public void QuerySpecial()
        {
            Task.Run(async () =>
            {
                url = $"/api/MarketingSpecial/QuerySpecial?id={activity.ID}";
                var apiResult = await GetAsync<SpecialVm>(url);
                Assert.NotNull(apiResult.Data);
            }).Wait();
        }

    }
}