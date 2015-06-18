using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ComponetDrag : 
    MonoBehaviour, 
    IBeginDragHandler,
    IDragHandler, 
    IEndDragHandler, 
    IPointerDownHandler
{

    RectTransform mDragTarget ;

    Vector3 mStart;

    Vector2 absloPosition;

    public UnityEvent onBeginDrag;

	// Use this for initialization
	void Start () {
	      mDragTarget =  GetComponent<RectTransform>();
          mStart = mDragTarget.position;
	}
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag.Invoke();
    }

    public void OnDrag(PointerEventData data)
    {      
        mDragTarget.position = data.position - absloPosition;            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mDragTarget.position = mStart;
    }

    public void OnPointerDown(PointerEventData eventData)
    {       
        absloPosition = eventData.position;
    }

   
}
