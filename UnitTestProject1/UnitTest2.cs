using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataValidation;
using TestModels;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        public UnitTest2()
        {
            // エラーメッセージリソースの設定
            DataValidation.Configuration.DefaultErrorMessageResourceType = typeof(ErrorMessage2);
            DataValidation.Configuration.DefaultErrorMessageResourceNameProvider = (attr) => attr.GetType().Name.Replace("Attribute", string.Empty);
        }

        [TestMethod]
        public void Test1_ErrorMessageResourceからのメッセージ取得テスト()
        {
            var syain = new Staff2();
            var context = syain.GetValidationContext("MailAddress", DisplayNames.ResourceManager.GetString("MailAddress"));
            var results = context.GetPropErrors();
            results.Count.Is(1);
            if (results.Count != 1) return;

            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.Required, context.DisplayName));

        }


        [TestMethod]
        public void Test2_Requiredのテスト()
        {
            var syain = new Staff2();

            var results = syain.GetValidationContext().GetAllErrors();
            results.Count.Is(4);        // 必須項目は４つ
        }

        [TestMethod]
        public void Test2_StringLengthのテスト()
        {
            var syain = new Staff2();

            Func<List<ValidationResult>> check = () => syain.GetValidationContext("SyainNo").GetPropErrors();

            syain.SyainNo = "1234567890";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.SyainNo = "12345678901";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.StringLength, "社員番号", 10));

        }

        [TestMethod]
        public void Test2_Rangeのテスト()
        {
            var syain = new Staff2();

            Func<List<ValidationResult>> check = () => syain.GetValidationContext("Age").GetPropErrors();
            syain.Age = "20";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.Age = "120";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.Range, "年齢", 16, 80));

        }

        [TestMethod]
        public void Test2_CheckDateのテスト()
        {
            var syain = new Staff2();

            Func<List<ValidationResult>> check = () => syain.GetValidationContext("HireDate").GetPropErrors();

            syain.HireDate = "2015/04/01";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.HireDate = "不正な日付データ";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.CheckDate, "入社年月日"));

            // 時、分、秒は許可されない
            syain.HireDate = "2015/04/01 11:11:11";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.CheckDate, "入社年月日"));
        }

        [TestMethod]
        public void Test2_EmailAddressのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("MailAddress").GetPropErrors();

            syain.MailAddress = "foo@bar.com";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.MailAddress = "foo-bar.com";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.EmailAddress, "MailAddress"));
        }

        [TestMethod]
        public void Test2_Compareのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("MailAddressConfirm").GetPropErrors();

            syain.MailAddress = "foo@bar.com";
            syain.MailAddressConfirm = "foo@bar.com";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.MailAddressConfirm = "foo@bar.net";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.Compare, "メールアドレス"));
        }

        [TestMethod]
        public void Test2_Urlのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("PageUrl").GetPropErrors();

            syain.PageUrl = "http://google.com";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.PageUrl = "jjj";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.Url, "PageUrl"));
        }

        [TestMethod]
        public void Test2_EnumDataTypeのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("KoyouKbn").GetPropErrors();

            syain.KoyouKbn = ((int)KoyouKbn.Seiki).ToString();
            var results = check.Invoke();
            results.Count().Is(0);

            syain.KoyouKbn = "99";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.EnumDataType, "雇用区分"));
        }

        [TestMethod]
        public void Test2_RegularExpressionのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("MailAddress2").GetPropErrors();

            syain.MailAddress2 = string.Empty;
            var results = check.Invoke();
            results.Count().Is(0);

            syain.MailAddress2 = "nj00@nekoni.net";
            results = check.Invoke();
            results.Count().Is(0);

            syain.MailAddress2 = "nj00@outlook.com";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.InvalidDomainName, "MailAddress2"));
        }

        [TestMethod]
        public void Test2_CheckValidのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext("SyainNoIsNotUnique").GetPropErrors();

            syain.SyainNoIsNotUnique = false;
            var results = check.Invoke();
            results.Count().Is(0);

            syain.SyainNoIsNotUnique = true;
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage2.NotUnique, "社員番号"));

        }


        [TestMethod]
        public void Test2_CustomValidationのテスト()
        {
            var syain = new Staff2();
            Func<List<ValidationResult>> check = () => syain.GetValidationContext().GetClassLevelErrors();

            syain.SyainNo = "1";
            syain.MailAddress = "nomiya@dream.jp";
            syain.MailAddressConfirm = "nomiya@dream.jp";


            syain.HireDate = "2015/04/01";
            syain.RetireDate = "2017/06/30";
            var results = check.Invoke();
            results.Count().Is(0);

            syain.HireDate = "2015/04/01";
            syain.RetireDate = "2013/06/30";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(ErrorMessage2.InvalidRetireDate);

        }
    }
}
