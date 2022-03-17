using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BerryManager : MonoBehaviour
{
    //�����յ� ��ȣ �ٿ��ֱ� ��
    static int Prefabcount = 0;
    int prefabnum;


    [Serializable]
    public class BerryStruct 
    {
        public Sprite berryImage;
        public string berryName, berryTxt;
        public int berryValue;
        //public bool exist;
        
        public BerryStruct(Sprite berryImage, string berryName, string berryTxt, int berryValue, bool exist) 
        {
            this.berryImage = berryImage;
            this.berryName = berryName;
            this.berryTxt = berryTxt;
            this.berryValue = berryValue;
            //this.exist = DataController.instance.gameData.isBerryUnlock[0];

        }
    }

    [Header("======berry Image=====")]
    public Sprite yesBerryImage;

    [SerializeField]
    private GameObject berryImagePanel;//�̹����� ���� ������Ʈ ���

    [Header("==========BERRY STRUCT==========")]
    [SerializeField]
    BerryStruct[] berryInfo;



    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;


    



    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//�̰� ����ȭ ����. �ٸ� ����� ������

        //�����յ鿡�� ��ȣ�� �ٿ� ����
        /*
        if (Prefabcount >= 32)
        {    Prefabcount -= 32;    }
        */
        if (Prefabcount % 32==0)
        { Prefabcount =0; }

        prefabnum = Prefabcount;
        Prefabcount++;

        berryImageChange();

        //Debug.Log("exist====" + berryInfo[prefabnum].exist);
    }



    //=====================================================================================================
    public void Explanation()
    {

            ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//berryExp
            ExpChildren2 = ExpChildren.transform.GetChild(1).transform.gameObject;//berryExpImage

            try
            {
            //if (berryInfo[prefabnum].exist == true)
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                    //����â ����.
                    //����â�� ���.
                    ExpChildren.SetActive(true);
                    //Explanation ������ ä���.
                    ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite = berryInfo[prefabnum].berryImage;//�̹��� ����
                    ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryName;//�̸� ����
                    ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryTxt;//���� ����
                }
            }
            catch
            {
                //ExpChildren.SetActive(false);
                Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
            }


    }




    public void berryImageChange()//���� ����Ʈ�� �̹����� ���δ�.
    {
        for (int i = 0; i < berryInfo.Length; i++)
        {
            if (prefabnum == i && DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                this.transform.GetComponent<Image>().sprite = yesBerryImage;
                berryImagePanel.transform.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                berryImagePanel.GetComponent<Image>().sprite = berryInfo[i].berryImage;
            }
        }

    }


}

