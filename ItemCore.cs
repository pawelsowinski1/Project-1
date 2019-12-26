using UnityEngine;
using System.Collections;

public enum EItem {none, rock, roundRock, sharpRock, largeRock, flatRock, flint, handAxe, bark, firewood, plantMaterial,
                    meat, berries, smallLog, bigLog, wood, stoneSpear, cordage, cookedMeat, barkTorch, stick};

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // An item, which can be picked up. Can be equipped if it's a tool. Can burn in fire if it's flammable.

    // parent class:  BodyCore
    // child classes: -

    public EItem item;

    public bool isTool = false;

    // =================== METHODS ========================

    /// ItemInitialize()
    /// FollowIfCarried()

    // -------------------------------------------------

    public void ItemInitialize()
    {
        //GetComponent<SpriteRenderer>().sortingOrder = 10;

        switch (item)
        {
            case EItem.rock:
            {
                name = "Rock";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_rock;
                transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                isTool = true;
                break;
            }
            case EItem.roundRock:
            {
                name = "Round rock";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_roundRock;
                transform.localScale = new Vector3(0.55f,0.55f,0.55f);
                isTool = true;
                break;
            }
            case EItem.sharpRock:
            {
                name = "Sharp rock";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_sharpRock;
                transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                isTool = true;
                break;
            }
            case EItem.flatRock:
            {
                name = "Flat rock";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flatRock;
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                break;
            }
            case EItem.wood:
            {
                name = "Wood";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_wood;
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 4f;

                break;
            }
            case EItem.meat:
            {
                name = "Meat";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_meat;
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);

                break;
            }      
            case EItem.berries:
            {
                name = "Berries";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry;

                break;
            }
            case EItem.flint:
            {
                name = "Flint";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flint;
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                break;
            }
            case EItem.handAxe:
            {
                name = "Hand axe";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_handAxe;
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                isTool = true;
                break;
            }  
            case EItem.stoneSpear:
            {
                name = "Stone spear";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_stoneSpear;
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                isTool = true;
                break;
            }  
            case EItem.cordage:
            {
                name = "Cordage";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_cordage;
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 0.5f;

                break;
            }  
            case EItem.smallLog:
            {
                name = "Small log";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_smallLog;
                transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                isTool = true;

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 4f;

                break;
            }
            case EItem.plantMaterial:
            {
                name = "Plant material";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_plantMaterial;
                transform.localScale = new Vector3(0.6f,0.6f,0.6f);

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 1f;

                break;
            }  
            case EItem.firewood:
            {
                name = "Firewood";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_firewood;
                transform.localScale = new Vector3(0.6f,0.6f,0.6f);

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 2f;

                break;
            }  
            case EItem.bark:
            {
                name = "Bark";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_bark;
                transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                isTool = true;

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 0.5f;

                break;
            }  
            case EItem.cookedMeat:
            {
                name = "Cooked meat";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_cookedMeat;
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);

                break;
            }  

            case EItem.barkTorch:
            {
                name = "Bark torch";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_barkTorch;
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                isTool = true;

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 2f;

                break;
            }

            case EItem.stick:
            {
                name = "Stick";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_stick;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                isTool = true;

                gameObject.AddComponent<Burnable>();
                GetComponent<Burnable>().fuel = 1f;

                break;
            }
            
        }

        // resize box collider 2D to fit the sprite; 
        Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        //

        // if it's not a tool, then set box collider 2D offset 
        if (isTool == false)
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (0, (S.y / 2));
        //
    }

    // -------------------------------------------------

    void FollowIfCarried()
    {
        if (isCarried == true)
        {
            if (carrier != null)
            {
                // if carried by a critter

                if (carrier.GetComponent<CritterCore>())
                {
                    // if carried in tool slot

                    if (carrier.GetComponent<ManCore>())
                    if (carrier.GetComponent<ManCore>().hand1Slot == gameObject)
                    {
                        Vector3 v1;
                            
                        Quaternion q;
                        q = new Quaternion();

                        if (carrier.GetComponent<CritterCore>().directionRight == true)
                        {
                            v1 = new Vector3(0.25f,0f,0f);
                            q.Set(0f,0f,-0.38f,0.92f);
                        }
                        else
                        {
                            v1 = new Vector3(-0.25f,0f,0f);
                            q.Set(0f,0f,0.38f,0.92f);
                        }

                        transform.SetPositionAndRotation(carrier.transform.position + new Vector3(0f,0.6f,0f) + v1,q);
                    }

                    // if carried in inventory slot
                    else
                    {
                        transform.position = carrier.transform.position + new Vector3(0,0.6f,0); 
                    }
                }
                else
                // if carried by a projectile
                {
                    transform.position = carrier.transform.position + new Vector3(0f,0f,0f); 
                }
            }
        }
    }

    // =================================================


    /// ----- START -----

	void Start()
	{
		BodyInitialize();
        ItemInitialize();
	}

    /// ----- UPDATE -----

	void Update() 
	{
		Gravity();
        FollowIfCarried();
	}

    void OnDestroy()
    {
        GameCore.Core.items.Remove(gameObject); 
    }
}
