using UnityEngine;
using System.Collections;

public class ProjectileCore : BodyCore // <-- doesn't need to be a full BodyCore child class, because projectile doesn't need the functionality to be able to be carried.
{
    // ============= PROJECTILE CORE ===================

	public GameObject parent;
    public int team;

    Vector3 prevPos = new Vector3(0,0,0);

    // =================================================

	void Start ()
	{
        if (parent)
        team = parent.GetComponent<ManCore>().team;

        transform.position += new Vector3(0,0.4f,0);

        GetComponent<SpriteRenderer>().sortingOrder = 20;
	}


	void Update() 
	{
		Gravity();

        float f1 = GetComponent<Rigidbody2D>().velocity.x;

        // if this is a spear, then face the direction of movement

        if (GetComponent<ItemCore>().item == EItem.stoneSpear)
        {
            Vector3 moveDirection = gameObject.transform.position - prevPos; 
            if (moveDirection != Vector3.zero) 
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle+270f, Vector3.forward);
             
            }
        
            prevPos = gameObject.transform.position;
        }

        // else, rotate around (rotation speed based on velocity.x)

        else
        transform.RotateAround(transform.position, new Vector3(0,0,1), -f1*0.35f);
        //
        
	}


	void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.gameObject.GetComponent<CritterCore>()) // fixes null reference exception bug
		if (other.gameObject.GetComponent<CritterCore>().team != team)
        if (other.gameObject.GetComponent<CritterCore>().alive == true)
		{
			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;

            if (parent.GetComponent<CritterCore>().team == 1)
            {
                if (other.gameObject.GetComponent<CritterCore>().team == 0)
                other.GetComponent<CritterCore>().attitude -= 1f;
                else
                GameCore.Core.teams[other.gameObject.GetComponent<CritterCore>().team].attitude -= 1f;
            }


            // apply damage

            other.gameObject.GetComponent<CritterCore>().hp -= GetComponent<Rigidbody2D>().velocity.magnitude;

             if ((other.gameObject.GetComponent<CritterCore>().hp <= 0)
             && (other.gameObject.GetComponent<CritterCore>().downed == false))
             {
                other.gameObject.GetComponent<CritterCore>().downed = true;
                other.transform.Rotate(0,0,90f);

                if (other.gameObject.GetComponent<ManCore>())
                {
                    if (other.gameObject.GetComponent<ManCore>().hand1Slot != null)
                    {
                        other.gameObject.GetComponent<ManCore>().DropHand1Slot();
                    }
                }

             }

             if (other.gameObject.GetComponent<CritterCore>().hp <= -other.gameObject.GetComponent<CritterCore>().hpMax)
             {
                other.gameObject.GetComponent<CritterCore>().alive = false;
             }
             //

             Destroy(gameObject.GetComponent<ProjectileCore>());
             gameObject.GetComponent<ItemCore>().enabled = true;
             gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
             gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
		}
	}


}
