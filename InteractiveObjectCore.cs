using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables kind and type.
// Enables being highlighted ?

// Type enum is possibly obsolete, if enums are used properly.

public enum EKind {none, critter, item, projectile, plant, structure};
public enum EType {none,
    /*critters*/ man, herbi, carni,
      /*plants*/ tree,
       /*items*/ //wood, meat, berry, hammerstone, flint, flint_blade
       };

public class InteractiveObjectCore : MonoBehaviour
{
    public EKind kind = EKind.none;
    public EType type = EType.none;
}
