using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStructure {none, campfire};

public class StructureCore : InteractiveObjectCore
{
	//================== STRUCTURE =====================

    // A large, immovable physical body.

    // Enables staying ground.
    // Allows object to be set on fire.


    // parent class:  InteractiveObjectCore

    // child classes: FireplaceCore

    // =================== VARIABLES =====================

    public EStructure structure;

    public float groundY;

    // =================== METHODS ========================

    public void StructureInitialize()
    {
        kind = EKind.structure;
        GetComponent<SpriteRenderer>().sortingOrder = 5;

        switch (structure)
        {
            case EStructure.campfire:
            {
                name = "Campfire";
                //GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_campfire;
                //transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                break;
            }
        }
    }

    // =========================== MAIN LOOP ================================

    void Start()
    {
        StructureInitialize();
        CalculateLand();

        groundY = GameCore.Core.landPointY[landSection-1] + (transform.position.x-GameCore.Core.landPointX[landSection-1]) * Mathf.Tan(landSteepness);
        transform.position = new Vector2 (transform.position.x, groundY);
    }




}
