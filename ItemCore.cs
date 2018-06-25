using UnityEngine;
using System.Collections;

public enum ItemEnum {none, wood, meat, berry, hammerstone, flint, flint_blade};

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // parent class:  BodyCore
    // child classes: -

    public ItemEnum item;

    public int  quantity = 1;
    public bool isTool = false;
    public int  maxStack = 1; // <---- !

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
                maxStack = 5;
                break;
            }
            case ItemEnum.meat:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_meat;
                maxStack = 5;
                break;
            }      
            case ItemEnum.berry:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry;
                maxStack = 10;
                break;
            }
            case ItemEnum.hammerstone:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_hammerstone;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                maxStack = 5;
                isTool = true;
                break;
            }
            case ItemEnum.flint:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flint;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                maxStack = 5;
                break;
            }
            case ItemEnum.flint_blade:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_flint_blade;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                maxStack = 5;
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

                if (isTool == true)
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

                    //rotation
                    transform.SetPositionAndRotation(carrier.transform.position + new Vector3(0,0.6f,0) + v1,q);
                }
                else
                {
                    transform.position = carrier.transform.position + new Vector3(0,0.6f,0);
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
