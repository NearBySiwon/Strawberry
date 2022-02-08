using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    GameManager gm1;
    //�߰� �� Prefab ��
    static int Prefabcount=0;

    //Research Info  ������ �͵�=====================================================================
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    //Research Info==========================================================================================================================================
    //text���Ϸ� ������ �� TextAsset���� �о�͵� �Ǳ��ϴ�. �ٵ� �ϴ� ���� �����ϱ�
    string[] ResearchName = { "���� ��ġ ���", "���� ����Ⱓ ����", "Ʈ�� ���� ���", "�� �´�!", "���� ����", "���� ����", "����" };
    string[] ResearchExplanation = { "������ ��ġ�� 5%->10% ��������", " ������ ����ð��� 5%->10% �����Ѵ�",
            "Ʈ������ ���⸦ �Ǹ��� �� 5%->10% ���� �������� �Ǹ��Ѵ�.","�ҳ��Ⱑ ���� Ȯ���� 5%->10% �����Ѵ�.",
            "���ʰ� ���� Ȯ���� 5%->10% �����Ѵ�.","������ ���� Ȯ���� 5%->10% �����Ѵ�.","����"};
    int[] ResearchPrice = { 100, 200, 200, 300, 300, 300, 0 };
    int[] ResearchLevel = { 3, 1, 1, 1, 1, 1, 0 };

   


    void Start()
    {
        GameManager gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
        int c = 100;
        gm1.coin = c;
        Debug.Log("coin=" + gm1.coin);
    }


    void Update()
    {
       
    }


    //coin ��ư�� ������ ����
    public void clickCoin() {

        //�� �̰� ����..?
        Prefabcount = 0;
        //������ �ö󰣴�.
        levelNum.GetComponent<Text>().text= ResearchLevel[Prefabcount]++.ToString();
        
        
    }


    public void InfoUpdate()
    {
        titleText.GetComponent<Text>().text = ResearchName[Prefabcount];
        explanationText.GetComponent<Text>().text = ResearchExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = ResearchPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = ResearchLevel[Prefabcount].ToString();
        Prefabcount++;
    }
}
