using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelCore : MonoBehaviour
{
    public GameObject buttonSPrefab;
    public GameObject ghostObject;

    List<GameObject> buttonsS = new List<GameObject>();


    public void AddButtonS(bool _isStructure, int index)
    {
        GameObject clone, clone2;

        // create button    

        clone = Instantiate(buttonSPrefab, new Vector3(0f,0f,0f) , Quaternion.identity) as GameObject;
        buttonsS.Add(clone);
        clone.transform.SetParent(GameCore.Core.myCanvas.transform,false);

        // create image

        clone2 = Instantiate(GameCore.Core.imagePrefab, new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
        clone2.transform.SetParent(clone.transform,false);

        // if structure

        if (_isStructure == true)
        {
            clone.transform.position += new Vector3(-400f,200f,0f);
            clone.GetComponent<ButtonSCore>().isStructure = true;
            clone.GetComponent<ButtonSCore>().structure = EStructure.shelter;

            clone2.GetComponent<Image>().sprite = GameCore.Core.spr_shelter;
        }

        // if item

        else
        {
            clone.GetComponent<ButtonSCore>().isStructure = false;
        
            if (index == 0)
            {
                clone.transform.position += new Vector3(-400f,-100f,0f);
                clone.GetComponent<ButtonSCore>().item = EItem.handAxe;

                clone2.GetComponent<Image>().sprite = GameCore.Core.spr_handAxe;
            }
            else
            if (index == 1)
            {
                clone.transform.position += new Vector3(-400f+125f*index,-100f,0f);
                clone.GetComponent<ButtonSCore>().item = EItem.stoneSpear;

                clone2.GetComponent<Image>().sprite = GameCore.Core.spr_stoneSpear;
            }
        }
      
    }

    public void Draw()
    {
        AddButtonS(true,0);

        AddButtonS(false,0);
        AddButtonS(false,1);
    }

    public void Clear()
    {
        int i;

        for (i=0; i<buttonsS.Count; i++)
        {
            Destroy(buttonsS[i]);
        }

        buttonsS.Clear();
    }


    // ================================= main loop ======================================


    void OnEnable()
    {
        Draw();

        GameCore.Core.mouseOverGUI = true;

        transform.SetSiblingIndex(0); // test 
    }

    void OnDisable()
    {
        Clear();

        GameCore.Core.mouseOverGUI = false;
    }

}
