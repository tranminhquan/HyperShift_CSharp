using System.ComponentModel;
using HyberShift_CSharp.Model;
using Prism.Commands;

// import namespace of model

namespace HyberShift_CSharp.ViewModel
{
    internal class
        FooViewModel : INotifyPropertyChanged // this interface is for automatically change property on the view
    {
        private readonly FooModel obj;
        private DelegateCommand objCommand;

        // constructor
        public FooViewModel()
        {
            obj = new FooModel();
            objCommand =
                new DelegateCommand(obj.Method1,
                    obj.IsValidAttribute1); // delegate to Method1 as Execute() and IsValidAttribute1 as CanExecute()
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // For example, this method is invoked by a button from view
        // and use Method1 of obj (from Model)
        public void Method1()
        {
            obj.Method1();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("label1")); // this will automatically update label1
        }
    }
}