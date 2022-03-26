using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum berryType { classic, special, unique }
public class BerryManager : MonoBehaviour
{
    //�����յ� ��ȣ �ٿ��ֱ� ��
    static int Prefabcount = 0;
    int prefabnum;

    [Header("======berry Type=====")]
    public berryType berryType;

    [Header("======berry Image=====")]
    public Sprite yesBerryImage;
    [SerializeField]
    private GameObject berryImagePanel;//�̹����� ���� ������Ʈ ���


    [Header("==========BERRY PREFAB==========")]
    public GameObject[] berryPrefabs;


    //���� ���� ����
    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;


    int startNum;



    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//�̰� ����ȭ ����. �ٸ� ����� ������

        //�����յ鿡�� ��ȣ�� �ٿ� ���� 32����
        if (Prefabcount % 32==0)
        { Prefabcount =0; }

        prefabnum = Prefabcount;
        Prefabcount++;

        //������ �ִ� �������� ���δ�.
        berryImageChange();

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

                //����â ����
                ExpChildren.SetActive(true);

                //Explanation ������ ä���.
                /*
                ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite 
                    = berryPrefabs[prefabnum].GetComponent<SpriteRenderer>().sprite;//�̹��� ����

                ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text 
                    = berryPrefabs[prefabnum].GetComponent<Berry>().berryName;//�̸� ����

                ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text 
                    = berryPrefabs[prefabnum].GetComponent<Berry>().berryExplain;//���� ����
                */

                //prefabNum�����ʿ�
                ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite
                    = Globalvariable.instance.berryListAll[prefabnum].GetComponent<SpriteRenderer>().sprite;
                ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text
                    = Globalvariable.instance.berryListAll[prefabnum].GetComponent<Berry>().berryName;
                ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text
                    = Globalvariable.instance.berryListAll[prefabnum].GetComponent<Berry>().berryExplain;
            }
        }
        catch
        {
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }


    }




    public void berryImageChange()//���� ����Ʈ�� �̹����� ���δ�.
    {
        
        switch (berryType) {
            case berryType.classic: startNum = 0; break;
            case berryType.special: startNum = 32; break;
            case berryType.unique: startNum = 64; break;
        }
        
        for (int i = 0; i < berryPrefabs.Length; i++)
        {
            if (prefabnum == i && DataController.instance.gameData.isBerryUnlock[startNum] == true)//���� unlock���� ���
            {
                this.transform.GetComponent<Image>().sprite = yesBerryImage;//��� �̹��� ����
                berryImagePanel.transform.GetComponent<Image>().color = new Color(1f,1f,1f,1f);//���� -> ������
                //berryImagePanel.GetComponent<Image>().sprite = berryPrefabs[i].GetComponent<SpriteRenderer>().sprite;//���� �̹��� ���̱�
                berryImagePanel.GetComponent<Image>().sprite = Globalvariable.instance.berryListAll[i].GetComponent<SpriteRenderer>().sprite;


            }
            startNum++;
        }

    }


}

