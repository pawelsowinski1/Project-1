using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class GhostObjectCore : PhysicalObject
{
    void Start()
    {
        transform.localScale = new Vector3(1.5f,1.5f,1.5f);
    }

	void Update()
    {
		transform.position = GameCore.Core.mousePos + new Vector3(0f,0f,0.5f);

        UpdateLandSection();
        groundY = GetGroundY();

        transform.position = new Vector3(transform.position.x, groundY ,transform.position.z);

        if(Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);

            GameObject clone;
            clone = Instantiate(GameCore.Core.projectPrefab, transform.position, Quaternion.identity) as GameObject;
            clone.GetComponent<ProjectCore>().action = EAction.buildShelter;

            //clone = GameCore.Core.SpawnStructure(EStructure.shelter);
            //clone.transform.position = transform.position;

        }
	}
}
