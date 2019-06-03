﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nekoni.DataValidation;
using Nekoni.DataValidation.ForValidation;
using Nekoni.DataValidation.Validator;
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
            Configuration.DefaultErrorMessageResourceType = typeof(ErrorMessage);
            Configuration.DefaultErrorMessageResourceNameProvider = (attr) => {
                var attrName = attr.GetType().Name.Replace("Attribute", string.Empty);
                if (attrName.StartsWith("Check"))
                {
                    return attrName;
                }
                else
                {
                    return $"Check{attrName}";
                }
            };
        }

        [TestMethod]
        public void Test1_ErrorMessageResourceからのメッセージ取得テスト()
        {
            var model = new Staff2();
            var context = model.ForValidation("MailAddress", DisplayNames.ResourceManager.GetString("MailAddress"));
            var results = context.GetPropErrors();
            results.Count.Is(1);
            if (results.Count != 1) return;

            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRequired, context.DisplayName));

        }


        [TestMethod]
        public void Test2_Requiredのテスト()
        {
            var model = new Staff2();

            var results = model.ForValidation().GetAllErrors();
            results.Count.Is(4);        // 必須項目は４つ
        }

        [TestMethod]
        public void Test2_StringLengthのテスト()
        {
            var model = new Staff2();

            Func<List<ValidationResult>> check = () => model.ForValidation("SyainNo").GetPropErrors();

            model.SyainNo = "1234567890";
            var results = check.Invoke();
            results.Count().Is(0);

            model.SyainNo = "12345678901";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckStringLength, "社員番号", 10));

        }

        [TestMethod]
        public void Test2_Rangeのテスト()
        {
            var model = new Staff2();

            Func<List<ValidationResult>> check = () => model.ForValidation("Age").GetPropErrors();
            model.Age = "20";
            var results = check.Invoke();
            results.Count().Is(0);

            model.Age = "120";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckRange, "年齢", 16, 80));

        }

        [TestMethod]
        public void Test2_CheckDateのテスト()
        {
            var model = new Staff2();

            Func<List<ValidationResult>> check = () => model.ForValidation("HireDate").GetPropErrors();

            model.HireDate = "2015/04/01";
            var results = check.Invoke();
            results.Count().Is(0);

            model.HireDate = "不正な日付データ";
            results = check.Invoke();
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
        public void Test2_EmailAddressのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("MailAddress").GetPropErrors();

            model.MailAddress = "staff1@nekoni.net";
            var results = check.Invoke();
            results.Count().Is(0);

            model.MailAddress = "staff1-nekoni.net";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEmailAddress, "MailAddress"));
        }

        [TestMethod]
        public void Test2_Compareのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("MailAddressConfirm").GetPropErrors();

            model.MailAddress = "staff1@nekoni.net";
            model.MailAddressConfirm = "staff1@nekoni.net";
            var results = check.Invoke();
            results.Count().Is(0);

            model.MailAddressConfirm = "staff2@nekoni.net";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckCompare, "メールアドレス"));
        }

        [TestMethod]
        public void Test2_Urlのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("PageUrl").GetPropErrors();

            model.PageUrl = "http://google.com";
            var results = check.Invoke();
            results.Count().Is(0);

            model.PageUrl = "jjj";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckUrl, "PageUrl"));
        }

        [TestMethod]
        public void Test2_EnumDataTypeのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("KoyouKbn").GetPropErrors();

            model.KoyouKbn = ((int)KoyouKbn.Seiki).ToString();
            var results = check.Invoke();
            results.Count().Is(0);

            model.KoyouKbn = "99";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.CheckEnumDataType, "雇用区分"));
        }

        [TestMethod]
        public void Test2_RegularExpressionのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("MailAddress2").GetPropErrors();

            model.MailAddress2 = string.Empty;
            var results = check.Invoke();
            results.Count().Is(0);

            model.MailAddress2 = "staff1@nekoni.net";
            results = check.Invoke();
            results.Count().Is(0);

            model.MailAddress2 = "staff1@nekoni.com";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.InvalidDomainName, "MailAddress2"));
        }

        [TestMethod]
        public void Test2_CheckValidのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation("SyainNoIsNotUnique").GetPropErrors();

            model.SyainNoIsNotUnique = false;
            var results = check.Invoke();
            results.Count().Is(0);

            model.SyainNoIsNotUnique = true;
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(string.Format(ErrorMessage.NotUnique, "社員番号"));

        }


        [TestMethod]
        public void Test2_CustomValidationのテスト()
        {
            var model = new Staff2();
            Func<List<ValidationResult>> check = () => model.ForValidation().GetClassLevelErrors();

            model.SyainNo = "1";
            model.MailAddress = "staff1@nekoni.net";
            model.MailAddressConfirm = "staff1@nekoni.net";


            model.HireDate = "2015/04/01";
            model.RetireDate = "2017/06/30";
            var results = check.Invoke();
            results.Count().Is(0);

            model.HireDate = "2015/04/01";
            model.RetireDate = "2013/06/30";
            results = check.Invoke();
            results.Count().Is(1);
            if (results.Count != 1) return;
            var res = results.First();
            res.ErrorMessage.Is(ErrorMessage.InvalidRetireDate);

        }
    }
}
