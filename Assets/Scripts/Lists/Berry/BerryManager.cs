using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] berryImg;//���� �̹��� �ҽ���. ��������Ʈ��


    [Header("BERRY EXPLANATION")]//���� ���� ����ü�� �����ұ�..?
    public string[] berryName;
    public string[] berryTxt;

    [SerializeField]
    private GameObject berryImage;//�̹����� ���� ������Ʈ ���


    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;

    //�����յ� ��ȣ �ٿ��ֱ� ��
    static int Prefabcount = 0;
    int prefabnum;



void Start()
    {
        berryExp = GameObject.Find("berryExplanation");


        //�����յ鿡�� ��ȣ�� �ٿ� ����
        if (Prefabcount >= 32)
        {    Prefabcount -= 32;    }
        prefabnum = Prefabcount;
        Prefabcount++;


        berryImageChange();
    }

    void Update()
    {

    }




    public void Explanation()
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//Expchildren�� ���̶�Ű�� berryExplanation�� �ڽ� berryExp�� �ǹ��Ѵ�.

        //������ ������ ����â�� ���.
        if (ExpChildren.activeSelf == true)
        {
            ExpChildren.SetActive(false);//���� ��״� ��������
        }
        else
        {
            ExpChildren.SetActive(true);//����â�� ���.

            ExpChildren2 = ExpChildren.transform.GetChild(0).transform.gameObject;//Expchlidren2�� Expchildren1�� �ڽ��� berryExpImage�� �ǹ�
            ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite = berryImg[prefabnum];
            ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text = berryName[prefabnum];
            ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = berryTxt[prefabnum];

        }



    }

    public void OffExplanation() 
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;
        ExpChildren.SetActive(false);

    }



    public void berryImageChange()
    {
        for (int i = 0; i < berryImg.Length; i++)
        {
            if (prefabnum == i)
                berryImage.GetComponent<Image>().sprite = berryImg[i];
        }
    }


}
