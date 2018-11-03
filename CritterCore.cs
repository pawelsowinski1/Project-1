using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Lists

public enum EAction {none, idle, move_right, move_left, move, follow, attack, pickup, drop_all, eat, chop, gather_wood,
                        gather_plant_material, cut_down, craftHandaxe};

public class CritterCore : BodyCore
{
    // ================= CRITTER CORE ==================

    // A creature. Can move, hit and receive damage. 

    // parent class:  BodyCore

    // child classes: HerbiCore
    //                ManCore

    public int   team = -1;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public float hp = 100f;
    public float hpmax = 100f;
    public bool  downed = false;
    public bool  isCarrying = false;
    public float targetX;
    public int   timerMove = 1;

    public GameObject target = null;
    public EAction action = EAction.none;
    public EAction command = EAction.none;
    public List<GameObject> carriedBodies = new List<GameObject>();

	public GameObject slashPrefab;
	public GameObject projectilePrefab;

    public LayerMask groundLayer;

    public GameObject clone;

    public int i;
    public int timerHit = 0;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Follow(1)
    /// Hit()
    /// PickUp()
    /// DropAll()
    /// DamageColorize()
    /// SearchForTarget()
    /// AttackTarget()
    /// AI_Critter()

    //--------------------------------------------------

	public void MoveLeft()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(-GameCore.MOVE_FORCE,0));
        action = EAction.move_left;
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GameCore.MOVE_FORCE,0));
        action = EAction.move_right;
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
	
    public void Hit()
    {
        if (hitCooldown <= 0)
        {
            clone = Instantiate(slashPrefab, Vector3.zero, transform.rotation) as GameObject;
            clone.GetComponent<SlashCore>().parent = gameObject;
            clone.transform.parent = gameObject.transform; // fixes slash wobbling bug
            clone.GetComponent<SlashCore>().team = team;

            hitCooldown = 25;

        }
	}
    
    //--------------------------------------------------

    public virtual void PickUp(GameObject body) // virtual, so it can be overriden in ManCore / not sure if this is needed, though
    {
        if (body)
        {
            if (body.GetComponent<BodyCore>().isCarried == false)
            {
                if (Mathf.Abs(transform.position.x - body.transform.position.x) < 1f)
                {
                    if (body.GetComponent<InteractiveObjectCore>().kind == EKind.item)
                    {
                        if (body.GetComponent<ItemCore>().isTool == true)
                        {
                            //Equip(body);
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

        if (GameCore.Core.player == gameObject)
        GameCore.Core.InventoryManager();
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

    public void SearchForTarget()
    {
        i = GameCore.Core.critters.Count;
        target = null;

        for (i=0; i <= GameCore.Core.critters.Count-1; i++)
        {
            if ((GameCore.Core.critters[i].GetComponent<CritterCore>().team != team)
            && (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
            && (GameCore.Core.critters[i].GetComponent<CritterCore>().team != 0)) // do not attack neutral critters
            {
                target = GameCore.Core.critters[i];
                command = EAction.attack;
            }
        }

        if (target == null)
        {
            if (team == 1)
            {
                command = EAction.follow;
                target = GameCore.Core.player;
            }
            else
            command = EAction.idle;
        }
    }

    //--------------------------------------------------

    public void AttackTarget()
    {
        if (command == EAction.attack)
        {
            if ((target != null)
            && (downed == false))
            {
                if (target.transform.position.x > transform.position.x)
                {
                    directionRight = true;
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    directionRight = false;
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }

                timerMove--;
                timerHit--;

                if (timerMove <= 0)
                {
                    timerMove = Random.Range(30, 70);

                    if (target.GetComponent<Transform>().position.x > transform.position.x)
                    targetX = target.GetComponent<Transform>().position.x - 3f;
                    else
                    targetX = target.GetComponent<Transform>().position.x + 3f;

                    targetX += Random.Range(-5f, 5f);
                }

                // attacking

                if (timerHit <= 0)
                {
                    Hit();
                    //timerHit = Random.Range(1, 100);
                    timerHit = 1;
                }
            }
        }

        // ---------------

        if (hitCooldown > 0)
        hitCooldown--;
    }
    
    //--------------------------------------------------

    public void AI_Critter()
    {
        // if there is any command

        if ((command != EAction.none)
        && (downed == false))
        {
            // if idle, walk around

            if (command == EAction.idle)
            {
                timerMove--;

                if (timerMove <= 0)
                {
                    targetX = transform.position.x + Random.Range(-10f,10f);
                    action = EAction.move;
                    timerMove = 300;
                }
            }
            else

            // if not idle, then set targetX, calculate distance and move towards target

            if ((target)
            && (command != EAction.attack))
            {
                if (command == EAction.follow) // if command is "follow", then walk around the target
                {
                    if (timerMove <= 0)
                    {
                        targetX = target.transform.position.x + Random.Range(-10f,10f);
                        timerMove = 150;
                    }

                    timerMove--;
                }
                else // if command is not "follow", then walk directly to target
                targetX = target.transform.position.x;
            }


            // ========== move  ===========

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
                    case EAction.move:
                    {
                        command = EAction.none;
                        break;
                    }

                    case EAction.follow:
                    {
                        break;
                    }

                    case EAction.pickup:
                    {
                        PickUp(target);
                        command = EAction.none;
                        break;
                    }

                    case EAction.eat:
                    {
                        break;
                    }
                    case EAction.drop_all:
                    {
                        DropAll();
                        command = EAction.none;
                        break;
                    }
                }
            

                // ====================
            }
        }
    }

    //--------------------------------------------------
}
