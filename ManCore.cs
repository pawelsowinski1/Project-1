using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists


public class ManCore : CritterCore
{
    // =================== MAN CORE ====================

    // Human or humanoidal creature. Can use tools and throw items.

    // parent class:  CritterCore
    // child classes: PlayerCore

    public GameObject tool = null;

    /// Throw()
    /// Equip(1)
    /// Unequip(1)
    /// DropTool()
    /// Chop(1)
    /// CutDown(1)
    /// ObtainMeat(1)
    /// CraftHandAxe(1)
    /// Convert(1)
    /// ProcessHemp(1)
    /// SetOnFire(1)

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

    public void Equip(GameObject _body)
    {
        if (_body)
        {
            if (_body.GetComponent<ItemCore>())
            {
                if (_body.GetComponent<ItemCore>().isTool == true)
                {
                    if (tool != null)
                    {
                        Unequip(tool);
                    }
                    
                    tool = _body;

                    isCarrying = true;
                    _body.GetComponent<BodyCore>().isCarried = true;
                    _body.GetComponent<BodyCore>().carrier = gameObject;

                    int i;

                    if (GameCore.Core.player == gameObject) // if player
                    {
                        for (i=0; i<carriedBodies.Count; i++)
                        {
                            // check if the target body is in player's inventory
                            if (GetComponent<CritterCore>().carriedBodies[i] == _body)
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

    public void DropTool()
    {
        tool.GetComponent<BodyCore>().carrier = null;
        tool.GetComponent<BodyCore>().isCarried = false;
        tool.GetComponent<BodyCore>().isFalling = true;

        tool = null;

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
    
    public void CraftHandAxe(GameObject _flint)
    {
        if (_flint)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.handAxe);
            clone.transform.position = _flint.transform.position;

            Destroy(_flint);
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

    public void ProcessHemp(GameObject _hemp )
    {
        if (_hemp)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.fibers);
            clone.transform.position = _hemp.transform.position;
            clone = GameCore.Core.SpawnItem(EItem.fibers);
            clone.transform.position = _hemp.transform.position + new Vector3(-0.2f,0f,0f);
            clone = GameCore.Core.SpawnItem(EItem.plantMaterial);
            clone.transform.position = _hemp.transform.position + new Vector3(0.2f,0f,0f);;

            Destroy(_hemp);
            GameCore.Core.InventoryManager();
        }
    }

    //--------------------------------------------------

    public void SetOnFire(GameObject _obj)
    {
        if (_obj)
        if (_obj.GetComponent<BodyCore>().onFire == false)
        {
            GameObject clone;

            _obj.GetComponent<BodyCore>().onFire = true;
            
            clone = Instantiate(GameCore.Core.firePrefab, _obj.transform.position, transform.rotation) as GameObject;
            clone.transform.parent = _obj.transform;


            if (_obj.GetComponent<ItemCore>())
            if (_obj.GetComponent<ItemCore>().item == EItem.firewood)
            {
                clone = GameCore.Core.SpawnStructure(EStructure.campfire);
                clone.transform.position = _obj.transform.position;
                clone.GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_firewood;
                clone.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

                _obj.GetComponent<BodyCore>().onFire = true;
            
                Destroy(_obj);

            }
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

            int i;

            for (i=0; i < _objToConsume.Count; i++)
            {
                Destroy(_objToConsume[i]);
            }

        }
    }

    //--------------------------------------------------

    public void ProcessTree(GameObject _tree)
    {
        if (_tree)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.smallLog);
            clone.transform.position = _tree.transform.position;
            clone = GameCore.Core.SpawnItem(EItem.firewood);
            clone.transform.position = _tree.transform.position + new Vector3(-0.2f,0f,0f);
            clone = GameCore.Core.SpawnItem(EItem.plantMaterial);
            clone.transform.position = _tree.transform.position + new Vector3(0.2f,0f,0f);

            Destroy(_tree);
        }
    }

    //--------------------------------------------------

    public void CollectBark(GameObject _tree)
    {
        if (_tree)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.bark);
            clone.transform.position = _tree.transform.position + new Vector3(0f,0f,0f);
        }
    }

    //--------------------------------------------------
    
    public void AddFuel(GameObject _fireplace, GameObject _fuel)
    {
        if (_fuel)
        {
            _fireplace.GetComponent<FireplaceCore>().GainFuel(_fuel);
            Destroy(_fuel);

            if (_fuel == tool)
            tool = null;
            else
            carriedBodies.Remove(_fuel);

            if (gameObject == GameCore.Core.player)
            {
                GameCore.Core.InventoryManager();
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
                Stop();
                break;
            }

            case EAction.cutDown:
            {
                if (tool)
                {
                    if ((tool.GetComponent<ItemCore>().item == EItem.sharpRock)
                    || (tool.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        Hit();
                    }
                }
                break;
            }

            case EAction.craftHandAxe:
            {
                if (tool)
                {
                    if (tool.GetComponent<ItemCore>().item == EItem.roundRock)
                    {
                        Hit();
                    }
                }
                break;
            }

            case EAction.processHemp:
            {
                if (tool)
                {
                    if (tool.GetComponent<ItemCore>().item == EItem.roundRock)
                    {
                        if (target.GetComponent<InteractiveObjectCore>().projectAttached)
                        {
                            if (target.GetComponent<InteractiveObjectCore>().projectAttached.GetComponent<ProjectCore>().ready == true)
                            Hit();
                        }
                    }
                }
                break;
            }

            case EAction.setOnFire:
            {
                SetOnFire(target);
                Stop();
                break;
            }

            case EAction.processTree:
            {
                if (tool)
                {
                    if ((tool.GetComponent<ItemCore>().item == EItem.sharpRock)
                    || (tool.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        Hit();
                    }
                }
                break;
            }

            case EAction.collectBark:
            {
                if (tool)
                {
                    if ((tool.GetComponent<ItemCore>().item == EItem.sharpRock)
                    || (tool.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        Hit();
                    }
                }
                break;
            }

            case EAction.addFuel:
            {
                if (gameObject == GameCore.Core.player)
                AddFuel(target, GameCore.Core.player.GetComponent<PlayerCore>().chosenObject);
                //else
                //AddFuel(target, GameCore.Core.player.GetComponent<PlayerCore>().?);

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
	
	//--------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (downed == false)
        {
            if (team != 0)
            {
                if (tool == null)
                {
                    bool b = false;

                    if (other.gameObject.GetComponent<ItemCore>())
                    if (other.gameObject.GetComponent<ItemCore>().isTool == true)
                    b = true;

                    if (b == true)
                    {
                        if (other.gameObject.GetComponent<BodyCore>().isCarried == false)
                        {
                            PickUp(other.gameObject);
                        }
                    }
                }
            }
        }
    }

	//==================================================
}
