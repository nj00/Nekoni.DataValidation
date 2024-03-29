﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Nekoni.DataValidation.Context;
using Nekoni.DataValidation.Validator;

namespace SampleModels
{
    /// <summary>
    /// ViewModelBase
    /// see: https://blog.okazuki.jp/entry/20100418/1271594953
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ViewModelBase()
        {
            _AllErrors = new List<ValidationResult>();

            RaiseErrorChanged(string.Empty);
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            var result = _AllErrors.Where(_ => _.MemberNames.FirstOrDefault() == propertyName);
            if (result.Count() > 0)
                return result;
            else
                return null;
        }

        // PropertyChangedとErrorChangedイベントを発行する
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            RaisePropertyChanged(propertyName);
            RaiseErrorChanged(propertyName);

            // HasErrorsプロパティにも変更があったことを通知する
            RaisePropertyChanged("AllErrors");
            RaisePropertyChanged("HasErrors");
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h == null) return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaiseErrorChanged(string propertyName)
        {
            _AllErrors = this.ForValidation().GetAllErrors().ToList();

            var h = ErrorsChanged;
            if (h == null) return;
            h(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void SetPropertyValue<T>(ref T backingStore, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, newValue)) return;

            backingStore = newValue;
            OnPropertyChanged(propertyName);
        }

        // オブジェクトにエラーがあったらtrue返す
        public bool HasErrors
        {
            get
            {
                return AllErrors.Count > 0;
            }
        }

        // 全エラー
        public List<ValidationResult> AllErrors
        {
            get
            {
                return _AllErrors;
            }
            protected set
            {
                _AllErrors = value;
            }
        }
        private List<ValidationResult> _AllErrors;
    }
}
