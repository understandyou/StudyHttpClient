using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Duo.System.Model.EnumModel.Member;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestApplication.Member
{
    public class MemberStoredValueRuleControllerTeset : BaseTest
    {
        private Member_StoredValueRule rule = null;
        [Test]
        [Order(1)]
        public void AddMemberRule()
        {
            Task.Run(async () =>
            {

                url = "/api/MemberStoredValueRule/AddMemberRule";
                MemberRuleAddDto addDto = new MemberRuleAddDto()
                {
                    GrantType = EnumMemberGrantType.None,
                    GrantValue = 0,
                    Remark = "",
                    RuleName = "不赠送",
                    StoredValue = 11
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Member_StoredValueRule>(url, content);
                rule = apiResult.Data;
                Assert.NotNull(rule);
            }).Wait();
        }
        [Test]
        [Order(2)]
        public void UpdateRule()
        {
            Task.Run(async () =>
                {
                    url = "/api/MemberStoredValueRule/UpdateRule";
                    var updateDto = rule.Adapt<MemberRuleUpdateDto>();
                    updateDto.GrantType = EnumMemberGrantType.None;
                    updateDto.GrantValue = 1;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateDto));
                    var apiResult = await PostAsync<Member_StoredValueRule>(url, content);
                    Assert.NotNull(apiResult.Data);
                }).Wait();
        }
        [Test]
        [Order(3)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberStoredValueRule/QueryPages";
                MemberRulePageDto page = new MemberRulePageDto()
                {
                    GrantType = EnumMemberGrantType.None
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(page));
                var apiResult = await PostAsync<PagedInfo<Member_StoredValueRule>>(url, content);
                Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1, apiResult.Message);
            }).Wait();
        }
        [Test]
        [Order(4)]
        public void Query()
        {
            Task.Run(async () =>
                {
                    url = "/api/MemberStoredValueRule/Query?id=" + rule.ID;
                    var apiResult = await GetAsync<Member_StoredValueRule>(url);
                    Assert.NotNull(apiResult.Data, apiResult.Message);
                }).Wait();
        }
        [Test]
        [Order(5)]
        public void DeleteMemberRule()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberStoredValueRule/DeleteMemberRule?id=" + rule.ID;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();

        }

    }
}