using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BerryManager : MonoBehaviour
{
    [Serializable]
    public struct BerryStruct 
    {
        public Sprite berryImage;
        public string berryName, berryTxt;
        
        public BerryStruct(Sprite berryImage, string berryName, string berryTxt) 
        {
            this.berryImage = berryImage;
            this.berryName = berryName;
            this.berryTxt = berryTxt;

        }
    }


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
        berryExp = GameObject.Find("berryExplanation");

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
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//Expchildren = ���̶�Ű�� berryExplanation�� �ڽ� berryExp�� �ǹ�
        ExpChildren2 = ExpChildren.transform.GetChild(0).transform.gameObject;//Expchlidren2 = Expchildren1�� �ڽ��� berryExpImage�� �ǹ�


        //����â�� ���.
        ExpChildren.SetActive(true);

            

        try
        {
            //Explanation ������ ä���.
            ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite = berryInfo[prefabnum].berryImage;//�̹��� ����
            ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryName;//�̸� ����
            ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryTxt;//���� ����
        }
        catch(System.IndexOutOfRangeException exception) 
        {
            ExpChildren.SetActive(false);
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }


    }




    public void berryImageChange()//���� ����Ʈ�� �̹����� ���δ�.
    {
        for (int i = 0; i < berryInfo.Length; i++)
        {
            if (prefabnum == i)
                berryImagePanel.GetComponent<Image>().sprite = berryInfo[i].berryImage;
        }
    }


}
