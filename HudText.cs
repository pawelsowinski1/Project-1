using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudText : MonoBehaviour

{
    public Text text;

    public GameObject objectToFollow;

    //

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (GameCore.Core.hideHUD == true)
        {
            text.text = "";
        }
        else
        if (objectToFollow)
        {
            if (objectToFollow.activeSelf == false)
            {
                text.text = "";
            }
            else
            {
                transform.position = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);

                text.text = "\n\n"+ objectToFollow.name;

                if (objectToFollow.GetComponent<CritterCore>())
                {
                    text.text += "\nhp: "+objectToFollow.GetComponent<CritterCore>().hp + "/" + objectToFollow.GetComponent<CritterCore>().hpMax;
                    text.text += "\ntarget: "+objectToFollow.GetComponent<CritterCore>().target;
                    text.text += "\naction: "+objectToFollow.GetComponent<CritterCore>().action;

                    if (objectToFollow.GetComponent<ManCore>())
                    {
                        text.text += "\ncommand: "+objectToFollow.GetComponent<CritterCore>().command;
                        text.text += "\ncommandTarget: "+objectToFollow.GetComponent<CritterCore>().commandTarget;

                        if (objectToFollow.GetComponent<CritterCore>().team != 1)
                        {
                            text.text += "\nattitude: "+objectToFollow.GetComponent<CritterCore>().attitude;
                        }
                    }

                }
            }
        }

    }

}
