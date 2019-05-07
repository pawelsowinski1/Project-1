using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists


public class ManCore : CritterCore
{
    // =================== MAN  ====================

    // A human. Can equip, use and throw tools. Can process and craft items. Collects points.

    // parent class:  CritterCore
    // child classes: PlayerCore

    public GameObject tool = null;

    public float rp, bp, gp, yp; // red, blue, green and yellow points

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

            action = EAction.none;
        }
    }

    //--------------------------------------------------

    public void ProcessHemp(GameObject _hemp )
    {
        if (_hemp)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.cordage);
            clone.transform.position = _hemp.transform.position;
            clone = GameCore.Core.SpawnItem(EItem.cordage);
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

    public void CraftStoneSpear(List<GameObject> _objToConsume)
    {
        GameObject clone;
        clone = GameCore.Core.SpawnItem(EItem.stoneSpear);
        clone.transform.position = transform.position;

        int i;

        for (i=0; i < _objToConsume.Count; i++)
        {
            Destroy(_objToConsume[i]);
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

            case EAction.putItemInFireplace: 

            // add fuel to fireplace / put fireproof container on the fireplace
            {
                if (gameObject == GameCore.Core.player)
                if (GameCore.Core.chosenObject)
                {
                    if (target.GetComponent<FireplaceCore>())
                    {
                        // if item being put into fireplace is flammable    

                        if (GameCore.Core.chosenObject.GetComponent<ItemCore>().isFlammable == true)
                        {
                            AddFuel(target, GameCore.Core.chosenObject);
                        }

                        // if not, 
                        else
                        {
                            // check if item being put into fireplace is a fireproof container (e.g. flat rock, clay pot)

                            if (GameCore.Core.chosenObject.GetComponent<ItemCore>().item == EItem.flatRock)
                            {
                                // put flat rock in the fireplace
                                
                                DropItem(GameCore.Core.chosenObject);
                                
                                GameCore.Core.chosenObject.transform.position =
                                new Vector3(target.transform.position.x,
                                            GameCore.Core.chosenObject.transform.position.y,
                                            GameCore.Core.chosenObject.transform.position.z);

                                target.GetComponent<FireplaceCore>().itemInFire = GameCore.Core.chosenObject;
                                GameCore.Core.chosenObject.GetComponent<ItemCore>().isCarried = true; // prevent from being picked up
                                

                            }
                        }
                    }
                }

                break;
            }

            case EAction.heatItem:
            // put item into the heated container in the fireplace
            {
                if (gameObject == GameCore.Core.player)
                if (GameCore.Core.chosenObject)
                {
                    // if there is any heated container in fireplace (e.g. flat rock, clay pot)

                    if (target.GetComponent<FireplaceCore>().itemInFire)
                    {
                        if (target.GetComponent<FireplaceCore>().itemInFire.GetComponent<ItemCore>().item == EItem.flatRock)
                        {
                            if ((GameCore.Core.chosenObject.GetComponent<ItemCore>().item == EItem.meat)
                            && !(target.GetComponent<FireplaceCore>().itemHeated))
                            {
                                // put meat on top of flat rock

                                DropItem(GameCore.Core.chosenObject);

                                GameCore.Core.chosenObject.transform.position =
                                new Vector3(target.transform.position.x, target.transform.position.y,
                                            GameCore.Core.chosenObject.transform.position.z);

                                target.GetComponent<FireplaceCore>().itemHeated = GameCore.Core.chosenObject;

                                // add project

                                clone = Instantiate(GameCore.Core.projectPrefab, target.transform.position, Quaternion.identity);
                                clone.GetComponent<ProjectCore>().action = EAction.heating;
                                clone.GetComponent<ProjectCore>().target = target; // set fireplace as target
                                clone.GetComponent<ProjectCore>().secondaryTarget = GameCore.Core.chosenObject; // set meat as secondary target
                                target.GetComponent<InteractiveObjectCore>().hasProject = true;

                            }
                        }
                    }
                }

                break;

            }


        }
    }

    //--------------------------------------------------
    
	// =========================================== MAIN LOOP ===========================================
	
	void Start()
	{
		BodyInitialize();

        rp = 0f;
        bp = 0f;
        gp = 0f;
        yp = 0f;

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
