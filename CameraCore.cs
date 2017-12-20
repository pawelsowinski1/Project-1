// 21-11-2017

using UnityEngine;
using System.Collections;

public class CameraCore : MonoBehaviour
{
    // ================= CAMERA CORE ===================

	Vector3 mousePos;
	Vector3 diff;

	GameObject player;

    GameObject game;

    // =================================================

    /// ----- START -----

	void Start () 
	{
        //Application.targetFrameRate = -1; // for performance check (remember to turn v-sync off) //move this to GameCore.cs
        game = GameObject.Find("Game");
        player = game.GetComponent<GameCore>().player;
	}
    /// ----- LATE UPDATE -----

    void LateUpdate () 
	{
        if (player != null)
        {
		    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
		    diff = mousePos - player.transform.position;

		    transform.position = new Vector3(player.transform.position.x+diff.x/2, player.transform.position.y+diff.y/2, -1);
        }
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{	
         var d = Input.GetAxis("Mouse ScrollWheel");

         if (d > 0f)
         {
             // scroll up

             Camera.main.orthographicSize -= 1f;

             if (Camera.main.orthographicSize <= 1f)
             Camera.main.orthographicSize = 1f;
         }
         else if (d < 0f)
         {
             // scroll down

             Camera.main.orthographicSize += 1f;
         }

        if (player == null)
        {
            if(Input.GetKey(KeyCode.W))
		    transform.position += new Vector3(0,1,0);

            if(Input.GetKey(KeyCode.S))
		    transform.position += new Vector3(0,-1,0);

		    if(Input.GetKey(KeyCode.A))
		    transform.position += new Vector3(-1,0,0);
		
		    if(Input.GetKey(KeyCode.D))
		    transform.position += new Vector3(1,0,0);
        }
	}
}