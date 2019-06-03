using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// 日付チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CheckDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckDateAttribute()
        {
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

            DateTime date;
            if (!DateTime.TryParse(value.ToString(), out date)) return false;

            if (date.Hour != 0 || date.Minute != 0 || date.Second != 0 || date.Millisecond != 0) return false;

            return true;
        }
    }
}
