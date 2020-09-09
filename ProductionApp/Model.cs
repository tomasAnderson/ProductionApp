using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp
{
    class Model : BindableBase
    {
        readonly ObservableCollection<string> myValues = new ObservableCollection<string>();
        public readonly ReadOnlyObservableCollection<string> MyPublicValues;

        public Model() {
            MyPublicValues = new ReadOnlyObservableCollection<string>(myValues);
        }
    }
}
