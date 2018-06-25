using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Lists

public enum ActionEnum {none, move_right, move_left, move_to_x, pickup, drop, eat, chop};

public class CritterCore : BodyCore
{
    // ================= CRITTER CORE ==================

    // parent class:  BodyCore

    // child classes: PlayerCore
    //                ManCore
    //                HerbiCore

    public int   team = -1;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public int   hp = 100;
    public float hpmax = 100;
    public bool  downed = false;
    public bool  isCarrying = false;
    public float targetX;

    public ActionEnum action = ActionEnum.none;
    public ActionEnum command = ActionEnum.none;

    public List<GameObject> carriedBodies = new List<GameObject>();

	public GameObject slashPrefab;
	public GameObject projectilePrefab;
    public GameObject clone;
    public GameObject target = null;

    public LayerMask groundLayer;

    int i;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Shoot()
    /// Hit()
    /// PickUp(1)
    /// DropAll()
    /// Chop(1)
    /// DamageColorize()
    /// AI()

    //==================================================

	public void MoveLeft()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50,0));
        action = ActionEnum.move_left;
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50,0));
        action = ActionEnum.move_right;
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

    public void PickUp(GameObject body)
    {
        if (body)
        {
            if (body.GetComponent<BodyCore>().isCarried == false)
            {
                carriedBodies.Add(body);
                isCarrying = true;
                body.GetComponent<BodyCore>().isCarried = true;
                body.GetComponent<BodyCore>().carrier = gameObject;
            }
        }
    }

    //--------------------------------------------------

    public void DropAll()
    {
        if (carriedBodies.Count > 0)
        {
            int j = carriedBodies.Count;

            for (i=0; i<j; i++)
            {

                carriedBodies[i].GetComponent<BodyCore>().carrier = null;
                carriedBodies[i].GetComponent<BodyCore>().isCarried = false;
                carriedBodies[i].GetComponent<BodyCore>().isFalling = true;
            }
            carriedBodies.Clear();

            isCarrying = false;
        }
    }

    //--------------------------------------------------

    public void Chop(GameObject obj)
    {
        if (obj)
        {
            GameObject i;
            i = GameCore.Core.SpawnItem(ItemEnum.wood);
            i.transform.position = obj.transform.position;

            Destroy(obj);
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

    public void AI()
    {
        if (command != ActionEnum.none)
        {
            if (target)
            targetX = target.transform.position.x;

            float dist = transform.position.x - targetX;

            if (Mathf.Abs(dist) > 0.1)
            {
                if (targetX > transform.position.x)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            }
            else
            {
                switch (command)
                {
                    case ActionEnum.pickup:
                    {
                        PickUp(target);
                        command = ActionEnum.none;
                        break;
                    }
                    case ActionEnum.eat:
                    {
                        break;
                    }
                    case ActionEnum.chop:
                    {
                        Chop(target);
                        command = ActionEnum.none;
                        break;
                    }
                    case ActionEnum.drop:
                    {
                        DropAll();
                        command = ActionEnum.none;
                        break;
                    }
                }
            }
        }

    }
}
