using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using Nekoni.DataValidation.Validator;
using Reactive.Bindings;

namespace Nekoni.DataValidation.ReactiveProperty
{
    public static class ForValidationExtensions
    {
        /// <summary>
        /// ReactivePropertyにValidationAttributeを設定するメソッドのMethodInfo
        /// </summary>
        static List<MethodInfo> SetValidationMethodInfos = typeof(ReactivePropertyExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(_ => _.Name == nameof(ReactivePropertyExtensions.SetNekoniDataValidationAttribute)
                    && _.IsGenericMethodDefinition
                    && _.GetParameters()[0].ParameterType.IsGenericType
                    && _.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ReactiveProperty<>)
            ).ToList();

        /// <summary>
        /// 全ReactivePropertyにValidationAttributeによる検証ロジックを設定する
        /// </summary>
        /// <param name="context"></param>
        public static ForValidation SetupReactiveProperties(this ForValidation context)
        {
            var propValues = context.GetTargetPropValues();
            foreach (var prop in propValues)
            {
                var type = prop.Value.GetType();
                if (!type.IsGenericType) continue;

                var rpType = type.GetGenericTypeDefinition();
                if (rpType != typeof(ReactiveProperty<>)) continue;

                // MethodInfoの場合はこう？
                // https://ja.stackoverflow.com/questions/12801/c-type%E5%9E%8B%E3%81%A7%E6%8C%87%E5%AE%9A%E3%81%97%E3%81%9F%E5%9E%8B%E3%81%AB%E5%8B%95%E7%9A%84%E3%82%AD%E3%83%A3%E3%82%B9%E3%83%88%E3%81%99%E3%82%8B%E3%81%AB%E3%81%AF
                var typeParameter = type.GetGenericArguments()[0];
                var methodInfo = SetValidationMethodInfos.First(_ =>
                        _.GetParameters()[1].ParameterType == typeof(PropertyInfo));
                var genericMethod = methodInfo.MakeGenericMethod(new[] { typeParameter });
                genericMethod.Invoke(null, new[] { prop.Value, context.GetTargetPropInfo(prop.Key) });
            }

            return context;
        }

        /// <summary>
        /// 全ReactivePropertyの検証エラーを返すヘルパ
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IObservable<IEnumerable<ValidationResult>> GetAllErrorsObservable(this ForValidation context)
        {
            var targets = context.GetTargetPropValues().Where(_ =>
            {
                var type = _.Value.GetType();
                if (!type.IsGenericType) return false;

                var rpType = type.GetGenericTypeDefinition();
                if (rpType != typeof(ReactiveProperty<>)) return false;

                return true;
            })
            .Select(_ =>
            {
                var type = _.Value.GetType();
                var prop = type.GetProperty("ObserveErrorChanged");
                var observable = prop.GetValue(_.Value) as IObservable<System.Collections.IEnumerable>;
                if (observable == null) return null;
                return observable.Select(e => e?.OfType<ValidationResult>());
            })
            .Where(_ => _ != null);

            return Observable.CombineLatest(targets)
                .Select(_ => _.Where(e => e != null).SelectMany(e => e));
        }
    }
}
