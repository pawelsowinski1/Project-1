using UnityEngine;
using System.Collections;

public class BodyCore : PhysicalObject 
{
    //================== BODY =====================

    // A small, movable physical body.

    // Enables gravity and collision with the ground.
    // Allows object to be picked up by a critter.

    // =============================================

    // parent class:  PhysicalObject

    // child classes: CritterCore
    //                ItemCore

    public bool   isFalling = true;
    public bool   isCarried = false;

    public GameObject carrier = null;


    public float landSectionLeftPointX, landSectionRightPointX; // X positions of left and right point of the land section line

    
    /// BodyInitialize()
    /// CalculateLand()
    /// PlaceOnGround()

	//==================================================

	public void BodyInitialize()
	{
        land = GameCore.Core.currentLand;
        isFalling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = GameCore.GRAVITY;

        //UpdateLandSection(); <-- uncommenting this will cause a bug

        landSectionLeftPointX = 0f;
        landSectionRightPointX = 0f;
	}

	//----------------------------------------------

	public void Gravity()
	{
        if ((transform.position.x > landSectionRightPointX)
        || (transform.position.x < landSectionLeftPointX))
        {
            UpdateLandSection(); // <---- !
            landSectionLeftPointX = GameCore.Core.landPointX[landSection-1];
            landSectionRightPointX = GameCore.Core.landPointX[landSection];
        }

        groundY = GetGroundY(); 

		if (isFalling == false)
		{
			transform.position = new Vector2 (transform.position.x, groundY);
		}
		else
		{
		    if (transform.position.y < groundY)
		    {
			    transform.position = new Vector2 (transform.position.x, groundY);
			    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                isFalling = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);

                // if this body is a projectile, then change itself to item

                if (GetComponent<ProjectileCore>())
                {
                    Destroy(GetComponent<ProjectileCore>());
                    GetComponent<ItemCore>().enabled = true;
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    GetComponent<Rigidbody2D>().gravityScale = 0f;
                }
                //
            
		    }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = GameCore.GRAVITY;
            }
        }

        // linear drag in x axis for moving critters

        if (GetComponent<CritterCore>())
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f * GetComponent<Rigidbody2D>().velocity.x,0));

	}

    // ================= MAIN LOOP ================= 

    /// ----- START -----

	void Start()
	{
		BodyInitialize();
	}

    /// ----- UPDATE -----

	void Update() 
	{
		Gravity();
	}
}

