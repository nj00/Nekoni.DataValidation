using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.Validation.Attributes
{
    /// <summary>
    /// 文字列長チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckStringLengthAttribute : StringLengthAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maximumLength">最大長</param>
        public CheckStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            this.SetupErrorMessageResource();
        }
    }
}
