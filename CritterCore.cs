using UnityEngine;
using System.Collections;

public class CritterCore : BodyCore 
{
    // ================= CRITTER CORE ==================

    // parent class:  BodyCore

    // child classes: PlayerCore
    //                ManCore
    //                AnimalCore

    public int   team = -1;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public int   hp = 100;
    public float hpmax = 100;
    public bool  downed = false;
    public bool  isCarrying = false;
    public float targetX;

    public GameObject bodyCarried = null;
	public GameObject slashPrefab;
	public GameObject projectilePrefab;
    public GameObject clone;
    public GameObject target = null;

    public LayerMask groundLayer;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Shoot()
    /// Hit()
    /// PickupItem(1)
    /// DropItem()
    /// DamageColorize()

    //==================================================

	public void MoveLeft()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50,0));;
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50,0));
	}

    //--------------------------------------------------
	
	public void Jump()
	{
        if (isFalling == false)
        {
			isFalling = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2000));
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
        }
        else
        {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;
            float distance = 0.16f;
    
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

            if (hit.collider != null)
            {
			    isFalling = true;
			    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2000));
			    gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            }
        }
	}

    //--------------------------------------------------
	
	public void Shoot()
	{
		clone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
        clone.GetComponent<ProjectileCore>().parent = gameObject;
		clone.GetComponent<ProjectileCore>().team = team;
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		clone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);
	}

    //--------------------------------------------------

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

    //--------------------------------------------------

    public void PickupBody(GameObject body)
    {
        if ((isCarrying == false) && (body != null))
        {
            bodyCarried = body;
            isCarrying = true;
            body.GetComponent<BodyCore>().isCarried = true;
            body.GetComponent<BodyCore>().carrier = gameObject;
        }
    }

    //--------------------------------------------------

    public void DropItem()
    {
        if (isCarrying == true)
        {
            bodyCarried.GetComponent<BodyCore>().carrier = null;
            bodyCarried.GetComponent<BodyCore>().isCarried = false;
            bodyCarried.GetComponent<BodyCore>().isFalling = true;

            bodyCarried = null;
            isCarrying = false;
        }
    }

    //--------------------------------------------------

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

    //--------------------------------------------------
}
