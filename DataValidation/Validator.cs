using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DataValidation
{
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
            var props = TypeDescriptor.GetProperties(context.ObjectInstance);
            foreach (var prop in props.Cast<PropertyDescriptor>())
            {
                var required = prop.Attributes.Cast<Attribute>().OfType<RequiredAttribute>().FirstOrDefault();
                if (required != null)
                {
                    var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items)
                    {
                        MemberName = prop.Name
                    };
                    ret.AddErrors(ctx, required, prop.GetValue(context.ObjectInstance));
                }
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

            var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items)
            {
                MemberName = context.MemberName,
                DisplayName = context.DisplayName ?? string.Empty
            };

            var props = TypeDescriptor.GetProperties(ctx.ObjectInstance);
            var prop = props.Find(ctx.MemberName, false);
            var value = prop.GetValue(ctx.ObjectInstance);
            if (string.IsNullOrEmpty(ctx.DisplayName))
            {
                ctx.DisplayName = prop.DisplayName;
            }
            var validations = prop.Attributes.Cast<Attribute>().OfType<ValidationAttribute>().ToList();

            // 必須チェック
            var required = validations.FirstOrDefault(_ => _ is RequiredAttribute);
            if (required != null)
            {
                ret.AddErrors(ctx, required, value);
                if (ret.Count() > 0) return ret;
            }

            // その他
            foreach(var attr in validations.Where(_ => _ != required))
            {
                ret.AddErrors(ctx, attr, value);
            }
            return ret;
        }



        /// <summary>
        /// 全プロパティの検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllPropsErrors(this ValidationContext context)
        {
            var ret = new List<ValidationResult>();
            var props = TypeDescriptor.GetProperties(context.ObjectInstance);
            foreach (var prop in props.Cast<PropertyDescriptor>())
            {
                var ctx = new ValidationContext(context.ObjectInstance, context.ServiceContainer, context.Items)
                {
                    MemberName = prop.Name
                };
                ret.AddRange(ctx.GetPropErrors());
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


    /// <summary>
    /// ValidationContextを作成する拡張メソッド
    /// </summary>
    public static class ComponentExtensions
    {
        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider, IDictionary<object, object> items, 
            string memberName, string displayName)
        {
            return new ValidationContext(component, serviceProvider, items) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider, IDictionary<object, object> items,
            string memberName)
        {
            return new ValidationContext(component, serviceProvider, items) { MemberName = memberName };
        }
        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            return new ValidationContext(component, serviceProvider, items);
        }

        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider)
        {
            return new ValidationContext(component, serviceProvider, null);
        }
        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider,
            string memberName, string displayName)
        {
            return new ValidationContext(component, serviceProvider, null) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext GetValidationContext(this object component, IServiceProvider serviceProvider,
            string memberName)
        {
            return new ValidationContext(component, serviceProvider, null) { MemberName = memberName };
        }

        public static ValidationContext GetValidationContext(this object component, IDictionary<object, object> items)
        {
            return new ValidationContext(component, null, items);
        }
        public static ValidationContext GetValidationContext(this object component, IDictionary<object, object> items,
            string memberName, string displayName)
        {
            return new ValidationContext(component, null, items) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext GetValidationContext(this object component, IDictionary<object, object> items,
            string memberName)
        {
            return new ValidationContext(component, null, items) { MemberName = memberName };
        }

        public static ValidationContext GetValidationContext(this object component)
        {
            return new ValidationContext(component);
        }

        public static ValidationContext GetValidationContext(this object component, string memberName, string displayName)
        {
            return new ValidationContext(component) { MemberName = memberName, DisplayName = displayName };
        }

        public static ValidationContext GetValidationContext(this object component, string memberName)
        {
            return new ValidationContext(component) { MemberName = memberName };
        }
    }
}
