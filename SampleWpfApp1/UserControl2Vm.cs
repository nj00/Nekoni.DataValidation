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

namespace SampleWpfApp1
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
        /// メールアドレス
        /// </summary>
        [Display(Name = "メールアドレス")]
        [Required]
        [EmailAddress]
        public ReactiveProperty<string> MailAddress { get; private set; } = new ReactiveProperty<string>();


        [Display(Name = "メールアドレス（確認）")]
        [Required]
        [Compare("MailAddress", ErrorMessage = "{0}がメールアドレスと等しくありません。")]
        public ReactiveProperty<string> MailAddressConfirm { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 雇用区分
        /// </summary>
        [Display(Name = "雇用区分")]
        [EnumDataType(typeof(KoyouKbn))]
        public ReactiveProperty<string> KoyouKbn { get; private set; } = new ReactiveProperty<string>();

    }
}
