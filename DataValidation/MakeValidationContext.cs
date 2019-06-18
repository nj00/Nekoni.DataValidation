using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Nekoni.DataValidation.Validator;

namespace Nekoni.DataValidation.Context
{
    /// <summary>
    /// コンテキストクラス用拡張メソッド
    /// </summary>
    public static class MakeForValidationExtensions
    {
        public static ForValidation ForValidation(this INotifyPropertyChanged instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            return new ForValidation(new ValidationContext(instance, serviceProvider, items));
        }

        public static ForValidation ForValidation(this INotifyPropertyChanged instance, IServiceProvider serviceProvider)
        {
            return new ForValidation(new ValidationContext(instance, serviceProvider, null));
        }

        public static ForValidation ForValidation(this INotifyPropertyChanged instance, IDictionary<object, object> items)
        {
            return new ForValidation(new ValidationContext(instance, null, items));
        }

        public static ForValidation ForValidation(this INotifyPropertyChanged instance)
        {
            return new ForValidation(new ValidationContext(instance));
        }
    }
}
