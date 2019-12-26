using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists


public class ManCore : CritterCore
{
    // =================== MAN  ====================

    // A human. Can equip, use and throw items. Can process and craft items. Collects points.

    // parent class:  CritterCore
    // child classes: PlayerCore

    public GameObject hand1Slot, hand2slot, headSlot, bodySlot, feetSlot = null;


    public float rp, bp, gp, yp; // red, blue, green and yellow points

    /// Throw()
    /// Equip(1)
    /// Unequip(1)
    /// DropHand1Slot() ----!!
    /// Chop(1)
    /// CutDown(1)
    /// ObtainMeat(1)
    /// CraftHandAxe(1)
    /// Convert(1)
    /// ProcessHemp(1)
    /// SetOnFire(1)
    /// ProcessTree(1)
    /// CraftBarkTorch(1)
    /// AddFuel(1)

    // =================================================

    //--------------------------------------------------

    public void Throw()
	{
        if (hand1Slot != null)
        {
            /*
		    clone = Instantiate (projectilePrefab,transform.position,transform.rotation) as GameObject;
            clone.GetComponent<InteractiveObjectCore>().kind = EKind.projectile;
            clone.GetComponent<ProjectileCore>().parent = gameObject;
		    clone.GetComponent<ProjectileCore>().team = team;
		    */

            hand1Slot.AddComponent<ProjectileCore>();
            hand1Slot.GetComponent<ProjectileCore>().parent = gameObject;
            hand1Slot.GetComponent<ProjectileCore>().team = team;
            hand1Slot.GetComponent<ItemCore>().enabled = false;

		    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    
            hand1Slot.GetComponent<BodyCore>().isCarried = false;
            hand1Slot.GetComponent<BodyCore>().carrier = null;

            // if player
            Vector3 v1 = mousePos-transform.position;
            v1.Normalize();
            v1 *= 30f;
            //

            hand1Slot.GetComponent<Rigidbody2D>().AddForce(v1, ForceMode2D.Impulse);
            

            hand1Slot = null;

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
                    if (hand1Slot != null)
                    {
                        Unequip(hand1Slot);
                    }
                    
                    hand1Slot = _body;

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
        if (hand1Slot == item) // if item is in hand1Slot slot
        {
            carriedBodies.Add(item);
            hand1Slot = null;
        }

        if (GameCore.Core.player == gameObject)
        GameCore.Core.InventoryManager();
    } 

    //--------------------------------------------------

    public void DropHand1Slot()
    {
        hand1Slot.GetComponent<BodyCore>().carrier = null;
        hand1Slot.GetComponent<BodyCore>().isCarried = false;
        hand1Slot.GetComponent<BodyCore>().isFalling = true;

        hand1Slot = null;

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
            obj.GetComponent<PlantCore>().isRooted = false;
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

    public void Convert(GameObject _obj)
    {
        if (_obj)
        {
            if (_obj.GetComponent<CritterCore>().attitude >= 100f)
            {
                _obj.GetComponent<CritterCore>().team = team;

                PantsCore p = _obj.GetComponentInChildren<PantsCore>();
                p.team = team;
                p.RefreshColor();

            }
            else
            {
                MessageText("He refused to join the tribe.");
            }
        }

        Stop();
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
        _obj.GetComponent<Burnable>().SetOnFire();


        /*

        if (_obj)
        if (_obj.GetComponent<BodyCore>().onFire == false)
        {
            // create fire

            GameObject clone;

            _obj.GetComponent<BodyCore>().onFire = true;
            
            clone = Instantiate(GameCore.Core.firePrefab, _obj.transform.position, transform.rotation) as GameObject;
            clone.transform.parent = _obj.transform;

            if (_obj.GetComponent<ItemCore>())
            {

                // if bark torch

                if (_obj.GetComponent<ItemCore>().item == EItem.barkTorch)
                {
                    // translate fire according to torch rotation

                    clone.transform.rotation = clone.transform.parent.rotation;
                    clone.transform.Translate(Vector3.up*0.75f, Space.Self);
                }

                // if firewood
            
                if (_obj.GetComponent<ItemCore>().item == EItem.firewood)
                {
                    // spawn campfire and destory firewood

                    clone = GameCore.Core.SpawnStructure(EStructure.campfire);
                    clone.transform.position = _obj.transform.position;
                    clone.GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_firewood;
                    clone.transform.localScale = new Vector3(0.6f,0.6f,0.6f);

                    //_obj.GetComponent<BodyCore>().onFire = true; <---- ???
            
                    Destroy(_obj);
                }
            }
        }

        */
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
            _tree.GetComponent<Tree>().DropResources();    

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
            clone.transform.position = _tree.transform.position;
        }
    }

    //--------------------------------------------------
    
    public void AddFuel(GameObject _fireplace, GameObject _fuel)
    {
        if (_fuel)
        {
            _fireplace.GetComponent<FireplaceCore>().GainFuel(_fuel);
            Destroy(_fuel);

            if (_fuel == hand1Slot)
            hand1Slot = null;
            else
            carriedBodies.Remove(_fuel);

            if (gameObject == GameCore.Core.player)
            {
                GameCore.Core.InventoryManager();
            }
        }
    }

    //--------------------------------------------------

        public void CraftCordage(GameObject _grass)
    {
        if (_grass)
        {
            GameObject clone;
            clone = GameCore.Core.SpawnItem(EItem.cordage);
            clone.transform.position = _grass.transform.position;

            Destroy(_grass);
        }
    }

    //--------------------------------------------------

    public void CraftBarkTorch(List<GameObject> _objToConsume)
    {
        GameObject clone;
        clone = GameCore.Core.SpawnItem(EItem.barkTorch);
        clone.transform.position = transform.position;

        int i;

        for (i=0; i < _objToConsume.Count; i++)
        {
            Destroy(_objToConsume[i]);
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
                if (hand1Slot)
                {
                    if ((hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                    || (hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        Hit();
                    }
                }
                break;
            }

            case EAction.craftHandAxe:
            {
                if (hand1Slot)
                {
                    if (hand1Slot.GetComponent<ItemCore>().item == EItem.roundRock)
                    {
                        Hit();
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
                if (hand1Slot)
                {
                    if ((hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                    || (hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
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
                        // if chosen object is burnable    

                        if (GameCore.Core.chosenObject.GetComponent<Burnable>())
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

                                GameObject clone;

                                clone = Instantiate(GameCore.Core.projectPrefab, target.transform.position, Quaternion.identity);
                                clone.GetComponent<ProjectCore>().action = EAction.heating;
                                clone.GetComponent<ProjectCore>().target = target; // set fireplace as target
                                clone.GetComponent<ProjectCore>().secondaryTarget = GameCore.Core.chosenObject; // set meat as secondary target
                                target.GetComponent<PhysicalObject>().hasProject = true;

                            }
                        }
                    }
                }

                break;

            }


        }
    }

    //--------------------------------------------------
    
	// =========================================== MAIN ===========================================
	
	void Start()
	{
		BodyInitialize();

        // add hudText

        GameObject clone;
        clone = Instantiate(GameCore.Core.hudTextPrefab, GameCore.Core.myCanvas.transform);
        clone.GetComponent<HudText>().objectToFollow = gameObject;

        //

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
		DamageColorize();
    }

	void FixedUpdate()
	{
		Gravity();
        AI();
    }
	
	//--------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (downed == false)
        {
            if (team != 0)
            {
                if (hand1Slot == null)
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

    void OnEnable()
    {
        AddHpBar();
    }

    void OnDisable()
    {
        Destroy(hpBar);
    }
}
