using System;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Duo.System.Model.EnumModel.Member;
using Duo.System.Model.View.Member;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestApplication.Member
{
    public class MemberPointsControllerTest : BaseTest
    {
        private Member_Points points;
        [Test]
        [Order(1)]
        public void AddMemberPoints()
        {
            Task.Run(async () =>
            {
                MemberPointsDto addDto = new MemberPointsDto()
                {
                    IntegralType = EnumMemberIntegralType.IntegralTypeCzzs,
                    MemberCard = "666",
                    MemberManagerID = "02BFC051-CD16-48C3-BF8E-190096EF3BA9"
                    ,
                    MemberName = "",
                    OperationType = EnumMemberOperationType.OperationTypeZj,
                    Phone = "15828458695",
                    Remark = "",
                    Variation = 9
                };
                url = "/api/MemberPoints/AddMemberPoints";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Member_Points>(url, content);
                points = apiResult.Data;
                Assert.NotNull(points);
            }).Wait();
        }
        [Test]
        [Order(2)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberPoints/QueryPages";
                MemberPointsPageDto pageDto = new MemberPointsPageDto()
                {
                    StartDateTime = DateTime.Now
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(pageDto));
                var apiResult = await PostAsync<PagedInfo<MemberPointsVM>>(url, content);
                Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1);
            }).Wait();
        }
    }
}