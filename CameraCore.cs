// 05-11-2017

using UnityEngine;
using System.Collections;

public class CameraCore : MonoBehaviour {

	Vector3 mousePos;
	public Vector3 diff;
	GameObject player;

    

	void Start () 
	{
		player = GameObject.Find("Player");

        //Application.targetFrameRate = -1; // for performance check (remember to turn v-sync off) //move this to GameCore.cs
        
	}

	void LateUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		diff = mousePos - player.transform.position;

		transform.position = new Vector3(player.transform.position.x+diff.x/2, player.transform.position.y+diff.y/2, -1);

	}

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
    }
}
