using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;
using Nekoni.DataValidation;
using Nekoni.DataValidation.Context;
using Nekoni.DataValidation.ReactiveProperty;
using Reactive.Bindings.Extensions;
using Nekoni.DataValidation.Attributes;

namespace SampleModels
{
    public class UserContorol2Vm: ReactivePropertyViewModelBase
    {
        /// <summary>
        /// 社員番号
        /// </summary>
        [Display(Name = "社員番号")]
        [Required]
        [StringLength(10)]
        public ReactiveProperty<string> SyainNo { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 年齢
        /// </summary>
        [Display(Name = "年齢")]
        [Range(16, 80)]
        public ReactiveProperty<string> Age { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 入社年月日
        /// </summary>
        [Display(Name = "入社年月日")]
        [Required]
        [CheckDate]
        public ReactiveProperty<string> HireDate { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Display(Name = "メールアドレス")]
        [Required]
        [EmailAddress]
        public ReactiveProperty<string> MailAddress { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 雇用区分
        /// </summary>
        [Display(Name = "雇用区分")]
        [EnumDataType(typeof(KoyouKbn))]
        public ReactiveProperty<string> KoyouKbn { get; private set; } = new ReactiveProperty<string>();

    }
}
