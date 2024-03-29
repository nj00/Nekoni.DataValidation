﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace Nekoni.DataValidation
{
    /// <summary>
    /// 基本設定クラス
    /// </summary>
    public static class ValidationConfig
    {
        /// <summary>
        /// 既定のエラーメッセージリソースを決定する関数
        /// </summary>
        public static Func<ValidationAttribute, Type> DefaultErrorMessageResourceTypeProvider { get; set; }

        /// <summary>
        /// 既定のエラーメッセージリソース名を決定する関数
        /// </summary>
        public static Func<ValidationAttribute, string> DefaultErrorMessageResourceNameProvider { get; set; } =
            (va) => va.GetType().Name.Replace("Attribute", string.Empty);

        /// <summary>
        /// 最優先検証を行うValidationAttributeを決定する関数。エラーが有ったらその他の検証は行わない。
        /// </summary>
        public static Func<IEnumerable<Type>> FirstValidationAttributesProvider { get; set; } = 
            () => new[] { typeof(RequiredAttribute) };

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
            va.SetupErrorMessageResource(errMsgResourceType, ValidationConfig.DefaultErrorMessageResourceNameProvider.Invoke(va));
        }
        /// <summary>
        /// 検証属性にメッセージリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        /// <param name="errMsgResourceTypeProvider">エラーメッセージが設定されているリソースを決定するFuncデリゲート</param>
        /// <param name="errMsgResourceNameProvider">エラーメッセージリソース名を決定するFuncデリゲート</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va,
            Func<ValidationAttribute, Type> errMsgResourceTypeProvider,
            Func<ValidationAttribute, string> errMsgResourceNameProvider)
        {
            va.SetupErrorMessageResource(errMsgResourceTypeProvider.Invoke(va), errMsgResourceNameProvider.Invoke(va));
        }
        /// <summary>
        /// 検証属性に既定のリソースを設定する拡張メソッド
        /// </summary>
        /// <param name="va">検証属性</param>
        public static void SetupErrorMessageResource(this ValidationAttribute va)
        {
            va.SetupErrorMessageResource(
                ValidationConfig.DefaultErrorMessageResourceTypeProvider, 
                ValidationConfig.DefaultErrorMessageResourceNameProvider);
        }
    }
}
