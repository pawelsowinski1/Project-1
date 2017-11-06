// 05-11-2017

using UnityEngine;
using System.Collections;

public class EnemyCore : CritterCore
{
	public GameObject target;
	float targetX;

    int timerMove = 0;
    int timerHit = 0;

	//__________________________________________________
	
	void Start()
	{
		BodyInitialize();
        //team = 1;

        timerMove = 1;
        timerHit = Random.Range(30, 70);
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
        if (target != null)
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

            if (timerHit <= 0)
            {
                Hit();
                //timerHit = Random.Range(1, 100);
                timerHit = 1;
            }
        }

        if (hitCooldown > 0)
        hitCooldown--;

    }
	
	//==================================================
}
