using UnityEngine;
using System.Collections;

public class SlashCore : MonoBehaviour
{
    // ================= SLASH =========================

	public GameObject parent;
    public int team;

	Vector3 v;
	Transform target;

	public bool directionRight;
    public bool alive = true; // fixes multiple hit issue

    // =================================================

	void Start () 
	{
        team = parent.GetComponent<CritterCore>().team;
        directionRight = parent.GetComponent<CritterCore>().directionRight;

        // set position vector and life time

        if (parent.GetComponent<ManCore>())
        {
            if (parent.GetComponent<ManCore>().hand1Slot == null)
            {
		        v.Set(0,0.85f,0); 
                Destroy(gameObject,0.10f);
            }
            else
            {
                v.Set(0, 1.5f, 0);
                Destroy(gameObject, 0.18f);
            }
        }

        // activate collider component

        GetComponent<Collider2D>().enabled = true;

        //

		target = parent.GetComponent<Transform>();

		transform.position = target.position + v;
		//transform.Rotate(0,0,90f);

        if (parent.GetComponent<ManCore>())
        {
            if (parent.GetComponent<ManCore>().hand1Slot == null)
            {
                // rotate the slash according to mouse or target position

                if ((parent == GameCore.Core.player) // aim at mouse position
                && (GameCore.Core.combatMode == true))
                {
                    // Get Angle in Radians
                    float AngleRad = Mathf.Atan2(GameCore.Core.mousePos.y - transform.position.y, GameCore.Core.mousePos.x - transform.position.x);
                    // Get Angle in Degrees
                    float AngleDeg = (180 / Mathf.PI) * AngleRad;
                    // Rotate Object
                    this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
                }
                else                              // aim at target
                {
                    // Get Angle in Radians
                    float AngleRad = Mathf.Atan2(parent.GetComponent<ManCore>().target.transform.position.y - transform.position.y,
                                                parent.GetComponent<ManCore>().target.transform.position.x - transform.position.x);
                    // Get Angle in Degrees
                    float AngleDeg = (180 / Mathf.PI) * AngleRad;
                    // Rotate Object
                    this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
                }
            }
        }

        // set the sprite 

        if (parent.GetComponent<ManCore>())
        {
            // if parent has a hand1Slot, copy the hand1Slot's sprite

            if (parent.GetComponent<ManCore>().hand1Slot != null)
            {
                GetComponent<SpriteRenderer>().sprite = parent.GetComponent<ManCore>().hand1Slot.GetComponent<SpriteRenderer>().sprite;
                transform.localScale = parent.GetComponent<ManCore>().hand1Slot.transform.localScale/2;
            }            
        }
        //

        // resize box collider 2D to fit the sprite; 
        Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (0, (S.y / 2));
        //

	}

	void FixedUpdate()
	{
        
        if (parent.GetComponent<ManCore>())
        if (parent.GetComponent<ManCore>().hand1Slot == null)
        {
            transform.Translate(Vector3.right*0.2f);
        }
        else
        {
            //transform.position = target.position + v;

		    if (directionRight == true)
            {
        	    transform.RotateAround(target.position + new Vector3(0,0.85f,0), new Vector3(0,0,1), -20f); // slash rotation 
            }
		    else
            {
		        transform.RotateAround(target.position + new Vector3(0,0.85f,0), new Vector3(0,0,1), 20f); // slash rotation 
            }
        }
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
        // -----------------------------------------------------------------------------------------------------

        // if hit critter

        if (other.gameObject.GetComponent<CritterCore>())
        if (alive == true)
        if (other.gameObject.GetComponent<CritterCore>().alive == true)
		if (other.gameObject.GetComponent<CritterCore>().team != team)
		{
            if (parent.GetComponent<CritterCore>().team == 1)
            {
                if (other.gameObject.GetComponent<CritterCore>().team == 0)
                other.GetComponent<CritterCore>().attitude -= 1f;
                else
                GameCore.Core.teams[other.gameObject.GetComponent<CritterCore>().team].attitude -= 1f;
            }

			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;
            other.gameObject.GetComponent<CritterCore>().hp -= 9f;

            if ((other.gameObject.GetComponent<CritterCore>().hp <= 0f)
            && (other.gameObject.GetComponent<CritterCore>().downed == false))
            {
            other.gameObject.GetComponent<CritterCore>().downed = true;
                
                if (other.gameObject.GetComponent<CarniCore>())
                other.transform.Rotate(0,0,180f);
                else
                other.transform.Rotate(0,0,90f);

                if (other.gameObject.GetComponent<ManCore>())
                {
                    if (other.gameObject.GetComponent<ManCore>().hand1Slot != null)
                    {
                        other.gameObject.GetComponent<ManCore>().DropHand1Slot();
                    }
                }
            }

            if (other.gameObject.GetComponent<CritterCore>().hp <= -other.gameObject.GetComponent<CritterCore>().hpMax)
            {
                other.gameObject.GetComponent<CritterCore>().alive = false;
                GameCore.Core.critters.Remove(other.gameObject);

                if (other.gameObject.GetComponent<CritterCore>().hpBar)
                {
                    Destroy(other.gameObject.GetComponent<CritterCore>().hpBar);
                    Destroy(other.gameObject.GetComponent<CritterCore>().hpBar.GetComponent<HpBar>().frame);
                }
            }
            
            alive = false;
            Destroy(gameObject,0.0f);
		}

        // -----------------------------------------------------------------------------------------------------

        // if hit project

        if (other.gameObject.GetComponent<ProjectCore>())
        {
            bool b = false;

            // check conditions

            switch (other.gameObject.GetComponent<ProjectCore>().action)
            {
                case EAction.cutDown:
                {
                    if (parent.GetComponent<ManCore>().hand1Slot)
                    {
                        if ((parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                        || (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                        {
                            b = true;
                        }
                    }

                    break;
                }

                case EAction.obtainMeat:
                {
                    if (parent.GetComponent<ManCore>().hand1Slot)
                    {
                        if ((parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                        || (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                        {
                            b = true;
                        }
                    }

                    break;
                }

                case EAction.craftHandAxe:
                {
                    if (parent.GetComponent<ManCore>().hand1Slot)
                    {
                        if (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.roundRock)
                        {
                            b = true;
                        }
                    }

                    break;
                }

                case EAction.craftStoneSpear:
                {
                    if (parent.GetComponent<ManCore>().hand1Slot)
                    {
                        if ((parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.roundRock)
                        || (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe)
                        || (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock))
                        {
                            b = true;
                        }
                    }

                    break;
                }

                case EAction.processTree:
                {
                    if (parent.GetComponent<ManCore>().hand1Slot)
                    {
                        if ((parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                        || (parent.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                        {
                            b = true;
                        }
                    }

                    break;
                }

                case EAction.buildShelter:
                {
                    b = true;

                    break;
                }

                case EAction.craftCordage:
                {
                    b = true;

                    break;
                }

                case EAction.craftBarkTorch:
                {
                    b = true;

                    break;
                }

            }

            // execute a hit

            if (b == true)
            {
                if (other.GetComponent<ProjectCore>().ready == true)
                {
                    other.gameObject.GetComponent<ProjectCore>().progress += 1f;
                    other.gameObject.GetComponent<PhysicalObject>().StartCoroutine("HitColorize");

                    if (other.gameObject.GetComponent<ProjectCore>().progress >= other.gameObject.GetComponent<ProjectCore>().maxProgress)
                    {
                        Destroy(other.gameObject.GetComponent<ProjectCore>().progressBar);
                        Destroy(other.gameObject.GetComponent<ProjectCore>().progressBar.GetComponent<ProgressBar>().frame);
                    
                        switch (other.gameObject.GetComponent<ProjectCore>().action)
                        {
                            case EAction.cutDown:
                            {
                                parent.GetComponent<ManCore>().CutDown(other.gameObject.GetComponent<ProjectCore>().target);
                                break;
                            }

                            case EAction.obtainMeat:
                            {
                                parent.GetComponent<ManCore>().ObtainMeat(other.gameObject.GetComponent<ProjectCore>().target);
                                break;
                            }

                            case EAction.craftHandAxe:
                            {
                                if (other.gameObject.GetComponent<ProjectCore>().target)
                                parent.GetComponent<ManCore>().CraftHandAxe(other.gameObject.GetComponent<ProjectCore>().target);
                                else
                                parent.GetComponent<ManCore>().CraftHandAxe(other.gameObject.GetComponent<ProjectCore>().itemsToConsume[0]);

                                break;
                            }

                            case EAction.craftStoneSpear:
                            {
                                parent.GetComponent<ManCore>().CraftStoneSpear(other.gameObject.GetComponent<ProjectCore>().itemsToConsume);
                                break;
                            }

                            case EAction.processTree:
                            {
                                parent.GetComponent<ManCore>().ProcessTree(other.gameObject.GetComponent<ProjectCore>().target);
                                break;
                            }

                            case EAction.buildShelter:
                            {
                                // build shelter    

                                GameObject clone = GameCore.Core.SpawnStructure(EStructure.shelter);
                                clone.transform.position = other.transform.position;

                                int i1 = other.gameObject.GetComponent<ProjectCore>().itemsToConsume.Count;

                                for (int i=0; i < i1; i++)
                                {
                                    //other.gameObject.GetComponent<ProjectCore>().itemsToConsume.Remove(other.gameObject);
                                    Destroy(other.gameObject.GetComponent<ProjectCore>().itemsToConsume[0]);
                                }

                                //

                                break;
                            }

                            case EAction.craftCordage:
                            {
                                
                                parent.GetComponent<ManCore>().CraftCordage(other.gameObject.GetComponent<ProjectCore>().itemsToConsume[0]);
                                break;
                            }

                            case EAction.craftBarkTorch:
                            {
                                
                                parent.GetComponent<ManCore>().CraftBarkTorch(other.gameObject.GetComponent<ProjectCore>().itemsToConsume);
                                break;
                            }

                        }

                        /*
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.cutDown)
                        parent.GetComponent<ManCore>().CutDown(other.gameObject.GetComponent<ProjectCore>().target);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.obtainMeat)
                        parent.GetComponent<ManCore>().ObtainMeat(other.gameObject.GetComponent<ProjectCore>().target);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.craftHandAxe)
                        {
                            if (other.gameObject.GetComponent<ProjectCore>().target)
                            parent.GetComponent<ManCore>().CraftHandAxe(other.gameObject.GetComponent<ProjectCore>().target);
                            else
                            parent.GetComponent<ManCore>().CraftHandAxe(other.gameObject.GetComponent<ProjectCore>().itemsToConsume[0]);
                        }
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.processHemp)
                        parent.GetComponent<ManCore>().ProcessHemp(other.gameObject.GetComponent<ProjectCore>().target);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.craftStoneSpear)
                        parent.GetComponent<ManCore>().CraftStoneSpear(other.gameObject.GetComponent<ProjectCore>().itemsToConsume);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.processTree)
                        parent.GetComponent<ManCore>().ProcessTree(other.gameObject.GetComponent<ProjectCore>().target);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.collectBark)
                        parent.GetComponent<ManCore>().CollectBark(other.gameObject.GetComponent<ProjectCore>().target);
                        else
                        if (other.gameObject.GetComponent<ProjectCore>().action == EAction.buildShelter)
                        {
                            // build shelter    

                            GameObject clone = GameCore.Core.SpawnStructure(EStructure.shelter);
                            clone.transform.position = other.transform.position;

                            for (int i=0; i < other.gameObject.GetComponent<ProjectCore>().itemsToConsume.Count; i++)
                            {
                                Destroy(other.gameObject.GetComponent<ProjectCore>().itemsToConsume[i]);
                            }

                            //

                        }
                        */

                        parent.GetComponent<CritterCore>().Stop();

                        if (other.gameObject.GetComponent<ProjectCore>().target)
                        other.gameObject.GetComponent<ProjectCore>().target.GetComponent<PhysicalObject>().hasProject = false;

                        Destroy(other.gameObject);
                    }

                    alive = false;
                    Destroy(gameObject);
                }
            }

            //
        }
	}
}
