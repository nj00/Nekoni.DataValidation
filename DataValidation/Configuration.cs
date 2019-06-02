using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Nekoni.DataValidation
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
        public static Func<ValidationAttribute, string> DefaultErrorMessageResourceNameProvider { get; set; } = 
            (va) =>  va.GetType().Name
            .Replace("Check", String.Empty)
            .Replace("Attribute", string.Empty) + "ErrorMessage";

    }

    /// <summary>
    /// ValidationAttribute 拡張メソッド
    /// </summary>
    public static class ValidationAttributeExtensions
    {
        /// <summary>
        /// 検証属性にメッセージリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        /// <param name="errMsgResourceType">エラーメッセージが設定されているリソースのType</param>
        /// <param name="errMsgResourceName">エラーメッセージリソースのキー名</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va, Type errMsgResourceType, string errMsgResourceName)
        {
            //if (!string.IsNullOrEmpty(va.ErrorMessage)) return;
            // hack: EmailAddressAttributeやUrlAttributeなど、コンストラクタでinternalのDefaultErrorMessageプロパティがセットされている。
            // ValidationAttribute.ErrorMessageプロパティは自身のバッキングストア(_errorMessage)がnullの場合、DefaultErrorMessageを返す。
            // 属性指定でErrorMessageを指定しているかどうかの判定がErrorMessageプロパティではわからないので、リフレクションを使用してバッキングストア
            // に直接アクセスする。
            var vaType = typeof(ValidationAttribute);
            var field = vaType.GetField("_errorMessage", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null) return;
            var errorMessage = field.GetValue(va) as string;
            if (!string.IsNullOrEmpty(errorMessage)) return;

            va.ErrorMessageResourceType = va.ErrorMessageResourceType ?? errMsgResourceType;
            va.ErrorMessageResourceName = va.ErrorMessageResourceName ?? errMsgResourceName;
        }
        /// <summary>
        /// 検証属性にメッセージリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        /// <param name="errMsgResourceType">エラーメッセージが設定されているリソースのType</param>
        /// <param name="errMsgResourceNameProvider">エラーメッセージリソース名を決定するFuncデリゲート</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va, Type errMsgResourceType,
            Func<ValidationAttribute, string> errMsgResourceNameProvider)
        {
            va.SetupErrorMessageResource(errMsgResourceType, errMsgResourceNameProvider.Invoke(va));
        }
        /// <summary>
        /// 検証属性にメッセージリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        /// <param name="errMsgResourceType">エラーメッセージが設定されているリソースのType</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va, Type errMsgResourceType)
        {
            va.SetupErrorMessageResource(errMsgResourceType, Configuration.DefaultErrorMessageResourceNameProvider.Invoke(va));
        }
        /// <summary>
        /// 検証属性に既定のリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va)
        {
            va.SetupErrorMessageResource(Configuration.DefaultErrorMessageResourceType, Configuration.DefaultErrorMessageResourceNameProvider);
        }
    }
}
