using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Lists

public enum EAction {none, idle, moveRight, moveLeft, move, follow, attack, pickUp, dropAll, eat, chop,
                        cutDown, craftHandAxe, obtainMeat, convert, processHemp, craftStoneSpear, runAway};

public enum ECommand {none, guard}

public class CritterCore : BodyCore
{
    // ================= CRITTER CORE ==================

    // A creature. Can move and carry objects. Can hit, receive damage and be killed. 

    // parent class:  BodyCore

    // child classes: HerbiCore
    //                CarniCore
    //                ManCore

    public int   team = -1;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public float hp = 100f;
    public float hpMax = 100f;
    public bool  downed = false;
    public bool  alive = true;
    public bool  isCarrying = false;
    public float targetX;

    public GameObject target = null;
    public GameObject commandTarget = null;
    public EAction action = EAction.none;
    public ECommand command = ECommand.none;
    public List<GameObject> carriedBodies = new List<GameObject>();

	public GameObject slashPrefab;
	public GameObject projectilePrefab;

    public LayerMask groundLayer;

    public GameObject clone;

    public int i;
    public int timerMove = 1;
    public int timerHit = 0;
    public int timerAI = 1;

    public float targetXDistance;
    public float commandTargetDistance;

    /// MoveLeft()
    /// MoveRight()
    /// Jump()
    /// Follow(1)
    /// Hit()
    /// PickUp()
    /// DropAll()
    /// ExecuteAction() -> virtual
    /// DamageColorize()
    /// SearchForTarget()
    /// AttackTarget()
    /// AI()

    //--------------------------------------------------

	public void MoveLeft()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(-GameCore.MOVE_FORCE,0));
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GameCore.MOVE_FORCE,0));
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

            hitCooldown = 30;
        }
	}
    
    //--------------------------------------------------

    public void PickUp(GameObject body)
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
                            // equip

                            if (GetComponent<ManCore>())
                            {
                                GetComponent<ManCore>().Equip(body);
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

    public virtual void ExecuteAction()
    {
        switch (action)
        {
            case EAction.move:
            {
                action = EAction.none;
                break;
            }

            case EAction.idle:
            {
                //action = EAction.none;
                break;
            }

            case EAction.follow:
            {
                //action = EAction.none;
                break;
            }

            case EAction.pickUp:
            {
                PickUp(target);
                action = EAction.none;
                break;
            }

            case EAction.eat:
            {
                break;
            }
            case EAction.dropAll:
            {
                DropAll();
                action = EAction.none;
                break;
            }
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

    public void SearchForTarget()
    {
        i = GameCore.Core.critters.Count;
        target = null;

        for (i=0; i <= GameCore.Core.critters.Count-1; i++)
        {
            if (GameCore.Core.critters[i])
            {
                if ((GameCore.Core.critters[i].GetComponent<CritterCore>().team != team)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().team != 0)) // do not attack neutral critters
                {
                    target = GameCore.Core.critters[i];
                    action = EAction.attack;
                    
                    if (targetX == 0f) // fixes strange 'enemy men walking to the scene origin' bug
                    targetX = transform.position.x;
                }
            }
        }

        if (target == null)
        {
            if (team == 1)
            {
                action = EAction.follow;
                target = GameCore.Core.player;
            }
            else
            action = EAction.idle;
        }
    }

    //--------------------------------------------------

    public void AttackTarget()
    {
        if (target)
        {
            if (downed == false)
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

                // random movement around target

                if (timerMove <= 0)
                {
                    timerMove = Random.Range(30, 70);

                    if (target.GetComponent<Transform>().position.x > transform.position.x)
                    targetX = target.GetComponent<Transform>().position.x - 2f;
                    else
                    targetX = target.GetComponent<Transform>().position.x + 2f;

                    targetX += Random.Range(-3f, 3f);
                }

                // attacking

                if (timerHit <= 0) 
                {
                    Hit();
                    timerHit = 1; // todo
                }

                //
            }
        }

        if (hitCooldown > 0)
        hitCooldown--;
    }
    
    //--------------------------------------------------

    public void AI()
    {
        if (downed == false)
        {
            // ------------- AI TIMER ---------------

            timerAI--;
            timerMove--;
            timerHit--;

            targetXDistance = Mathf.Abs(transform.position.x - targetX);

            if (timerAI <= 0)
            {
                
                timerAI = Random.Range(100,200);

                if (commandTarget)
                commandTargetDistance = Mathf.Abs(transform.position.x - commandTarget.transform.position.x);

                // ---------- 1. commands -----------

                // if there is a command

                if (command != ECommand.none)
                {
                    if (command == ECommand.guard)
                    {
                        if (commandTargetDistance > 30f)
                        {
                            action = EAction.follow;
                            target = commandTarget;
                        }
                        else
                        {
                            SearchForTarget();
                        }
                    }
                }
                else // if command is none
                {
                    if (type == EType.man)
                    {
                        if (gameObject != GameCore.Core.player)
                        {
                            if (team != 0)
                            {
                                SearchForTarget();
                            }
                        }
                    }
                }

                // ------------ 2. running away -------------

                if (type == EType.herbi)
                {
                    GameObject o = null;
                    float f = 0f;
                    float dist;

                    // check for men and carni

                    for (i=0; i<GameCore.Core.critters.Count; i++)
                    {
                        if ((GameCore.Core.critters[i].GetComponent<InteractiveObjectCore>().type == EType.man)
                        || (GameCore.Core.critters[i].GetComponent<InteractiveObjectCore>().type == EType.carni))
                        {
                            if (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                            {
                                dist = Mathf.Abs(transform.position.x - GameCore.Core.critters[i].transform.position.x);

                                if (dist < 10f)
                                {
                                    if (o == null)
                                    {
                                        o = GameCore.Core.critters[i];
                                        f = dist;
                                    }
                                    else
                                    if (dist < f)
                                    {
                                        o = GameCore.Core.critters[i];
                                        f = dist;
                                    }
                                }                                                                                   
                            }
                        }
                    }

                    if (o)
                    {
                        target = o;
                        action = EAction.runAway;
                    }
                    else
                    {
                        if (action == EAction.runAway)
                        action = EAction.idle;
                    }

                    //
                }

                // ------------ 3. actions -------------

                // if there is any action

                if (action != EAction.none)
                {
                    // if running away

                    if (action == EAction.runAway)
                    {
                        if (target.transform.position.x > transform.position.x)
                        targetX = 0f;
                        else
                        targetX = GameCore.Core.landPointX[GameCore.Core.landSections-1];
                    }

                    // if idle, set some random targetX

                    else
                    if (action == EAction.idle)
                    {
                        if (timerMove <= 0)
                        {
                            targetX = transform.position.x + Random.Range(-10f,10f);
                            timerMove = 100 + Random.Range(0,200);
                        }
                    }

                    // if action is "follow", then walk around the target

                    else
                    if (action == EAction.follow) 
                    {
                        if (target)
                        {
                            if (timerMove <= 0)
                            {
                                targetX = target.transform.position.x + Random.Range(-10f,10f);
                                timerMove = 150;
                            }
                        }
                    }

                    // if action is not any of the above, then walk directly to the target to execute action

                    else 
                    if (target)
                    targetX = target.transform.position.x;
                }

                // if action is none, then set it to idle

                else
                if (gameObject != GameCore.Core.player)
                action = EAction.idle;
            }

            // ---------------------- END OF AI TIMER ---------------------- 

            // if attacking, set targetX and hit

            if (action == EAction.attack)
            {
                if (targetXDistance < 5f)
                AttackTarget();
            }

            // if doing any action, move towards the target to execute the action

            if (action != EAction.none)
            {
                if (targetXDistance > 0.8f)
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
                else // if distance to the target is very small, then execute the action
                {
                    ExecuteAction();
                }
            }

            //

        }
    }

    //--------------------------------------------------

    void OnDestroy()
    {
        GameCore.Core.critters.Remove(gameObject); 
    }
}