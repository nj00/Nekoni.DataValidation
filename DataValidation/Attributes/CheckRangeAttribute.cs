using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation.Attributes
{
    /// <summary>
    /// 範囲チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckRangeAttribute : RangeAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="minimum">最小値</param>
        /// <param name="maximum">最大値</param>
        public CheckRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            this.SetupErrorMessageResource();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="minimum">最小値</param>
        /// <param name="maximum">最大値</param>
        public CheckRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            this.SetupErrorMessageResource();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">データ型</param>
        /// <param name="minimum">最小値</param>
        /// <param name="maximum">最大値</param>
        public CheckRangeAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
            this.SetupErrorMessageResource();
        }
    }
}
