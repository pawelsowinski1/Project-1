using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Button type A: action button
//
// Button created by clicking RMB, allows player to make an action.

public class ButtonACore : MonoBehaviour
{
    public Vector3 pos;     // position of the click
    public GameObject obj;  // object clicked with RMB
    public GameObject text; // text object reference

    public EAction action = EAction.none;
    public int index;

    public bool isAnchoredToButtonO = false;
    public GameObject anchorObject;

    public bool isMouseOver = false;
    public bool darkened = false; // if requirements for an action are not met, then the button is darkened

    
    // ========================================== MAIN ================================================

    void Awake()
    {
        darkened = false;
    }

	void Start ()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);  // <--- when changing this, change ButtonO.cs and ButtonI.cs too


        if (darkened)
        {
            ColorBlock colors = GetComponent<Button> ().colors;
            colors.highlightedColor = Color.black;
            colors.pressedColor = Color.red;
            GetComponent<Button> ().colors = colors;
        }

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

        if (isMouseOver)
        {
            obj.GetComponent<PhysicalObject>().Highlight();

        }
    }

    // =============================================== METHODS ======================================================


    public void AddProject()
    {
        if (obj.GetComponent<PhysicalObject>().hasProject == false)
        {
            GameObject clone;
            clone = Instantiate(GameCore.Core.projectPrefab, obj.transform.position, Quaternion.identity) as GameObject;
            GameCore.Core.projects.Add(clone);

            clone.GetComponent<ProjectCore>().target = obj;
            clone.GetComponent<ProjectCore>().action = action;

            obj.GetComponent<PhysicalObject>().hasProject = true;
            obj.GetComponent<PhysicalObject>().projectAttached = clone;
        }
    }

    public void TaskOnClick()
    {
        GameCore.Core.player.GetComponent<CritterCore>().action = action; // <---- this isn't the best solution
        GameCore.Core.player.GetComponent<CritterCore>().target = obj;    // <---- 

        // add projects

        if (obj)
        if (obj.GetComponent<PhysicalObject>().hasProject == false) 
        {
            if ((action != EAction.pickUp)                                // <---- this isn't the best solution
            && (action != EAction.setOnFire)
            && (action != EAction.putItemInFireplace)
            && (action != EAction.heatItem)
            && (action != EAction.convert)
            && (action != EAction.giveItem)
            && (action != EAction.deleteProject)
            && (action != EAction.cutDown)
            && (action != EAction.equip)
            && (action != EAction.unequip)
            && (action != EAction.drop)
            && (action != EAction.processTree)
            && (action != EAction.obtainMeat)
            && (action != EAction.continueProject)
            && (action != EAction.craftHandAxe))
            AddProject();
        }

        switch (action)
        {
            case EAction.move:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = pos.x;
                break;
            }

            case EAction.pickUp:
            {
                GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                break;
            }

            case EAction.cutDown:
            {
                if (darkened)
                GameCore.Core.player.GetComponent<PhysicalObject>().MessageText("I need a sharp tool to cut this down.");
                else
                {
                    GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                    GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                    AddProject();
                }

                break;
            }

            case EAction.craftHandAxe:
            {
                if (darkened)
                GameCore.Core.player.GetComponent<PhysicalObject>().MessageText("I need a hammerstone to make flint tools.");
                else
                {
                    GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                    GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                    AddProject();
                }
                break;
            }

            case EAction.obtainMeat:
            {
                if (darkened)
                GameCore.Core.player.GetComponent<PhysicalObject>().MessageText("I need a sharp tool to cut the meat.");
                else
                {
                    GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                    GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                    AddProject();
                }
                break;
            }

            case EAction.processTree:
            {
                if (darkened)
                GameCore.Core.player.GetComponent<PhysicalObject>().MessageText("I need a sharp tool to process this tree.");
                else
                {
                    GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.transform.position.x;
                    GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                    AddProject();
                }

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
                    bool b = false;

                    if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<Burnable>())
                    {
                        b = true;
                    }

                    if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>())
                    {
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().item == EItem.flatRock)
                        {
                            b = true;
                        }
                    }

                    if (b == true)
                    {
                        // colorize 

                        ColorBlock colors =  GameCore.Core.buttonsI[i].GetComponent<Button>().colors;
                        colors.normalColor = Color.green;
                        colors.highlightedColor = Color.white;
                        GameCore.Core.buttonsI[i].GetComponent<Button>().colors = colors;

                        //
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
                            // colorize 

                            ColorBlock colors =  GameCore.Core.buttonsI[i].GetComponent<Button>().colors;
                            colors.normalColor = Color.green;
                            colors.highlightedColor = Color.white;
                            GameCore.Core.buttonsI[i].GetComponent<Button>().colors = colors;

                            //

                        }                    
                    }
                }

                break;
            }

            case EAction.deleteProject:
            {
                if (obj.GetComponent<ProjectCore>().target)
                obj.GetComponent<ProjectCore>().target.GetComponent<PhysicalObject>().hasProject = false;   
                
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
                        if (GameCore.Core.buttonsI[i].GetComponent<ButtonICore>().obj.GetComponent<ItemCore>().item == EItem.cookedMeat)
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
            
            case EAction.craftCordage:
            {
                //obj.GetComponent<PhysicalObject>().projectAttached.GetComponent<ProjectCore>().itemsToConsume.Add(obj);

                break;
            }

            case EAction.convert:
            {
                GameCore.Core.player.GetComponent<CritterCore>().action = EAction.convert;
                GameCore.Core.player.GetComponent<CritterCore>().target = obj;

                break;
            }

            case EAction.continueProject:
            {
                GameCore.Core.player.GetComponent<CritterCore>().action = obj.GetComponent<ProjectCore>().action;

                if (obj.GetComponent<ProjectCore>().target)
                {
                    GameCore.Core.player.GetComponent<CritterCore>().target = obj.GetComponent<ProjectCore>().target;

                    GameCore.Core.player.GetComponent<CritterCore>().preciseMovement = true;
                    GameCore.Core.player.GetComponent<CritterCore>().targetX = obj.GetComponent<ProjectCore>().target.transform.position.x;
                }

                break;
            }

            case EAction.equip:
            {
                GameCore.Core.player.GetComponent<ManCore>().Equip(obj);

                break;
            }

            case EAction.unequip:
            {
                GameCore.Core.player.GetComponent<ManCore>().Unequip(obj);

                break;
            }
            case EAction.drop:
            {
                GameCore.Core.player.GetComponent<ManCore>().DropItem(obj);

                break;
            }            
        }
        
        if (darkened == false)
        {
            GameCore.Core.ClearButtons();
            GameCore.Core.mouseOverGUI = false;
        }
        
    }

    // ---------------------- events ---------------------

    public void TaskOnEnter()
    {
        isMouseOver = true;
        GameCore.Core.mouseOverGUI = true;
    }


    public void TaskOnExit()
    {
        isMouseOver = false;
        obj.GetComponent<PhysicalObject>().highlightAmount = 0f;

        float f = obj.GetComponent<SpriteRenderer>().color.a;

        obj.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,f);

        GameCore.Core.mouseOverGUI = false;


    }
    




}