using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NormalLoadingCircle : MonoBehaviour
{

	[SerializeField]
	Image[]
		points;

	IEnumerator Start ()
	{
		foreach (Image point in points) {
			point.transform.localScale = Vector3.one * 0.2f;
			point.color = Color.clear;
		}
		while (gameObject.activeInHierarchy) {
			
			foreach (Image point in points) {
				point.transform.localScale = Vector3.one * 0.3f;
				point.color = Color.white;
				iTween.ScaleTo (point.gameObject, Vector3.one * 0.2f, 0.1f * points.Length);
				iTween.ColorTo (point.gameObject, Color.white * 0.1f, 0.1f * points.Length);
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
	
	public void Roll ()
	{
		StartCoroutine (Start ());
	}

}
