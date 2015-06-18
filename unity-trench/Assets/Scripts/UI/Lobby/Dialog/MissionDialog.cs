using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MissionDialog : Dialog
{
	[SerializeField]
	Transform
		content;
	[SerializeField]
	MissionInfoItem
		mission;
	List<MissionInfoItem> missions = new List<MissionInfoItem> ();

//	public override void Init ()
//	{
//		if (missions.Count == 0) {
//			Initial (new StageMissionData[] {
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData ()
//			});
//		} else {
//			Refresh (new StageMissionData[] {
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData (),
//				new StageMissionData ()
//			});
//		}
//	}

	public void Initial (StageMissionData[] missionData)
	{
		if (missions.Count > 0) {
			foreach (MissionInfoItem item in missions) {
				Destroy (item.gameObject);
			}
			missions.Clear ();
		}

		foreach (StageMissionData data in missionData) {
			MissionInfoItem item = Instantiate (mission, content.position, mission.transform.rotation) as MissionInfoItem;
			item.transform.SetParent (content);
            item.transform.localScale = Vector3.one;
            //item.GetComponent<LayoutElement>().minWidth = content.parent.transform.GetComponent<RectTransform>().rect.width;
            //item.GetComponent<LayoutElement>().minHeight = content.parent.transform.GetComponent<RectTransform>().rect.height;

            //item.GetComponent<LayoutElement>().preferredWidth = content.parent.transform.GetComponent<RectTransform>().rect.width;
            //item.GetComponent<LayoutElement>().preferredHeight = content.parent.transform.GetComponent<RectTransform>().rect.height;

            //item.GetComponent<LayoutElement>().flexibleWidth = content.parent.transform.GetComponent<RectTransform>().rect.width;
            //item.GetComponent<LayoutElement>().flexibleHeight = content.parent.transform.GetComponent<RectTransform>().rect.height;

			item.gameObject.SetActive (true);
			item.Init (data);
			missions.Add (item);
		}
	}

	public void Refresh (StageMissionData[] missionData)
	{
		if (missions.Count > 0 && missions.Count == missionData.Length) {
			int i = 0;
			foreach (StageMissionData data in missionData) {
				missions [i].Init (data);
				i++;
			}
		} else {
			Debug.Log ("Wrong mission or data!");
		}
	}
}
