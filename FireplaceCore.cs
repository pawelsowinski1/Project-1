using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceCore : StructureCore
{
    public GameObject fire; // fire object
    public GameObject itemInFire; // item laying directly in fire (e.g. flat rock, clay pot)
    public GameObject itemHeated; // item laying on top or inside the item in fire (e.g. meat, water)

    //public enum EFireplace {none, campfire, bonfire, stove, kiln, furnace};

    public float fuel;

    public void GainFuel(GameObject _fuel)
    {
        fuel += _fuel.GetComponent<Burnable>().fuel;
    }

    // =============================================== MAIN ================================================

    void Start()
    {
        fuel = 1f;

        StructureInitialize();

        UpdateLandSection();
        groundY = GetGroundY();
        transform.position = new Vector2 (transform.position.x, groundY);

        fire = Instantiate(GameCore.Core.firePrefab, transform.position, Quaternion.identity);
        fire.transform.parent = transform;

        StartCoroutine("FireplaceUpdate");
    }

    IEnumerator FireplaceUpdate()
    {
        for (;;)
        {
            fuel -= 0.02f + fire.GetComponent<FireCore>().size*0.004f; // <-- when changing this, change Burnable.cs too

            fire.GetComponent<FireCore>().size = fuel;

            // when out of fuel

            if (fuel < 0)
            {
                if (itemInFire) 
                {
                    itemInFire.GetComponent<BodyCore>().isCarried = false; // enable item in fire to be picked up
                }

                if (itemHeated)
                {
                    if (projectAttached)
                    {
                        Destroy(projectAttached); // destroy heating project
                    }
                }

                Destroy(gameObject);
                GameCore.Core.structures.Remove(gameObject);

            }


            yield return new WaitForSeconds(1f);
        }
    }



}
