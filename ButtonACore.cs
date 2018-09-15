using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button type A: action button
//
// Button created by clicking RMB, allows player to make actions.

public class ButtonACore : MonoBehaviour
{
    public Vector3 pos;
    public GameObject obj;

    public EAction action = EAction.none;

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

        if (action == EAction.move)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;

        if (action == EAction.drop_all)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;

        Destroy(gameObject);
    }
}