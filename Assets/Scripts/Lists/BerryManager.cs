using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BerryManager : MonoBehaviour
{
    [Serializable]
    public class BerryStruct 
    {
        public Sprite berryImage;
        public string berryName, berryTxt;
        public int berryValue;
        public bool exist;
        
        public BerryStruct(Sprite berryImage, string berryName, string berryTxt, int berryValue, bool exist) 
        {
            this.berryImage = berryImage;
            this.berryName = berryName;
            this.berryTxt = berryTxt;
            this.berryValue = berryValue;
            this.exist = exist;

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


    //�����յ� ��ȣ �ٿ��ֱ� ��
    static int Prefabcount = 0;
    int prefabnum;



    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//�̰� ����ȭ ����. �ٸ� ����� ������

        //�����յ鿡�� ��ȣ�� �ٿ� ����
        if (Prefabcount >= 32)
        {    Prefabcount -= 32;    }
        prefabnum = Prefabcount;
        Prefabcount++;

        berryImageChange();
    }

    void Update()
    {

    }



    //=====================================================================================================
    public void Explanation()
    {

            ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//berryExp
            ExpChildren2 = ExpChildren.transform.GetChild(1).transform.gameObject;//berryExpImage

            try
            {
                if (berryInfo[prefabnum].exist == true)
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
            if (prefabnum == i && berryInfo[i].exist == true)
            {
                this.transform.GetComponent<Image>().sprite = yesBerryImage;
                berryImagePanel.transform.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                berryImagePanel.GetComponent<Image>().sprite = berryInfo[i].berryImage;
            }
        }
    }


}

