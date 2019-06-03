using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nekoni.DataValidation.ForValidation
{
    /// <summary>
    /// ValidationContext作成
    /// </summary>
    public static class MakeValidationContextExtensions
    {
        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider, IDictionary<object, object> items,
            string memberName, string displayName)
        {
            return new ValidationContext(instance, serviceProvider, items) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider, IDictionary<object, object> items,
            string memberName)
        {
            return new ValidationContext(instance, serviceProvider, items) { MemberName = memberName };
        }
        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            return new ValidationContext(instance, serviceProvider, items);
        }

        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider)
        {
            return new ValidationContext(instance, serviceProvider, null);
        }
        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider,
            string memberName, string displayName)
        {
            return new ValidationContext(instance, serviceProvider, null) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext ForValidation(this object instance, IServiceProvider serviceProvider,
            string memberName)
        {
            return new ValidationContext(instance, serviceProvider, null) { MemberName = memberName };
        }

        public static ValidationContext ForValidation(this object instance, IDictionary<object, object> items)
        {
            return new ValidationContext(instance, null, items);
        }
        public static ValidationContext ForValidation(this object instance, IDictionary<object, object> items,
            string memberName, string displayName)
        {
            return new ValidationContext(instance, null, items) { MemberName = memberName, DisplayName = displayName };
        }
        public static ValidationContext ForValidation(this object instance, IDictionary<object, object> items,
            string memberName)
        {
            return new ValidationContext(instance, null, items) { MemberName = memberName };
        }

        public static ValidationContext ForValidation(this object instance)
        {
            return new ValidationContext(instance);
        }

        public static ValidationContext ForValidation(this object instance, string memberName, string displayName)
        {
            return new ValidationContext(instance) { MemberName = memberName, DisplayName = displayName };
        }

        public static ValidationContext ForValidation(this object instance, string memberName)
        {
            return new ValidationContext(instance) { MemberName = memberName };
        }
    }
}
