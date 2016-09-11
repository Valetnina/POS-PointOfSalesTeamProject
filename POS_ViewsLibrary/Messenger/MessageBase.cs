using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ViewsLibrary.Messenger
{
    public class MessageBase
    {
        public Object Target { get; protected set; }

        public Object Sender { get; protected set; }


        public MessageBase() { }

        public MessageBase(Object sender)
        {
            Sender = sender;
   }

    
    public MessageBase(Object sender, Object target)
    {
        Sender = sender;
        Target = target;
    }


}
}
