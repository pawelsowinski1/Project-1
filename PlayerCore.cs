using UnityEngine;
using System.Collections;

public class PlayerCore : ManCore 
{
    // =================== PLAYER CORE ==================

    // parent class:  ManCore
    // child classes: -

    GameObject pickupTarget;

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
		BodyInitialize();
        team = 0;
        pickupTarget = null;

        GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{
        AI();

        if (downed == false)
        {
            action = ActionEnum.none;

		    if(Input.GetKey(KeyCode.A))
            {
		        command = ActionEnum.none;
                MoveLeft();
            }
		
		    if(Input.GetKey(KeyCode.D))
            {
                command = ActionEnum.none;
		        MoveRight();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                command = ActionEnum.none;

                if (pickupTarget)
                {
		            PickUp(pickupTarget);
                    pickupTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);

                    pickupTarget = null;
                }
                else
                DropAll();
            }

            if (GameCore.Core.combatMode == true)
            {
		        if(Input.GetMouseButton(0))
                {
                    command = ActionEnum.none;
		            Hit();
                }
		
		        if(Input.GetMouseButton(1))
                {
                    command = ActionEnum.none;
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
		    if(Input.GetKeyDown(KeyCode.W))
            {
                command = ActionEnum.none;
                PlaceOnGround();
		        Jump();
            }
        }

        CalculateLand();
        PlaceOnGround();
		DamageColorize();
		SetDirection();

        if (hitCooldown > 0)
        hitCooldown--;
	}

    /// ----- ON TRIGGER -----

    void OnTriggerEnter2D(Collider2D other)
    {   
        // todo
        if ((other.gameObject.name == "item(Clone)")
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
        if ((other.gameObject.name == "item(Clone)")
        || (other.gameObject.name == "herbi(Clone)"))
        //
        {
            other.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }
    }


}
