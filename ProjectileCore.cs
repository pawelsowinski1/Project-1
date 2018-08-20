using UnityEngine;
using System.Collections;

public class ProjectileCore : BodyCore // <-- doesn't need to be a full BodyCore child class, because projectile doesn't need the functionality to be able to be carried.
{
    // ============= PROJECTILE CORE ===================

	public GameObject parent;
    public GameObject carriedItem;

    public int team;

    // =================================================

	void Start ()
	{
        team = parent.GetComponent<PlayerCore>().team;

        transform.position += new Vector3(0,0.75f,0);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.gameObject.GetComponent<CritterCore>()) // fixes null reference exception bug
		if (other.gameObject.GetComponent<CritterCore>().team != team)
		{
			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;
			Destroy(gameObject,0.0f);

            // free carried item and destroy itself 

            //NullReferenceException: Object reference not set to an instance of an object
            //BodyCore.PlaceOnGround () (at Assets/BodyCore.cs:88)
            //PlantCore.Update () (at Assets/PlantCore.cs:16)

            carriedItem.GetComponent<BodyCore>().isCarried = false;
            carriedItem.GetComponent<BodyCore>().carrier = null;
            carriedItem.GetComponent<BodyCore>().isFalling = true;

            Destroy(gameObject);

            //
		}
	}
}
