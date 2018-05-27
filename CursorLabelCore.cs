using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLabelCore : MonoBehaviour 
{
    Vector3 v1;

    // set in editor
    public GameCore Core;

	void Start()
    {
		v1 = new Vector3(2f,-0.5f,1f); // offset
	}
	
	void FixedUpdate ()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + v1;
        

        if (Core.rhit2D.Length == 0)
        {
            gameObject.GetComponent<TextMesh>().text = " ";
        }
        else if (Core.rhit2D.Length == 1)
        {
            gameObject.GetComponent<TextMesh>().text = Core.rhit2D[0].transform.gameObject.name+
            "\n transform.position = "+Core.rhit2D[0].transform.position.ToString();
        }
        else
        {
            gameObject.GetComponent<TextMesh>().text = "objects: "+Core.rhit2D.Length.ToString();
        }
	}
}
