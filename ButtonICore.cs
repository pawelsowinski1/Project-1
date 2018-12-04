﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 using UnityEngine.EventSystems;
 using UnityEngine.Events;

// Button type I: inventory object button
//
// Buttons at the bottom of the screen, showing objects carried by player.

public class ButtonICore : MonoBehaviour
{
    public GameObject obj; // object represented by a button
    public Sprite sprite;

    public int index;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}



    public void TaskOnLMBClick()
    {
        if (GameCore.Core.chooseFromInventoryMode == false)
        {
            // LMB: equip / unequip

            if (GameCore.Core.player.GetComponent<ManCore>().tool == obj)
            {
                GameCore.Core.player.GetComponent<ManCore>().Unequip(obj);
                GameCore.Core.mouseOverGUI = false;
            }
            else
            {
                if (obj.GetComponent<ItemCore>())
                if (obj.GetComponent<ItemCore>().isTool == true)
                {
                    GameCore.Core.player.GetComponent<ManCore>().Equip(obj);
                    GameCore.Core.mouseOverGUI = false;
                }
            }


            GameCore.Core.player.GetComponent<CritterCore>().Stop();
        }
        
        // choose item mode

        else
        {
            if (obj.GetComponent<ItemCore>())
            if (obj.GetComponent<ItemCore>().isFlammable == true)
            {
                if (GameCore.Core.player.GetComponent<CritterCore>().target)
                if (GameCore.Core.player.GetComponent<CritterCore>().target.GetComponent<StructureCore>())
                if (GameCore.Core.player.GetComponent<CritterCore>().target.GetComponent<StructureCore>().structure == EStructure.campfire)
                {
                    GameCore.Core.player.GetComponent<CritterCore>().action = EAction.addFuel;
                    GameCore.Core.player.GetComponent<PlayerCore>().chosenObject = obj;

                }
            }

            if (GameCore.Core.player.GetComponent<CritterCore>().action != EAction.addFuel)
            {
                GameCore.Core.chooseFromInventoryMode = false;
                GameCore.Core.InventoryManager(); // to clear the colors of buttons type I
            }
        }
        
    }

    public void TaskOnRMBClick()
    {
        // RMB: drop

        if (GameCore.Core.player.GetComponent<ManCore>().tool == obj)
        {
            GameCore.Core.player.GetComponent<ManCore>().Unequip(obj);
            GameCore.Core.player.GetComponent<CritterCore>().DropItem(GameCore.Core.player.GetComponent<CritterCore>().carriedBodies.Count-1);
        }
        else
        GameCore.Core.player.GetComponent<CritterCore>().DropItem(index);


        GameCore.Core.player.GetComponent<CritterCore>().Stop();
        GameCore.Core.mouseOverGUI = false;
    }


    public void TaskOnEnter()
    {
        GameCore.Core.mouseOverGUI = true;
    }

    public void TaskOnExit()
    {
        GameCore.Core.mouseOverGUI = false;
    }
}
