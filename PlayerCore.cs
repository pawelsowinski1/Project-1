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
        AI_Man();

        if (downed == false)
        {
            action = EAction.none;

		    if(Input.GetKey(KeyCode.A))
            {
		        command = EAction.none;
                MoveLeft();
            }
		
		    if(Input.GetKey(KeyCode.D))
            {
                command = EAction.none;
		        MoveRight();
            }

            if (GameCore.Core.combatMode == true)
            {
		        if(Input.GetMouseButton(0))
                {
                    command = EAction.none;
		            Hit();
                }
		
		        if(Input.GetMouseButton(1))
                {
                    command = EAction.none;
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
                command = EAction.none;
                PlaceOnGround();
		        Jump();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                command = EAction.none;

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

    void OnTriggerEnter2D(Collider2D other)
    {   
        // todo
        if ((other.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.item)
        || (other.gameObject.name == "herbi(Clone)"))
        //
        {
            if (pickupTarget != null)
            pickupTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
            
            pickupTarget = other.gameObject;
            pickupTarget.GetComponent<SpriteRenderer>().color = new Color(0f,0.7f,0.7f,1f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // todo
        if ((other.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.item)
        || (other.gameObject.name == "herbi(Clone)"))
        //
        {
            other.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }
    }


}
