using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables kind and type.
// Enables being highlighted ?

// Type enum is possibly obsolete, if enums are used properly.

public enum KindEnum {none, critter, item, projectile, plant, structure};
public enum TypeEnum {none,
    /*critters*/ player, man, herbi,
      /*plants*/ tree,
       /*items*/ wood, meat, berry, hammerstone, flint, flint_blade};

public class InteractiveObjectCore : MonoBehaviour
{
    public KindEnum kind = KindEnum.none;
    public TypeEnum type = TypeEnum.none;
}
