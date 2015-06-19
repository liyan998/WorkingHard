using UnityEngine;
using System.Collections;

public class LoadingCircle : MonoBehaviour
{

	[SerializeField]
	Card[]
		cards;

	IEnumerator Start ()
	{
		foreach (Card card in cards) {
			card.SetCard (CardType.Back, 0);
			card.SetNumScale (true);
		}
		int cardType = 1;
		int cardNum = 1;
		while (gameObject.activeInHierarchy) {

			foreach (Card card in cards) {
				if (card.type == CardType.Back) {
					//Debug.Log(cardType+" "+cardNum);
					card.SetCard ((CardType)cardType, cardNum);
					card.SetCard (CardType.Back, 0);
					card.FlipForward ();
					cardNum++;
					if (cardNum > 13) {
						cardNum = 1;
						cardType++;
						if (cardType > 4) {
							cardType = 1;
						}
					}
				} else {
					card.FlipBackward ();
				}
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}
