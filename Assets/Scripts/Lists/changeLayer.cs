using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeLayer : MonoBehaviour
{
    public GameObject[] ScrollViews;
    GameObject selectScrollView;
    GameObject selectTag;

    public Sprite selectTagImage;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //��ư�� ������ �� �ش� �з��� ���⸦ ���δ�.
    public void selectBerryTag(int index)
    {


        //�ٸ� ��ũ�Ѻ� ��Ȱ��ȭ
        for (int i = 0; i < ScrollViews.Length; i++) {
            ScrollViews[i].SetActive(false);
        }
        //�ش� ��ũ�Ѻ� Ȱ��ȭ
        ScrollViews[index].SetActive(true);

        //horizontal scrollbar ó������ ������


        //�ٸ� ��ư ��������Ʈ �������� ����
        //�ش� ��ư ��������Ʈ �������� ����

    }
}
