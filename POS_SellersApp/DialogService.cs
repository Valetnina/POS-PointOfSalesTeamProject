using POS_SellersApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace POS_ViewsLibrary
{
    class DialogService
    {
        public void Show(FrameworkElement view, PaimentViewModel ChildVM)
        {
            //Window window = new Window();
            //window.Content = view;
            //window.DataContext = ChildVM;

            //// For closing this dialog using MVVM
            //ChildVM.RequestClose += delegate
            //{
            //    window.Close();
            //};

            //window.Show();
        }
    }
}
