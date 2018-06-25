using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLabelCore : MonoBehaviour 
{
    Vector3 v1;


	void Start()
    {
		v1 = new Vector3(2f,-0.5f,1f); // offset
	}
	
	void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + v1;
        
        // --- CHECK OBJECT UNDER MOUSE ===

        if (GameCore.Core.rhit2D.Length == 0)
        {
            // if nothing

            gameObject.GetComponent<TextMesh>().text = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString()+
            "\nplayer carriedBodies.count = "+GameCore.Core.player.GetComponent<CritterCore>().carriedBodies.Count.ToString();

        }
        else if (GameCore.Core.rhit2D.Length == 1)
        {
            if (GameCore.Core.rhit2D[0])
            {
                // if any interactive object        

                gameObject.GetComponent<TextMesh>().text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                "\nsortingOrder = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder.ToString()+
                "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString();

                // if critter

                if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == KindEnum.critter)
                {
                    gameObject.GetComponent<TextMesh>().text = GameCore.Core.rhit2D[0].transform.gameObject.name+
                    "\nsortingOrder = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder.ToString()+
                    "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                    "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString()+
                    "\naction = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().action.ToString()+
                    "\ncommand = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().command.ToString()+
                    "\ntargetX = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<CritterCore>().targetX.ToString();
                }
            }
        }

        // if multiple objects

        else
        {
            gameObject.GetComponent<TextMesh>().text = "objects: "+GameCore.Core.rhit2D.Length.ToString();
        }

        // -------
	}
}
