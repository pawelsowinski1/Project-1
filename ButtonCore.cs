using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCore : MonoBehaviour
{
    public Vector3 pos;
    public GameObject obj;

    public ActionEnum action = ActionEnum.none;

	void Start ()
    {
		transform.position = Camera.main.WorldToScreenPoint(pos);
	}
	
	void Update ()
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
	}

    public void TaskOnClick()
    {
        GameCore.Core.player.GetComponent<CritterCore>().command = action;
        GameCore.Core.player.GetComponent<CritterCore>().target = obj;

        if (action == ActionEnum.drop)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;

        Destroy(gameObject);
    }
}