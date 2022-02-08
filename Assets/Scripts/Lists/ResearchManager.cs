using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    //�߰� �� Prefab ��
    static int Prefabcount=0;

    //Research Info  ������ �͵�
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //Research Info
    //text���Ϸ� ������ �� TextAsset���� �о�͵� �Ǳ��ϴ�. �ٵ� �ϴ� ���� �����ϱ�
    string[] ResearchName = { "���� ��ġ ���", "���� ����Ⱓ ����", "Ʈ�� ���� ���", "�� �´�!", "���� ����", "���� ����", "����" };
    string[] ResearchExplanation = { "������ ��ġ�� 5%->10% ��������", " ������ ����ð��� 5%->10% �����Ѵ�",
            "Ʈ������ ���⸦ �Ǹ��� �� 5%->10% ���� �������� �Ǹ��Ѵ�.","�ҳ��Ⱑ ���� Ȯ���� 5%->10% �����Ѵ�.",
            "���ʰ� ���� Ȯ���� 5%->10% �����Ѵ�.","������ ���� Ȯ���� 5%->10% �����Ѵ�.","����"};
    int[] ResearchPrice = { 100, 200, 200, 300, 300, 300, 0 };
    int[] ResearchLevel = { 3, 1, 1, 1, 1, 1, 0 };

    


    void Start()
    {
        
        titleText.GetComponent<Text>().text = ResearchName[Prefabcount];
        explanationText.GetComponent<Text>().text = ResearchExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = ResearchPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = ResearchLevel[Prefabcount].ToString();
        Prefabcount++;

    }

    
    void Update()
    {
        //ListAdd���� count���� ���� �޾ƿ´�.
        //count�������� ���� ���� ����Ʈ ���� ����

        //count=0�̸� level �迭 0��° ������ ����Ʈ level text�� �ִ´�
        
    }
}
