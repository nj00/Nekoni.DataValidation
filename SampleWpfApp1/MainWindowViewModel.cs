using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Nekoni.DataValidation.Attributes;

namespace SampleWpfApp1
{
    public class MainWindowViewModel: ViewModelBase
    {
        /// <summary>
        /// 社員番号
        /// </summary>
        [Display(Name = "社員番号")]
        [Required]
        [StringLength(10)]
        public string SyainNo
        {
            get
            {
                return _SyainNo;
            }
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
            get
            {
                return _MailAddress;
            }
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
            get
            {
                return _MailAddressConfirm;
            }
            set
            {
                if (_MailAddressConfirm == value) return;
                _MailAddressConfirm = value;
                OnPropertyChanged();
            }
        }
        private string _MailAddressConfirm;


        //public ReactiveProperty<string> MailAddressConfirm { get; } 


        public MainWindowViewModel()
        {
            //MailAddressConfirm = new ReactiveProperty<string>().SetValidateAttribute(() => this.MailAddressConfirm);
        }
    }
}
