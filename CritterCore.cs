using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Lists

public enum ActionEnum {none, idle, move_right, move_left, move, follow, pickup, drop_all, eat, chop, gather_wood, gather_plant_material, cut_down};

public class CritterCore : BodyCore
{
    // ================= CRITTER CORE ==================

    // A creature. Can move, hit and receive damage.

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
    public int   timerMove = 1;

    public GameObject target = null;
    public GameObject tool = null;
    public ActionEnum action = ActionEnum.none;
    public ActionEnum command = ActionEnum.none;
    public List<GameObject> carriedBodies = new List<GameObject>();

	public GameObject slashPrefab;
	public GameObject projectilePrefab;

    public LayerMask groundLayer;

    public GameObject clone;

    int i;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Follow(1)
    /// Hit()
    /// DamageColorize()


    //==================================================

	public void MoveLeft()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(-GameCore.MOVE_FORCE,0));
        action = ActionEnum.move_left;
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GameCore.MOVE_FORCE,0));
        action = ActionEnum.move_right;
	}

    //--------------------------------------------------
	
	public void Jump()
	{
        if (isFalling == false)
        {
			isFalling = true;

			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,GameCore.JUMP_FORCE), ForceMode2D.Impulse);
			GetComponent<Rigidbody2D>().gravityScale = GameCore.GRAVITY;
        }
        else
        {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;
            float distance = 0.16f;
    
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

            if (hit.collider != null) // if standing on platform
            {
			    isFalling = true;
			    GetComponent<Rigidbody2D>().AddForce(new Vector2(0,GameCore.JUMP_FORCE), ForceMode2D.Impulse);
			    GetComponent<Rigidbody2D>().gravityScale = GameCore.GRAVITY;
            }
        }
	}

    //--------------------------------------------------
	
	public void Throw()
	{
        if (tool != null)
        {
		    clone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
            clone.GetComponent<InteractiveObjectCore>().kind = KindEnum.projectile;
            clone.GetComponent<ProjectileCore>().parent = gameObject;
		    clone.GetComponent<ProjectileCore>().team = team;
		
		    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		    clone.GetComponent<Rigidbody2D>().AddForce((mousePos-transform.position)*200);

            tool.GetComponent<BodyCore>().carrier = clone;
            clone.GetComponent<ProjectileCore>().carriedItem = tool;
            tool = null;

            if (GameCore.Core.player == gameObject)
            {
                GameCore.Core.InventoryManager();
            }
        }
	} //-> ManCore

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
                if (body.GetComponent<InteractiveObjectCore>().kind == KindEnum.item)
                {
                    if (body.GetComponent<ItemCore>().isTool == true)
                    {
                        Equip(body);
                    }
                    else
                    {
                        // pick up

                        carriedBodies.Add(body);

                        isCarrying = true;
                        body.GetComponent<BodyCore>().isCarried = true;
                        body.GetComponent<BodyCore>().carrier = gameObject;

                        //
                    }
                }
                else
                {
                    // pick up

                    carriedBodies.Add(body);

                    isCarrying = true;
                    body.GetComponent<BodyCore>().isCarried = true;
                    body.GetComponent<BodyCore>().carrier = gameObject;

                    //
                }

                if (GameCore.Core.player == gameObject)
                GameCore.Core.InventoryManager();
            }
        }
    } //-> ManCore

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

        if (GameCore.Core.player == gameObject)
        GameCore.Core.InventoryManager();
    } //-> ManCore

    //--------------------------------------------------

    public void Equip(GameObject body)
    {
        if (body)
        {
            if (body.GetComponent<InteractiveObjectCore>().kind == KindEnum.item)
            {
                if (body.GetComponent<ItemCore>().isTool == true)
                {
                    if (tool != null)
                    {
                        Unequip(tool);
                    }
                    
                    tool = body;

                    isCarrying = true;
                    body.GetComponent<BodyCore>().isCarried = true;
                    body.GetComponent<BodyCore>().carrier = gameObject;
                    

                    if (GameCore.Core.player == gameObject) // if player
                    {
                        for (i=0; i<carriedBodies.Count; i++)
                        {
                            // check if the target body is in player's inventory
                            if (GetComponent<CritterCore>().carriedBodies[i] == body)
                            {
                                // if yes, remove it from player's inventory
                               GetComponent<CritterCore>().carriedBodies.RemoveAt(i);
                            }

                        }
            
                        GameCore.Core.InventoryManager();
                    }
                }
            }
        }
    } //-> ManCore

    //--------------------------------------------------

    public void Unequip(GameObject item)
    {
        if (tool == item) // if item is in tool slot
        {
            carriedBodies.Add(item);
            tool = null;
        }

        if (GameCore.Core.player == gameObject)
        GameCore.Core.InventoryManager();
    } //-> ManCore

    //--------------------------------------------------

    public void Chop(GameObject obj)
    {
        if (obj)
        {
            GameObject i;
            i = GameCore.Core.SpawnItem(ItemEnum.wood);
            i.transform.position = obj.transform.position;
            
            //GameCore.Core.plants.Remove(obj); <- not necessary
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
        // if man has any command

        if ((command != ActionEnum.none)
        && (downed == false))
        {
            // if idle, walk around

            if (command == ActionEnum.idle)
            {
                timerMove--;

                if (timerMove <= 0)
                {
                    targetX = transform.position.x + Random.Range(-10f,10f);
                    timerMove = 300;
                }
            }

            // if not idle
            // set targetX, calculate distance, move

            if (target)
            {
                if (command == ActionEnum.follow)
                {
                    if (timerMove <= 0)
                    {
                        targetX = target.transform.position.x + Random.Range(-10f,10f);
                        timerMove = 150;
                    }
                }

                else
                targetX = target.transform.position.x;

                timerMove--;
            }

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
            else // if distance to the target is very small, then execute the command
            {
                switch (command)
                {
                    case ActionEnum.move:
                    {
                        command = ActionEnum.none;
                        break;
                    }

                    case ActionEnum.follow:
                    {
                        break;
                    }

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
                    case ActionEnum.drop_all:
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
