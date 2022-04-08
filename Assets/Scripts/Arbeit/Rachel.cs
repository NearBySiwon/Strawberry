using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rachel: MonoBehaviour
{
    void FixedUpdate()
    {
        for(int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if(!DataController.instance.gameData.berryFieldData[i].isPlant)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // �ɴ´�                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // üũ ���� ����
            }
        }
    }
}
