using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeLayer : MonoBehaviour
{
    //��Ʈ�ѹ�
    public GameObject scrollBar;

    [SerializeField]
    private GameObject[] tagButtons;

    //���� ������Ʈ �θ� ������Ʈ
    [SerializeField]
    GameObject parent_content;


    private GameObject[] target_berry;

    void Start()
    {
        selectBerryTag("berry_classic");
    }

    
    void Update()
    {
        
    }

    public void TagImageChange(Sprite selectImage) {
        tagButtons[1].GetComponent<Image>().sprite = selectImage;
    
    }

    //��ư�� ������ �� �ش� �з��� ���⸦ ���δ�.name=tag�̸�
    public void selectBerryTag(string name)
    {

        //���� ������ ���̰� Ȱ��ȭ
        Active(name);
        
        //�ٸ� ������ �Ⱥ��̰� ��Ȱ��ȭ
        switch (name) {
            case "berry_classic": inActive("berry_special"); inActive("berry_unique"); break;
            case "berry_special": inActive("berry_classic"); inActive("berry_unique"); break;
            case "berry_unique": inActive("berry_special"); inActive("berry_classic"); break;
        }


        //horizontal scrollbar ó������ ������
        

    }

    public void inActive(string name) {

        //�ش� �±׸� ���� ������Ʈ�� ã�´�.
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //�� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(false);
        }

    }

    public void Active(string name)
    {
        //��� ���� ������Ʈ Ȱ��ȭ
        int iCount = parent_content.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = parent_content.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
        

        //�ش� ������ ���̰� Ȱ��ȭ
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //�� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(true);
        }

    }


}
