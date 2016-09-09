using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace POS_ViewsLibrary
{
    public class ObservableObject : INotifyPropertyChanged
    {

        //     protected virtual void SetProperty<T>(ref T member, T val,
        //[CallerMemberName] string propertyName = null)
        //     {
        //         if (object.Equals(member, val)) return;

        //         member = val;
        //         PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //     }

        //     protected virtual void OnPropertyChanged(string propertyName)
        //     {
        //         PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //     }

        //     public event PropertyChangedEventHandler PropertyChanged = delegate { };
        // }


   // public event PropertyChangedEventHandler PropertyChanged;

   //     protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
   //     {
   //         PropertyChangedEventHandler handler = PropertyChanged;
   //         if (handler != null)
   //         {
   //             handler(this, new PropertyChangedEventArgs(propertyName));
   //         }
   //     }
         // boiler-plate
       
   public event PropertyChangedEventHandler PropertyChanged;

   public void RaisePropertyChanged(string property)
   {
       if (PropertyChanged != null)
       {
           PropertyChanged(this, new PropertyChangedEventArgs(property));
       }

   }
    }
}

