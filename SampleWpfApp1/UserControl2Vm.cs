using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;
using Nekoni.DataValidation;

namespace SampleWpfApp1
{
    public class UserContorol2Vm: ViewModelBase
    {
        /// <summary>
        /// 社員番号
        /// </summary>
        [Display(Name = "社員番号")]
        [Required]
        [StringLength(10)]
        public ReactiveProperty<string> SyainNo { get; private set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Display(Name = "メールアドレス")]
        [Required]
        [EmailAddress]
        public ReactiveProperty<string> MailAddress { get; private set; }


        [Display(Name = "メールアドレス（確認）")]
        [Required]
        [Compare("MailAddress", ErrorMessage = "{0}がメールアドレスと等しくありません。")]
        public ReactiveProperty<string> MailAddressConfirm { get; private set; }


        public UserContorol2Vm()
        {
            SyainNo = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => SyainNo);
            MailAddress = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => MailAddress);
            MailAddressConfirm = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => MailAddressConfirm);

            // todo: 最新のリストにする方法がわからない...
            AllErrors = SyainNo.ObserveErrorChanged
                .Merge(MailAddress.ObserveErrorChanged)
                .Merge(MailAddressConfirm.ObserveErrorChanged)
                .Where(e => e != null)
                .SelectMany(e => e.OfType<ValidationResult>())
                .ToReadOnlyReactiveCollection();
        }

        public new ReadOnlyReactiveCollection<ValidationResult> AllErrors { get; }
    }
}
