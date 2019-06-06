﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.Validation.Attributes
{
    /// <summary>
    /// 正規表現チェック属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CheckRegularExpressionAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckRegularExpressionAttribute(string pattern) : base(pattern)
        {
            this.SetupErrorMessageResource();
        }
    }
}
