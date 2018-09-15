using UnityEngine;
using System.Collections;

public class BodyCore : InteractiveObjectCore 
{
	//================== BODY CORE =====================

    // Enables gravity, collision with ground, and allows object to be picked up by a critter.

    // parent class:  InteractiveObjectCore

    // child classes: CritterCore
    //                ItemCore

    public int    land;
	public int    landSection;
	public float  landSteepness;
    public bool   isFalling = true;
    public bool   isCarried = false;

    public GameObject carrier = null;  
    
    /// BodyInitialize()
    /// CalculateLand()
    /// PlaceOnGround()

	//==================================================

	public void BodyInitialize()
	{
        land = GameCore.Core.currentLand;
        isFalling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = GameCore.GRAVITY;
	}

	//----------------------------------------------

	public void CalculateLand()
	{
		// calculates current landSection and landSteepness
		
		int i;
		
		for (i=1; i<GameCore.Core.landSections; i++) // BUG HERE ! NullReferenceException: Object reference not set to an instance of an object
		{
			if (transform.position.x < GameCore.Core.landPointX[i])
			{
				if (i != landSection)
				{
					landSection = i;
					landSteepness = Mathf.Atan2(GameCore.Core.landPointY[i]-GameCore.Core.landPointY[i-1],GameCore.Core.landPointX[i]-GameCore.Core.landPointX[i-1]);
				}
				break;
			}
		}
	}

	//----------------------------------------------

	public void PlaceOnGround() // name of the method is misleading, should be f.e. "Gravity"
	{
        // this shouldn't be calculated every frame
        // cut and paste as separate CalculateGroundY() method
		float groundY = GameCore.Core.landPointY[landSection-1] + (transform.position.x-GameCore.Core.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
		//

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

                if (kind == EKind.projectile)
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

        if (GetComponent<InteractiveObjectCore>().kind == EKind.critter)
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
		CalculateLand();
		PlaceOnGround();
	}
}

