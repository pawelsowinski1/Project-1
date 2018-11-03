using UnityEngine;
using System.Collections;

public class SlashCore : InteractiveObjectCore
{
    // ================= SLASH CORE ====================

	public GameObject parent;
    public int team;

	Vector3 v;
	Transform target;

	public bool directionRight;
    public bool alive = true; // fixes multiple hit bug

    // =================================================

	void Start () 
	{
        GetComponent<SpriteRenderer>().sortingOrder = 30;

        team = parent.GetComponent<CritterCore>().team;
        directionRight = parent.GetComponent<CritterCore>().directionRight;

        // set position vector and life time

        if (parent.GetComponent<ManCore>().tool == null)
        {
		    v.Set(0,0.85f,0); 
            Destroy(gameObject,0.06f);
        }
        else
        {
            v.Set(0,1.5f,0);
            Destroy(gameObject,0.15f);
        }

		target = parent.GetComponent<Transform>();

		transform.position = target.position + v;
		//transform.Rotate(0,0,90f);

        if (parent.GetComponent<ManCore>().tool == null)
        {
            if (parent == GameCore.Core.player) // aim at mouse position
            {
                // Get Angle in Radians
                float AngleRad = Mathf.Atan2(GameCore.Core.mousePos.y - transform.position.y, GameCore.Core.mousePos.x - transform.position.x);
                // Get Angle in Degrees
                float AngleDeg = (180 / Mathf.PI) * AngleRad;
                // Rotate Object
                this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            }
            else                              // aim at target
            {
                // Get Angle in Radians
                float AngleRad = Mathf.Atan2(parent.GetComponent<ManCore>().target.transform.position.y - transform.position.y,
                                            parent.GetComponent<ManCore>().target.transform.position.x - transform.position.x);
                // Get Angle in Degrees
                float AngleDeg = (180 / Mathf.PI) * AngleRad;
                // Rotate Object
                this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            }
        }

        // set the sprite 

        if (parent.GetComponent<CritterCore>().type == EType.man)
        {
            if (parent.GetComponent<ManCore>().tool != null)
            {
                GetComponent<SpriteRenderer>().sprite = parent.GetComponent<ManCore>().tool.GetComponent<SpriteRenderer>().sprite;
                transform.localScale = parent.GetComponent<ManCore>().tool.transform.localScale/2;
            }
        }
        //

        // resize box collider 2D to fit the sprite; 
        Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (0, (S.y / 2));
        //

	}

	void FixedUpdate()
	{
        if (parent.GetComponent<ManCore>().tool == null)
        {
            transform.Translate(Vector3.right*0.3f);
        }
        else
        {
            //transform.position = target.position + v;

		    if (directionRight == true)
            {
        	    transform.RotateAround(target.position + new Vector3(0,0.85f,0), new Vector3(0,0,1), -20f); // slash rotation 
            }
		    else
            {
		        transform.RotateAround(target.position + new Vector3(0,0.85f,0), new Vector3(0,0,1), 20f); // slash rotation 
            }
        }
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.gameObject.GetComponent<CritterCore>() != null) // fixes null reference exception bug
        if (alive == true)
		if (other.gameObject.GetComponent<CritterCore>().team != team)
		{
			other.gameObject.GetComponent<CritterCore>().damageColorIntensity = 1f;
            other.gameObject.GetComponent<CritterCore>().hp -= 9;

             if ((other.gameObject.GetComponent<CritterCore>().hp <= 0)
             && (other.gameObject.GetComponent<CritterCore>().downed == false))
             {
                other.gameObject.GetComponent<CritterCore>().downed = true;
                other.transform.Rotate(0,0,90f);

                if (other.gameObject == GameCore.Core.player)
                {
                    if (GameCore.Core.player.GetComponent<ManCore>().tool != null)
                    {
                        GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().isCarried = false;
                        GameCore.Core.player.GetComponent<ManCore>().tool.GetComponent<ItemCore>().carrier = null;
                    }
                }
             }
            
            alive = false;
            Destroy(gameObject,0.0f);
		}
	}
}
