using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.DataValidation.Attributes
{
    /// <summary>
    /// 必須チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CheckRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckRequiredAttribute() : base()
        {
            this.SetupErrorMessageResource();
        }
    }
}
