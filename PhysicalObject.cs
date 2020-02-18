using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ================= PHYSICAL OBJECT ======================

// A physical object. Has a 2D collider.

// Exists in a specific land.
// Can be hit.
// Allows calculating current landSection and landSteepness <--- ?
// Can be detected / highlighted by mouse.
// Can have a project attached to itself.
// Can spawn message texts.


// 
// parent class:  -

// child classes: BodyCore
//                StructureCore
//                ProjectCore


// ===========================================================

public class PhysicalObject : MonoBehaviour
{
    public int   land;
	public int   landSection;
	public float landSteepness;
    public float groundY;

    public float highlightAmount = 0f; // currently highlight is black, 0f is no highlight, 1f is max

    public bool hasProject = false;
    public GameObject projectAttached;

    // ======================================================

    public void UpdateLandSection()
    {
        // set current landSection and landSteepness

        for (int i=1; i<GameCore.Core.landSections; i++)
	    {
		    if (transform.position.x < GameCore.Core.landPointX[i])
		    {
			    if (i != landSection)
			    {
				    landSection = i;
			    }
			    break;
		    }
	    }

        landSteepness = Mathf.Atan2(GameCore.Core.landPointY[landSection] - GameCore.Core.landPointY[landSection - 1], GameCore.Core.landPointX[landSection] - GameCore.Core.landPointX[landSection - 1]);


    }

    public float GetGroundY()
    {
        // return ground y position based on current land steepness

        float groundY = GameCore.Core.landPointY[landSection-1] + (transform.position.x - GameCore.Core.landPointX[landSection-1]) * Mathf.Tan(landSteepness);

        return groundY;
    }

    public IEnumerator HitColorize()
    {

        for (float f = 1f; f >= 0; f -= 0.1f) 
        {
            float f1 = GetComponent<SpriteRenderer>().color.a;

		    GetComponent<SpriteRenderer>().color = new Color(1f,1f-f,1f-f,f1);
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void MessageText(string _message)
    {
        GameObject clone;
        clone = Instantiate(GameCore.Core.messageTextPrefab, transform.position, Quaternion.identity);
        clone.transform.SetParent(GameCore.Core.myCanvas.transform,false);
        clone.GetComponent<MessageText>().objectToFollow = gameObject;

        clone.GetComponent<Text>().text = _message;


    }

    public void Highlight()
    {
        float f =  GetComponent<SpriteRenderer>().color.a;

        GetComponent<SpriteRenderer>().color = new Color(1f-highlightAmount, 1f-highlightAmount, 1f-highlightAmount,f);

        highlightAmount = 0.2f * Mathf.Sin(Time.time*15f);


    }

}