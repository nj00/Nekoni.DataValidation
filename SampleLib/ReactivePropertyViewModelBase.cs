using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Nekoni.DataValidation.Context;
using Nekoni.DataValidation.ReactiveProperty;

namespace SampleModels
{
    /// <summary>
    /// ViewModelBase
    /// </summary>
    public class ReactivePropertyViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public CompositeDisposable Disposer { get; private set; } = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyReactivePropertySlim<IEnumerable<ValidationResult>> AllErrors { get; }

        public ReactivePropertyViewModelBase()
        {
            // 全エラーリスト
            AllErrors = this.ForValidation().SetupReactiveProperties()
                .GetAllErrorsObservable()
                .ToReadOnlyReactivePropertySlim().AddTo(Disposer);
        }

        // PropertyChangedとErrorChangedイベントを発行する
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            RaisePropertyChanged(propertyName);
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h == null) return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    Disposer.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~ViewModelBaseWithDisposable()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
