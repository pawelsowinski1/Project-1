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

            //fuel -= 0.005f + fire.GetComponent<FireCore>().size*0.001f;
            fuel -= 0.02f + fire.GetComponent<FireCore>().size*0.004f;


            if (fuel < 0)
            {
                fuel = 0;
                
                // todo
            }

            fire.GetComponent<FireCore>().size = fuel;

            yield return new WaitForSeconds(1f);
        }
    }



}
