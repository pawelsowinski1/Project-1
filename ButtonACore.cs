using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Button type A: action button
//
// Button created by clicking RMB, allows player to make actions.

public class ButtonACore : MonoBehaviour
{
    // input:
    public Vector3 pos;     // position of the click
    public GameObject obj;  // object clicked with RMB

    public EAction action = EAction.none;
    public int index;
    //

    public bool isAnchoredToButtonO = false;
    public GameObject anchorObject;

    // methods

    public void AddProject()
    {
        GameObject clone;
        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, GameCore.Core.transform.rotation) as GameObject;
        clone.GetComponent<ProjectCore>().target = obj;
        clone.GetComponent<ProjectCore>().action = action;

        obj.GetComponent<InteractiveObjectCore>().hasProject = true;
        obj.GetComponent<InteractiveObjectCore>().projectAttached = clone;
    }

    // ========================================== MAIN LOOP ================================================

	void Start ()
    {
        if (isAnchoredToButtonO == false)
        {
		    transform.position = Camera.main.ScreenToWorldPoint(pos) + GameCore.Core.v4*index*100f;
        }
        else
        {
		    transform.position = anchorObject.transform.position + new Vector3(0f,-75f,0f) + GameCore.Core.v4*index*100f;
        }
	}
	
	void Update ()  
    {
        if (isAnchoredToButtonO == false)
        {
            transform.position = Camera.main.ScreenToWorldPoint(pos);// + GameCore.Core.v4*index*100f;
        }
        else
        {
		    transform.position = anchorObject.transform.position + new Vector3(0f,-75f,0f) + GameCore.Core.v4*index*100f;
        }
    }

    public void TaskOnClick()
    {
        GameCore.Core.player.GetComponent<CritterCore>().action = action;
        GameCore.Core.player.GetComponent<CritterCore>().target = obj;

        if (action == EAction.move)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
        else
        if (action == EAction.dropAll)
        GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
        else
        if (action == EAction.cutDown)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.craftHandAxe)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.processHemp)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.obtainMeat)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.processTree)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.collectBark)
        {
            GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
            GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
        }
        else
        if (action == EAction.addFuel)
        {
            GameCore.Core.chooseFromInventoryMode = true;

            int i;

            for (i=0; i < GameCore.Core.buttonsI.Count; i++)
            {
                if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>())
                if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().isFlammable == true)
                GameCore.Core.buttonsI[i].GetComponent<Image>().color = Color.green;
            }
        }

        

        // adding projects

        if (obj)
        if (obj.GetComponent<InteractiveObjectCore>().hasProject == false)
        {
            if ((action != EAction.pickUp)
            && (action != EAction.setOnFire)
            && (action != EAction.addFuel)
            && (action != EAction.convert))
            AddProject();
        }

        //

        Destroy(gameObject);
    }
}