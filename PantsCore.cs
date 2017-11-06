//06-11-2017

using System.Collections;
using UnityEngine;

public class PantsCore : MonoBehaviour {

    public GameObject target;

	void Start()
    {
        int i = 1;
        GetComponent<SpriteRenderer>().sortingLayerID = i;
	}
	
    void LateUpdate()
    {
        transform.position = target.transform.position;
        transform.position += new Vector3(0,0.4f,0);
	}
}
