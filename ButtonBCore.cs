using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Button in the build panel.

public class ButtonBCore : MonoBehaviour
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
                    break;
                }

                case EItem.stoneSpear:
                {
                    clone.GetComponent<ProjectCore>().action = EAction.craftStoneSpear;
                    break;
                }

                case EItem.cordage:
                {
                    clone.GetComponent<ProjectCore>().action = EAction.craftCordage;
                    break;
                }

                case EItem.barkTorch:
                {
                    clone.GetComponent<ProjectCore>().action = EAction.craftBarkTorch;
                    break;
                }                
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
            s = "Shelter\n\nTools needed: -\nResources needed: 2x plant material, cordage";
        }
        else
        {
            switch (item)
            {
                case EItem.cordage:
                s = "Cordage\n\nTools needed: -\nResources needed: grass";
                break;

                case EItem.handAxe:
                s = "Hand axe\n\nTools needed: round rock\nResources needed: flint";
                break;

                case EItem.stoneSpear:
                s = "Stone spear\n\nTools needed: round rock, hand axe\nResources needed: flint, small log, cordage";
                break;

                case EItem.barkTorch:
                s = "Bark torch\n\nTools needed: -\nResources needed: birch bark, stick, cordage";
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
