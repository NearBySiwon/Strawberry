using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Strawberry : MonoBehaviour
{
    //�����յ� ��ȣ �ٿ��ֱ�
    static int Prefabcount = 0;
    int prefabnum;

    //���� ������ ��ƿ� ����Ʈ
    List<GameObject> BERRY;

    [Header("======Strawberry=====")]
    [SerializeField]
    private Sprite yesBerryImage;//���� ���� �� ��� �̹���
    [SerializeField]
    private GameObject berryImagePanel;//�̹����� ���� ������Ʈ ���

    //���� ����â
    private GameObject berryExp;


    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.FindGameObjectWithTag("BerryExplanation");
        berryExp = berryExp.transform.GetChild(0).gameObject;
        //�����յ鿡�� ��ȣ�� �ٿ� ����
        prefabnum = Prefabcount;
        Prefabcount++;

        //���� ���� ��������
        BERRY = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        //�������� ���δ�.
        berryImageChange();

    }
    private void Update()
    {
        //�������� ���δ�.
        berryImageChange();//�̷��ʿ䰡�ֳ�
    }

    //���� ����Ʈ�� �̹����� ���δ�.===========================================================================
    public void berryImageChange()
    { 
        //���� ������ �����ϰ� && ������ unlock �Ǿ��ٸ� 
        if (BERRY[prefabnum] != null && DataController.instance.gameData.isBerryUnlock[prefabnum]==true)
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = yesBerryImage;//��� �̹��� ����
            berryImagePanel.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);//���� -> ������

            berryImagePanel.GetComponent<Image>().sprite 
                = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//�ش� ���� �̹��� ���̱�
        }
    }


    //���� ����â ����========================================================================================
    public void BerryExplanation() {

        //GameManager.instance.Explanation(BERRY[prefabnum],prefabnum);
        try
        {

            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                
                //����â ����
                berryExp.SetActive(true);
                Debug.Log("test");
                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;

                //Explanation ������ ä���.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//�̹��� ����

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//�̸� ����

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//���� ����


            }
        }
        catch
        {
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }

    }

}

