// 26-07-2017

using UnityEngine;
using System.Collections;

public class EnemyCore : CritterCore
{
	GameObject player;
	float targetX;

    int timerMove = 0;



	//==================================================
	
	void Start()
	{
		BodyInitialize();

		player = GameObject.Find("Player");
	}
	
	//__________________________________________________
	
	void Update()
	{
		CalculateLand();
		PlaceOnGround();
		DamageColorize();
	}

	void FixedUpdate()
	{
        timerMove++;
        if (timerMove >= 50)

        {
            timerMove = 0;

            if (player.GetComponent<Transform>().position.x > transform.position.x)
            targetX = player.GetComponent<Transform>().position.x - 5f;
            else
            targetX = player.GetComponent<Transform>().position.x + 5f;

            targetX += Random.Range(-5f, 5f);
        }

        if (targetX > transform.position.x)
        MoveRight();
        else
        MoveLeft();
    }
	
	//==================================================
}
