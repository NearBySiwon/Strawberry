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
    public berryType berryType;//���� Ÿ��

    [Header("======berry Image=====")]
    public Sprite yesBerryImage;//���� ���� �� ��� �̹���
    [SerializeField]
    private GameObject berryImagePanel;//�̹����� ���� ������Ʈ ���




    //���� ���� ����
    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;


    int startNum;

    List<GameObject> BERRY;

    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//�̰� ����ȭ ����. �ٸ� ����� ������

        //�����յ鿡�� ��ȣ�� �ٿ� ����==========
        if (Prefabcount % 32==0)
        { Prefabcount =0; }

        prefabnum = Prefabcount;
        Prefabcount++;

        //
        switch (berryType)
        {
            case berryType.classic: BERRY = Globalvariable.instance.classicBerryList; break;
            case berryType.special: BERRY = Globalvariable.instance.specialBerryList; break;
            case berryType.unique: BERRY = Globalvariable.instance.uniqueBerryList; break;
        }


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
                ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//�̹��� ����
                ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//�̸� ����
                ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//���� ����
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
        
        for (int i = 0; i < 32; i++)
        {
            if (prefabnum == i && DataController.instance.gameData.isBerryUnlock[startNum] == true)//���� unlock���� ���
            {
                this.transform.GetComponent<Image>().sprite = yesBerryImage;//��� �̹��� ����
                berryImagePanel.transform.GetComponent<Image>().color = new Color(1f,1f,1f,1f);//���� -> ������
                berryImagePanel.GetComponent<Image>().sprite = BERRY[i].GetComponent<SpriteRenderer>().sprite;//�ش� ���� �̹��� ���̱�

            }
            startNum++;
        }

    }


}

