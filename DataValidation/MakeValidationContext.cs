using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nekoni.DataValidation.Context
{
    /// <summary>
    /// ValidationContext作成
    /// </summary>
    public static class MakeValidationContextExtensions
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
