using UnityEngine;
using System.Collections;



public class ManCore : CritterCore
{
    // =================== MAN CORE ====================

    // Human or humanoidal creature. Can pick up, equip and throw items. Can gather and craft.

    // parent class:  CritterCore
    // child classes: PlayerCore

    public GameObject tool = null;

    /// PickUp(1) -> override from CritterCore
    /// Hit() -> test override without virtual function
    /// Throw()
    /// DropAll()
    /// Equip(1)
    /// Unequip(1)
    /// Chop(1)

    // =================================================

    public override void PickUp(GameObject body) // overrides PickUp() from CritterCore
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
                }

                if (GameCore.Core.player == gameObject)
                GameCore.Core.InventoryManager();
            }
        }
    }

    //--------------------------------------------------

    new public void Hit()
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


    public void Throw()
	{
        if (tool != null)
        {
            /*
		    clone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
            clone.GetComponent<InteractiveObjectCore>().kind = EKind.projectile;
            clone.GetComponent<ProjectileCore>().parent = gameObject;
		    clone.GetComponent<ProjectileCore>().team = team;
		    */

            tool.AddComponent<ProjectileCore>();
            tool.GetComponent<ProjectileCore>().kind = EKind.projectile;
            tool.GetComponent<ProjectileCore>().parent = gameObject;
            tool.GetComponent<ProjectileCore>().team = team;
            tool.GetComponent<ItemCore>().enabled = false;

		    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    
            tool.GetComponent<BodyCore>().isCarried = false;
            tool.GetComponent<BodyCore>().carrier = null;

            // if player
            Vector3 v1 = mousePos-transform.position;
            v1.Normalize();
            v1 *= 30f;
            //

            tool.GetComponent<Rigidbody2D>().AddForce(v1, ForceMode2D.Impulse);
            

            tool = null;

            if (GameCore.Core.player == gameObject)
            {
                GameCore.Core.InventoryManager();
            }
        }
	}

    //--------------------------------------------------

    public void Equip(GameObject body)
    {
        if (body)
        {
            if (body.GetComponent<InteractiveObjectCore>().kind == EKind.item)
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
            i = GameCore.Core.SpawnItem(EItem.wood);
            i.transform.position = obj.transform.position;
            
            //GameCore.Core.plants.Remove(obj); <- not necessary
            Destroy(obj);

            target = null;

        }
    }

    //--------------------------------------------------

    public void AI_Man()
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
                    case EAction.chop:
                    {
                        Chop(target);
                        command = EAction.none;
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
    
    
	// =========================================== MAIN LOOP ===========================================
	
	void Start()
	{

		BodyInitialize();

        timerMove = 1;
        timerHit = Random.Range(30, 70);
	}
	
	//--------------------------------------------------
	
	void Update()
	{
        CalculateLand();
		PlaceOnGround();
		DamageColorize();
    }
    

	void FixedUpdate()
	{
        SearchForTarget();
        AttackTarget();

        AI_Man();
    }
	
	//==================================================
}
