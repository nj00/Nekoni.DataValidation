using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// Urlチェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckUrlAttribute : ValidationAttribute
    {
        // sealedなので、内包する。
        UrlAttribute Attr;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckUrlAttribute() : base()
        {
            this.SetupErrorMessageResource();

            Attr = new UrlAttribute();
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
