﻿using UnityEngine;
using System.Collections;

public class ManCore : CritterCore
{
    // =================== MAN CORE ====================

    // parent class:  CritterCore
    // child classes: PlayerCore

    int timerMove = 0;
    int timerHit = 0;

    ItemEnum tool = ItemEnum.none;


    // =================================================

	//--------------------------------------------------
	
	void Start()
	{

		BodyInitialize();

        timerMove = 1;
        timerHit = Random.Range(30, 70);
	}
	
	//--------------------------------------------------
	
	void Update()
	{
        CalculateLand();
		PlaceOnGround();
		DamageColorize();
    }
    

	void FixedUpdate()
	{
        // --- searching for target ---

        int i;

        i = GameCore.Core.critters.Count;
        target = null; 

        for (i=0; i <= GameCore.Core.critters.Count-1; i++)
        {
            if ((GameCore.Core.critters[i].GetComponent<CritterCore>().team != team)
            && (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false))
            {
                target = GameCore.Core.critters[i];
            }
        }

        // --- targeting and attacking --- 

        if ((target != null)
        && (downed == false))
        {
            if (target.transform.position.x > transform.position.x)
            {
                directionRight = true;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            else
            {
                directionRight = false;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            timerMove--;
            timerHit--;

            if (timerMove <= 0)

            {
                timerMove = Random.Range(30, 70);

                if (target.GetComponent<Transform>().position.x > transform.position.x)
                targetX = target.GetComponent<Transform>().position.x - 3f;
                else
                targetX = target.GetComponent<Transform>().position.x + 3f;

                targetX += Random.Range(-5f, 5f);
            }

            if (targetX > transform.position.x)
            MoveRight();
            else
            MoveLeft();

            // attacking

            if (timerHit <= 0)
            {
                Hit();
                //timerHit = Random.Range(1, 100);
                timerHit = 1;
            }
        }

        // ---------------

        if (hitCooldown > 0)
        hitCooldown--;
    }
	
	//==================================================
}
