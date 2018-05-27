// 19-08-2017

using UnityEngine;
using System.Collections;

public class SlashCore : MonoBehaviour
{
	public GameObject parent;
    public int team;

	Vector3 v;
	Transform target;

	public bool directionRight;
    public bool alive = true; // fixes multiple hit bug

	void Start () 
	{
        Destroy(gameObject,0.25f);

        team = parent.GetComponent<CritterCore>().team;
        directionRight = parent.GetComponent<CritterCore>().directionRight;

		v.Set(0,2f,0);

		target = parent.GetComponent<Transform>();

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
        if (other.gameObject.GetComponent<CritterCore>() != null) // fixes null reference exception bug
        if (alive == true)
		if (other.gameObject.GetComponent<CritterCore>().team != team)
		{
			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;
            
            alive = false;
            Destroy(gameObject,0.0f);
		}
	}
}
