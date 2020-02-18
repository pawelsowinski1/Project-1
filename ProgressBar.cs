using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // ================= PROGRESS BAR ===================

    public GameObject parent;
    public GameObject frame;

    public Vector3 frameScale; // needed to make frame invisible via localScale

    Vector3 v1 = new Vector3(0,-0.5f,0); // progress bar position

    // =================================================

    void Start()
    {
        frame = Instantiate(GameCore.Core.progressBarFramePrefab, transform.position, Quaternion.identity);

        frameScale = frame.transform.localScale;
    }

	void LateUpdate() 
    {

        if (parent)
        {

            if (parent.GetComponent<ProjectCore>().progress > 0f)
            {
                float a;

                a = parent.GetComponent<ProjectCore>().progress / parent.GetComponent<ProjectCore>().maxProgress;

                transform.localScale = new Vector3(2.5f * a, 0.2f, 1f);
                frame.transform.localScale = frameScale;
            }
            else // if progress is 0f, then make progress bar and frame invisible
            {
                transform.localScale = new Vector3(0f, 0f, 0f);
                frame.transform.localScale = new Vector3(0f, 0f, 0f);
            }


            transform.position = parent.transform.position + v1;
            frame.transform.position = transform.position;
        }
	}

    void OnDestroy()
    {
        Destroy(frame);
    }
}
