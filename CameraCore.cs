// 26-07-2017

using UnityEngine;
using System.Collections;

public class CameraCore : MonoBehaviour {

	Vector3 mousePos;
	public Vector3 diff;
	GameObject plr;

	void Start () 
	{
		plr = GameObject.Find("Player");
	}

	void LateUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		diff = mousePos - plr.transform.position;

		transform.position = new Vector3(plr.transform.position.x+diff.x/2, plr.transform.position.y+diff.y/2, -1);

		//transform.position = new Vector3(mousePos.x, mousePos.y, -1);
	}
}
