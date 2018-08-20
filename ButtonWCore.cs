using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWCore : MonoBehaviour 
{
    public bool isLand = false;
    public int  landIndex = -1;
    public bool isVisitable = false;

    int i;

    public void Colorize()
    {
        // green color

        if (GameCore.Core.travelMode == true)
        {
            isVisitable = false;

            GameCore.Land land;

            land = GameCore.Core.lands[landIndex];

            if (GameCore.Core.travelRight == false) // if player is on the left edge
            {
                if ((land.nodeL == GameCore.Core.lands[GameCore.Core.currentLand].nodeL)
                || (land.nodeR == GameCore.Core.lands[GameCore.Core.currentLand].nodeL))
                {
                    isVisitable = true;
                }
            }
            else                                    // if player is on the right edge 
            {
                if ((land.nodeL == GameCore.Core.lands[GameCore.Core.currentLand].nodeR)
                || (land.nodeR == GameCore.Core.lands[GameCore.Core.currentLand].nodeR))
                {
                    isVisitable = true;
                }
            }

            if (isVisitable == true)
            {
                GetComponent<Image>().color = Color.green;
            }
            else
            {
                GetComponent<Image>().color = Color.gray;
            }
        }

        if (landIndex == GameCore.Core.currentLand)
        {
            GetComponent<Image>().color = Color.blue;
        }
    }

    public void TaskOnClick()
    {
        if (isLand == true)
        {
            if ((GameCore.Core.travelMode == true)
            && (isVisitable == true))
            {
                // -------- move player on the correct edge of the land ----------------

                GameCore.Land land;
                land = GameCore.Core.lands[landIndex];

                if (GameCore.Core.travelRight == false) // if player is on the left edge
                {
                    if ((land.nodeR.b > GameCore.Core.lands[GameCore.Core.currentLand].nodeL.b)
                    || (land.nodeR.a < GameCore.Core.lands[GameCore.Core.currentLand].nodeL.a))
                    {
                        GameCore.Core.player.transform.position = // place player on the left edge
                        new Vector3(GameCore.Core.landPointX[35],GameCore.Core.landPointY[35],GameCore.Core.player.transform.position.z);
                    }
                    else
                    {
                        GameCore.Core.player.transform.position = // place player on the right edge
                        new Vector3(GameCore.Core.landPointX[GameCore.Core.landSections-35],GameCore.Core.landPointY[GameCore.Core.landSections-35],GameCore.Core.player.transform.position.z);
                    }

                }
                else                                    // if player is on the right edge
                {
                    if ((land.nodeL.b < GameCore.Core.lands[GameCore.Core.currentLand].nodeR.b)
                    || (land.nodeL.a > GameCore.Core.lands[GameCore.Core.currentLand].nodeR.a))
                    {
                        GameCore.Core.player.transform.position = // place player on the right edge
                        new Vector3(GameCore.Core.landPointX[GameCore.Core.landSections-35],GameCore.Core.landPointY[GameCore.Core.landSections-35],GameCore.Core.player.transform.position.z);
                    }
                    else
                    {
                        GameCore.Core.player.transform.position = // place player on the left edge
                        new Vector3(GameCore.Core.landPointX[35],GameCore.Core.landPointY[35],GameCore.Core.player.transform.position.z);
                    }
                }

            }

            
            // load new land
        
            GameCore.Core.LoadLand(landIndex);

            GameCore.Core.worldMap.GetComponent<WorldMapCore>().ClearMap();
            GameCore.Core.worldMap.GetComponent<WorldMapCore>().DrawMap();


            //
        }
    }
}

