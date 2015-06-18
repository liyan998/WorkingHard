using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkEye : MonoBehaviour
{
    [SerializeField]
    Image
        eye;
    [SerializeField]
    Sprite[]
        blinkFrames;
    [SerializeField]
    float
        interval = 5f;
    //float frameTime = 0.02f;
    [SerializeField]
    bool
        isAllowDoubleBlink = true;
    bool isDoingIdleAnim = false;

    public void StartIdleAnim()
    {
        isDoingIdleAnim = true;
        StartCoroutine(DoIdleAnim());
    }

    public void StopIdleAnim()
    {
        isDoingIdleAnim = false;
        StopCoroutine(DoIdleAnim());
        OpenEye();
    }

    public void Blink()
    {
        StartCoroutine(DoBlink());
    }

    public void CloseEye()
    {
        StartCoroutine(DoCloseEye());
    }

    public void OpenEye()
    {
        StartCoroutine(DoOpenEye());
    }

    IEnumerator DoIdleAnim()
    {
        while (isDoingIdleAnim)
        {
            yield return new WaitForSeconds(interval + Random.Range(-1f, 1f));
            Blink();
            yield return new WaitForSeconds(interval + Random.Range(-1f, 1f));
            yield return StartCoroutine(DoBlink());
            if (isAllowDoubleBlink)
            {
                Blink();
            }
        }
    }

    IEnumerator DoBlink()
    {
        yield return StartCoroutine(DoCloseEye());
        yield return new WaitForSeconds(Time.deltaTime);
        yield return StartCoroutine(DoOpenEye());
    }

    IEnumerator DoCloseEye()
    {
        eye.sprite = blinkFrames [0];
        eye.gameObject.SetActive(true);
        //Debug.Log("Blink");
        yield return new WaitForSeconds(Time.deltaTime);
        eye.sprite = blinkFrames [1];
        yield return new WaitForSeconds(Time.deltaTime);
        eye.sprite = blinkFrames [2];
    }

    IEnumerator DoOpenEye()
    {
        eye.sprite = blinkFrames [1];
        yield return new WaitForSeconds(Time.deltaTime);
        eye.sprite = blinkFrames [0];
        yield return new WaitForSeconds(Time.deltaTime);
        eye.gameObject.SetActive(false);
    }

}
