using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHerbi {none, chick, deer}

public class HerbiCore : CritterCore
{
    // ========= HERBI CORE ===========

    // A herbivore.

    // parent class:  CritterCore
    // child classes: -

    public EHerbi herbi;

    public void HerbiInitialize()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10;

        switch (herbi)
        {
            case EHerbi.chick:
            {
                name = "Chick";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_chick;
                transform.localScale = new Vector3(0.3f,0.3f,0.3f);

                break;
            }

            case EHerbi.deer:
            {
                name = "Deer";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_deer;
                transform.localScale = new Vector3(1.3f,1.3f,1.3f);
                
                break;
            }
        }
    }


	void Start () 
    {
        team = 0;
        hp = 30f;


    
    	BodyInitialize();
        HerbiInitialize();
        timerMove = 1;
	}
	
	void Update ()
    {
    	CalculateLand();
		PlaceOnGround();
		DamageColorize();

        // enable being carried
        if (isCarried == true)
        {
            if (carrier != null)
            {
                transform.position = carrier.transform.position + new Vector3(0,0.6f,0);
            }
        }
        //
	}

    void FixedUpdate()
    {
        AI();
    }

}
