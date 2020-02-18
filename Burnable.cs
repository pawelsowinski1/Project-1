using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour

{
    public GameObject fire; // fire object

    public float fuel;

    IEnumerator Burn()
    {
        for (;;)
        {
            fuel -= 0.02f + fire.GetComponent<FireCore>().size*0.004f; // <-- when changing this, change FireplaceCore.cs too
            
            // when out of fuel

            if (fuel < 0)
            {
                if (GetComponent<ItemCore>())
                {
                    Destroy(fire);
                    StopCoroutine("Burn");

                    // if bark torch, turn to stick

                    if (GetComponent<ItemCore>().item == EItem.barkTorch)
                    {
                        GetComponent<ItemCore>().item = EItem.stick;
                        GetComponent<ItemCore>().ItemInitialize();

                        GameCore.Core.InventoryManager();
                    }

                    // if anything else

                    else
                    {
                        Destroy(fire);
                        Destroy(gameObject);
                    }
                }
            }

            fire.GetComponent<FireCore>().size = fuel;

            yield return new WaitForSeconds(1f);
        }
    }

    public void SetOnFire()
    {
        fire = Instantiate(GameCore.Core.firePrefab, transform.position, Quaternion.identity);
        fire.transform.parent = transform;

        // if bark torch

        if (GetComponent<ItemCore>().item == EItem.barkTorch)
        {
            // translate fire according to torch rotation

            fire.transform.rotation = fire.transform.parent.rotation;
            fire.transform.Translate(Vector3.up*0.75f, Space.Self);
        }

        // if firewood
            
        if (GetComponent<ItemCore>().item == EItem.firewood)
        {
            // spawn campfire and destroy firewood

            GameObject clone;

            clone = GameCore.Core.SpawnStructure(EStructure.campfire);
            clone.transform.position = transform.position;
            clone.GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_firewood;
            clone.transform.localScale = new Vector3(0.6f,0.6f,0.6f);

            //_obj.GetComponent<BodyCore>().onFire = true; <---- ???
            
            Destroy(gameObject);

        }

        StartCoroutine("Burn");
    }

}
