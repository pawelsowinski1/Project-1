// 20-11-2017

using UnityEngine;
using System.Collections;

public class PlayerCore : CritterCore 
{
    // =================== PLAYER CORE ==================

    // parent class:  CritterCore
    // child classes: -

    /// SetDirection()

	//  =================================================

	void SetDirection()
	{
		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
		{
			directionRight = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}
		else
		{
			directionRight = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	//==================================================

    ///  ----- START -----
	
	void Start ()
	{
		BodyInitialize();
        team = 0;

        int i = 0;
        GetComponent<SpriteRenderer>().sortingLayerID = i;
	}
	
    /// ----- UPDATE -----

	void Update ()
	{
		CalculateLand();
		PlaceOnGround();
		DamageColorize();
		SetDirection();
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{	
        if (downed == false)
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
            // -> move to GameCore
         
		    if (Input.GetKeyDown(KeyCode.F))
		    Screen.fullScreen = !Screen.fullScreen;
        
            // ---

            if (hitCooldown > 0)
            hitCooldown--;
        }
	}
}
