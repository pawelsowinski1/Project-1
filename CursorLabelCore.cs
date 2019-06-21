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
        if (GameCore.Core.combatMode == false)
        {
            transform.position = Input.mousePosition + v1;

            if (GameCore.Core.rhit2D.Length == 0)
            {
                text.text = "";
            }
            else
            if (GameCore.Core.rhit2D.Length == 1)
            {
                text.text = GameCore.Core.rhit2D[0].transform.gameObject.name;
            }
            else
            text.text = "objects: "+GameCore.Core.rhit2D.Length.ToString();
        }
        else
        text.text = "";
    }

    // -------------------------- main ------------------------------

	void Start()
    {
		isHidden = false;

        
	}
	
	void Update()
    {
        transform.SetSiblingIndex(100);

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

                    text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                
                    "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                    "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString();

                    // if plant

                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.plant)
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                        "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
                        "\nage = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<PlantCore>().age+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                        
                    }

                    // if structure

                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.structure)
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                        "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    }

                    // if fireplace

                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>())
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().kind+
                        "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().type+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder+
                        "\nfuel = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().fuel+
                        "\nitem in fire = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().itemInFire+
                        "\nitem heated = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<FireplaceCore>().itemHeated;
                    }

                    // if critter

                    else
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.critter)
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                        "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
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
                    if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.project)
                    {
                        text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                        "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                        "\nsorting order = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder+
                        "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
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
