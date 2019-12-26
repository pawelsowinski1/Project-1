using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour 
{
    // ================= HP BAR CORE ===================

    public GameObject parent;
    public GameObject frame;

    Vector3 v1 = new Vector3(0,-0.5f,0); // hpbar position

    // =================================================

    void Awake()
    {
        // this piece of code is in Awake(), because it needs to be executed also if hp bar is deactivated

        //frame = Instantiate(GameCore.Core.progressBarFramePrefab, transform.position, Quaternion.identity);
    }


	void LateUpdate() 
    {

        if (parent)
        {

            float a;

            a = parent.GetComponent<CritterCore>().hp / parent.GetComponent<CritterCore>().hpMax;

            transform.localScale = new Vector3(2.5f * a, 0.2f, 1f);

            if (parent.GetComponent<CritterCore>().hp <= 0)
            {
                GetComponent<SpriteRenderer>().color = Color.red;

                // if parent hp < -100 then hide
                if (parent.GetComponent<CritterCore>().hp <= -100)
                transform.localScale = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            transform.position = parent.transform.position + v1;

            if (frame)
            frame.transform.position = transform.position;
        }
	}
}
