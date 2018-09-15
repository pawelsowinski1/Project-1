using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button type I: inventory item button
//
// Buttons at the bottom of the screen, showing items carried by player.

public class ButtonICore : MonoBehaviour
{
    public GameObject obj;
    public Sprite sprite;

    public int index;
    public int type; // type 0: inventory slot; type 1: tool slot

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void TaskOnClick()
    {
        if (type == 0) // item in inventory
        {
            GameCore.Core.player.GetComponent<ManCore>().Equip(GameCore.Core.player.GetComponent<ManCore>().carriedBodies[index]);
        }
        else
        if (type == 1) // tool in tool slot
        {
            GameCore.Core.player.GetComponent<ManCore>().Unequip(GameCore.Core.player.GetComponent<ManCore>().tool);
        }
    }
}
