using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CursorLabelCore : MonoBehaviour 
{
    Vector3 v1;

    public Text text;


	void Start()
    {
		v1 = new Vector3(200f,-200f,1f); // offset
	}
	
	void Update()
    {
        transform.position = Input.mousePosition + v1;
        
        // --- CHECK OBJECT UNDER MOUSE ===

        if (GameCore.Core.rhit2D.Length == 0) // <--- BUG HERE

            //NullReferenceException: Object reference not set to an instance of an object
            //CursorLabelCore.Update () (at Assets/CursorLabelCore.cs:24

        {
            // if nothing

            text.text = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString();
            //text.text = "";

        }
        else if (GameCore.Core.rhit2D.Length == 1)
        {
            if (GameCore.Core.rhit2D[0])
            {
                // if any interactive object        

                text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                
                "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString();

                // if critter

                if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.critter)
                {
                    text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                    "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                    "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
                    "\nteam = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().team.ToString()+
                    "\naction = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().action.ToString()+
                    "\ncommand = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().command.ToString()+
                    "\ntargetX = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().targetX.ToString()+
                    "\ntarget = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().target+
                    "\ntimerAI = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerAI+
                    "\ntimerMove = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerMove+
                    "\ntimerHit = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().timerHit;

                }
                
                // if project

                else
                if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == EKind.project)
                {
                    text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                    "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                    "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
                    "\ntarget = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().target.ToString()+
                    "\naction = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().action.ToString()+
                    "\nready = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().ready.ToString()+
                    "\nprogress = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().progress.ToString()+
                    " / "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().maxProgress.ToString()+
                    "\ncollision objects.Count = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<ProjectCore>().collisionObjects.Count.ToString();

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
