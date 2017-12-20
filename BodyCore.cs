// 20-11-2017

using UnityEngine;
using System.Collections;

public class BodyCore : MonoBehaviour 
{
	//================== BODY CORE =====================

    // parent class:  -

    // child classes: CritterCore
    //                ItemCore

	public int    landSection;
	public float  landSteepness;
	public bool   isGrounded = false;
    public string label = "label";
	
	public GameObject obj;
	public GameCore gameCore;

    /// BodyInitialize()
    /// CalculateLand()
    /// PlaceOnGround()

	//==================================================

	public void BodyInitialize()
	{
		obj = GameObject.Find("Game");
		gameCore = obj.GetComponent<GameCore>();
		
		isGrounded = false;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
	}

	//__________________________________________________

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

        // ---
	}

	//__________________________________________________

	public void PlaceOnGround()
	{
		float groundY = gameCore.landPointY[landSection-1] + (transform.position.x-gameCore.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
		
		if (isGrounded == true)
		{
			transform.position = new Vector3 (transform.position.x, groundY);
		}
		else
		{
		    if (transform.position.y < groundY)
		    {
			    transform.position = new Vector3 (transform.position.x, groundY);
			    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			    isGrounded = true;
		    }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            }
        }

	}

	//__________________________________________________

}
