using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation.Attributes
{
    /// <summary>
    /// カスタムチェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, 
	AllowMultiple = true)]
    public class CheckCustomValidationAttribute : ValidationAttribute
    {
        // CustomValidationAttributeはsealedなので、内包する。
        CustomValidationAttribute Attr;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckCustomValidationAttribute(Type validatorType, string method) : base()
        {
            this.SetupMessageResource();

            Attr = new CustomValidationAttribute(validatorType, method);
        }

        /// <summary>
        /// エラーメッセージの設定
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            Attr.ErrorMessage = this.ErrorMessage;
            Attr.ErrorMessageResourceType = this.ErrorMessageResourceType;
            Attr.ErrorMessageResourceName = this.ErrorMessageResourceName;

            return Attr.FormatErrorMessage(name);
        }

        /// <summary>
        /// データチェック
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>検証成功時はtrue</returns>
        public override bool IsValid(object value)
        {
            return Attr.IsValid(value);
        }
    }
}
