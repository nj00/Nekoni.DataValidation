using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// 文字列または配列の最大長チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckMaxLengthAttribute : MaxLengthAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckMaxLengthAttribute() : base()
        {
            this.SetupErrorMessageResource();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckMaxLengthAttribute(int length) : base(length)
        {
            this.SetupErrorMessageResource();
        }
    }
}
