using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables kind and type.
// Enables being highlighted ?
// Enables having a project on itself

// Type enum is possibly obsolete, if enums are used properly.

public enum EKind {none, critter, slash, projectile, item, plant, structure, project};
public enum EType {none,
    /*critters*/ man, herbi, carni,
      /*plants*/ tree,
       /*items*/ //wood, meat, berry, hammerstone, flint, flint_blade
       };

public class InteractiveObjectCore : MonoBehaviour
{
    public EKind kind = EKind.none;
    public EType type = EType.none;

    public bool hasProject = false;
}
