using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EdgeLight : MonoBehaviour
{
    [SerializeField]
    Image
        image;
//    [SerializeField]
//    Vector3
//        from;
    [SerializeField]
    Vector3
        to;
    [SerializeField]
    Vector3
        angleAdd;
    [SerializeField]
    float
        time = 2f;
    [SerializeField]
    float
        interval = 10f;
    Vector3 oriPos;

    void Awake()
    {
        oriPos = transform.localPosition;
    }

    void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(EdgeLightAnim());
    }

    IEnumerator EdgeLightAnim()
    {
        yield return new WaitForSeconds(time * 0.5f);
        while (gameObject.activeInHierarchy)
        {
            transform.localPosition = oriPos;
            iTween.MoveTo(gameObject, iTween.Hash("position", oriPos + to, "time", time, "islocal", true));//
            iTween.RotateAdd(gameObject, angleAdd, time);
            image.color = Color.white;
//            iTween.ColorFrom(gameObject, Color.clear, time * 0.2f);
            yield return new WaitForSeconds(time * 0.8f);
            iTween.ColorTo(gameObject, Color.clear, time * 0.2f);
            yield return new WaitForSeconds(time * 0.2f);
            yield return new WaitForSeconds(interval);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        //Gizmos.DrawCube(transform.position, from);
        Gizmos.DrawLine(transform.position, transform.position + to);
    }


}
