using UnityEngine;
using System.Collections;

public class LandCore : MonoBehaviour 
{

	public int landSections;
	public float[] landPointX;
	public float[] landPointY;

	// ==================================================	

	void Start () 
	{
		//------ generate land ------

		landSections = 50;

		landPointX = new float[landSections];
		landPointY = new float[landSections];

		int i;

		for (i=0; i<landSections; i++)
		{
			landPointX[i] = i * 3 + Random.value;
			landPointY[i] = Random.value * 5;
		}

		//--------------------------
	
	}

	// ==================================================

	void Update ()
	{
		// ------- draw land --------

		Vector3 startPoint = new Vector3 (0, 0, 0);
		Vector3 endPoint = new Vector3 (0, 0, 0);

		int i;

		for (i=1; i<landSections; i++)
		{
			startPoint.Set (landPointX[i-1],landPointY[i-1],0);
			endPoint.Set (landPointX[i],landPointY[i],0);
			
			Debug.DrawLine(startPoint, endPoint, Color.red);
		}

		// ---------------------------
	
	}

	// ==================================================
}
