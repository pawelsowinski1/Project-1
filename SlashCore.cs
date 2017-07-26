// 26-07-2017

using UnityEngine;
using System.Collections;

public class SlashCore : MonoBehaviour
{
	GameObject obj;
	public GameObject parent;

	Vector3 v;
	Transform target;

	public bool directionRight = true;

	void Start () 
	{
		Destroy(gameObject,0.25f);

		v.Set(0,2f,0);

		obj = GameObject.Find("Player");
		target = obj.GetComponent<Transform>();

		transform.position = target.position + v;

		transform.Rotate(0,0,90f);
	}

	void FixedUpdate()
	{
		if (directionRight == true)
		transform.RotateAround(target.position, new Vector3(0,0,1), -10f);
		else
		transform.RotateAround(target.position, new Vector3(0,0,1), 10f);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Enemy")
		{
			Destroy(gameObject,0.0f);
			other.GetComponent<CritterCore>().damageColorIntensity = 1f;
		}
	}
}
