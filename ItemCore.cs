using UnityEngine;
using System.Collections;

public enum ItemEnum {nothing, wood, meat, berry, hammerstone, flint, flint_blade};

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // parent class:  BodyCore
    // child classes: -

    public ItemEnum item;

    public Sprite spr_wood;
    public Sprite spr_meat;
    public Sprite spr_berry;
    public Sprite spr_hammerstone;
    public Sprite spr_flint;
    public Sprite spr_flint_blade;

    public int  quantity = 1;
    public bool isTool = false;
    public int  maxStack = 1;

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
                GetComponent<SpriteRenderer>().sprite = spr_wood;
                maxStack = 5;
                break;
            }
            case ItemEnum.meat:
            {
                GetComponent<SpriteRenderer>().sprite = spr_meat;
                maxStack = 5;
                break;
            }      
            case ItemEnum.berry:
            {
                GetComponent<SpriteRenderer>().sprite = spr_berry;
                maxStack = 10;
                break;
            }
            case ItemEnum.hammerstone:
            {
                GetComponent<SpriteRenderer>().sprite = spr_hammerstone;
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                maxStack = 5;
                isTool = true;
                break;
            }
            case ItemEnum.flint:
            {
                GetComponent<SpriteRenderer>().sprite = spr_flint;
                maxStack = 5;
                break;
            }
            case ItemEnum.flint_blade:
            {
                GetComponent<SpriteRenderer>().sprite = spr_flint_blade;
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

        label = "\n\n\n"+"name: "+gameObject.name+"\nquantity: "+quantity.ToString()+"/"+maxStack.ToString()+"\nitem: "+item.ToString();
	}
}
