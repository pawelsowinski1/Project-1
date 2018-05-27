using UnityEngine;
using System.Collections;

public class PlayerCore : CritterCore 
{
    // =================== PLAYER CORE ==================

    // parent class:  CritterCore
    // child classes: -

    GameObject pickupTarget;

    /// SetDirection()

	// =================================================

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

        label = gameObject.name;

        int i = 0;
        GetComponent<SpriteRenderer>().sortingLayerID = i;
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
		
		    if(Input.GetKey(KeyCode.W))
		    Jump();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if ((isCarrying == false) && (pickupTarget != null))
                {
		            PickupBody(pickupTarget);
                    pickupTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);

                    pickupTarget = null;
                }
                else
                {
                    DropItem();
                }
            }

		    if(Input.GetMouseButton(0))
		    Hit();
		
		    //if(Input.GetMouseButton(1))
		    //Shoot();

            //

            if (hitCooldown > 0)
            hitCooldown--;
        }
	}


    /// ----- UPDATE -----

	void Update()
	{
		CalculateLand();
		PlaceOnGround();
		DamageColorize();
		SetDirection();
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
