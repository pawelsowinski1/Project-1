using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceCore : StructureCore
{
    public GameObject fire;

    public float fuel;

    public void GainFuel(GameObject _fuel)
    {
        fuel += 1f;
        
    }

    void Start()
    {
        fuel = 1f;

        StructureInitialize();
        CalculateLand();

        groundY = GameCore.Core.landPointY[landSection-1] + (transform.position.x-GameCore.Core.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
        transform.position = new Vector2 (transform.position.x, groundY);

        fire = Instantiate(GameCore.Core.firePrefab, transform.position, Quaternion.identity) as GameObject;
        fire.transform.parent = transform;

    }

    void Update()
    {
        fire.GetComponent<FireCore>().myLight.GetComponent<Light>().intensity = fuel/2;
    }

}
