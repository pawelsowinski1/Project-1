// 20-11-2017

using UnityEngine;
using System.Collections;

public class CritterCore : BodyCore 
{
    // ================= CRITTER CORE ==================

    // parent class:  BodyCore

    // child classes: PlayerCore
    //                ManCore

    public int   team = -1;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public int   hp = 100;
    public float hpmax = 100;
    public bool  downed = false;

	public GameObject slashPrefab;
	public GameObject projectilePrefab;
    GameObject clone;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Shoot()
    /// Hit()
    /// DamageColorize()

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
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2500));
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
		}
	}

	//__________________________________________________
	
	public void Shoot()
	{
		clone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
        clone.GetComponent<ProjectileCore>().parent = gameObject;
		clone.GetComponent<ProjectileCore>().team = team;
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		clone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);
	}

    //__________________________________________________

    public void Hit()
    {
        if (hitCooldown <= 0)
        {
            clone = Instantiate(slashPrefab, Vector3.zero, transform.rotation) as GameObject;
            clone.GetComponent<SlashCore>().parent = gameObject;
            clone.transform.parent = gameObject.transform; // fixes slash wobbling bug
            clone.GetComponent<SlashCore>().team = team;

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

    //__________________________________________________
}
