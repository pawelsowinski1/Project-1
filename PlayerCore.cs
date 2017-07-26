// 26-07-2017

using UnityEngine;
using System.Collections;

public class PlayerCore : CritterCore 
{
	//==================================================

	void SetDirection()
	{
		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
		{
			directionRight = true;
		}
		else
		{
			directionRight = false;
		}
	}

	//==================================================
	
	void Start ()
	{
		BodyInitialize();
	}
	
	//__________________________________________________
	
	void Update ()
	{
		CalculateLand();
		PlaceOnGround();
		DamageColorize();
		SetDirection();


	}
	
	//__________________________________________________

	void FixedUpdate()
	{	
		if(Input.GetKey(KeyCode.A))
		MoveLeft();
		
		if(Input.GetKey(KeyCode.D))
		MoveRight();
		
		if(Input.GetKey(KeyCode.Space))
		Jump();

		if(Input.GetMouseButton(0))
		Hit();
		
		if(Input.GetMouseButton(1))
		Shoot();

		// works only in standalone
		if (Input.GetKeyDown(KeyCode.F))
		Screen.fullScreen = !Screen.fullScreen;
	}

	//==================================================

}
