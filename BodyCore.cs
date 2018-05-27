using UnityEngine;
using System.Collections;

public enum ActionEnum {nothing, eat};


public class BodyCore : MonoBehaviour 
{
	//================== BODY CORE =====================

    // enables gravity and RMB interaction (RMB interaction should be another class)

    // parent class:  -

    // child classes: CritterCore
    //                ItemCore

    public int    land = 1;
	public int    landSection;
	public float  landSteepness;
    public bool   isFalling = true;
    public string label = "label"; // <--- !
    public bool   isCarried = false;

    public GameObject carrier = null;  
    
    // storing reference to game core in every object is inefficient - fix this by making a single global reference (somehow)
	public GameCore gameCore;
    //

    /// BodyInitialize()
    /// CalculateLand()
    /// PlaceOnGround()

	//==================================================

	public void BodyInitialize()
	{
        GameObject obj;
		obj = GameObject.Find("Game");
		gameCore = obj.GetComponent<GameCore>();

        land = gameCore.currentLand;
        isFalling = true;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
	}

	//----------------------------------------------

	public void CalculateLand()
	{
		// calculates current landSection and landSteepness
		
		int i;
		
		for (i=1; i<gameCore.landSections; i++)
		{
			if (transform.position.x < gameCore.landPointX[i])
			{
				if (i != landSection)
				{
					landSection = i;
					landSteepness = Mathf.Atan2(gameCore.landPointY[i]-gameCore.landPointY[i-1],gameCore.landPointX[i]-gameCore.landPointX[i-1]);
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
		float groundY = gameCore.landPointY[landSection-1] + (transform.position.x-gameCore.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
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

        //int i = 2;
        //GetComponent<SpriteRenderer>().sortingLayerID = i;
	}

    /// ----- UPDATE -----

	void Update() 
	{
		CalculateLand();
		PlaceOnGround();
	}
}

