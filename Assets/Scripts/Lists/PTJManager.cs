using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJManager : MonoBehaviour
{
    //�߰� �� Prefab ��
    static int Prefabcount = 0;

    //Research Info  ������ �͵�
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //PTJ Info
    string[] PTJName = { "�䳢", "��", "����", "�����", "�ܽ���", "������", "����" };
    string[] PTJExplanation = { "���� �ڵ��ɱ�", "���� �ڵ� ��Ȯ", "�ŷ��� ����, Ʈ�� ���� ���", "���� �ּ���, �ҳ��� ������", "���� �ڵ� �̱�", "���� �ڵ� ���̱�", "����" };
    int[] PTJPrice = { 100, 200, 200, 300, 300, 300, 0 };
    int[] PTJLevel = { 3, 1, 1, 1, 1, 1, 0 };




    void Start()
    {
        titleText.GetComponent<Text>().text = PTJName[Prefabcount];
        explanationText.GetComponent<Text>().text = PTJExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = PTJPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = PTJLevel[Prefabcount].ToString();
        Prefabcount++;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
