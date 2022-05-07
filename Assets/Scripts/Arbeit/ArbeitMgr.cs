using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbeitMgr : MonoBehaviour
{   
    public bool OnRachel;
    public bool OnThomson;
    public bool OnHamsworth;
    public bool OnFubo;

    void FixedUpdate()
    {
        Rachel();
        Thomson();
        Fubo();
        Hamsworth();
    }
    void Rachel()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[0] > 0)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // �ɴ´�                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // üũ ���� ����
                DataController.instance.gameData.PTJNum[0]--;
                Debug.Log("����ÿ ���� Ƚ��: " + DataController.instance.gameData.PTJNum[0]);
            }
        }
    }
    void Thomson()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4] &&
                !GameManager.instance.farmList[i].isHarvest &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[1] > 0)
            {
                GameManager.instance.Harvest(GameManager.instance.stemList[i]); // ��Ȯ�Ѵ�
                DataController.instance.gameData.PTJNum[1]--;
                Debug.Log("�轼 ���� Ƚ��: " + DataController.instance.gameData.PTJNum[1]);
            }
        }
    }   
    void Fubo()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[3] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasBug = false;
                DataController.instance.gameData.PTJNum[3]--;
                Debug.Log("Ǫ�� ���� Ƚ��: " + DataController.instance.gameData.PTJNum[3]);
                StartCoroutine(DeleteBugByFubo(GameManager.instance.bugList[i]));
            }
        }
    }
    IEnumerator DeleteBugByFubo(Bug bug)
    {
        yield return new WaitForSeconds(0.75f);
        bug.DieBug();
        
    }
    void Hamsworth()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed &&
                DataController.instance.gameData.PTJNum[4] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasWeed = false;
                DataController.instance.gameData.PTJNum[4]--;
                Debug.Log("�ܽ����� ���� Ƚ��: " + DataController.instance.gameData.PTJNum[4]);
                StartCoroutine(DeleteWeedByHamsworth(GameManager.instance.farmList[i]));
            }
        }
    }
    IEnumerator DeleteWeedByHamsworth(Farm farm)
    {
        yield return new WaitForSeconds(0.75f);
        farm.weed.DeleteWeed();               
    }
}
