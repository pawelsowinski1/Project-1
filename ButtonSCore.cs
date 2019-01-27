using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button in the build panel. Should be named ButtonB.

public class ButtonSCore : MonoBehaviour
{
    public bool isStructure; // if false, then it is an item
    public EStructure structure;
    public EItem item;

    // ----------------------------- main -----------------------------------

    public void Start()
    {
        //
    }

    public void TaskOnLMBClick()
    {
        // if structure

        if (isStructure == true)
        {
            GameCore.Core.buildPanel.GetComponent<BuildPanelCore>().ghostObject.SetActive(true);
        }

        // if item

        else
        {
            GameObject clone;
            clone = Instantiate(GameCore.Core.projectPrefab, GameCore.Core.player.transform.position, Quaternion.identity) as GameObject;

            switch (item)
            {
                case EItem.handAxe:
                {
                    clone.GetComponent<ProjectCore>().action = EAction.craftHandAxe;
                }
                

                break;

                case EItem.stoneSpear:
                {
                    clone.GetComponent<ProjectCore>().action = EAction.craftStoneSpear;
                }

                break;
            }

            //clone.GetComponent<ProjectCore>().target = obj;
            //clone.GetComponent<ProjectCore>().action = action;
        }
    }

    // ----------------------------------------------------------------------

    public void PointerEnter()
    {
        string s = "";

        if (isStructure)
        {
            if (structure == EStructure.shelter)
            s = "Shelter\n\nTools needed: -\nResources needed: plant material, cordage";
        }
        else
        {
            switch (item)
            {
                case EItem.handAxe:
                s = "Hand axe\n\nTools needed: round rock\nResources needed: flint";
                break;

                case EItem.stoneSpear:
                s = "Stone spear\n\nTools needed: round rock, hand axe\nResources needed: flint, small log, cordage";
                break;
            }
        }

        GameCore.Core.cursorLabel.GetComponent<CursorLabelCore>().text.text = s;
    }

    public void PointerExit()
    {
        GameCore.Core.cursorLabel.GetComponent<CursorLabelCore>().text.text = "";
    }


}
