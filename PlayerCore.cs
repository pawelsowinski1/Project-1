using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists


public class PlayerCore : ManCore 
{
    // ============================================ PLAYER CORE ====================================================

    // parent class:  ManCore
    // child classes: -

    public List<GameObject> pickupTargets;
    

    //public GameObject chosenObject; // object chosen by clicking highlighted button type I

    /// SetDirection()

	void SetDirection()
	{
		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
		{
			directionRight = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}
		else
		{
			directionRight = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	// ============================================= MAIN LOOP ====================================================




    ///  ----- START -----
	
	void Start()
	{
        name = "Player";

		BodyInitialize();
        team = 1;
        pickupTargets.Clear();

        Component[] c;

        c = GetComponentsInChildren<HpBar>();

        //hpBar = c[0].transform.gameObject;

        // add hudText

        GameObject clone;
        clone = Instantiate(GameCore.Core.hudTextPrefab, GameCore.Core.myCanvas.transform);
        clone.GetComponent<HudText>().objectToFollow = gameObject;

        //

	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{
        Gravity();
        AI();

        if ((downed == false)
        && (GameCore.Core.gamePaused == false))
        {
            //action = EAction.none;

		    if(Input.GetKey(KeyCode.A))
            {
		        Stop();
                MoveLeft();
                GetComponent<SpriteRenderer>().flipX = true;

                if (GameCore.Core.combatMode == false)
                directionRight = false;

            }
		
		    if(Input.GetKey(KeyCode.D))
            {
                Stop();
		        MoveRight();
                GetComponent<SpriteRenderer>().flipX = false;

                if (GameCore.Core.combatMode == false)
                directionRight = true;

            }

            if ((GameCore.Core.combatMode == true)
            && (GameCore.Core.mouseOverGUI == false))
            {
		        if(Input.GetMouseButton(0))
                {
                    Stop();
		            Hit();
                }
		
		        if(Input.GetMouseButton(1))
                {
                    Stop();
		            Throw();
                }
            }
        }
	}

    /// ----- UPDATE -----

	void Update()
	{
        if ((downed == false)
        && (GameCore.Core.gamePaused == false))
        {
            if (GameCore.Core.combatMode == true)
            SetDirection();

		    if(Input.GetKeyDown(KeyCode.W))
            {
                action = EAction.none;
                Gravity();
		        Jump();

            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                action = EAction.none;

                if (pickupTargets.Count > 0)
                {
		            PickUp(pickupTargets[0]);
                    //pickupTargets[0].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);

                    pickupTargets.RemoveAt(0);

                    GameCore.Core.ClearButtons();
                }
                //else
                //DropAll();
            }
        }

		DamageColorize();

        if (hitCooldown > 0)
        hitCooldown--;

        /*
        if (pickupTargets.Count > 0)
        {
            for (var i = 0; i<pickupTargets.Count; i++)
            {
                if (Mathf.Abs(pickupTargets[i].transform.position.x - transform.position.x) > 1.5f)
                {
                    pickupTargets[i].GetComponent<SpriteRenderer>().color = Color.white;
                    pickupTargets.RemoveAt(i);
                }
            }
        }
        */

	}
    
    /// ----- ON TRIGGER -----
    
    // detecting an object to pick up
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        bool b = false;

        if (other.gameObject.GetComponent<ItemCore>())
        b = true;

        if (other.gameObject.GetComponent<HerbiCore>())
        b = true;

        if (other.gameObject.GetComponent<PlantCore>())
        {
            if (other.gameObject.GetComponent<PlantCore>().isRooted == false) 
            b = true;
        }

        if (b == true)
        {
            if (other.gameObject.GetComponent<BodyCore>().isCarried == false)
            {
                //if (pickupTarget)
                //pickupTarget.GetComponent<SpriteRenderer>().color = Color.white;

                pickupTargets.Add(other.gameObject);
                //pickupTarget.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (pickupTargets.Contains(other.gameObject))
        {
            pickupTargets.Remove(gameObject);
        }
    }
    
    
    void OnEnable()
    {
        AddHpBar();
    }

    void OnDisable()
    {
        Destroy(hpBar);
    }
    
}
