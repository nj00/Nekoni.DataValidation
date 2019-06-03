using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// Boolean型チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CheckBooleanAttributeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 検証が正しいとする値
        /// </summary>
        public bool ValidValue { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="validValue"></param>
        public CheckBooleanAttributeAttribute(bool validValue)
        {
            this.ValidValue = validValue;

            this.SetupErrorMessageResource();
        }

        /// <summary>
        /// データチェック
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>検証成功時はtrue</returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrEmpty(value.ToString())) return true;

            bool suggestValue;
            if (!bool.TryParse(value.ToString(), out suggestValue)) return true;

            return suggestValue == this.ValidValue;
        }
    }
}
