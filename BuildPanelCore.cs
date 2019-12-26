using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelCore : MonoBehaviour
{
    public GameObject buttonBPrefab;
    public GameObject ghostObject;

    List<GameObject> buttonsB = new List<GameObject>();


    public void AddButtonB(bool _isStructure, int index)
    {
        GameObject clone, clone2;

        // create button    

        clone = Instantiate(buttonBPrefab, new Vector3(0f,0f,0f) , Quaternion.identity) as GameObject;
        buttonsB.Add(clone);
        clone.transform.SetParent(GameCore.Core.myCanvas.transform,false);

        // create image

        clone2 = Instantiate(GameCore.Core.imagePrefab, new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
        clone2.transform.SetParent(clone.transform,false);

        // if structure

        if (_isStructure == true)
        {
            clone.transform.position += new Vector3(-400f,200f,0f);
            clone.GetComponent<ButtonBCore>().isStructure = true;
            clone.GetComponent<ButtonBCore>().structure = EStructure.shelter;

            clone2.GetComponent<Image>().sprite = GameCore.Core.spr_shelter;
        }

        // if item

        else
        {
            clone.GetComponent<ButtonBCore>().isStructure = false;

            clone.transform.position += new Vector3(-400f+125f*index,-100f,0f);
        
            switch (index)
            {
                case 0:
                {
                    clone.GetComponent<ButtonBCore>().item = EItem.cordage;
                    clone2.GetComponent<Image>().sprite = GameCore.Core.spr_cordage;

                    break;
                }
                case 1:
                {
                    clone.GetComponent<ButtonBCore>().item = EItem.handAxe;

                    clone2.GetComponent<Image>().sprite = GameCore.Core.spr_handAxe;

                    break;
                }
                case 2:
                {
                    clone.GetComponent<ButtonBCore>().item = EItem.stoneSpear;

                    clone2.GetComponent<Image>().sprite = GameCore.Core.spr_stoneSpear;

                    break;
                }

                case 3:
                {
                    clone.GetComponent<ButtonBCore>().item = EItem.barkTorch;

                    clone2.GetComponent<Image>().sprite = GameCore.Core.spr_barkTorch;

                    break;
                }
            }

        }
      
    }

    public void Draw()
    {
        AddButtonB(true,0);

        AddButtonB(false,0);
        AddButtonB(false,1);
        AddButtonB(false,2);
        AddButtonB(false,3);
    }

    public void Clear()
    {
        int i;

        for (i=0; i<buttonsB.Count; i++)
        {
            Destroy(buttonsB[i]);
        }

        buttonsB.Clear();
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
