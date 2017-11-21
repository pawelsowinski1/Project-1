using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelCore : MonoBehaviour 
{
    public GameObject parent;
    Vector3 v1 = new Vector3(0f,2f,0f);

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position = parent.transform.position + v1;
	}
}
