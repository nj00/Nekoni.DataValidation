using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// 他のプロパティと同値か比較する属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CheckCompareAttribute : CompareAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckCompareAttribute(string otherProperty) : base(otherProperty)
        {
            this.SetupErrorMessageResource();
        }
    }
}
