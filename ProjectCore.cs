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
    public EAction action;                                             // action executed when project is completed
    public List<GameObject> collisionObjects = new List<GameObject>(); // objects in collision
    public List<GameObject> objectsToConsume = new List<GameObject>(); // objects to be consumed in project

    //public GameObject obj; // OBSOLETE! // used to pass data to input man core methods 

    public float colorIntensity = 0f;

    public int conditionsMet;
    public int conditionsAll;

    int i;

    // Methods

    public void ProjectInitialize()
    {
        kind = EKind.project;

        conditionsMet = 1;
        conditionsAll = 1;

        progress = 0f;
        maxProgress = 3f;

        // actions, which require more than 1 object

        if (action == EAction.processHemp)
        {
            conditionsMet = 1;
            conditionsAll = 2;
            ready = false;

        }

        if (action == EAction.craftStoneSpear)
        ready = false;


    }

    //

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

    // =================================================== Main =======================================================

	void Start ()
    {
        ProjectInitialize();

        GetComponent<SpriteRenderer>().sortingOrder = 20;
	}
	
	void Update ()
    {
		Colorize();

        if (target)
        transform.position = target.transform.position;

        // process hemp update

        conditionsMet = 1; // hemp

        if (action == EAction.processHemp)
        {
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
        }

        // craft hand axe update

        else
        if (action == EAction.craftHandAxe)
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
                                objectsToConsume.Add(collisionObjects[i]);
                                ready = true;
                            }
                        }
                    }
                }
            }
        }

        // craft stone spear update

        else 
        if (action == EAction.craftStoneSpear)
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
                                        objectsToConsume.Add(collisionObjects[i]);
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
                                        objectsToConsume.Add(collisionObjects[i]);
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
                                        objectsToConsume.Add(collisionObjects[i]);
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
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<InteractiveObjectCore>())
        if (other.GetComponent<InteractiveObjectCore>().kind != EKind.slash)
        if (other.GetComponent<InteractiveObjectCore>().kind != EKind.projectile)
        collisionObjects.Add(other.gameObject);
    }


    void OnTriggerExit2D(Collider2D other) 
    {
        collisionObjects.Remove(other.gameObject);
    }


}
