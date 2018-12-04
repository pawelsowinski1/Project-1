using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================= INTERACTIVE OBJECT ======================

// A physical object which can be interacted with.

// Allows calculations for standing on the ground.
// Enables being highlighted by mouse.
// Enables having a project on itself.
// Allows object to be set on fire. 

// 
// parent class:  MonoBehaviour

// child classes: BodyCore
//                StructureCore

// ====================== ENUMS ==============================

public enum EKind {none, critter, slash, projectile, item, plant, structure, project};
public enum EType {none,
    /*critters*/ man, herbi, carni,
      /*plants*/ tree,
       /*items*/ 
       };

// ===========================================================

public class InteractiveObjectCore : MonoBehaviour
{
    public EKind kind = EKind.none;
    public EType type = EType.none;

    public int    land;
	public int    landSection;
	public float  landSteepness;

    public bool hasProject = false;
    public GameObject projectAttached;

    public bool onFire = false;

    // ======================================================

    public void CalculateLand()
    {
	    // calculates current landSection and landSteepness
		
	    int i;
		
	    for (i=1; i<GameCore.Core.landSections; i++)
	    {
		    if (transform.position.x < GameCore.Core.landPointX[i])
		    {
			    if (i != landSection)
			    {
				    landSection = i;
				    landSteepness = Mathf.Atan2(GameCore.Core.landPointY[i]-GameCore.Core.landPointY[i-1],GameCore.Core.landPointX[i]-GameCore.Core.landPointX[i-1]);
			    }
			    break;
		    }
	    }
    }

}