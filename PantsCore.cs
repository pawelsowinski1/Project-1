using System.Collections;
using UnityEngine;

public class PantsCore : MonoBehaviour
{
    public int team;

    public void RefreshColor()
    {
        /*
        if (transform.parent.GetComponent<CritterCore>().team == 0)
        GetComponent<SpriteRenderer>().color = Color.gray;
        else
        if (transform.parent.GetComponent<CritterCore>().team == 1)
        GetComponent<SpriteRenderer>().color = Color.blue;
        else
        if (transform.parent.GetComponent<CritterCore>().team == 2)
        GetComponent<SpriteRenderer>().color = Color.red;
        */

        GetComponent<SpriteRenderer>().color = GameCore.Core.teams[transform.parent.GetComponent<CritterCore>().team].color;

    }
    
	void Start()
    {
        RefreshColor();

        //int i = 1;
        //GetComponent<SpriteRenderer>().sortingLayerID = i;
	}
}
	
