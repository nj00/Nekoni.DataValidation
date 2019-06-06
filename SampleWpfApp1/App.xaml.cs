using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nekoni.DataValidation;

namespace SampleWpfApp1
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // エラーメッセージリソースの設定
            Configuration.DefaultErrorMessageResourceType = typeof(ErrorMessage);
            Configuration.DefaultErrorMessageResourceNameProvider = (attr) => {
                var attrName = attr.GetType().Name.Replace("Attribute", string.Empty);
                return attrName.StartsWith("Check") ? attrName : $"Check{attrName}";
            };
        }
    }
}
