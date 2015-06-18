using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameEvent
{
    /// <summary>
    /// 事件系统
    /// </summary>
    public class EventSystem
    {
        #region
        private static EventSystem mInstance;

        public static EventSystem Instance
        {

            get
            {
                if (mInstance == null)
                {
                    mInstance = new EventSystem();
                    mInstance.init();
                }

                return mInstance;
            }
        }

        private void init()
        {
            mAllEventHandler = new Dictionary<int, List<IEventHandler>>();
        }

        private Dictionary<int, List<IEventHandler>> mAllEventHandler;
        #endregion


        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="handler"></param>
        public void RegisterEvent(int EventId, IEventHandler handler)
        {
            if (mAllEventHandler.ContainsKey(EventId))
            {
                mAllEventHandler[EventId].Add(handler);
            }
            else
            {
                List<IEventHandler> allhandler = new List<IEventHandler>();
                allhandler.Add(handler);
                mAllEventHandler.Add(EventId, allhandler);
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="handler"></param>
        public void RemoveRegisterEVent(int eventid, IEventHandler handler)
        {
            if(mAllEventHandler.ContainsKey(eventid))
            {
                List<IEventHandler> allhandler = mAllEventHandler[eventid];

                foreach(IEventHandler ieh in allhandler)
                {
                    if (ieh == handler)
                    {
                        allhandler.Remove(handler);

                        if(allhandler.Count == 0)
                        {
                            mAllEventHandler.Remove(eventid);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 下发事件
        /// </summary>
        /// <param name="gameevent"></param>
        public void DispatchEvent(CEvent gameevent)
        {
            if(!mAllEventHandler.ContainsKey(gameevent.mEventId))
            {
                return;
            }

            foreach (IEventHandler ieh in mAllEventHandler[gameevent.mEventId])
            {
                ieh.ActionEvent(gameevent);
            }
        }
    }
}