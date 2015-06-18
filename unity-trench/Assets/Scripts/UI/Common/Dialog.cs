using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {

	[SerializeField]
	protected Tweener tweener;

	public virtual void Init()
	{

	}

    public virtual void Show()
    {
		tweener.FromTarget ();
        SOUND.Instance.OneShotSound(Sfx.inst.show);
	}

    

    public virtual void Hide()
    {
		tweener.ToTarget (delegate() {
			gameObject.SetActive(false);
		});
        SOUND.Instance.OneShotSound(Sfx.inst.hide);
	}
}
