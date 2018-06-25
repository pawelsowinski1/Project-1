using UnityEngine;
using System.Collections;

public class BodyCore : InteractiveObjectCore 
{
	//================== BODY CORE =====================

    // enables gravity

    // parent class:  InteractiveObjectCore

    // child classes: CritterCore
    //                ItemCore

    public int    land = 1;
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
        //GameObject obj;
		//obj = GameObject.Find("Game");
		//gameCore = obj.GetComponent<GameCore>();

        land = GameCore.Core.currentLand;
        isFalling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
	}

	//----------------------------------------------

	public void CalculateLand()
	{
		// calculates current landSection and landSteepness
		
		int i;
		
		for (i=1; i<GameCore.Core.landSections; i++)
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

	public void PlaceOnGround()
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		    }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            }
        }
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

