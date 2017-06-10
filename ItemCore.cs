using UnityEngine;
using System.Collections;

public class ItemCore : BodyCore
{
	void Start()
	{
		BodyInitialize();
	}

	void Update() 
	{
		CalculateLand();
		PlaceOnGround();
	}

}
