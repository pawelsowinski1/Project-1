using UnityEngine;
using System.Collections;

public enum ItemEnum {none, wood, meat, berry, hammerstone, flint, flint_blade};

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // An item, which can be picked up. Can be equipped if it's a tool.

    // parent class:  BodyCore
    // child classes: -

    public ItemEnum item;

    public bool isTool = false;

    // ------------ METHODS ---------------

    /// ItemInitialize()
    /// FollowIfCarried()

    // -------------------------------------------------

    void ItemInitialize()
    {
        switch (item)
        {
            case ItemEnum.wood:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_wood;
                break;
            }
            case ItemEnum.meat:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_meat;
                break;
            }      
            case ItemEnum.berry:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry;
                break;
            }
            case ItemEnum.hammerstone:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_hammerstone;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                isTool = true;
                break;
            }
            case ItemEnum.flint:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flint;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                break;
            }
            case ItemEnum.flint_blade:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flint_blade;
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                isTool = true;
                break;
            }        
        }
    }


    // -------------------------------------------------

    void FollowIfCarried()
    {
        if (isCarried == true)
        {
            if (carrier != null)
            {
                // if carried by a critter

                if (carrier.GetComponent<InteractiveObjectCore>().kind == KindEnum.critter)
                {
                    // if carried in tool slot
                    if ((isTool == true)
                    && (GameCore.Core.player.GetComponent<CritterCore>().tool == gameObject))
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

        //int i = 2;
        //GetComponent<SpriteRenderer>().sortingLayerID = i;
        
	}

    /// ----- UPDATE -----

	void Update() 
	{
		CalculateLand();
		PlaceOnGround();
        FollowIfCarried();
	}
}
