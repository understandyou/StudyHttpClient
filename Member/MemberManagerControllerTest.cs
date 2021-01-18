using System;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Duo.System.Model.EnumModel.Member;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Member
{
    public class MemberManagerControllerTest : BaseTest
    {
        private Member_Manager manager = null;
        [Test]
        [Order(1)]
        public void AddMemberManager()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberManager/AddMemberManager";
                MemberManagerAddDto addDto = new MemberManagerAddDto()
                {
                    AccountIntegral = 0,
                    BalanceMoney = 0,
                    Birthday = DateTime.Now,
                    Gender = EnumGender.FeMale,
                    Label = "青铜会员"
                    ,
                    MembeClassID = "73C1738B-557F-41CA-985D-2BD4722FB332",
                    MemberCard = "123456",
                    MemberName = "张三",
                    MemberPwd = "666",
                    Phone = "15828465698",
                    Source = EnumMemberSource.Pc,
                    Status = EnumMemberStatus.StatusZc
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Member_Manager>(url, content);
                manager = apiResult.Data;
                Assert.NotNull(manager, apiResult.Message);
            }).Wait();

        }

        [Test]
        [Order(2)]
        public void UpdateMemberManager()
        {
            Task.Run(async () =>
                {
                    url = "/api/MemberManager/UpdateMemberManager";

                    var updateDto = manager.Adapt<MemberManagerUpdateDto>();
                    updateDto.Phone = "11111111";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync<Member_Manager>(url, content);
                    Assert.NotNull(apiResult.Data, apiResult.Message);
                }).Wait();
        }

        [Test]
        [Order(3)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberManager/QueryPages";
                MemberManagerPageDto pageDto = new MemberManagerPageDto()
                {
                    Phone = "11111111"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(pageDto));
                var apiResult = await PostAsync<PagedInfo<Member_Manager>>(url, content);
                Assert.Greater(apiResult.Data.TotalCount, 0);
            }).Wait();
        }

        [Test]
        [Order(4)]
        public void Query()
        {
            Task.Run(async () =>
            {
                url = $"/api/MemberManager/Query?userId={manager.ID}";
                var apiResult = await GetAsync<Member_Manager>(url);
                Assert.NotNull(apiResult.Data);
            }).Wait();
        }

    }
}