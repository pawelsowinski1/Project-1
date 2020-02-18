using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// using UnityEngine.EventSystems;
// using UnityEngine.Events;

// Button type I: inventory object button
//
// Buttons at the bottom of the screen, showing objects carried by player.

public class ButtonICore : MonoBehaviour
{
    public GameObject obj; // object represented by a button
    
    public int index;
    
    public void Start()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f); // <--- when changing this, change ButtonO.cs and ButtonA.cs too
    }

    public void TaskOnLMBClick()
    {
        if (GameCore.Core.chooseFromInventoryMode == false)
        {
            // LMB: equip / unequip

            if (GameCore.Core.player.GetComponent<ManCore>().hand1Slot == obj)
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
        
        // if 'choose from inventory mode' is on

        else
        {
            GameCore.Core.chosenObject = obj;

            GameCore.Core.chooseFromInventoryMode = false;
            GameCore.Core.InventoryManager(); // to update all buttons type I

            GameCore.Core.mouseOverGUI = false;

        }
        
    }

    public void TaskOnRMBClick()
    {
        // RMB: drop

        if (GameCore.Core.player.GetComponent<ManCore>().hand1Slot == obj)
        {
            GameCore.Core.player.GetComponent<ManCore>().Unequip(obj);
            GameCore.Core.player.GetComponent<CritterCore>().DropItem(obj);
        }
        else
        GameCore.Core.player.GetComponent<CritterCore>().DropItem(GameCore.Core.player.GetComponent<CritterCore>().carriedBodies[index]);

        GameCore.Core.mouseOverGUI = false;
    }


    public void TaskOnEnter()
    {
        //if (GameCore.Core.worldMap.activeSelf == false)
        GameCore.Core.mouseOverGUI = true;

        string s = "";

        if (GameCore.Core.player.GetComponent<ManCore>().hand1Slot == obj)
        s += obj.name + "\n\nthis item is currently equipped\n\nLMB to unequip\nRMB to drop";
        else
        {
            if (obj.GetComponent<ItemCore>())
            {
                if (obj.GetComponent<ItemCore>().isTool)
                {
                    s += obj.name + "\n\nLMB to equip\nRMB to drop";
                }
                else
                {
                    s += obj.name + "\n\nRMB to drop";
                }
            }
            else
            s += obj.name + "\n\nRMB to drop";
        }

        GameCore.Core.cursorLabel.GetComponent<CursorLabelCore>().text.text = s;
    }

    public void TaskOnExit()
    {
        if (GameCore.Core.worldMap.activeSelf == false)
        GameCore.Core.mouseOverGUI = false;

        GameCore.Core.cursorLabel.GetComponent<CursorLabelCore>().text.text = "";
    }
}
