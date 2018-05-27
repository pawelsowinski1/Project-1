// 19-08-2017

using UnityEngine;
using System.Collections;

public class LandCore : MonoBehaviour 
{

	public int landSections;
	public float[] landPointX;
	public float[] landPointY;

	// ==================================================	

	void Awake() 
	{
		//------ generate land ------

		landSections = 100;

		landPointX = new float[landSections];
		landPointY = new float[landSections];

		int i;

		for (i=0; i<landSections; i++)
		{
            if (i == 0)
            landPointX[i] = 0;
            else
            landPointX[i] = landPointX[i-1] + Random.Range(1f, 2f);

			

            if (i == 0)
            landPointY[i] = Random.value * 5;
            else
            landPointY[i] = landPointY[i-1] + Random.Range(-1f, 1f);
		}

		//--------------------------
	
	}

	// ==================================================

	void Update()
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
