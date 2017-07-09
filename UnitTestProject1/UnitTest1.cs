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
    public class UnitTest1
    {
        public UnitTest1()
        {
            // エラーメッセージリソースの設定
            Configuration.DefaultErrorMessageResourceType = typeof(ErrorMessage);
            Configuration.ErrorMessageResourceNameProvider = (attr) => attr.GetType().Name.Replace("Attribute", string.Empty);
        }

        [TestMethod]
        public void Test1_ErrorMessageResourceからのメッセージ取得テスト()
        {
            var syain = new Syain();

            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(syain.MailAddress, new ValidationContext(syain, null, null)
            {
                MemberName = "MailAddress",
                DisplayName = DisplayNames.ResourceManager.GetString("MailAddress")
            }, results);
            results.Count.Is(1);
            if (results.Count != 1) return;

            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRequired, "メールアドレス"));

        }


        [TestMethod]
        public void Test2_CheckRequiredのテスト()
        {
            var syain = new Syain();

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(syain, new ValidationContext(syain, null, null), results, true);
            results.Count.Is(4);        // 必須項目は４つ
        }

        [TestMethod]
        public void Test2_CheckStringLengthのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.SyainNo, new ValidationContext(syain, null, null)
                {
                    MemberName = "SyainNo"
                }, results);

            };

            syain.SyainNo = "1234567890";
            check.Invoke();
            results.Count().Is(0);

            syain.SyainNo = "12345678901";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckStringLength, "社員番号", 10));

        }

        [TestMethod]
        public void Test2_CheckRangeのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.Age, new ValidationContext(syain, null, null)
                {
                    MemberName = "Age"
                }, results);

            };

            syain.Age = "20";
            check.Invoke();
            results.Count().Is(0);

            syain.Age = "120";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRange, "年齢", 16, 80));

        }

        [TestMethod]
        public void Test2_CheckDateのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.HireDate, new ValidationContext(syain, null, null)
                {
                    MemberName = "HireDate"
                }, results);

            };

            syain.HireDate = "2015/04/01";
            check.Invoke();
            results.Count().Is(0);

            syain.HireDate = "不正な日付データ";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckDate, "入社年月日"));

            // 時、分、秒は許可されない
            syain.HireDate = "2015/04/01 11:11:11";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckDate, "入社年月日"));
        }

        [TestMethod]
        public void Test2_CheckEmailAddressのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.MailAddress, new ValidationContext(syain, null, null)
                {
                    MemberName = "MailAddress"
                }, results);

            };

            syain.MailAddress = "foo@bar.com";
            check.Invoke();
            results.Count().Is(0);

            syain.MailAddress = "foo-bar.com";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEmailAddress, "MailAddress"));
        }

        [TestMethod]
        public void Test2_CheckCompareのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.MailAddressConfirm, new ValidationContext(syain, null, null)
                {
                    MemberName = "MailAddressConfirm"
                }, results);

            };

            syain.MailAddress = "foo@bar.com";
            syain.MailAddressConfirm = "foo@bar.com";
            check.Invoke();
            results.Count().Is(0);

            syain.MailAddressConfirm = "foo@bar.net";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckCompare, "メールアドレス"));
        }

        [TestMethod]
        public void Test2_CheckUrlのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.PageUrl, new ValidationContext(syain, null, null)
                {
                    MemberName = "PageUrl"
                }, results);

            };

            syain.PageUrl = "http://google.com";
            check.Invoke();
            results.Count().Is(0);

            syain.PageUrl = "jjj";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckUrl, "PageUrl"));
        }

        [TestMethod]
        public void Test2_CheckEnumDataTypeのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.KoyouKbn, new ValidationContext(syain, null, null)
                {
                    MemberName = "KoyouKbn"
                }, results);

            };

            syain.KoyouKbn = ((int)KoyouKbn.Seiki).ToString();
            check.Invoke();
            results.Count().Is(0);

            syain.KoyouKbn = "99";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEnumDataType, "雇用区分"));
        }

        [TestMethod]
        public void Test2_CheckRegularExpressionのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.MailAddress2, new ValidationContext(syain, null, null)
                {
                    MemberName = "MailAddress2"
                }, results);

            };

            syain.MailAddress2 = string.Empty;
            check.Invoke();
            results.Count().Is(0);

            syain.MailAddress2 = "nomiya@cosmo.cc";
            check.Invoke();
            results.Count().Is(0);

            syain.MailAddress2 = "nomiya@dream.jp";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.InvalidDomainName, "MailAddress2"));
        }

        [TestMethod]
        public void Test2_CheckValidのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateProperty(syain.SyainNoIsNotUnique, new ValidationContext(syain, null, null)
                {
                    MemberName = "SyainNoIsNotUnique"
                }, results);

            };

            syain.SyainNoIsNotUnique = false;
            check.Invoke();
            results.Count().Is(0);

            syain.SyainNoIsNotUnique = true;
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.NotUnique, "社員番号"));

        }


        [TestMethod]
        public void Test2_CheckCustomValidationのテスト()
        {
            var syain = new Syain();
            var results = new List<ValidationResult>();
            Action check = () =>
            {
                results.Clear();
                Validator.TryValidateObject(syain, new ValidationContext(syain, null, null), results, false);
            };

            syain.SyainNo = "1";
            syain.MailAddress = "nomiya@dream.jp";
            syain.MailAddressConfirm = "nomiya@dream.jp";


            syain.HireDate = "2015/04/01";
            syain.RetireDate = "2017/06/30";
            check.Invoke();
            results.Count().Is(0);

            syain.HireDate = "2015/04/01";
            syain.RetireDate = "2013/06/30";
            check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(ErrorMessage.InvalidRetireDate);

        }
    }
}
