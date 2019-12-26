using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CursorLabelCore : MonoBehaviour 
{
    Vector3 v1 = new Vector3(180f,-160f,1f); // offset

    public bool isHidden;

    public Text text;


    public void WorldMap()
    {
        transform.position = Input.mousePosition + v1;
    }

    public void BuildPanel()
    {
        transform.position = Input.mousePosition + v1;
    }

    public void Game()
    {
        transform.position = Input.mousePosition + v1;

        if (GameCore.Core.rhit2D.Length == 0)
        {
            text.text = "";
        }
        else
        {
            for (int i = 0; i<GameCore.Core.rhit2D.Length; i++)
            {
                if (GameCore.Core.rhit2D[i].transform.gameObject.GetComponent<ProjectCore>())
                {
                    text.text = GameCore.Core.rhit2D[i].transform.gameObject.GetComponent<ProjectCore>().label;
                    
                    break;
                }
                else
                if (GameCore.Core.rhit2D[i].transform.gameObject.GetComponent<FireplaceCore>())
                {
                    text.text = "Campfire\n\nfuel: "+GameCore.Core.rhit2D[i].transform.gameObject.GetComponent<FireplaceCore>().fuel;
                }

            }
        }

        /*
        if (GameCore.Core.rhit2D.Length == 1)
        {
            text.text = GameCore.Core.rhit2D[0].transform.gameObject.name;
        }
        else
        text.text = "objects: "+GameCore.Core.rhit2D.Length.ToString();
        */
    }

    // -------------------------- main ------------------------------

	void Start()
    {
		isHidden = false;

        
	}
	
	void Update()
    {        

        transform.SetSiblingIndex(100); // <----- should it be in update?

        if (GameCore.Core.worldMap.activeSelf == true)
        {
            WorldMap();
        }
        else
        if (GameCore.Core.buildPanel.activeSelf == true)
        {
            BuildPanel();
        }
        else
        {
            Game();
        }

        if (isHidden == true)
        {

        }
        
        // if HUD is on, show more info
        
        if (GameCore.Core.hideHUD == false)
        {
            if (GameCore.Core.rhit2D.Length == 0)
            {
                // if nothing

                text.text = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString();

            }
            else if (GameCore.Core.rhit2D.Length == 1)
            {
                if (GameCore.Core.rhit2D[0])
                {
                    // if any interactive object        

                    text.text = GameCore.Core.rhit2D[0].transform.gameObject.name;
                
                    // if plant

                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<PlantCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nsize = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<PlantCore>().size+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                        
                    }

                    // if structure

                    else
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<StructureCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    }

                    // if fireplace

                    else
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder+
                        "\nfuel = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().fuel+
                        "\nitem in fire = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().itemInFire+
                        "\nitem heated = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().itemHeated;
                    }

                    // if critter

                    else
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder+
                        "\nteam = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().team.ToString()+
                        "\naction = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().action.ToString()+
                        "\ncommand = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().command.ToString()+
                        "\ntargetX = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().targetX.ToString()+
                        "\ntarget = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().target+
                        "\ntimerAI = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerAI+
                        "\ntimerMove = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerMove+
                        "\ntimerHit = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerHit+
                        "\nisCarrying = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().isCarrying+
                        "\nattitude = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().attitude;

                    }
                
                    // if project

                    else
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder+
                        "\ntarget = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().target.ToString()+
                        "\naction = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().action.ToString()+
                        "\nready = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().ready.ToString()+
                        "\nprogress = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().progress.ToString()+
                        " / "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().maxProgress.ToString()+
                        "\ncollision objects.Count = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().collisionObjects.Count.ToString()+
                        "\nconditions = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().conditionsMet+
                        " / "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().conditionsAll;
                    }
                }
            }

            // if multiple objects

            else
            {
                text.text = "objects: "+GameCore.Core.rhit2D.Length.ToString();
            }

            // -------

        }
        
	}
}
