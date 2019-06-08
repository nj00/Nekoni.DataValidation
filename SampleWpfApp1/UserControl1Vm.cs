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
            set
            {
                if (_SyainNo == value) return;
                _SyainNo = value;
                OnPropertyChanged();
            }
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
            set
            {
                if (_MailAddress == value) return;
                _MailAddress = value;
                OnPropertyChanged();
            }
        }
        private string _MailAddress;

        [Display(Name = "メールアドレス（確認）")]
        [Required]
        [Compare("MailAddress", ErrorMessage="{0}がメールアドレスと等しくありません。")]
        public string MailAddressConfirm
        {
            get => _MailAddressConfirm;
            set
            {
                if (_MailAddressConfirm == value) return;
                _MailAddressConfirm = value;
                OnPropertyChanged();
            }
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
            set
            {
                if (_KoyouKbn == value) return;
                _KoyouKbn = value;
                OnPropertyChanged();
            }
        }
        private string _KoyouKbn;

        public UserContorol1Vm(): base()
        {
            RaiseErrorChanged(string.Empty);
        }
    }
}
