using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonE : MonoBehaviour
{
    // ButtonE

    // Button used in Escape Menu and all of its screens.

    public int index; 

    void Start()
    {
        GameCore.Core.buttonsE.Add(gameObject);

        GetComponentInChildren<Text>().fontSize = 30;
        GetComponentInChildren<Text>().color = Color.white;


        switch (index)
        {
            // esc menu

            case 0:
            {
                GetComponentInChildren<Text>().text = "RESUME";
                break;
            }

            case 1:
            {
                GetComponentInChildren<Text>().text = "HELP";
                break;
            }

            case 2:
            {
                GetComponentInChildren<Text>().text = "RESTART";
                break;
            }

            case 3:
            {
                GetComponentInChildren<Text>().text = "QUIT";
                break;
            }

            // help panel

            case 4:
            {
                GetComponentInChildren<Text>().text = "OK";

                break;
            }

        }

    }

    public void TaskOnClick()
    {
        switch (index)
        {

            case 1: // help
            {
                GameCore.Core.escMenu.SetActive(false);
                GameCore.Core.helpPanel.SetActive(true);

                break;
            }

            case 2: // restart
            {
		        Application.LoadLevel(0);
                break;
            }

            case 3: // quit
            {
                Application.Quit();
                break;
            }

            case 0:
            case 4: // ok = resume game
            {
                GameCore.Core.escMenu.SetActive(false);
                GameCore.Core.helpPanel.SetActive(false);

                if (GameCore.Core.gamePaused)
                GameCore.Core.UnpauseGame();

                break;
            }
        }
    }

}
