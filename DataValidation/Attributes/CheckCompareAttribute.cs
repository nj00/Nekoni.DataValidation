﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Nekoni.Validation.Attributes
{
    /// <summary>
    /// 他のプロパティと同値か比較する属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
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
