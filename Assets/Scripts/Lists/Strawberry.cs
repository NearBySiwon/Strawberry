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
    [SerializeField]
    private GameObject ExclamationMark;


    //���� ����â
    private GameObject berryExp;

    //=====================================================================================================
    //=====================================================================================================

    private void Awake()
    {
        //�����յ鿡�� ��ȣ�� �ٿ� ����
        prefabnum = Prefabcount;
        Prefabcount++;
    }
    void Start()
    {
        berryExp = GameObject.FindGameObjectWithTag("BerryExplanation");
        berryExp = berryExp.transform.GetChild(0).gameObject;

        //���� ���� ��������
        BERRY = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        //�������� ���δ�.
        berryImageChange();

    }
    private void Update()
    {
        //�������� ���δ�.
        berryImageChange();
        //�̰� ���ֱ�
        //gameManager���� newBerry�߰��ɶ����� �����ϴ� ����� �ִ�.
    }

    //=====================================================================================================
    //=====================================================================================================
    //���� ����Ʈ�� �̹����� ���δ�
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

        //���� ���� �ѹ��� Ȯ������ �ʾҴٸ� ����ǥ ǥ��. �̹� �� �� ������ ���ֱ�
        if (DataController.instance.gameData.isBerryEM[prefabnum] == true)
        { ExclamationMark.SetActive(true); }
        else { ExclamationMark.SetActive(false); }

    }


    //���� ����â ����
    public void BerryExplanation() {

        try
        {

            if (DataController.instance.gameData.isBerryEM[prefabnum] == true) 
            { DataController.instance.gameData.isBerryEM[prefabnum] = false; }

            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                
                //����â ����
                berryExp.SetActive(true);
                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;
                GameObject berryExpPrice= berryExp.transform.GetChild(5).gameObject;

                //Explanation ������ ä���.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//�̹��� ����

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//�̸� ����

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//���� ����

                berryExpPrice.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryPrice.ToString()+"A";//���� ����


            }
        }
        catch
        {
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }

    }

}

