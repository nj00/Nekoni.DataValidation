using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Nekoni.DataValidation.Attributes;

namespace SampleModels
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
        /// 年齢
        /// </summary>
        [Display(Name = "年齢")]
        [Range(16, 80)]
        public string Age
        {
            get => _Age;
            set => SetPropertyValue(ref _Age, value);
        }
        private string _Age;

        /// <summary>
        /// 入社年月日
        /// </summary>
        [Display(Name = "入社年月日")]
        [Required]
        [CheckDate]
        public string HireDate
        {
            get => _HireDate;
            set => SetPropertyValue(ref _HireDate, value);
        }
        private string _HireDate;

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
