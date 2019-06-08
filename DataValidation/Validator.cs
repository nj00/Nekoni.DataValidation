using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Nekoni.DataValidation.Validator
{
    /// <summary>
    /// コンテキストクラス
    /// </summary>
    public class ForValidation
    {
        /// <summary>
        /// 検証属性のDictionary
        /// </summary>
        private static Dictionary<Type, Dictionary<string, List<ValidationAttribute>>> TypeAttributes =
            new Dictionary<Type, Dictionary<string, List<ValidationAttribute>>>();

        private static string ClassLevelKey = "@";

        private Dictionary<string, object> PropertyValues = new Dictionary<string, object>();

        public ValidationContext Context { get; set; }


        public ForValidation(ValidationContext context)
        {
            Context = context;

            // Typeの情報収集
            AddTypeInfo(context.ObjectType);

            // 値リスト(検証属性のあるものだけ)
            foreach (var prop in context.ObjectType.GetProperties())
            {
                if (GetValidationAttributes(prop.Name).Count() == 0) continue;

                PropertyValues.Add(prop.Name, prop.GetValue(Context.ObjectInstance));
            }
        }

        /// <summary>
        /// Typeの情報収集
        /// </summary>
        /// <param name="type"></param>
        private void AddTypeInfo(Type type)
        {
            if (TypeAttributes.ContainsKey(type)) return;

            void AddAttributes(string key, IEnumerable<ValidationAttribute> validationAttributes)
            {
                if (TypeAttributes.ContainsKey(type))
                {
                    TypeAttributes[type].Add(key, validationAttributes.ToList());
                }
                else
                {
                    var dic = new Dictionary<string, List<ValidationAttribute>>();
                    dic.Add(key, validationAttributes.ToList());
                    TypeAttributes.Add(type, dic);
                }
            }

            // Class Attributes
            var classAttributes = type.GetCustomAttributes(true).OfType<ValidationAttribute>();
            AddAttributes(ClassLevelKey, classAttributes);

            // Property Attributes
            foreach (var prop in type.GetProperties().Cast<PropertyInfo>())
            {
                var propAttributes = prop.GetCustomAttributes().OfType<ValidationAttribute>();
                AddAttributes(prop.Name, propAttributes);
            }
        }

        public List<ValidationAttribute> GetClassValidationAttributes()
        {
            return TypeAttributes[Context.ObjectType][ClassLevelKey];
        }

        public List<ValidationAttribute> GetValidationAttributes(string propertyName)
        {
            var typeAttributes = TypeAttributes[Context.ObjectType];
            if (!typeAttributes.ContainsKey(propertyName)) return new List<ValidationAttribute>();
            return typeAttributes[propertyName];
        }

        public Dictionary<string, object> GetTargetPropValues()
        {
            return PropertyValues;
        }

        public object GetTargetPropValue(string propertyName)
        {
            if (!PropertyValues.ContainsKey(propertyName)) return null;
            return PropertyValues[propertyName];
        }
    }

    /// <summary>
    /// 検証メソッド
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// 検証結果を追加する
        /// </summary>
        /// <param name="errors">エラー結果のリスト</param>
        /// <param name="context">検証コンテキスト</param>
        /// <param name="va">検証属性</param>
        /// <param name="value">検査対象の値</param>
        public static void AddErrors(this IList<ValidationResult> errors, ValidationContext context, ValidationAttribute va, object value)
        {
            va.SetupErrorMessageResource();
            var result = va.GetValidationResult(value, context);
            if (result != ValidationResult.Success)
            {
                errors.Add(result);
            }
        }

        /// <summary>
        /// 全プロパティの最優先検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllFisrtValidationErrors(this ForValidation forValidation)
        {
            var firsts = Configuration.FirstValidationAttributesProvider.Invoke();
            var context = forValidation.Context;
            var ret = new List<ValidationResult>();
            foreach (var prop in forValidation.GetTargetPropValues().Keys)
            {
                var value = forValidation.GetTargetPropValue(prop);

                var firstVAs = forValidation.GetValidationAttributes(prop).Where(va => firsts.Contains(va.GetType()));
                foreach (var va in firstVAs)
                {
                    var ctx = new ValidationContext(context.ObjectInstance, context, context.Items);
                    ctx.MemberName = prop;
                    ret.AddErrors(ctx, va, value);
                }
            }
            return ret;
        }


        /// <summary>
        /// プロパティの検証エラー取得
        /// </summary>
        /// <param name="forValidation"></param>
        /// <param name="propertyName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetPropErrors(this ForValidation forValidation, string propertyName, string displayName)
        {
            var ret = new List<ValidationResult>();
            var validations = forValidation.GetValidationAttributes(propertyName);
            if (validations.Count() == 0) return ret;

            var context = forValidation.Context;
            var ctx = new ValidationContext(context.ObjectInstance, context, context.Items);
            ctx.MemberName = propertyName;
            if (!string.IsNullOrEmpty(displayName))
                ctx.DisplayName = displayName;
            var value = forValidation.GetTargetPropValue(propertyName);


            // 最優先チェック
            var firsts = Configuration.FirstValidationAttributesProvider.Invoke();
            foreach (var attr in validations.Where(va => firsts.Contains(va.GetType())))
            {
                ret.AddErrors(ctx, attr, value);
            }
            if (ret.Count() > 0) return ret;


            // その他
            foreach (var attr in validations.Where(va => !firsts.Contains(va.GetType())))
            {
                ret.AddErrors(ctx, attr, value);
            }
            return ret;
        }

        /// <summary>
        /// プロパティの検証エラー取得
        /// </summary>
        /// <param name="forValidation"></param>
        /// <param name="propertyName"></param>
        public static List<ValidationResult> GetPropErrors(this ForValidation forValidation, string propertyName)
        {
            return forValidation.GetPropErrors(propertyName, string.Empty);
        }

        /// <summary>
        /// 全プロパティの検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetAllPropsErrors(this ForValidation forValidation)
        {
            var ret = new List<ValidationResult>();
            var ctx = forValidation.Context;
            foreach (var prop in forValidation.GetTargetPropValues().Keys)
            {
                ctx.MemberName = prop;
                ret.AddRange(forValidation.GetPropErrors(prop));
            }
            return ret;
        }

        /// <summary>
        /// クラスレベルの検証エラー取得
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ValidationResult> GetClassLevelErrors(this ForValidation forValidation)
        {
            var context = forValidation.Context;
            var ret = new List<ValidationResult>();
            var value = context.ObjectInstance;
            var validations = forValidation.GetClassValidationAttributes();

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
        public static List<ValidationResult> GetAllErrors(this ForValidation forValidation)
        {
            var ret = new List<ValidationResult>();
            ret.AddRange(forValidation.GetAllPropsErrors());
            if (ret.Count() == 0)            
                ret.AddRange(forValidation.GetClassLevelErrors());
            return ret;
        }
    }

}
