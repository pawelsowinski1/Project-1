// 26-07-2017

using UnityEngine;
using System.Collections;

public class EnemyCore : CritterCore
{
	GameObject player;
	float targetX;



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
		targetX = player.GetComponent<Transform>().position.x;

		if (targetX > transform.position.x)
		MoveRight();
		else
		MoveLeft();
	}
	
	//==================================================
}
