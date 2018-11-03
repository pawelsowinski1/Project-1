using UnityEngine;
using System.Collections;

public class PlayerCore : ManCore 
{
    // =================== PLAYER CORE ==================

    // parent class:  ManCore
    // child classes: -

    public GameObject pickupTarget;

    /// SetDirection()

	// ====================================================

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

	// ==================== MAIN LOOP =======================

    ///  ----- START -----
	
	void Start()
	{
        name = "Player";

		BodyInitialize();
        team = 1;
        pickupTarget = null;

        
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{
        AI();

        if (downed == false)
        {
            //action = EAction.none;

		    if(Input.GetKey(KeyCode.A))
            {
		        action = EAction.none;
                MoveLeft();
            }
		
		    if(Input.GetKey(KeyCode.D))
            {
                action = EAction.none;
		        MoveRight();
            }

            if (GameCore.Core.combatMode == true)
            {
		        if(Input.GetMouseButton(0))
                {
                    action = EAction.none;
		            Hit();
                }
		
		        if(Input.GetMouseButton(1))
                {
                    action = EAction.none;
		            Throw();
                }
            }
        }
	}

    /// ----- UPDATE -----

	void Update()
	{
        if (downed == false)
        {
            SetDirection();

		    if(Input.GetKeyDown(KeyCode.W))
            {
                action = EAction.none;
                PlaceOnGround();
		        Jump();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                action = EAction.none;

                if (pickupTarget)
                {
		            PickUp(pickupTarget);
                    pickupTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);

                    pickupTarget = null;
                }
                else
                DropAll();
            }
        }


        CalculateLand();
        PlaceOnGround();
		DamageColorize();
		

        if (hitCooldown > 0)
        hitCooldown--;
	}

    /// ----- ON TRIGGER -----
    
    // detecting an object to pick up

    void OnTriggerEnter2D(Collider2D other)
    {   
        bool b = false;
        

        if (other.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.item)
        b = true;

        if (other.gameObject.GetComponent<InteractiveObjectCore>().type == EType.herbi)
        b = true;

        if (other.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.plant)
        {
            if (other.gameObject.GetComponent<PlantCore>().rooted == false) 
            b = true;
        }

        if (b == true)
        {
            pickupTarget = other.gameObject;
        }
    }

}
