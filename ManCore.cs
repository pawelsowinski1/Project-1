using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists


public class ManCore : CritterCore
{
    // =================== MAN CORE ====================

    // Human or humanoidal creature. Can pick up, equip and throw items. Can gather and craft.

    // parent class:  CritterCore
    // child classes: PlayerCore

    public GameObject tool = null;

    /// Throw()
    /// DropAll()
    /// Equip(1)
    /// Unequip(1)
    /// Chop(1)

    // =================================================

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
    }

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
    } 

    //--------------------------------------------------

    public void Chop(GameObject obj)
    {
        if (obj)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.wood);
            clone.transform.position = obj.transform.position;
            
            //GameCore.Core.plants.Remove(obj); <- not necessary
            Destroy(obj);

            target = null;

        }
    }

    //--------------------------------------------------

    public void CutDown(GameObject obj)
    {
        if (obj)
        {
            obj.GetComponent<PlantCore>().rooted = false;
            obj.transform.Rotate(0,0,90f);
        }
    }

    //--------------------------------------------------

    public void ObtainMeat(GameObject obj)
    {
        if (obj)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.meat);
            clone.transform.position = obj.transform.position;

            Destroy(obj);
        }
    }

    //--------------------------------------------------
    
    public void CraftHandAxe(GameObject obj)
    {
        if (obj)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.handAxe);
            clone.transform.position = obj.transform.position;

            Destroy(obj);
        }
    }

    //--------------------------------------------------

    public void Convert(GameObject obj)
    {
        if (obj)
        {
            obj.GetComponent<CritterCore>().team = team;

            PantsCore p = obj.GetComponentInChildren<PantsCore>();
            p.team = team;
            p.RefreshColor();
        }
    }

    //--------------------------------------------------

    public void ProcessHemp(GameObject obj, GameObject _hemp )
    {
        if (obj)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.fibers);
            clone.transform.position = obj.transform.position;

            Destroy(_hemp);
            GameCore.Core.InventoryManager();
        }
    }

    //--------------------------------------------------

    public void CraftStoneSpear(GameObject _objBase, List<GameObject> _objToConsume)
    {
        if (_objBase)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.stoneSpear);
            clone.transform.position = _objBase.transform.position;

            for (i=0; i < _objToConsume.Count; i++)
            {
                Destroy(_objToConsume[i]);
            }

        }
    }

    //--------------------------------------------------

    public override void ExecuteAction()
    {
        base.ExecuteAction();

        switch (action)
        {
            case EAction.convert:
            {
                Convert(target);
                action = EAction.none;
                break;
            }
        }

    }

    //--------------------------------------------------
    
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
        AI();
    }
	
	//==================================================
}
