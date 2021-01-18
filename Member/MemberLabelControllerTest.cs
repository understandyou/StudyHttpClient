using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Duo.System.Model.Entity;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestApplication.Member
{
    public class MemberLabelControllerTest : BaseTest
    {
        private Member_Label label = null;
        [Test]
        [Order(1)]
        public void AddLabel()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberLabel/AddLabel";
                MemberLabelDto dto = new MemberLabelDto()
                {
                    LabelName = "超级会员"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Member_Label>(url, content);
                label = apiResult.Data;
                Assert.NotNull(label);
            }).Wait();

        }
        [Test]
        [Order(2)]
        public void UpdateLabel()
        {
            Task.Run(async () =>
            {
                MemberLabelUpdateDto dto = new MemberLabelUpdateDto()
                {
                    ID = label.ID,
                    LabelName = "青铜"
                };
                url = "/api/MemberLabel/UpdateLabel";
                StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                var apiResult = await PostAsync<Member_Label>(url, content);
                label = apiResult.Data;
                Assert.NotNull(label);
            }).Wait();

        }
        [Test]
        [Order(3)]
        public void QueryPages()
        {
            Task.Run(async () =>
                {

                    MemberLabelPageDto dto = new MemberLabelPageDto()
                    {
                        LabelName = label.LabelName
                    };
                    url = "/api/MemberLabel/QueryPages";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
                    var apiResult = await PostAsync<PagedInfo<Member_Label>>(url, content);
                    Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1, apiResult.Message);
                }).Wait();
        }
        [Test]
        [Order(4)]
        public void Query()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberLabel/Query?id=" + label.ID;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }
        [Test]
        [Order(5)]
        public void DeleteLabel()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberLabel/DeleteLabel?id=" + label.ID;
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }

    }
}