using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    //gameManager Script
    GameManager gm1;

    //�߰� �� Prefab ��
    static int Prefabcount=0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

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
    int[] ResearchPrice = { 100, 200, 300, 400, 500, 1000, 0 };
    int[] ResearchLevel = { 10, 1, 1, 1, 1, 1, 0 };

   


    void Start()
    {
        gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
    }


    void Update()
    {
       
    }


    //coin ��ư�� ������ ����
    public void clickCoin() {

        //������ �ö󰣴�.
        ResearchLevel[prefabnum]++;
        levelNum.GetComponent<Text>().text = ResearchLevel[prefabnum].ToString();

        //�ش� �ݾ��� ������ ���ҵȴ�.
        gm1.coin -= ResearchPrice[prefabnum];
        gm1.CoinText.text = gm1.coin.ToString() + " A";

    }


    public void InfoUpdate()
    {
        titleText.GetComponent<Text>().text = ResearchName[Prefabcount];
        explanationText.GetComponent<Text>().text = ResearchExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = ResearchPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = ResearchLevel[Prefabcount].ToString();
        prefabnum = Prefabcount;
        Prefabcount++;
        
    }
}
