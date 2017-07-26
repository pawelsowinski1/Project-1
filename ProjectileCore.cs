// 26-07-2017

using UnityEngine;
using System.Collections;

public class ProjectileCore : MonoBehaviour
{
	public GameObject parent;

	void Start () 
	{
		Destroy(gameObject,1.5f);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Enemy")
		{
			other.GetComponent<CritterCore>().damageColorIntensity = 1f;
			Destroy(gameObject,0.0f);
		}
	}
}
