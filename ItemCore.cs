using UnityEngine;
using System.Collections;

public enum EItem {none, rock, roundRock, sharpRock, largeRock, flatRock, flint, handaxe, birchBark, firewood, plantMaterial,
                    meat, berries, fibers, smallLog, bigLog, wood, stoneSpear};

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // An item, which can be picked up. Can be equipped if it's a tool.

    // parent class:  BodyCore
    // child classes: -

    public EItem item;

    public bool isTool = false;

    // ------------ METHODS ---------------

    /// ItemInitialize()
    /// FollowIfCarried()

    // -------------------------------------------------

    void ItemInitialize()
    {
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
            case EItem.wood:
            {
                name = "Wood";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_wood;
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                break;
            }
            case EItem.meat:
            {
                name = "Meat";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_meat;
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
                transform.localScale = new Vector3(0.45f,0.45f,0.45f);
                break;
            }
            case EItem.handaxe:
            {
                name = "Hand axe";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_handaxe;
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

                if (carrier.GetComponent<InteractiveObjectCore>().kind == EKind.critter)
                {
                    // if carried in tool slot
                    if ((isTool == true)
                    && (GameCore.Core.player.GetComponent<ManCore>().tool == gameObject))
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
                    else
                    {
                        // if carried in inventory slot
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

        GetComponent<SpriteRenderer>().sortingOrder = 10;
        
	}

    /// ----- UPDATE -----

	void Update() 
	{
		CalculateLand();
		PlaceOnGround();
        FollowIfCarried();
	}
}
