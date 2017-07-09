﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidation.Attributes
{
    /// <summary>
    /// 他のプロパティと同値か比較する属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckCompareAttribute : CompareAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckCompareAttribute(string otherProperty) : base(otherProperty)
        {
            this.SetupMessageResource();
        }
    }
}