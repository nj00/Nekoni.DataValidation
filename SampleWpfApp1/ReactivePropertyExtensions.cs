using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Nekoni.DataValidation.Validator;

namespace Nekoni.DataValidation
{
    static class ReactivePropertyExtensions
    {
        /// <summary>
        /// Set validation logic from DataAnnotations attributes.
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="self">Target ReactiveProperty</param>
        /// <param name="selfSelector">Target property as expression</param>
        /// <returns>Self</returns>
        public static ReactiveProperty<T> SetNekoniDataValidationAttribute<T>(this ReactiveProperty<T> self, Expression<Func<ReactiveProperty<T>>> selfSelector)
        {
            var memberExpression = (MemberExpression)selfSelector.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var propName = propertyInfo.Name;
            var displayAttr = propertyInfo.GetCustomAttributes().OfType<DisplayAttribute>().FirstOrDefault();
            var attrs = propertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();
            var context = new ValidationContext(self)
            {
                MemberName = nameof(ReactiveProperty<T>.Value)
            };
            if (displayAttr != null)
                context.DisplayName = displayAttr.Name;

            if (attrs.Count != 0)
            {
                self.SetValidateNotifyError(x =>
                {
                    var validationResults = new List<ValidationResult>();
                    attrs.ForEach(va => validationResults.AddErrors(context, va, x));
                    if (validationResults.Count == 0) return null;

                    return validationResults.Select(vr => new ValidationResult(vr.ErrorMessage, new[] { propName })).ToList();
                });
            }

            return self;
        }
    }
}
