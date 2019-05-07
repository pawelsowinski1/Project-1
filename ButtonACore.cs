using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Button type A: action button
//
// Button created by clicking RMB, allows player to make an action.

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
        clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, Quaternion.identity) as GameObject;
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
            transform.position = Camera.main.ScreenToWorldPoint(pos) + GameCore.Core.v4*index*100f;
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

        switch (action)
        {
            case EAction.move:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
                break;
            }
            
            case EAction.dropAll:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
                break;
            }

            case EAction.cutDown:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.craftHandAxe:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.processHemp:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.obtainMeat:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.processTree:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.collectBark:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                break;
            }

            case EAction.putItemInFireplace:
            {
                GameCore.Core.player.GetComponent<CritterCore>().action = EAction.putItemInFireplace;
                GameCore.Core.chooseFromInventoryMode = true;
                GameCore.Core.chosenObject = null;

                // colorize buttons type I

                for (var i=0; i < GameCore.Core.buttonsI.Count; i++)
                {
                    if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>())
                    {
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().isFlammable == true)
                        GameCore.Core.buttonsI[i].GetComponent<Image>().color = Color.green;
                        else
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().item == EItem.flatRock)
                        GameCore.Core.buttonsI[i].GetComponent<Image>().color = Color.green;
                    }
                }

                break;
            }

            case EAction.heatItem:
            {
                GameCore.Core.player.GetComponent<CritterCore>().action = EAction.heatItem;
                GameCore.Core.chooseFromInventoryMode = true;
                GameCore.Core.chosenObject = null;

                for (var i=0; i < GameCore.Core.buttonsI.Count; i++)
                {
                    if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>())
                    {
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().item == EItem.meat)
                        {
                            GameCore.Core.buttonsI[i].GetComponent<Image>().color = Color.green;
                        }                    
                    }
                }

                break;
            }

            case EAction.deleteProject:
            {
                if (obj.GetComponent<ProjectCore>().target)
                obj.GetComponent<ProjectCore>().target.GetComponent<InteractiveObjectCore>().hasProject = false;   
                
                Destroy(obj);

                break;
            }

            case EAction.giveItem:
            {
                GameCore.Core.player.GetComponent<CritterCore>().action = EAction.giveItem;
                GameCore.Core.player.GetComponent<CritterCore>().target = obj;
                GameCore.Core.chooseFromInventoryMode = true;
                GameCore.Core.chosenObject = null;

                for (var i=0; i < GameCore.Core.buttonsI.Count; i++)
                {
                    if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>())
                    {
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().item == EItem.meat)
                        {
                            GameCore.Core.buttonsI[i].GetComponent<Image>().color = Color.green;
                        }                    
                    }
                }

                
                break;
            }

            case EAction.setGatheringPoint:
            {
                GameCore.Core.gatheringPoint.SetActive(true);
                GameCore.Core.gatheringPointX = Camera.main.ScreenToWorldPoint(Camera.main.ScreenToWorldPoint(pos)).x;
                
                break;
            }

            case EAction.deleteGatheringPoint:
            {
                GameCore.Core.gatheringPoint.SetActive(false);

                break;
            }

        }
        

        // adding projects

        if (obj)
        if (obj.GetComponent<InteractiveObjectCore>().hasProject == false)
        {
            if ((action != EAction.pickUp)
            && (action != EAction.setOnFire)
            && (action != EAction.putItemInFireplace)
            && (action != EAction.heatItem)
            && (action != EAction.convert)
            && (action != EAction.giveItem)
            && (action != EAction.deleteProject))
            AddProject();
        }

        //

        Destroy(gameObject);
    }
}