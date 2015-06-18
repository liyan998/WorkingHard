using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TimerEvent : MonoBehaviour
{

    [SerializeField]
    UnityEvent
        events;
    [SerializeField]
    float
        interval = 5f;
    [Header("-1:Eternal")]
    [SerializeField]
    int
        loop = -1;
    bool active = true;

    IEnumerator Start()
    {
        while (loop!=0 && active)
        {
            yield return new WaitForSeconds(interval);
            events.Invoke();
            if (loop > 0)
            {
                loop--;
            }
        }
    }

    public void StartEvent()
    {
        active = true;
        StartCoroutine(Start());
    }

    public void StopEvent()
    {
        active = false;
    }
}
