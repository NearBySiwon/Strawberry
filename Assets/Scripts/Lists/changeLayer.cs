using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeLayer : MonoBehaviour
{

    [Header("====Berry List Buttons====")]
    [SerializeField]
    private GameObject[] tagButtons;
    public Sprite[] tagButtons_Image;
    public Sprite[] tagButtons_selectImage;

    [Header("====ScrollView====")]
    //���� ������Ʈ �θ� ������Ʈ
    [SerializeField]
    private GameObject content1;
    [SerializeField]
    private GameObject content2;

    //��Ʈ�ѹ�
    [SerializeField]
    private GameObject scrollBar;


    private GameObject[] target_berry;

    void Start()
    {
        //ó������ berry classic
        selectBerryTag("berry_classic");
    }

    
    void Update()
    {
        
    }

    public void TagImageChange(int index) {

        //��ư ��������Ʈ �� �ȴ����ŷ�
        for (int i = 0; i < tagButtons_Image.Length; i++) {
            tagButtons[i].GetComponent<Image>().sprite = tagButtons_Image[i];
        }

        //�ش� ��ư ��������Ʈ�� �����ŷ�
        tagButtons[index].GetComponent<Image>().sprite = tagButtons_selectImage[index];
    
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
        scrollBar.transform.GetComponent<Scrollbar>().value = 0;

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
        int iCount = content1.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = content1.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
        int iCount2 = content2.transform.childCount;
        for (int i = 0; i < iCount2; i++)
        {
            Transform trChild2 = content2.transform.GetChild(i);
            trChild2.gameObject.SetActive(true);
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
