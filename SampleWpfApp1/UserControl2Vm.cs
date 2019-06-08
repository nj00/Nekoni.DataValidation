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

        /// <summary>
        /// 雇用区分
        /// </summary>
        [Display(Name = "雇用区分")]
        [EnumDataType(typeof(KoyouKbn))]
        public ReactiveProperty<string> KoyouKbn { get; private set; }


        public UserContorol2Vm()
        {
            SyainNo = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => SyainNo);
            MailAddress = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => MailAddress);
            MailAddressConfirm = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => MailAddressConfirm);
            KoyouKbn = new ReactiveProperty<string>().SetNekoniDataValidationAttribute(() => KoyouKbn);


            AllErrors = Observable.CombineLatest(
                SyainNo.ObserveErrorChanged.Select(e => e?.OfType<ValidationResult>()),
                MailAddress.ObserveErrorChanged.Select(e => e?.OfType<ValidationResult>()),
                MailAddressConfirm.ObserveErrorChanged.Select(e => e?.OfType<ValidationResult>()),
                KoyouKbn.ObserveErrorChanged.Select(e => e?.OfType<ValidationResult>()),
                (syainNo, mailAddress, mailConfirm, KoyouKbn) =>
                {
                    var list = new List<ValidationResult>();
                    if (syainNo != null) list.AddRange(syainNo);
                    if (mailAddress != null) list.AddRange(mailAddress);
                    if (mailConfirm != null) list.AddRange(mailConfirm);
                    if (KoyouKbn != null) list.AddRange(KoyouKbn);
                    return list;
                }).ToReadOnlyReactiveProperty();
        }

        public new ReadOnlyReactiveProperty<List<ValidationResult>> AllErrors { get; }
    }
}
