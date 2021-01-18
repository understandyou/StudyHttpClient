using System.Net.Http;
using System.Threading.Tasks;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Mapster;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestApplication.Member
{
    public class MemberClassControllerTest : BaseTest
    {
        private Member_Class memberClass = null;
        [Test]
        [Order(1)]
        public void AddMemberClass()
        {
            Task.Run(async () =>
            {

                MemberClassAddDto addDto = new MemberClassAddDto()
                {
                    GradeName = "这是一个分类",
                    Remark = "备注"
                };
                url = "/api/MemberClass/AddMemberClass";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Member_Class>(url, content);
                memberClass = apiResult.Data;
                Assert.NotNull(memberClass);
            }).Wait();
        }
        [Test]
        [Order(2)]
        public void UpdateMemberClass()
        {
            Task.Run(async () =>
                {
                    url = "/api/MemberClass/UpdateMemberClass";
                    var memberClassUpdateDto = memberClass.Adapt<MemberClassUpdateDto>();
                    memberClassUpdateDto.GradeName = "更改后的名字";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(memberClassUpdateDto));
                    var apiResult = await PostAsync<Member_Class>(url, content);
                    Assert.NotNull(apiResult.Data);
                }).Wait();
        }
        [Test]
        [Order(3)]
        public void QueryPages()
        {
            Task.Run(async () =>
            {
                url = "/api/MemberClass/QueryPages";
                MemberClassPageDto pageDto = new MemberClassPageDto()
                {
                    GradeName = "更改后的名字"
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(pageDto));
                var apiResult = await PostAsync<PagedInfo<Member_Class>>(url, content);
                Assert.Greater(apiResult.Data.TotalCount, 0, apiResult.Message);
            }).Wait();
        }
        [Test]
        [Order(4)]
        public void Query()
        {
            Task.Run(async () =>
                {
                    url = $"/api/MemberClass/Query?classId={memberClass.ID}";
                    var apiResult = await GetAsync<Member_Class>(url);
                    Assert.NotNull(apiResult.Data);
                }).Wait();
        }
        [Test]
        [Order(5)]
        public void DeleteMemberClass()
        {
            Task.Run(async () =>
            {
                url = $"/api/MemberClass/DeleteMemberClass?userId={memberClass.ID}";
                var apiResult = await GetAsync(url);
                Assert.AreEqual(200, apiResult.StatusCode);
            }).Wait();
        }

    }
}