using UnityEngine;
using System.Collections;

public class Bgm : MonoBehaviour
{
	public static Bgm inst;
	public AudioClip logo;
	public AudioClip lobby;
	public AudioClip game;
	public AudioClip win;
	public AudioClip lose;
    public AudioClip battle;

	void Awake ()
	{
		inst = this;
	}
}
