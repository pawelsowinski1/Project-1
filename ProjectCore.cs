using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCore : InteractiveObjectCore
{
    public bool progressByHit;
    public bool ready = true; // all conditions met
    public float progress;
    public float maxProgress;
    public GameObject target = null;                                   // object to which project is attached
    public GameObject secondaryTarget;                                 // needed for certain actions (e.g. heat item)
    public EAction action;                                             // action executed when project is completed
    public List<GameObject> collisionObjects = new List<GameObject>(); // objects in collision
    public List<GameObject> itemsToConsume   = new List<GameObject>(); // items to be consumed in project

    public List<EItem> itemsNeeded = new List<EItem>();

    public float colorIntensity = 0f;

    public int conditionsMet;
    public int conditionsAll;

    int i;

    // Methods

    public void ProjectInitialize()
    {
        kind = EKind.project;

        conditionsMet = 0;
        conditionsAll = 0;

        progress = 0f;
        maxProgress = 3f;

        switch (action)
        {
            case EAction.processHemp:
            {
                conditionsMet = 1;
                conditionsAll = 2;
                ready = false;

                break;
            }

            case EAction.craftStoneSpear:
            {
                ready = false;
                break;
            }

            case EAction.heating:
            {
                maxProgress = 60f;
                break;
            }
            
            case EAction.buildShelter:
            {
                itemsNeeded.Add(EItem.plantMaterial);
                itemsNeeded.Add(EItem.plantMaterial);
                itemsNeeded.Add(EItem.cordage);

                conditionsAll = itemsNeeded.Count;

                break;
            }
        }

    }

    public void Colorize()
    {
		if (colorIntensity != 0f)
		{
			if (colorIntensity > 0f)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f,1f-colorIntensity,1f-colorIntensity,0.5f);
				colorIntensity -= 0.03f;
			}
			else
			{
				GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
				colorIntensity = 0f;
			}
		}
    }

    public void CheckConditions()
    {
		Colorize();

        if (target)
        transform.position = target.transform.position;

        switch (action)
        {
            // process hemp update

            case EAction.processHemp:
            {
                conditionsMet = 1;
                conditionsAll = 2;

                if (collisionObjects.Count > 0)
                {
                    ready = false;

                    for (i=0; i<collisionObjects.Count; i++)
                    {
                        if (collisionObjects[i])
                        if (collisionObjects[i].GetComponent<ItemCore>())
                        {
                            if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.flatRock)
                            {
                                if ((collisionObjects[i].gameObject.GetComponent<ItemCore>().isCarried == false)
                                && (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false))
                                {
                                    ready = true;
                                    //obj = collisionObjects[i];
                                    conditionsMet++;
                                }
                            }
                        }
                    }

                    if (ready == false)
                    progress = 0f;
                }

                break;
            }

            // craft hand axe update

            case EAction.craftHandAxe:
            {
                if (target == null)
                {
                    conditionsMet = 0;
                    ready = false;

                    if (collisionObjects.Count > 0)
                    {
                        for (i=0; i<collisionObjects.Count; i++)
                        {
                            if (collisionObjects[i].GetComponent<ItemCore>())
                            {
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.flint)
                                {
                                    conditionsMet++;
                                    itemsToConsume.Add(collisionObjects[i]);
                                    ready = true;
                                }
                            }
                        }
                    }
                }

                break;
            }

            // craft stone spear update

            case EAction.craftStoneSpear:
            {
            conditionsMet = 0;
            conditionsAll = 3;

                if (collisionObjects.Count > 0)
                {
                    ready = false;

                    bool flint = false;
                    bool cordage = false;
                    bool smallLog = false;

                    for (i=0; i<collisionObjects.Count; i++)
                    {
                        if (collisionObjects[i])
                        {
                            if (collisionObjects[i].GetComponent<ItemCore>())
                            {
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.flint)
                                {
                                    if (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false)
                                    {
                                        if (flint == false)
                                        {
                                            flint = true;
                                            conditionsMet++;
                                            itemsToConsume.Add(collisionObjects[i]);
                                        }
                                    }
                                }
                                else
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.smallLog)
                                {
                                    if (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false)
                                    {
                                        if (smallLog == false)
                                        {
                                            smallLog = true;
                                            conditionsMet++;
                                            itemsToConsume.Add(collisionObjects[i]);
                                        }
                                    }
                                }
                                else
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.cordage)
                                {
                                    if (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false)
                                    {
                                        if (cordage == false)
                                        {
                                            cordage = true;
                                            conditionsMet++;
                                            itemsToConsume.Add(collisionObjects[i]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (conditionsMet == conditionsAll)
                    {
                        ready = true;
                    }

                    if (ready == false)
                    {
                        progress = 0f;
                    }
                }

                break;
            }

            // build shelter update

            case EAction.buildShelter:
            {
                conditionsMet = 0;
                ready = false;

                if (collisionObjects.Count > 0)
                {
                    for (i=0; i<collisionObjects.Count; i++)
                    {
                        if (collisionObjects[i].GetComponent<ItemCore>())
                        {
                            if (collisionObjects[i].GetComponent<BodyCore>().isCarried == false)
                            {

                                foreach (EItem _item in itemsNeeded)
                                {
                                    if (collisionObjects[i].GetComponent<ItemCore>().item == _item)
                                    {
                                        itemsToConsume.Add(collisionObjects[i]);
                                        itemsNeeded.Remove(_item);
                                        conditionsMet++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                   
                    if (itemsToConsume.Count > 0)
                    foreach (GameObject _object in itemsToConsume)
                    {
                        itemsNeeded.Add(_object.GetComponent<ItemCore>().item);
                    }

                    if (conditionsMet >= conditionsAll)
                    {
                        ready = true;
                        conditionsMet = conditionsAll;
                    }

                    if (ready == false)
                    {
                        progress = 0f;
                        itemsToConsume.Clear();
                    }


                    //itemsToConsume.Clear();
                    
                }

                break;
            }

            // heating update
            
            case EAction.heating:
            {
                conditionsMet = 0;
                conditionsAll = 1;

                if (collisionObjects.Count > 0)
                {
                    ready = false;

                    bool heatedItem = false;

                    for (i=0; i<collisionObjects.Count; i++)
                    {
                        if (collisionObjects[i])
                        {
                            if (collisionObjects[i].GetComponent<ItemCore>())
                            {
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.meat)
                                {
                                    if (heatedItem == false)
                                    {
                                        heatedItem = true;
                                        conditionsMet++;
                                    }
                                }
                            }
                        }
                    }

                }

                if (conditionsMet == conditionsAll)
                {
                    progress += Time.deltaTime; // progress is equal to the amount of real life seconds passed

                    if (progress > maxProgress)
                    {
                        // turn raw meat to cooked meat

                        secondaryTarget.GetComponent<ItemCore>().item = EItem.cookedMeat;
                        secondaryTarget.GetComponent<ItemCore>().ItemInitialize();

                        target.GetComponent<InteractiveObjectCore>().hasProject = false;
                        Destroy(gameObject);
                    }
                }

                // if meat was picked up during heating, reset the progress

                else
                {
                    target.GetComponent<FireplaceCore>().itemHeated = null;
                    progress = 0f;


                    Destroy(gameObject); 
                }

                break;
            }
            

        }
	}

    // =================================================== Main =======================================================

	void Start ()
    {
        ProjectInitialize();

        //GetComponent<SpriteRenderer>().sortingOrder = 20;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<InteractiveObjectCore>())
        if (other.GetComponent<InteractiveObjectCore>().kind != EKind.slash)
        if (other.GetComponent<InteractiveObjectCore>().kind != EKind.projectile)
        collisionObjects.Add(other.gameObject);

        CheckConditions();
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        collisionObjects.Remove(other.gameObject);
        CheckConditions();
    }


}
