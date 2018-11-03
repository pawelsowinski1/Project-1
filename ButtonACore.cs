using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button type A: action button
//
// Button created by clicking RMB, allows player to make actions.

public class ButtonACore : MonoBehaviour
{
    // input:
    public Vector3 pos;     // position of the click
    public GameObject obj;  // object clicked with RMB
    public EAction action = EAction.none; // action
    //

	void Start ()
    {
		transform.position = Camera.main.WorldToScreenPoint(pos);
	}
	
	void Update ()
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
	}

    public void TaskOnClick()
    {
        GameCore.Core.player.GetComponent<CritterCore>().action = action;
        GameCore.Core.player.GetComponent<CritterCore>().target = obj;

        if (action == EAction.move)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
        
        if (action == EAction.dropAll)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;

        
        // adding projects

        if (obj)
        if (obj.GetComponent<InteractiveObjectCore>().hasProject == false)
        {
            if (action == EAction.cutDown)
            {
                if (GameCore.Core.player.GetComponent<ManCore>().tool)
                {
                    if ((GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.sharpRock)
                    ||  (GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        GameObject clone;
                        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, GameCore.Core.transform.rotation) as GameObject;
                        clone.GetComponent<ProjectCore>().action = EAction.cutDown;
                        clone.GetComponent<ProjectCore>().target = obj;

                        obj.GetComponent<InteractiveObjectCore>().hasProject = true;
                    }
                }
            }
            else
            if (action == EAction.obtainMeat)
            {
                if (GameCore.Core.player.GetComponent<ManCore>().tool)
                {
                    if ((GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.sharpRock)
                    ||  (GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.handAxe))
                    {
                        GameObject clone;
                        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, GameCore.Core.transform.rotation) as GameObject;
                        clone.GetComponent<ProjectCore>().action = EAction.obtainMeat;
                        clone.GetComponent<ProjectCore>().target = obj;

                        obj.GetComponent<InteractiveObjectCore>().hasProject = true;
                    }
                }
            }
            else
            if (action == EAction.craftHandAxe)
            {
                if (GameCore.Core.player.GetComponent<ManCore>().tool)
                {
                    if (GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.roundRock)
                    {
                        GameObject clone;
                        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, GameCore.Core.transform.rotation) as GameObject;
                        clone.GetComponent<ProjectCore>().action = EAction.craftHandAxe;
                        clone.GetComponent<ProjectCore>().target = obj;

                        obj.GetComponent<InteractiveObjectCore>().hasProject = true;
                    }
                }
            }
            else
            if (action == EAction.processHemp)
            {
                if (GameCore.Core.player.GetComponent<ManCore>().tool)
                {
                    if (GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().item == EItem.roundRock)
                    {
                        GameObject clone;
                        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, GameCore.Core.transform.rotation) as GameObject;
                        clone.GetComponent<ProjectCore>().action = EAction.processHemp;
                        clone.GetComponent<ProjectCore>().target = obj;

                        obj.GetComponent<InteractiveObjectCore>().hasProject = true;
                    }
                }
            }


        }

        //

        Destroy(gameObject);
    }
}