//06-11-2017

using System.Collections;
using UnityEngine;

public class PantsCore : MonoBehaviour
{
    public int team;

    public void RefreshColor()
    {
        if (team == 0)
        GetComponent<SpriteRenderer>().color = Color.gray;
        else
        if (team == 1)
        GetComponent<SpriteRenderer>().color = Color.blue;
        else
        if (team == 2)
        GetComponent<SpriteRenderer>().color = Color.red;

    }
    
	void Start()
    {
        RefreshColor();

        //int i = 1;
        //GetComponent<SpriteRenderer>().sortingLayerID = i;
	}
}
	
