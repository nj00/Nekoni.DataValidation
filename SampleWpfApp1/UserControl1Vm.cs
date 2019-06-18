using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SampleWpfApp1
{
    public class UserContorol1Vm: ViewModelBase
    {
        /// <summary>
        /// 社員番号
        /// </summary>
        [Display(Name = "社員番号")]
        [Required]
        [StringLength(10)]
        public string SyainNo
        {
            get => _SyainNo;
            set => SetPropertyValue(ref _SyainNo, value);
        }
        private string _SyainNo;

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Display(Name = "メールアドレス")]
        [Required]
        [EmailAddress]
        public string MailAddress {
            get =>  _MailAddress;
            set => SetPropertyValue(ref _MailAddress, value);
        }
        private string _MailAddress;

        [Display(Name = "メールアドレス（確認）")]
        [Required]
        [Compare("MailAddress", ErrorMessage="{0}がメールアドレスと等しくありません。")]
        public string MailAddressConfirm
        {
            get => _MailAddressConfirm;
            set => SetPropertyValue(ref _MailAddressConfirm, value);
        }
        private string _MailAddressConfirm;

        /// <summary>
        /// 雇用区分
        /// </summary>
        [Display(Name = "雇用区分")]
        [EnumDataType(typeof(KoyouKbn))]
        public string KoyouKbn
        {
            get => _KoyouKbn;
            set => SetPropertyValue(ref _KoyouKbn, value);
        }
        private string _KoyouKbn;

    }
}
