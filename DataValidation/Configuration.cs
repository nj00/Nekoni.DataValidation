using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation
{
    /// <summary>
    /// 基本設定クラス
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// 既定のエラーメッセージリソースのSystem.Type
        /// </summary>
        public static Type DefaultErrorMessageResourceType { get; set; }

        /// <summary>
        /// 既定のエラーメッセージリソース名を決定する関数
        /// </summary>
        public static Func<ValidationAttribute, string> ErrorMessageResourceNameProvider { get; set; } = 
            (va) =>  va.GetType().Name
            .Replace("Check", String.Empty)
            .Replace("Attribute", string.Empty) + "ErrorMessage";


        /// <summary>
        /// 検証属性にリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        public static void SetupMessageResource(this ValidationAttribute va)
        {
            if (!string.IsNullOrEmpty(va.ErrorMessage)) return;

            va.ErrorMessageResourceType = va.ErrorMessageResourceType ?? DefaultErrorMessageResourceType;
            va.ErrorMessageResourceName = va.ErrorMessageResourceName ?? ErrorMessageResourceNameProvider(va);
        }
    }
}
