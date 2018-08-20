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

        if (GameCore.Core.rhit2D.Length == 0) // <--- BUG HERE !!

            //NullReferenceException: Object reference not set to an instance of an object
            //CursorLabelCore.Update () (at Assets/CursorLabelCore.cs:24

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
                "\nsortingOrder = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder.ToString()+
                "\nkind = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind.ToString()+
                "\ntype = "+GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().type.ToString();

                // if critter

                if (GameCore.Core.rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind == KindEnum.critter)
                {
                    text.text = GameCore.Core.rhit2D[0].transform.gameObject.name+
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
            text.text = "objects: "+GameCore.Core.rhit2D.Length.ToString();
        }

        // -------
	}
}
