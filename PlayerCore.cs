using UnityEngine;
using System.Collections;

public class PlayerCore : MonoBehaviour 
{

	public int landSection;
	public float landSteepness;

	public bool isStanding;

	GameObject obj;
	LandCore land;

	public GameObject projectilePrefab;
	GameObject projectileClone;


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

	void FixedUpdate ()
	{	
		// movement left / right

		if(Input.GetKey(KeyCode.A))
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50,0));
		
		if(Input.GetKey(KeyCode.D))
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50,0));

		//

		if(Input.GetKey(KeyCode.W))
		{
			// jump

			if (isStanding == true)
			{
				isStanding = false;
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2000));
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
			}

			//
		}

		if(Input.GetKey(KeyCode.P))
		{
			// shoot

			projectileClone = Instantiate(projectilePrefab,transform.position,transform.rotation) as GameObject;

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			projectileClone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);

			//
		}
		// ----------------
	}

	// ==================================================
}
