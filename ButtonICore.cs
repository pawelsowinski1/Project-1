using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using UnityEngine.EventSystems;
// using UnityEngine.Events;

// Button type I: inventory object button
//
// Buttons at the bottom of the screen, showing objects carried by player.

public class ButtonICore : MonoBehaviour
{
    public GameObject obj; // object represented by a button
    //public Sprite sprite;

    public int index;

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
        
        // choose from inventory mode

        else
        {
            GameCore.Core.chosenObject = obj;

            GameCore.Core.chooseFromInventoryMode = false;
            GameCore.Core.InventoryManager(); // to clear the colors of buttons type I
        }
        
    }

    public void TaskOnRMBClick()
    {
        // RMB: drop

        if (GameCore.Core.player.GetComponent<ManCore>().tool == obj)
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
        if (GameCore.Core.worldMap.activeSelf == false)
        GameCore.Core.mouseOverGUI = true;
    }

    public void TaskOnExit()
    {
        if (GameCore.Core.worldMap.activeSelf == false)
        GameCore.Core.mouseOverGUI = false;
    }
}
