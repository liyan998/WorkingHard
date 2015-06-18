using UnityEngine;
using System.Collections;

public class MainMenu : ItemScroller
{
    protected override void OnAwake()
    {
        base.OnAwake();
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<TimerEvent>().StopEvent();
        }
    }

    protected override void OnSwitchEnd(System.Collections.Generic.List<GameObject> objects)
    {
        base.OnSwitchEnd(objects);
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<TimerEvent>().StopEvent();
        }
        topObj.GetComponent<TimerEvent>().StartEvent();
    }
}
