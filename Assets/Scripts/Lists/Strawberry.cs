using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Strawberry : MonoBehaviour
{
    //�����յ� ��ȣ �ٿ��ֱ� ��
    static int Prefabcount = 0;
    int prefabnum;

    [Header("======berry Image=====")]
    [SerializeField]
    private Sprite yesBerryImage;//���� ���� �� ��� �̹���
    [SerializeField]
    private GameObject berryImagePanel;//�̹����� ���� ������Ʈ ���




    //���� ���� ����
    /*
    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;
    */

    List<GameObject> BERRY;

    //=====================================================================================================
    void Start()
    {
        //berryExp = GameObject.Find("berryExplanation");//�̰� ����ȭ ����. �ٸ� ����� ������
        BERRY = Globalvariable.instance.berryListAll;


        //�����յ鿡�� ��ȣ�� �ٿ� ���� 0~96
        prefabnum = Prefabcount;
        Prefabcount++;

        //�������� ���δ�.
        berryImageChange();

    }
    //=====================================================================================================


    public void berryImageChange()//���� ����Ʈ�� �̹����� ���δ�.
    {

        //classic=0~31  special=32~63   unique=64~95
        if (BERRY[prefabnum] == null) {
            berryImagePanel.transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);//���� -> ������
        }
        else
        {
            this.transform.GetComponent<Image>().sprite = yesBerryImage;//��� �̹��� ����
            berryImagePanel.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);//���� -> ������

            berryImagePanel.GetComponent<Image>().sprite = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//�ش� ���� �̹��� ���̱�
        }
    }


    //=====================================================================================================
    /*
    public void Explanation()
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//berryExp
        ExpChildren2 = ExpChildren.transform.GetChild(1).transform.gameObject;//berryExpImage

        try
        {
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
    */






}

