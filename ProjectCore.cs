using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCore : PhysicalObject
{
    public bool ready = true; // all conditions met

    public float progress;
    public float maxProgress;
    public GameObject progressBar;

    public GameObject target = null;                                   // object to which project is attached
    public GameObject secondaryTarget;                                 // needed for certain actions (e.g. heat item)
    public EAction action;                                             // action executed when project is completed
    public List<GameObject> collisionObjects = new List<GameObject>(); // objects in collision
    public List<GameObject> itemsToConsume   = new List<GameObject>(); // items to be consumed in project

    public List<EItem> itemsNeeded = new List<EItem>();

    public int conditionsMet;
    public int conditionsAll;

    public string label = "";

    public bool isCoroutineRunning = false;

    // =================================================== Main =======================================================

	void Start()
    {
        ProjectInitialize();
	}

    void Update()
    {
        SetLabel();

        if (target)
        transform.position = target.transform.position;

        CheckConditions(); // <--- !!!
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((!other.GetComponent<SlashCore>())
        && (!other.GetComponent<ProjectileCore>())
        && (!collisionObjects.Contains(other.gameObject)))
        {
            collisionObjects.Add(other.gameObject);
            CheckConditions();
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        collisionObjects.Remove(other.gameObject);
        CheckConditions();
    }

    void OnDestroy()
    {
        Destroy(progressBar);
        GameCore.Core.projects.Remove(gameObject);
    }

    // ==============================================================================================

    public void ProjectInitialize()
    {
        GetComponent<Collider2D>().enabled = true;

        GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.2f); // transparency

        conditionsMet = 0;
        conditionsAll = 0;

        progress = 0f;
        maxProgress = 5f;

        progressBar = Instantiate(GameCore.Core.progressBarPrefab, transform.position, Quaternion.identity);
        progressBar.GetComponent<ProgressBar>().parent = gameObject;


        switch (action)
        {
            case EAction.craftStoneSpear:
            {
                ready = false;
                break;
            }

            case EAction.craftCordage:
            {
                maxProgress = 20f;

                break;
            }

            case EAction.heating:
            {
                maxProgress = 20f;

                GetComponent<SpriteRenderer>().enabled = false;

                break;
            }
            
            case EAction.buildShelter:
            {
                itemsNeeded.Add(EItem.plantMaterial);
                itemsNeeded.Add(EItem.cordage);
                itemsNeeded.Add(EItem.smallLog);

                conditionsAll = itemsNeeded.Count;

                break;
            }

            case (EAction.cutDown):
            {
                switch (target.GetComponent<PlantCore>().plant)
                {
                    case EPlant.spruce:
                    {
                        maxProgress = target.GetComponent<PlantCore>().size*0.3f;
                        break;
                    }

                    case EPlant.birch:
                    {
                        maxProgress = target.GetComponent<PlantCore>().size*0.2f;
                        break;
                    }
                }
                break;
            }

            case (EAction.processTree):
            {
                switch (target.GetComponent<PlantCore>().plant)
                {
                    case EPlant.spruce:
                    {
                        maxProgress = target.GetComponent<PlantCore>().size*0.3f*2f;
                        break;
                    }

                    case EPlant.birch:
                    {
                        maxProgress = target.GetComponent<PlantCore>().size*0.2f*2f;
                        break;
                    }
                }
                break;
            }
        }
    }

    // ================================================================================================

    public void CheckConditions()
    {
        //collisionObjects.Clear();

        switch (action)
        {
            // craft cordage (no-hit crafting project)

            case EAction.craftCordage:
            {
                //if (target == null) <-- no-hit crafting projects bevahe the same for both 'with a target' and 'without a target' projects
                {
                    conditionsMet = 0;
                    conditionsAll = 2; // no-hit crafting projects need to count crafter as 1 more condition
                    ready = false;

                    if (collisionObjects.Count > 0)
                    {
                        for (int i=0; i<collisionObjects.Count; i++)
                        {
                            if (collisionObjects[i].GetComponent<PlantCore>())
                            {
                                if (collisionObjects[i].GetComponent<PlantCore>().plant == EPlant.grass)
                                {
                                    if (collisionObjects[i].GetComponent<PlantCore>().isRooted == false)
                                    {
                                        if (!itemsToConsume.Contains(collisionObjects[i]))
                                        itemsToConsume.Add(collisionObjects[i]);

                                        conditionsMet++;

                                        // no-hit crafting projects "stick" to the detected resource item, if they need a single resource item

                                        if (target == null)
                                        {
                                            target = collisionObjects[i];
                                            collisionObjects[i].GetComponent<PhysicalObject>().hasProject = true;
                                        }

                                    }
                                }
                            }
                            else
                            if (collisionObjects[i].GetComponent<ManCore>())
                            {
                                if (collisionObjects[i].GetComponent<ManCore>().action == EAction.craftCordage)
                                {
                                    conditionsMet++;
                                }
                            }
                        }
                    }

                    if (conditionsMet == conditionsAll)
                    {
                        ready = true;

                        if (isCoroutineRunning == false)
                        {
                            StartCoroutine("Crafting");
                            isCoroutineRunning = true;
                        }
                    }

                    // if grass was picked up during crafting, destroy project
                    // but when only crafter leaves, keep the progress
                    
                    else
                    {
                        if (progress > 0)
                        {
                            if (!collisionObjects.Contains(itemsToConsume[0]))
                            {
                                progress = 0f;
                            }
                        }

                        StopCoroutine("Crafting");
                        isCoroutineRunning = false;

                    }
                    
                }

                break;
            }

            // craft hand axe

            case EAction.craftHandAxe:
            {
                if (target == null)
                {
                    conditionsMet = 0;
                    conditionsAll = 1;
                    ready = false;

                    if (collisionObjects.Count > 0)
                    {
                        for (int i=0; i<collisionObjects.Count; i++)
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

            // craft stone spear

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

                    for (int i=0; i<collisionObjects.Count; i++)
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

            // craft bark torch

            case EAction.craftBarkTorch:
            {
                conditionsMet = 0;
                conditionsAll = 3;

                if (collisionObjects.Count > 0)
                {
                    ready = false;

                    bool bark = false;
                    bool cordage = false;
                    bool stick = false;

                    for (int i=0; i<collisionObjects.Count; i++)
                    {
                        if (collisionObjects[i])
                        {
                            if (collisionObjects[i].GetComponent<ItemCore>())
                            {
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.bark)
                                {
                                    if (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false)
                                    {
                                        if (bark == false)
                                        {
                                            bark = true;
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
                                else
                                if (collisionObjects[i].gameObject.GetComponent<ItemCore>().item == EItem.stick)
                                {
                                    if (collisionObjects[i].gameObject.GetComponent<BodyCore>().isCarried == false)
                                    {
                                        if (stick == false)
                                        {
                                            stick = true;
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

            // build shelter

            case EAction.buildShelter:
            {
                conditionsMet = 0;
                conditionsAll = 3;
                ready = false;
                itemsToConsume.Clear();

                if (collisionObjects.Count > 0)
                {
                    for (int i=0; i<collisionObjects.Count; i++)
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


                    
                }

                break;
            }

            // heating
            
            case EAction.heating:
            {
                conditionsMet = 0;
                conditionsAll = 1;

                if (collisionObjects.Count > 0)
                {
                    ready = false;

                    bool heatedItem = false;

                    for (int i=0; i<collisionObjects.Count; i++)
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

                                        break;
                                    }
                                }
                            }
                        }
                    }

                }

                if (conditionsMet == conditionsAll)
                {
                    if (isCoroutineRunning == false)
                    {
                        StartCoroutine("Heating");
                        isCoroutineRunning = true;
                    }
                }

                // if meat was picked up during heating, destroy project

                else
                {
                    target.GetComponent<FireplaceCore>().itemHeated = null;
                    progress = 0f;

                    StopCoroutine("Heating");
                    isCoroutineRunning = false;

                    Destroy(gameObject); 

                }

                break;
            }
        }
	}

    public void SetLabel()
    {
        switch (action)
        {
            case EAction.cutDown:
            {
                label = "Cut down\n\ntools needed: hand axe\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.craftCordage:
            {
                label = "Craft cordage\n\nitems needed: 1x grass ("+conditionsMet+"/"+conditionsAll+")\ntools needed: -\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.processTree:
            {
                label = "Process tree\n\ntools needed: hand axe\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.craftHandAxe:
            {
                label = "Craft hand axe\n\nitems needed: 1x flint ("+conditionsMet+"/"+conditionsAll+")\ntools needed: round rock\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.craftStoneSpear:
            {
                label = "Craft stone spear\n\nitems needed: 1x flint, 1x small log, 1x cordage ("+conditionsMet+"/"+conditionsAll+")\ntools needed: round rock, hand axe\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.buildShelter:
            {
                label = "Build shelter\n\nitems needed: 2x plant material, 1x cordage ("+conditionsMet+"/"+conditionsAll+")\ntools needed: -\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.heating:
            {
                label = "Heating\n\nprogress: "+progress+" / "+maxProgress;

                break;
            }

            case EAction.craftBarkTorch:
            {
                label = "Craft bark torch\n\nitems needed: 1x birch bark, 1x stick, 1x cordage ("+conditionsMet+"/"+conditionsAll+")\ntools needed: -\nprogress: "+progress+" / "+maxProgress;

                break;
            }

        }
    }

    public IEnumerator Heating()
    {
        for (;;)  
        {
            progress += 1;

            if (progress > maxProgress)
            {
                // turn raw meat to cooked meat

                secondaryTarget.GetComponent<ItemCore>().item = EItem.cookedMeat;
                secondaryTarget.GetComponent<ItemCore>().ItemInitialize();

                target.GetComponent<PhysicalObject>().hasProject = false;
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator Crafting()
    {
        for (;;)  
        {
            progress += 1;

            if (progress > maxProgress)
            {
                switch (action)
                {
                    case EAction.craftCordage:
                    {
                        if (itemsToConsume[0])
                        {
                            // turn grass to cordage

                            GameObject clone;
                            clone = GameCore.Core.SpawnItem(EItem.cordage);
                            clone.transform.position = itemsToConsume[0].transform.position;

                            Destroy(itemsToConsume[0]);

                            //

                            Destroy(gameObject);
                        }
                    
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
