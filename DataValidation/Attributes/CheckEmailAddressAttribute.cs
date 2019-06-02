using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation.Attributes
{
    /// <summary>
    /// Eメールアドレスチェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckEmailAddressAttribute : ValidationAttribute
    {
        // sealedなので、内包する。
        EmailAddressAttribute Attr;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckEmailAddressAttribute() : base()
        {
            this.SetupErrorMessageResource();

            Attr = new EmailAddressAttribute();
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
