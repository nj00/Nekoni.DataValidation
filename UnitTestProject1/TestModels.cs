using System;
using DataValidation.Attributes;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// 社員
    /// </summary>
    [CheckCustomValidation(typeof(Syain), "CheckRetireDate", ErrorMessageResourceName = "InvalidRetireDate")]
    public class Syain
    {
        /// <summary>
        /// 社員番号
        /// </summary>
        [Display(Name ="社員番号")]
        [CheckRequired]
        [CheckStringLength(10)]
        public string SyainNo { get; set; }

        /// <summary>
        /// 社員番号が重複しているかどうか
        /// </summary>
        [Display(Name = "社員番号")]
        [CheckValid(false, ErrorMessageResourceName = "NotUnique")]
        public bool SyainNoIsNotUnique { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        [Display(Name = "姓")]
        public string FamilyName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [Display(Name = "名")]
        public string FirstName { get; set; }

        /// <summary>
        /// 社員名
        /// </summary>
        [Display(Name="社員名")]
        [CheckStringLength(30)]
        public string SyainName
        {
            get
            {
                return $"{this.FamilyName}　{this.FirstName}";
            }
        }

        /// <summary>
        /// 年齢
        /// </summary>
        [Display(Name = "年齢")]
        [CheckRange(16, 80)]
        public string Age { get; set; }

        /// <summary>
        /// 入社年月日
        /// </summary>
        [Display(Name = "入社年月日")]
        [CheckRequired]
        [CheckDate]
        public string HireDate { get; set; }

        /// <summary>
        /// 退社年月日
        /// </summary>
        [Display(Name = "退社年月日")]
        [CheckDate]
        public string RetireDate { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [CheckRequired]
        [CheckEmailAddress]
        public string MailAddress { get; set; }

        [Display(Name = "メールアドレス")]
        [CheckRequired]
        [CheckCompare("MailAddress")]
        public string MailAddressConfirm { get; set; }

        [CheckRegularExpression("^.+?@cosmo.cc", ErrorMessageResourceName = "InvalidDomainName")]
        public string MailAddress2 { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [CheckUrl()]
        public string PageUrl { get; set; }

        /// <summary>
        /// 雇用区分
        /// </summary>
        [Display(Name = "雇用区分")]
        [CheckEnumDataType(typeof(KoyouKbn))]
        public string KoyouKbn { get; set; }



        /// <summary>
        /// 退職年月日の検証メソッド
        /// </summary>
        /// <param name="syain">このクラスのインスタンス</param>
        /// <param name="context">コンテキスト</param>
        /// <returns></returns>
        public static ValidationResult CheckRetireDate(Syain syain, ValidationContext context) 
        {
            if (string.IsNullOrEmpty(syain.RetireDate)) return ValidationResult.Success;
            if (string.IsNullOrEmpty(syain.HireDate)) return ValidationResult.Success;
            DateTime retire;
            if (!DateTime.TryParse(syain.RetireDate, out retire)) return ValidationResult.Success;
            DateTime hire;
            if (!DateTime.TryParse(syain.HireDate, out hire)) return ValidationResult.Success;


            if (retire > hire) return ValidationResult.Success;

            // エラー
            return new ValidationResult(null);
        }
    }

    /// <summary>
    /// 雇用区分
    /// </summary>
    public enum KoyouKbn
    {
        Seiki,
        Haken,
        Syukou
    }

}
