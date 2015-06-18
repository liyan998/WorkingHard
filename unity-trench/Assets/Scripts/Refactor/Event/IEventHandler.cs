using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvent
{

    public interface IEventHandler
    {
        void ActionEvent(GameEvent.CEvent actionevent);
    }

}