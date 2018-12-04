using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCore : MonoBehaviour
{
    public GameObject myLight;

    //

	void Start ()
    {
		//transform.position += new Vector3(0f,0f,-1f);

        myLight = transform.Find("Light").gameObject;

	}
	
	void Update ()
    {
        
	}
}
