using UnityEngine;
using System.Collections;

public class ProjectileCore : MonoBehaviour {


	void Start () 
	{
		Destroy(gameObject,1.5f);
	}

	void FixedUpdate ()
	{

	}	

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log("collision");
			Destroy(gameObject,0.0f);
		}
	}
}
