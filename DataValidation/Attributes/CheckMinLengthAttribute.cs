using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation.Attributes
{
    /// <summary>
    /// 文字列または配列の最小長チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckMinLengthAttribute : MinLengthAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckMinLengthAttribute(int length) : base(length)
        {
            this.SetupErrorMessageResource();
        }
    }
}
