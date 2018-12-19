using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelCore : MonoBehaviour
{
    void OnEnable()
    {
        // Draw();

        GameCore.Core.mouseOverGUI = true;
    }

    void OnDisable()
    {
        // Clear();

        GameCore.Core.mouseOverGUI = false;
    }

}
