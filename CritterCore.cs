// 19-08-2017

using UnityEngine;
using System.Collections;

public class CritterCore : BodyCore 
{
    //==================================================

    public int team = -1;
	public bool directionRight = true;
	public float damageColorIntensity = 0f;
    public int hitCooldown = 0;

	public GameObject slashPrefab;
	GameObject slashClone;

	public GameObject projectilePrefab;
	GameObject projectileClone;

	
	//==================================================

	public void MoveLeft()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50,0));;
	}

	//__________________________________________________
	
	public void MoveRight()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50,0));
	}

	//__________________________________________________
	
	public void Jump()
	{
		if (isGrounded == true)
		{
			isGrounded = false;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2000));
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
		}
	}

	//__________________________________________________
	
	public void Shoot()
	{
		projectileClone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
		projectileClone.GetComponent<ProjectileCore>().parent = gameObject;
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		projectileClone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);
	}

    //__________________________________________________

    public void Hit()
    {
        if (hitCooldown <= 0)
        {
            slashClone = Instantiate(slashPrefab, Vector3.zero, transform.rotation) as GameObject;
            slashClone.GetComponent<SlashCore>().parent = gameObject;
            slashClone.transform.parent = gameObject.transform; // fixes slash wobbling bug

            hitCooldown = 40;
        }
	}

	//__________________________________________________
	
	public void DamageColorize()
	{
		if (damageColorIntensity != 0f)
		{
			if (damageColorIntensity > 0f)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f,1f-damageColorIntensity,1f-damageColorIntensity,1f);
				damageColorIntensity -= 0.03f;
			}
			else
			{
				GetComponent<SpriteRenderer>().color = Color.white;
				damageColorIntensity = 0f;
			}
		}
	}

	//==================================================
	/*
	void Start ()
	{
		BodyInitialize();
	}
	
	//__________________________________________________
	
	void Update ()
	{
		CalculateLand();
		PlaceOnGround();
		DamageColorize();
	}
    */
	//==================================================

}
