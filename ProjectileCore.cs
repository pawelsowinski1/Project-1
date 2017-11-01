// 19-08-2017

using UnityEngine;
using System.Collections;

public class ProjectileCore : MonoBehaviour
{
	public GameObject parent;

    public int team;

	void Start () 
	{
        team = parent.GetComponent<PlayerCore>().team;

		Destroy(gameObject,1.5f);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.gameObject.GetComponent<CritterCore>() != null) // fixes null reference exception bug
		if (other.gameObject.GetComponent<CritterCore>().team != team)
		{
			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;
			Destroy(gameObject,0.0f);
		}
	}
}
