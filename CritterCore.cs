using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Lists


public enum ECommand {none, guard}

public class CritterCore : BodyCore
{
    // ================= CRITTER CORE ==================

    // A creature. Can move and pick up objects. Can hit, receive damage and be killed. 

    // parent class:  BodyCores

    // child classes: HerbiCore
    //                CarniCore
    //                ManCore

    public float lastTimeStamp; // time stamp when critter was seen by player for the last time - used to calculate events
    public int   team = -1;
    public float attitude = 0f;
	public bool  directionRight = true;
	public float damageColorIntensity = 0f;
    public int   hitCooldown = 0;
    public float hp = 100f;
    public float hpMax = 100f;
    public float moveSpeed = 1f;
    public float sightRange = 20f;
    public bool  downed = false;
    public bool  alive = true;
    public bool  isCarrying = false;
    public float targetX;
    public bool  preciseMovement = false; // needed for certain tasks - cutting down trees etc.

    public GameObject target = null;
    public GameObject commandTarget = null;
    public EAction action = EAction.none;
    public ECommand command = ECommand.none;
    public List<GameObject> carriedBodies = new List<GameObject>();

	public GameObject slashPrefab;
	public GameObject projectilePrefab;


    public LayerMask groundLayer;

    public int timerMove = 1;
    public int timerHit = 0;
    public int timerAI = 1;

    public float targetXDistance;
    public float commandTargetDistance;

    /// MoveLeft()
    /// MoveRight()
    /// Stop()
    /// Jump()
    /// Follow(1)
    /// Hit()
    /// PickUp(1)
    /// DropItem(1)
    /// DropAll()
    /// ExecuteAction() -> virtual
    /// DamageColorize()
    /// SearchForTarget()
    /// AttackTarget()
    /// AI()
    /// MoveToLand(1)

    //--------------------------------------------------

	public void MoveLeft()
	{
		GetComponent<Rigidbody2D>().AddForce( new Vector2(-GameCore.MOVE_FORCE*moveSpeed,0));
	}

    //--------------------------------------------------
	
	public void MoveRight()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GameCore.MOVE_FORCE*moveSpeed,0));
	}

    //--------------------------------------------------
	
	public void Stop()
	{
		action = EAction.none;
        preciseMovement = false;
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
            GameObject clone;

            clone = Instantiate(slashPrefab, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<SlashCore>().parent = gameObject;
            clone.transform.parent = gameObject.transform; // fixes slash wobbling bug
            clone.GetComponent<SlashCore>().team = team;

            hitCooldown = 30;
        }
	}
    
    //--------------------------------------------------

    public void PickUp(GameObject _body)

    // note: currently, only men are able to pick up correctly
    // todo: make this method virtual, then override it in ManCore

    {
        if (_body)
        {
            if (_body.GetComponent<BodyCore>().isCarried == false)
            {
                //if (Mathf.Abs(transform.position.x - body.transform.position.x) < 1f)
                //{
                    if (_body.GetComponent<ItemCore>())
                    {
                        if ((_body.GetComponent<ItemCore>().isTool == true)
                        && (GetComponent<ManCore>().tool == null)) 
                        {
                            // equip
                            
                            if (GetComponent<ManCore>())
                            {
                                GetComponent<ManCore>().Equip(_body);
                            }
                        }
                        else
                        {
                            // pick up
                            carriedBodies.Add(_body);

                            isCarrying = true;
                            _body.GetComponent<BodyCore>().isCarried = true;
                            _body.GetComponent<BodyCore>().carrier = gameObject;

                            action = EAction.none;

                            //
                        }
                    }
                    else
                    {
                        // pick up

                        carriedBodies.Add(_body);

                        isCarrying = true;
                        _body.GetComponent<BodyCore>().isCarried = true;
                        _body.GetComponent<BodyCore>().carrier = gameObject;

                        action = EAction.none;

                        //
                    }

                    if (GameCore.Core.player == gameObject)
                    GameCore.Core.InventoryManager();
                //}
            }
        }
    }

    //--------------------------------------------------

    public void DropItem(GameObject _object)

    // doesn't work on item in hand slot ?
    {
        int i = carriedBodies.IndexOf(_object);
        
        carriedBodies[i].GetComponent<BodyCore>().carrier = null;
        carriedBodies[i].GetComponent<BodyCore>().isCarried = false;
        carriedBodies[i].GetComponent<BodyCore>().isFalling = true;

        carriedBodies.RemoveAt(i);
        
        if (carriedBodies.Count == 0)
        isCarrying = false;

        if (GameCore.Core.player == gameObject)
        GameCore.Core.InventoryManager();
       
        action = EAction.none;
    }

    //--------------------------------------------------

    public void DropAll()
    {
        int i;

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

        action = EAction.none;
    }

    //--------------------------------------------------

    public void GiveItem(GameObject _item, GameObject _critter)
    {
        DropItem(_item);
        _critter.GetComponent<CritterCore>().PickUp(_item);
        _critter.GetComponent<CritterCore>().attitude += 5f;
    }

    //--------------------------------------------------

    public virtual void ExecuteAction()
    {
        switch (action)
        {
            case EAction.move:
            {
                break;
            }

            case EAction.idle:
            {
                break;
            }

            case EAction.follow:
            {
                break;
            }

            case EAction.pickUp:
            {
                PickUp(target);
                break;
            }

            case EAction.eat:
            {
                break;
            }

            case EAction.dropAll:
            {
                DropAll();
                break;
            }

            case EAction.giveItem:
            {
                if (gameObject == GameCore.Core.player)
                if (GameCore.Core.chosenObject)
                {
                    GiveItem(GameCore.Core.chosenObject, GameCore.Core.player.GetComponent<CritterCore>().target);
                }
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
    /*
    public void SearchForTarget()
    {
        int i;

        i = GameCore.Core.critters.Count;
        target = null;

        for (i=0; i <= GameCore.Core.critters.Count-1; i++)
        {
            if (GameCore.Core.critters[i])
            {
                if ((GameCore.Core.critters[i].GetComponent<CritterCore>().team != team)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().team != 0) // do not attack neutral critters
                && (attitude < 0))
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
                //action = EAction.follow;
                //target = GameCore.Core.player;
            }
            else
            action = EAction.idle;
        }
    }
    */
    public void FindNearestEnemy()
    {
        int i;
        float d = 10000f; // distance to enemy
        float pd = 10000f; // distance to enemy (previous)


        target = null;

        for (i=0; i < GameCore.Core.critters.Count; i++)
        {
            if (GameCore.Core.critters[i])
            {
                if ((GameCore.Core.critters[i].GetComponent<CritterCore>().team != team)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                && (GameCore.Core.critters[i].GetComponent<CritterCore>().team != 0)) // do not attack peaceful critters
                {
                    bool isEnemy = false;

                    if (team == 1)
                    {
                        if (GameCore.Core.critters[i].GetComponent<CritterCore>().attitude < 0)
                        isEnemy = true;
                    }
                    else
                    {
                        if (attitude < 0)
                        isEnemy = true;
                    }

                    if (isEnemy == true)
                    {
                        d = Mathf.Abs(transform.position.x - GameCore.Core.critters[i].transform.position.x);

                        if (d < pd)
                        {

                            target = GameCore.Core.critters[i];
                            action = EAction.attack;
                    
                            if (targetX == 0f) // fixes strange 'enemy men walking to the scene origin' bug
                            targetX = transform.position.x;

                            pd = d;
                        }
                    }
                    
                }
            }
        }

        if (target == null)
        {
            if (team == 1)
            {
                //action = EAction.follow;
                //target = GameCore.Core.player;
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
                // turning

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
    
    //----------------------------------------------------------------------------------------------

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
                timerAI = Random.Range(50,100);

                if (team != 0)
                attitude = GameCore.Core.teams[team].attitude;

                if (commandTarget)
                commandTargetDistance = Mathf.Abs(transform.position.x - commandTarget.transform.position.x);

                // ---------- 0. gathering point check -----------

                if ((team == 1)
                && (gameObject != GameCore.Core.player))
                {
                    if (GameCore.Core.gatheringPoint.activeSelf == true)
                    {
                        command = ECommand.guard;
                        commandTarget = GameCore.Core.gatheringPoint;
                    }
                    else
                    {
                        command = ECommand.guard;
                        commandTarget = GameCore.Core.player;
                    }
                }


                // ---------- 1. check command -----------

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
                            if (team != 0)
                            FindNearestEnemy();
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
                                FindNearestEnemy();
                            }
                        }
                    }
                }

                // ------------ 2. check for critters to run away from -------------

                // if herbi

                if (GetComponent<HerbiCore>()) 
                {
                    GameObject o = null;
                    float f = 0f;
                    float dist;

                    int i;

                    // check for men and carni

                    for (i=0; i<GameCore.Core.critters.Count; i++)
                    {
                        if ((GameCore.Core.critters[i].GetComponent<ManCore>())
                        || (GameCore.Core.critters[i].GetComponent<CarniCore>()))
                        {
                            if (GameCore.Core.critters[i] != gameObject)
                            if (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                            {
                                dist = Mathf.Abs(transform.position.x - GameCore.Core.critters[i].transform.position.x);

                                if (dist < sightRange)
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

                // if wildman

                else
                if ((GetComponent<ManCore>())
                && (GetComponent<CritterCore>().team == 0))
                {
                    GameObject o = null;
                    float f = 0f;
                    float dist;

                    int i;

                    // check for men and carni

                    for (i=0; i<GameCore.Core.critters.Count; i++)
                    {
                        if ( 
                                (
                                    (GameCore.Core.critters[i].GetComponent<ManCore>())
                                    && (GameCore.Core.critters[i].GetComponent<ManCore>().team == 1)
                                    && (attitude < 0)
                                )
                                ||
                                (GameCore.Core.critters[i].GetComponent<CarniCore>())
                            )
                        {
                            if (GameCore.Core.critters[i] != gameObject)
                            if (GameCore.Core.critters[i].GetComponent<CritterCore>().downed == false)
                            {
                                dist = Mathf.Abs(transform.position.x - GameCore.Core.critters[i].transform.position.x);

                                float range = 0f;

                                // if carni -> max range
                                if (GameCore.Core.critters[i].GetComponent<CarniCore>())
                                {
                                    range = 1000f;
                                }

                                // if man -> attitude determines range
                                else
                                if (GameCore.Core.critters[i].GetComponent<ManCore>())
                                {
                                    range = Mathf.Abs(attitude*10f);
                                }
                                
                                if (dist < range)
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

                        // if it's wildman, if positive attitude, if it's dark, then check for campfires and go there
                        if (GameCore.Core.sunlight.GetComponent<Light>().intensity < 0.5f)
                        if (team == 0)
                        if (GetComponent<ManCore>())
                        if (attitude > 0)
                        {
                            int i;
                            float _dist = 5000f;

                            // create campfire list

                            List<GameObject> _list = new List<GameObject>();

                            if (GameCore.Core.structures.Count > 0)
                            {
                                for (i=0; i<GameCore.Core.structures.Count; i++)
                                {
                                    if (GameCore.Core.structures[i].GetComponent<StructureCore>().structure == EStructure.campfire)
                                    {
                                        _list.Add(GameCore.Core.structures[i]);

                                        if (Mathf.Abs(transform.position.x - GameCore.Core.structures[i].transform.position.x) < _dist)
                                        {
                                            _dist = Mathf.Abs(transform.position.x - GameCore.Core.structures[i].transform.position.x);
                                        }
                                    }
                                }

                                if (_list.Count > 0)
                                if (_dist > 5f)
                                {
                                    // choose random campfire

                                    int r = 0;

                                    if (_list.Count > 1)                                
                                    r = Random.Range(0, _list.Count);
                                    
                                    targetX = _list[r].transform.position.x;
                                }
                            }
                        }
                        //
                    }

                    // if action is "follow", then walk around the target

                    else
                    if (action == EAction.follow) 
                    {
                        if (timerMove <= 0)
                        {
                            if (target)
                            {
                                targetX = target.transform.position.x + Random.Range(-10f,10f);
                            }

                            timerMove = 150;
                        }

                        
                    }

                    // if action is none of the above, then walk directly to the target to execute action

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
                float f1;

                if (preciseMovement == true)
                f1 = 0.1f;
                else
                f1 = 1f;


                if (targetXDistance > f1)
                {
                    if (targetX > transform.position.x)
                    {
                        MoveRight();
                        directionRight = true;
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                    {
                        MoveLeft();
                        directionRight = false;
                        GetComponent<SpriteRenderer>().flipX = true;
                    }

                }
                else // if distance to the target is very small, then execute the action
                {
                    ExecuteAction();
                }
            }

            // slowly raise attitude

            attitude += 0.00001f;

        }
    }

    //--------------------------------------------------
    /*
    public void MoveToLand(int _index)
    {
        // if leaving current land

        if (land == GameCore.Core.currentLand)
        {
            GameCore.Core.critters.Remove(gameObject);
            GameCore.Core.lands[_index].critters.Add(gameObject);

            land = _index;
            gameObject.SetActive(false);
        }

        // if leaving non-current land

        else
        {
            land = _index;
        }

    }
    */
    void OnDestroy()
    {
        GameCore.Core.critters.Remove(gameObject); 
    }
}