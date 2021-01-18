using System;
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
    public class MarketingDiscountControllerTest:BaseTest
    {
        private Marketing_Discount activity=null;
        List<ActivityDiscountRangeDto> activityRanges = new List<ActivityDiscountRangeDto>()
        {
            new ActivityDiscountRangeDto()
            {
                Target = "id1"
            },
            new ActivityDiscountRangeDto()
            {
                Target = "id2"
            }
        };
        List<ActivityTypeAddDto> activityTypes = new List<ActivityTypeAddDto>()
        {
            new ActivityTypeAddDto()
            {
                Discount = 9,DiscountClass = 1,MeetMoney = 10
            }
        };
        [Test]
        [Order(1)]
        public void AddDiscount()
        {
            
            DiscountAddDto dto = new DiscountAddDto()
            {
                ActivityEndDate = DateTime.Now.AddDays(1),ActivityEndTime = DateTime.Now.AddDays(1).AddHours(1)
                ,ActivityName = "折扣活动",ActivityRanges = activityRanges,ActivityStartDate = DateTime.Now,ActivityStartTime = DateTime.Now,ActivityTypes =activityTypes,DiscountType = EnumDiscountType.MeetDiscount,PurposeType = EnumPurposeType.Shop,Range = EnumDisCountRange.TargetGoods
                ,
                UseType= EnumUseType.Day,
                UseTypeValue="1,2,3"
            };
            Task.Run(async () =>
            {
                url = "/api/MarketingDiscount/AddDiscount";
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                //满额折扣
                var postAsync = await PostAsync<Marketing_Discount>(url, content);
                activity = postAsync.Data;
                Assert.NotNull(activity,postAsync.Message);
            }).Wait();


        }

        [Test]
        [Order(2)]
        public void UpdateDiscount()
        {
            var discountUpdateDto = activity.Adapt<DiscountUpdateDto>();
            discountUpdateDto.ActivityRanges = activityRanges;
            discountUpdateDto.ActivityTypes = activityTypes;
            discountUpdateDto.ActivityName = "折扣修改名字";
            Task.Run(async () =>
            {
                url = "/api/MarketingDiscount/UpdateDiscount";
                StringContent content = new StringContent(JsonConvert.SerializeObject(discountUpdateDto));
                //满额折扣
                var postAsync = await PostAsync<Marketing_Discount>(url, content);
                activity = postAsync.Data;
                Assert.NotNull(activity, postAsync.Message);
            }).Wait();
        }

        [Test]
        [Order(3)]
        public void UpdateStatus()
        {
            Task.Run(async () =>
            {
                url = $"/api/MarketingDiscount/UpdateStatus?id={activity.ID}&status=true";
                var getAsync = await GetAsync<Marketing_Discount>(url);
                Assert.AreEqual(200, getAsync.StatusCode, getAsync.Message);
            }).Wait();
        }

        [Test]
        [Order(4)]
        public void QueryDiscount()
        {
            Task.Run(async () =>
            {
                url = $"/api/MarketingDiscount/QueryDiscount?id={activity.ID}";
                var getAsync = await GetAsync<Marketing_Discount>(url);
                Assert.NotNull(getAsync.Data, getAsync.Message);
            }).Wait();
        }


    }
}