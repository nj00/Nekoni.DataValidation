using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestModels;
using System.Linq;
using Nekoni.Validation;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            // エラーメッセージリソースの設定
            Configuration.DefaultErrorMessageResourceType = typeof(ErrorMessage);
        }

        [TestMethod]
        public void Test1_ErrorMessageResourceからのメッセージ取得テスト()
        {
            var model = new Staff();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null)
            {
                MemberName = "MailAddress",
                DisplayName = DisplayNames.ResourceManager.GetString("MailAddress")
            };
            Validator.TryValidateProperty(model.MailAddress, context, results);
            results.Count.Is(1);
            if (results.Count != 1) return;

            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRequired, context.DisplayName));

        }


        [TestMethod]
        public void Test2_CheckRequiredのテスト()
        {
            var model = new Staff();

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model, null, null), results, true);
            results.Count.Is(4);        // 必須項目は４つ
        }

        [TestMethod]
        public void Test2_CheckStringLengthのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.SyainNo, new ValidationContext(model, null, null)
                {
                    MemberName = "SyainNo"
                }, results);

            };

            model.SyainNo = "1234567890";
            check.Invoke();
            results.Count().Is(0);

            model.SyainNo = "12345678901";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckStringLength, "社員番号", 10));

        }

        [TestMethod]
        public void Test2_CheckRangeのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.Age, new ValidationContext(model, null, null)
                {
                    MemberName = "Age"
                }, results);

            };

            model.Age = "20";
            check.Invoke();
            results.Count().Is(0);

            model.Age = "120";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRange, "年齢", 16, 80));

        }

        [TestMethod]
        public void Test2_CheckDateのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.HireDate, new ValidationContext(model, null, null)
                {
                    MemberName = "HireDate"
                }, results);

            };

            model.HireDate = "2015/04/01";
            check.Invoke();
            results.Count().Is(0);

            model.HireDate = "不正な日付データ";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckDate, "入社年月日"));

            // 時、分、秒は許可されない
            model.HireDate = "2015/04/01 11:11:11";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckDate, "入社年月日"));
        }

        [TestMethod]
        public void Test2_CheckEmailAddressのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.MailAddress, new ValidationContext(model, null, null)
                {
                    MemberName = "MailAddress"
                }, results);

            };

            model.MailAddress = "staff1@nekoni.net";
            check.Invoke();
            results.Count().Is(0);

            model.MailAddress = "staff1-nekoni.net";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEmailAddress, "MailAddress"));
        }

        [TestMethod]
        public void Test2_CheckCompareのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.MailAddressConfirm, new ValidationContext(model, null, null)
                {
                    MemberName = "MailAddressConfirm"
                }, results);

            };

            model.MailAddress = "staff1@nekoni.net";
            model.MailAddressConfirm = "staff1@nekoni.net";
            check.Invoke();
            results.Count().Is(0);

            model.MailAddressConfirm = "staff2@nekoni.net";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckCompare, "メールアドレス"));
        }

        [TestMethod]
        public void Test2_CheckUrlのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.PageUrl, new ValidationContext(model, null, null)
                {
                    MemberName = "PageUrl"
                }, results);

            };

            model.PageUrl = "http://google.com";
            check.Invoke();
            results.Count().Is(0);

            model.PageUrl = "jjj";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckUrl, "PageUrl"));
        }

        [TestMethod]
        public void Test2_CheckEnumDataTypeのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.KoyouKbn, new ValidationContext(model, null, null)
                {
                    MemberName = "KoyouKbn"
                }, results);

            };

            model.KoyouKbn = ((int)KoyouKbn.Seiki).ToString();
            check.Invoke();
            results.Count().Is(0);

            model.KoyouKbn = "99";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEnumDataType, "雇用区分"));
        }

        [TestMethod]
        public void Test2_CheckRegularExpressionのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.MailAddress2, new ValidationContext(model, null, null)
                {
                    MemberName = "MailAddress2"
                }, results);

            };

            model.MailAddress2 = string.Empty;
            check.Invoke();
            results.Count().Is(0);

            model.MailAddress2 = "staff1@nekoni.net";
            check.Invoke();
            results.Count().Is(0);

            model.MailAddress2 = "staff1@nekoni.com";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.InvalidDomainName, "MailAddress2"));
        }

        [TestMethod]
        public void Test2_CheckValidのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(model.SyainNoIsNotUnique, new ValidationContext(model, null, null)
                {
                    MemberName = "SyainNoIsNotUnique"
                }, results);

            };

            model.SyainNoIsNotUnique = false;
            check.Invoke();
            results.Count().Is(0);

            model.SyainNoIsNotUnique = true;
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.NotUnique, "社員番号"));

        }


        [TestMethod]
        public void Test2_CheckCustomValidationのテスト()
        {
            var model = new Staff();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateObject(model, new ValidationContext(model, null, null), results, false);
            };

            model.SyainNo = "1";
            model.MailAddress = "staff1@nekoni.net";
            model.MailAddressConfirm = "staff1@nekoni.net";


            model.HireDate = "2015/04/01";
            model.RetireDate = "2017/06/30";
            check.Invoke();
            results.Count().Is(0);

            model.HireDate = "2015/04/01";
            model.RetireDate = "2013/06/30";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(ErrorMessage.InvalidRetireDate);

        }
    }
}
