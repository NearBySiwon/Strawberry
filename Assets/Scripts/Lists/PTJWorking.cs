using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJWorking : MonoBehaviour
{

    public Sprite[] FacePicture;
    public GameObject face;
    public GameObject employCount;


    //==========Prefab���� ���� �ο�==========
    static int Prefabcount = 0;
    int prefabnum;
    //==========��� Ƚ��==========
    private int PTJ_NUM_NOW;

    



    void Start()
    {

        //�����յ鿡�� ���� ��ȣ ���̱�
        prefabnum = Prefabcount;
        Prefabcount++;

        //�� �̹��� ����
        face.GetComponent<Image>().sprite = FacePicture[prefabnum];

    }


    void Update()
    {
        //�ڽ��� ���Ƚ���� ���� �ľ�
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];
    }

    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;


            //�˹� ����
            if (PTJ_NUM_NOW == 0)
            {
                face.SetActive(false);
                employCount.SetActive(false);

                gameObject.transform.SetAsLastSibling();
            }
            //�˹� �ϴ���
            else
            {
                face.SetActive(true);
                employCount.SetActive(true);

                gameObject.transform.SetAsFirstSibling();
                employCount.GetComponent<Text>().text = DataController.instance.gameData.PTJNum[prefabnum].ToString();
            }

           
            
        }
        get { return PTJ_NUM_NOW; }
    }

}
