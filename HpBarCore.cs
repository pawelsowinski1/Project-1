//20-11-2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCore : MonoBehaviour 
{
    // ================= HP BAR CORE ===================

    public GameObject parent;

    Vector3 v1 = new Vector3(0,1.7f,0); // hpbar position

    // =================================================

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        float a;

        a = parent.GetComponent<CritterCore>().hp / parent.GetComponent<CritterCore>().hpmax;

        transform.localScale = new Vector3(2.5f * a, 0.2f, 1f);

        if (parent.GetComponent<CritterCore>().hp <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            if (parent.GetComponent<CritterCore>().hp <= -100)
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        transform.position = parent.transform.position + v1;

	}
}
