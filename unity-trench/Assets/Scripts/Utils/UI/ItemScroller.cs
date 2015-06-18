using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemScroller : MonoBehaviour
{

    [SerializeField]
    Transform[]
        points;
    [SerializeField]
    protected List<GameObject>
        objs;
    [SerializeField]
    float
        tweenTime = 1f;
//    [SerializeField]
//    Button[]
//        switchBtns;
    Dictionary<GameObject,Transform> pointDict = new Dictionary<GameObject, Transform>();
    float touchStart;
    float minSensitivity = 10f;
    float lastTouchPos;
    bool isScolling = false;
    protected GameObject topObj;

    public bool IsTouching
    {
        get
        {
            return isTouching;
        }
        set
        {
            if (isTouching != value)
            {
                isTouching = value;
                if (isTouching)
                {
                    OnTouchStart();
                } else
                {
                    OnTouchEnd();
                }
            }
        }
    }

    bool isTouching = false;

    void Awake()
    {
        InitDict();
        OnAwake();
        ToPoint();
    }

    protected virtual void OnAwake()
    {
    
    }

    void OnDisable()
    {
        IsTouching = false;
        touchStart = 0;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))//Input.touches.Length > 0 ||
        {
//            touchStart = 0;
            IsTouching = true;
        } else
        {
//            touchStart = 0;
            IsTouching = false;
        }
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touches.Length == 1)
        {
            IsTouching = true;
            lastTouchPos=Input.touches[0].position.x;
        } else
        {
            IsTouching = false;
        }
#endif
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchPoint(false);
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchPoint(true);
        }
    }

    void OnTouchStart()
    {
        #if UNITY_EDITOR
        touchStart = Input.mousePosition.x;
        #elif UNITY_ANDROID || UNITY_IPHONE
        touchStart=Input.touches[0].position.x;
        #endif
//        foreach (Button btn in switchBtns)
//        {
//            btn.interactable = false;
//        }
    }

    void OnTouchEnd()
    {
        #if UNITY_EDITOR
        if (Input.mousePosition.x - touchStart > minSensitivity)
        {
            SwitchPoint(true);
        }

        if (touchStart - Input.mousePosition.x > minSensitivity)
        {
            SwitchPoint(false);
        }
        #elif UNITY_ANDROID || UNITY_IPHONE
        if (lastTouchPos - touchStart > minSensitivity)
        {
            SwitchPoint(true);
        }
        
        if (touchStart - lastTouchPos > minSensitivity)
        {
            SwitchPoint(false);
        }
        #endif
        touchStart = 0;
        lastTouchPos = 0;
//        foreach (Button btn in switchBtns)
//        {
//            btn.interactable = true;
//        }
    }

    public void OnSwitchBackBtn()
    {
        if (!isScolling)
        {
            isScolling = true;
            pointDict.Clear();
            GameObject obj = objs [0];
            objs.Remove(obj);
            objs.Add(obj);
            InitDict();
//            ToPoint();
            pointDict.Clear();
            GameObject obj2 = objs [0];
            objs.Remove(obj2);
            objs.Add(obj2);
            InitDict();
            ToPoint();
        }
    }

    public void SwitchPoint(bool isNext)
    {
        if (!isScolling)
        {
            isScolling = true;
            //Debug.Log("SwitchPoint");
            pointDict.Clear();
            if (isNext)
            {
                GameObject obj = objs [0];
                objs.Remove(obj);
                objs.Add(obj);
            } else
            {
                GameObject obj = objs [objs.Count - 1];
                objs.Remove(obj);
                objs.Insert(0, obj);
            }
            InitDict();
            ToPoint();
        }
    }

    void InitDict()
    {
        for (int i=0; i<points.Length; i++)
        {
            pointDict.Add(objs [i], points [i]);
        }
    }

    public void ToPoint()
    {
        foreach (KeyValuePair<GameObject,Transform> pair in pointDict)
        {
            GameObject obj = pair.Key;
            Transform target = pair.Value;
            obj.transform.SetParent(target);
            StartCoroutine(MoveUpdate(obj, target));
            iTween.ScaleTo(obj, target.localScale, tweenTime);
        }
        StartCoroutine(AvailScroller());
        OnSwitchEnd(objs);
    }

    IEnumerator MoveUpdate(GameObject obj, Transform target)
    {
        float timer = 0;
        Vector3 oriPos = obj.transform.position;
        while (timer<tweenTime)
        {
//            iTween.MoveUpdate(obj, iTween.Hash("position", target.position, "time", tweenTime, "islocal", false));
            //obj.transform.position = Vector3.Lerp(oriPos, target.position, (float)(timer / tweenTime));
            obj.transform.position = Vector3.Lerp(obj.transform.position, target.position, (float)(timer / tweenTime));
            yield return 0;//new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }
        obj.transform.position = target.position;
    }

    IEnumerator AvailScroller()
    {
        yield return new WaitForSeconds(tweenTime);
        isScolling = false;
    }

    protected virtual void OnSwitchEnd(List<GameObject> objects)
    {
        topObj = objects [2];
    }
}
