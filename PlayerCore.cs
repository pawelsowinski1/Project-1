using UnityEngine;
using System.Collections;

public class PlayerCore : CritterCore 
{

	void FixedUpdate ()
	{	

		if(Input.GetKey(KeyCode.A))
		MoveLeft();
		
		if(Input.GetKey(KeyCode.D))
		MoveRight();
		
		if(Input.GetKey(KeyCode.W))
		Jump();

		if(Input.GetMouseButton(0))
		Hit();
		
		if(Input.GetMouseButton(1))
		Shoot();

	}

}
