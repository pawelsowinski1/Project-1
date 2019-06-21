// 21-11-2017

using UnityEngine;
using System.Collections;

public class CameraCore : MonoBehaviour
{
    // ================= CAMERA CORE ===================

	Vector3 mousePos;
	Vector3 diff;

	GameObject player;

    //GameObject game;

    public Material imageEffectMaterial;

    public float zoom;
    public float targetZoom;

    // =================================================

    /// ----- START -----

	void Start () 
	{
        
        //game = GameObject.Find("Game");
        player = GameCore.Core.player;

        zoom = 10f;
        targetZoom = 10f;

        //Camera.main.backgroundColor = new Color(5f,20f,255f,255f);
        //Camera.main.backgroundColor = new Color(145,205,255,255);
	}
    /// ----- LATE UPDATE -----

    void LateUpdate () 
	{
        if (player != null)
        {
		    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
		    diff = mousePos - player.transform.position;

            if (GameCore.Core.combatMode)
            {
		        transform.position = new Vector3(player.transform.position.x+diff.x/2, player.transform.position.y+diff.y/2, -1);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);
            }



        }
	}

    /// ----- FIXED UPDATE -----

	void Update()
	{	
         var d = Input.GetAxis("Mouse ScrollWheel");

         if (d > 0f)
         {
             // scroll up

             targetZoom -= 2f;

             if (targetZoom <= 2f)
             targetZoom = 2f;
         }
         else if (d < 0f)
         {
             // scroll down

             targetZoom += 2f;
         }

         zoom += (targetZoom - zoom)*0.1f;

         Camera.main.orthographicSize = zoom;
    }

	void FixedUpdate()
	{	

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

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, imageEffectMaterial);
    }
}