using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Nekoni.Validation.Validator;

namespace Nekoni.Validation.Context
{
    /// <summary>
    /// コンテキストクラス用拡張メソッド
    /// </summary>
    public static class MakeForValidationExtensions
    {
        public static ForValidation ForValidation(this object instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            return new ForValidation(new ValidationContext(instance, serviceProvider, items));
        }

        public static ForValidation ForValidation(this object instance, IServiceProvider serviceProvider)
        {
            return new ForValidation(new ValidationContext(instance, serviceProvider, null));
        }

        public static ForValidation ForValidation(this object instance, IDictionary<object, object> items)
        {
            return new ForValidation(new ValidationContext(instance, null, items));
        }

        public static ForValidation ForValidation(this object instance)
        {
            return new ForValidation(new ValidationContext(instance));
        }
    }
}
