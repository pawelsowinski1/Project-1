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
        //GetComponent<SpriteRenderer>().sortingOrder = 30;

        // add hudText
        /*
        GameObject clone;
        clone = Instantiate(GameCore.Core.hudTextPrefab, GameCore.Core.myCanvas.transform);
        clone.GetComponent<HudText>().objectToFollow = gameObject;
        */
        //


        switch (herbi)
        {
            case EHerbi.chick:
            {
                name = "Chick";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_chick;
                transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                moveSpeed = 1.25f;
                hpMax = 20f;
                sightRange = 15f;


                break;
            }

            case EHerbi.deer:
            {
                name = "Deer";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_deer;
                transform.localScale = new Vector3(1.3f,1.3f,1.3f);
                moveSpeed = 1.75f;
                hpMax = 300f;
                sightRange = 30f;
                
                break;
            }
        }
    }


	void Start () 
    {
    	BodyInitialize();
        HerbiInitialize();

        hp = hpMax;
        team = 0;

        timerMove = 1;

	}
	
	void Update ()
    {
		Gravity();
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

    void OnEnable()
    {
        AddHpBar();
    }

    void OnDisable()
    {
        Destroy(hpBar);
    }

}
