using UnityEngine;
using System.Collections;

public class CritterCore : MonoBehaviour 
{
	
	int landSection;
	float landSteepness;
	
	bool isStanding;
	
	GameObject obj;
	LandCore land;
	
	public GameObject slashPrefab;
	GameObject slashClone;

	public GameObject projectilePrefab;
	GameObject projectileClone;
	
	// ==================================================

	public void MoveLeft()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50,0));
	}

	public void MoveRight()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50,0));
	}

	public void Jump()
	{
		if (isStanding == true)
		{
			isStanding = false;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2000));
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
		}
	}

	public void Shoot()
	{
		projectileClone = Instantiate(projectilePrefab,transform.position,transform.rotation) as GameObject;
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		projectileClone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);
	}

	public void Hit()
	{
		slashClone = Instantiate(slashPrefab,Vector3.zero,transform.rotation) as GameObject;
		//slashClone.GetComponent<SlashCore>().parent = gameObject; 
		slashClone.transform.parent = gameObject.transform;
	}
	
	// ==================================================
	
	void Start ()
	{
		obj = GameObject.Find("Land");
		land = obj.GetComponent<LandCore>();
		
		//
		
		isStanding = false;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
		

	}
	
	// ==================================================
	
	void Update ()
	{
		// calculate current landSection and landSteepness
		
		int i;
		
		for (i=1; i<land.landSections; i++)
		{
			if (transform.position.x < land.landPointX[i])
			{
				if (i != landSection)
				{
					landSection = i;
					landSteepness = Mathf.Atan2(land.landPointY[i]-land.landPointY[i-1],land.landPointX[i]-land.landPointX[i-1]);
				}
				break;
			}
		}
		
		//------------------------------------------------
		
		// place on the ground
		
		float groundY = land.landPointY[landSection-1] + (transform.position.x-land.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
		
		if (isStanding == true)
		{
			transform.position = new Vector3 (transform.position.x, groundY);
		}
		else
			
			if (transform.position.y < groundY)
		{
			transform.position = new Vector3 (transform.position.x, groundY);
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			isStanding = true;
		}
		// --------------------------------------------



	}

	// ==================================================

}
