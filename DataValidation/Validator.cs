﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Nekoni.DataValidation.Validator
{
    /// <summary>
    /// ValidationContext 拡張メソッド（検証メソッド）
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// 検証結果を追加する
        /// </summary>
        /// <param name="errors">エラー結果のリスト</param>
        /// <param name="context">検証コンテキスト</param>
        /// <param name="va">検証属性</param>
        /// <param name="value">検査対象の値</param>
        private static void AddErrors(this IList<ValidationResult> errors, ValidationContext context, ValidationAttribute va, object value)
        {
            va.SetupErrorMessageResource();
            var result = va.GetValidationResult(value, context);
            if (result != ValidationResult.Success)
            {
                errors.Add(result);
            }
        }

        /// <summary>
        /// 全プロパティの必須検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllRequiredErrors(this ValidationContext context)
        {
            var ret = new List<ValidationResult>();
            var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items);
            var props = TypeDescriptor.GetProperties(context.ObjectInstance);
            foreach (var prop in props.Cast<PropertyDescriptor>())
            {
                var required = prop.Attributes.Cast<Attribute>().OfType<RequiredAttribute>().FirstOrDefault();
                if (required != null)
                {
                    ctx.MemberName = prop.Name;
                    ret.AddErrors(ctx, required, prop.GetValue(ctx.ObjectInstance));
                }
            }
            return ret;
        }

        /// <summary>
        /// プロパティの検証エラー取得
        /// </summary>
        /// <param name="context">ValidationContext MemberNameプロパティが指定されていない場合は空のコレクションを返す</param>
        /// <returns></returns>
        private static List<ValidationResult> GetPropErrors(this ValidationContext context, PropertyDescriptor prop)
        {
            var ret = new List<ValidationResult>();
            if (prop == null) return ret;
            var value = prop.GetValue(context.ObjectInstance);
            var validations = prop.Attributes.Cast<Attribute>().OfType<ValidationAttribute>().ToList();

            // 必須チェック
            var required = validations.FirstOrDefault(_ => _ is RequiredAttribute);
            if (required != null)
            {
                ret.AddErrors(context, required, value);
                if (ret.Count() > 0) return ret;
            }

            // その他
            foreach (var attr in validations.Where(_ => _ != required))
            {
                ret.AddErrors(context, attr, value);
            }
            return ret;
        }

        /// <summary>
        /// プロパティの検証エラー取得
        /// </summary>
        /// <param name="context">ValidationContext MemberNameプロパティが指定されていない場合は空のコレクションを返す</param>
        /// <returns></returns>
        public static List<ValidationResult> GetPropErrors(this ValidationContext context)
        {
            var ret = new List<ValidationResult>();
            if (string.IsNullOrEmpty(context.MemberName)) return ret;
            var props = TypeDescriptor.GetProperties(context.ObjectInstance);
            var prop = props.Find(context.MemberName, false);
            ret.AddRange(context.GetPropErrors(prop));
            return ret;
        }

        /// <summary>
        /// プロパティの検証エラー取得
        /// </summary>
        /// <param name="context">ValidationContext</param>
        /// <param name="propetyName">プロパティ名</param>
        /// <returns></returns>
        public static List<ValidationResult> GetPropErrors(this ValidationContext context, string propetyName)
        {
            var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items)
            {
                MemberName = propetyName
            };
            return ctx.GetPropErrors();
        }


        /// <summary>
        /// 全プロパティの検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllPropsErrors(this ValidationContext context)
        {
            var ret = new List<ValidationResult>();
            var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items);
            var props = TypeDescriptor.GetProperties(context.ObjectInstance);
            foreach (var prop in props.Cast<PropertyDescriptor>())
            {
                ctx.MemberName = prop.Name;
                ret.AddRange(ctx.GetPropErrors(prop));
            }
            return ret;
        }

        /// <summary>
        /// クラスレベルの検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetClassLevelErrors(this ValidationContext context)
        {
            var ret = new List<ValidationResult>();
            if (!string.IsNullOrEmpty(context.MemberName)) return ret;
            var value = context.ObjectInstance;
            var validations = TypeDescriptor.GetAttributes(context.ObjectInstance).Cast<Attribute>().OfType<ValidationAttribute>();

            foreach (var attr in validations)
            {
                ret.AddErrors(context, attr, value);
            }
            return ret;
        }

        /// <summary>
        /// 全エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllErrors(this ValidationContext context)
        {
            // 必須エラーが有ったら必須エラーだけ返す
            var requireds = context.GetAllRequiredErrors();
            if (requireds.Count() > 0)
            {
                return requireds;
            }

            var ret = new List<ValidationResult>();
            ret.AddRange(context.GetAllPropsErrors());
            ret.AddRange(context.GetClassLevelErrors());
            return ret;
        }
    }

}
