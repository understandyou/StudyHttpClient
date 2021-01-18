using System;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Duo.System.Model;
using Duo.System.Model.Dto.Member;
using Duo.System.Model.EnumModel.Member;
using Duo.System.Model.View.Member;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestApplication.Member
{
    /// <summary>
    /// 会员储值
    /// </summary>
    public class MemberStoredValueControllerTest : BaseTest
    {
        private static string memberID = "02BFC051-CD16-48C3-BF8E-190096EF3BA9";
        MemberStoredValueAddDto addDto = new MemberStoredValueAddDto()
        {
            MemberManagerID = memberID,
            Pay = EnumPay.PayAliPay,
            Remark = "",
            RuleID = "48BD7F42-88D2-4271-89EC-C3662A1BD3B2",
            TopUpMoney = 10
        };

        private Member_StoredValue storedValue = null;
        [Test]
        [Order(1)]
        public void AddMemberValue()//符合规则的
        {
            Task.Run(async () =>
            {
                //原始信息
                var BeforeResult = Get<Member_Manager>($"/api/MemberManager/Query?userId={memberID}").Data;
                //开始添加信息
                url = "/api/MemberStoredValue/AddMemberValue";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = Post<Member_StoredValue>(url, content);
                storedValue = apiResult.Data;
                //添加后的信息
                var AfterResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                //规则
                var rule = Get<Member_StoredValueRule>($"/api/MemberStoredValueRule/Query?id=48BD7F42-88D2-4271-89EC-C3662A1BD3B2").Data;
                Assert.NotNull(storedValue, apiResult.Message);
                if (addDto.TopUpMoney >= rule.StoredValue)
                {
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney + rule.GrantValue, "充值后余额异常");
                }
                else
                {
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney, "充值后余额异常");
                }
            }).Wait();

        }
        [Test]
        [Order(2)]
        public void AddMemberValueNo()//不符合规则的
        {
            Task.Run(async () =>
            {
                addDto.TopUpMoney = 9;
                //原始信息
                var BeforeResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                //开始添加信息
                url = "/api/MemberStoredValue/AddMemberValue";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = Post<Member_StoredValue>(url, content);
                storedValue = apiResult.Data;
                //添加后的信息
                var AfterResult = Get<Member_Manager>($"/api/MemberManager/Query?userId={memberID}").Data;
                //规则
                var rule = Get<Member_StoredValueRule>($"/api/MemberStoredValueRule/Query?id=48BD7F42-88D2-4271-89EC-C3662A1BD3B2").Data;
                Assert.NotNull(storedValue, apiResult.Message);
                if (addDto.TopUpMoney >= rule.StoredValue)
                {
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney + rule.GrantValue, "充值后余额异常");
                }
                else
                {
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney, "充值后余额异常");
                }
            }).Wait();

        }
        /// <summary>
        /// 赠送积分的
        /// </summary>
        [Test]
        [Order(3)]
        public void AddMemberValuePonit()//符合条件的
        {
            Task.Run(async () =>
                {
                    addDto.RuleID = "E61CDD19-9F59-4D6D-88CF-8C38D223F60A";
                    addDto.TopUpMoney = 10;
                    //原始信息
                    var BeforeResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                    //开始添加信息
                    url = "/api/MemberStoredValue/AddMemberValue";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                    var apiResult = await PostAsync<Member_StoredValue>(url, content);
                    storedValue = apiResult.Data;
                    //添加后的信息
                    var AfterResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                    //规则
                    var rule = (await GetAsync<Member_StoredValueRule>($"/api/MemberStoredValueRule/Query?id=E61CDD19-9F59-4D6D-88CF-8C38D223F60A")).Data;
                    Assert.NotNull(storedValue, apiResult.Message);
                    if (addDto.TopUpMoney >= rule.StoredValue)
                    {
                        Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney, "充值后余额异常");
                        Assert.AreEqual(AfterResult.AccountIntegral, BeforeResult.AccountIntegral + rule.GrantValue, "充值后积分异常");
                    }
                    else
                    {//不符合条件积分不变
                        Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney, "充值后积分异常");
                    }
                }).Wait();
        }
        /// <summary>
        /// 赠送积分不符合条件
        /// </summary>
        [Test]
        [Order(4)]
        public void AddMemberValuePonitNo()//不符合条件的
        {
            Task.Run(async () =>
            {
                addDto.RuleID = "E61CDD19-9F59-4D6D-88CF-8C38D223F60A";
                addDto.TopUpMoney -= 1;
                //原始信息
                var BeforeResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                //开始添加信息
                url = "/api/MemberStoredValue/AddMemberValue";
                StringContent content = new StringContent(JsonConvert.SerializeObject(addDto));
                var apiResult = await PostAsync<Member_StoredValue>(url, content);
                storedValue = apiResult.Data;
                //添加后的信息
                var AfterResult = (await GetAsync<Member_Manager>($"/api/MemberManager/Query?userId={memberID}")).Data;
                //规则
                var rule = (await GetAsync<Member_StoredValueRule>($"/api/MemberStoredValueRule/Query?id=E61CDD19-9F59-4D6D-88CF-8C38D223F60A")).Data;
                Assert.NotNull(storedValue, apiResult.Message);
                if (addDto.TopUpMoney >= rule.StoredValue)
                {
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney, "充值后余额异常");
                    Assert.AreEqual(AfterResult.AccountIntegral, BeforeResult.AccountIntegral + rule.GrantValue, "充值后积分异常");
                }
                else
                {//不符合条件积分不变
                    Assert.AreEqual(AfterResult.BalanceMoney, BeforeResult.BalanceMoney + addDto.TopUpMoney, "充值后余额异常");
                    Assert.AreEqual(AfterResult.AccountIntegral, BeforeResult.AccountIntegral + rule.GrantValue, "充值后积分异常");
                }
            }).Wait();
        }


        [Test]
        [Order(5)]
        public void QueryPages()
        {
            Task.Run(async () =>
                {
                    url = "/api/MemberStoredValue/QueryPages";
                    MemberStoredValuePageDto page = new MemberStoredValuePageDto()
                    {
                        StartDateTime = DateTime.Now
                    };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(page));
                    var apiResult = await PostAsync<PagedInfo<MemberStoredValueVM>>(url, content);
                    Assert.GreaterOrEqual(apiResult.Data.TotalCount, 1);
                }).Wait();
        }

        [Test]
        [Order(6)]
        public void Query()
        {
            Task.Run(async () =>
            {
                url = $"/api/MemberStoredValue/Query?id={storedValue.ID}";
                var apiResult = await GetAsync<Member_StoredValue>(url);
                Assert.NotNull(apiResult.Data);
            }).Wait();

        }

    }
}