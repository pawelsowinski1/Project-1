using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button type O: object selection button
//
// These are displayed, when there are multiple objects und mouse and RMB is pressed.

public class ButtonOCore : MonoBehaviour
{
    public int        index;     // index of the button (for multiple objects)
    public Vector3    worldPos;  // anchor position in the world for button with index = 0
    public GameObject obj;       // object represented by this button
    public GameObject image;     // image displayed

    public bool isMouseOver = false;

    GameCore core = GameCore.Core;

    int i;

    // ---------------------------------------

    public void TaskOnClick()
    {/*
        if (type == 0) // item in inventory
        {
            GameCore.Core.player.GetComponent<ManCore>().Equip(GameCore.Core.player.GetComponent<ManCore>().carriedBodies[index]);
        }
        else
        if (type == 1) // tool in tool slot
        {
            GameCore.Core.player.GetComponent<ManCore>().Unequip(GameCore.Core.player.GetComponent<ManCore>().tool);
        }*/
    }

    public void TaskOnEnter()
    {
        // clear buttons (type A)  
        
        for (i=0; i < core.buttonsA.Count; i++)
        {
            Destroy(core.buttonsA[i]);
        }

        core.buttonsA.Clear();

        //

        isMouseOver = true;

        core.RMBclickedObj = obj;
        core.CreateButtonAList();

        for (i=0; i < core.buttonsA.Count; i++)
        {
            core.buttonsA[i].GetComponent<ButtonACore>().isAnchoredToButtonO = true;
            core.buttonsA[i].GetComponent<ButtonACore>().anchorObject = gameObject;
        }

        for (i=0; i < core.buttonsO.Count; i++)
        {
            if (core.buttonsO[i].GetComponent<ButtonOCore>().obj == core.RMBclickedObj)
            {
            }
        }

    }

    public void TaskOnExit()
    {
        isMouseOver = false;
        obj.GetComponent<PhysicalObject>().highlightAmount = 0f;
        obj.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);

    }

    // --------------- main ----------------

	void Start ()
    {
		image.transform.localScale = new Vector3(2f,2f,2f);
	}
	
	void Update ()
    {
        if (index == 0)
        transform.position = Camera.main.WorldToScreenPoint(worldPos);
        else
        transform.position = core.buttonsO[0].transform.position + new Vector3(105f,0f,0f)*index;// + GameCore.Core.v4*index*100f;

        if (isMouseOver)
        {
            obj.GetComponent<PhysicalObject>().Highlight();
        }

	}



}
