using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : PlantCore
{

    public void DropResources()
    {
        float f = size/10f; // f = 1f...10f
        int r = Mathf.FloorToInt(f); // r = 1...9

        f /= 10f; // f = 0.1f...1f

        switch (plant)
        {
            case EPlant.spruce:
            {
                for (int i=0; i<r; i++)
                {

                    GameObject o;
                    o = GameCore.Core.SpawnItem(EItem.smallLog);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);
                    o = GameCore.Core.SpawnItem(EItem.firewood);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);
                    o = GameCore.Core.SpawnItem(EItem.plantMaterial);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);

                }

                break;
            }

            case EPlant.birch:
            {
                for (int i=0; i<r; i++)
                {
                    GameObject o;
                    o = GameCore.Core.SpawnItem(EItem.smallLog);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);
                    o = GameCore.Core.SpawnItem(EItem.firewood);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);
                    o = GameCore.Core.SpawnItem(EItem.plantMaterial);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);
                    o = GameCore.Core.SpawnItem(EItem.bark);
                    o.transform.position = transform.position + new Vector3(Random.Range(-f,f),0f,0f);

                }

                break;
            }
        }

        //
    }

    void Start()
    {
    	BodyInitialize();
        PlantInitialize();
    }


}
