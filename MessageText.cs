using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessageText : MonoBehaviour
{
    public GameObject objectToFollow;

    public Vector3 v1 = new Vector3(0f,2f,0f);

    void Start()
    {
        StartCoroutine("FadeOut");
    }

    void LateUpdate()
    {
        if (objectToFollow)
        {
            transform.position = Camera.main.WorldToScreenPoint(objectToFollow.transform.position + v1);
        }
    }
    
    IEnumerator FadeOut()
    {
        for (float f = 5f; f>=0 ; f-=0.1f)
        {
            if (f < 1f)
            {
                GetComponent<Text>().color = new Color(1f,1f,1f,f);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }   

}
