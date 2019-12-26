using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // ================= PROGRESS BAR ===================

    public GameObject parent;
    public GameObject frame;

    Vector3 v1 = new Vector3(0,-0.5f,0); // progress bar position

    // =================================================

    void Start()
    {
        frame = Instantiate(GameCore.Core.progressBarFramePrefab, transform.position, Quaternion.identity);
    }

	void LateUpdate() 
    {

        if (parent)
        {

            float a;

            a = parent.GetComponent<ProjectCore>().progress / parent.GetComponent<ProjectCore>().maxProgress;

            transform.localScale = new Vector3(2.5f * a, 0.2f, 1f);

            transform.position = parent.transform.position + v1;
            frame.transform.position = transform.position;
        }
	}

    void OnDestroy()
    {
        Destroy(frame);
    }
}
