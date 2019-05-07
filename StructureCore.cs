using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStructure {none, campfire, shelter};

public class StructureCore : InteractiveObjectCore
{
	//================== STRUCTURE =====================

    // A large, immobile physical body.

    // Enables staying on the ground.
    // Allows object to be set on fire. ?


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
            case EStructure.shelter:
            {
                name = "Shelter";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_shelter;
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                break;
            }


        }

        // fit box collider 2D to the sprite; 
        gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center;
        //

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
