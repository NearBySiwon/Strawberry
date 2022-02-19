using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChangeLayer : MonoBehaviour
{

    //classic,special,unique,all
    public Button[] btns;

    //���� ������Ʈ���� �θ� ������Ʈ
    [SerializeField]
    GameObject parent_content;

    //Ÿ�� �������� �����ϴ� �迭
    private GameObject[] target_berry;

    String nowTap;

    void Start()
    {
        //�ǹ�ư �νı�� ����
        makeBtn();
    }

    void Update()
    {
        //���� ���� �������� �ν��ϰ� �ش� �±��� �������� ���δ�
        tap_changed();
    }

    //======================================================================================================================
    


    //��ư
    private void makeBtn() {
        for (int i = 0; i < this.btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => this.TaskOnClick(index));
        }
        
    }

    //��ư Ŭ���� 
    private void TaskOnClick(int index)
    {
        //���� ��ư�� �θ��� ������ �� ������
        btns[index].transform.parent.SetAsLastSibling();
        
        //���� �� �̸� ������Ʈ
        switch (index)
        {
            case 3: nowTap = "all"; break;
            case 0: nowTap = "classic"; break;
            case 1: nowTap = "special"; break;
            case 2: nowTap = "unique"; break;
        }

        //===========�߰� ����===============
        //���� ó�� ���������� ���� ����    Transform.SetAsLastSibling  Transform.SetAsFirstSibling
        //���� ����(index ��)    Transform.SetSiblingIndex   ���� ���� ��ȯ(index ��)    Transform.GetSiblingIndex

    }


    private void tap_changed()
    {
        //���� �ǿ� ���� ������Ʈ�� Ȱ��ȭ ��Ȱ��ȭ ��Ű��
        //�밡��;;;; 
        switch (nowTap)
        {
            case "classic":
                
                find_tag_active("berry_classic");
                find_tag_inactive("berry_special"); find_tag_inactive("berry_unique");
                break;
            case "special":
                
                find_tag_active("berry_special");
                find_tag_inactive("berry_classic"); find_tag_inactive("berry_unique");
                break;
            case "unique":
               
                find_tag_active("berry_unique");
                find_tag_inactive("berry_classic"); find_tag_inactive("berry_special");
                break;

        }

    }



    //�ش� �±��� ������Ʈ�� ã�Ƽ� Ȱ��ȭ��Ų��
    private void find_tag_active(string tag_name) {

        //��� ���� ������Ʈ Ȱ��ȭ�Ѵ�
        int iCount = parent_content.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = parent_content.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }

        //�ش� �±׸� ���� ������Ʈ�� ã�´�.
        target_berry = GameObject.FindGameObjectsWithTag(tag_name);
            
        //�� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++) {
            target_berry[i].SetActive(true);
        }

    }
   
    //�ش��±��� ������Ʈ�� ã�Ƽ� ��Ȱ��ȭ ��Ų��.
    private void find_tag_inactive(string tag_name) {

        //�ش� �±׸� ���� ������Ʈ�� ã�´�.
        target_berry = GameObject.FindGameObjectsWithTag(tag_name);

        //�� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(false);
        }
    }


    


}
