using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject berryExpPanel;//berryExplanation �� ȭ�鿡 �� ��
    
    [SerializeField]
    private Sprite[] berryImg;//��� ���� �̹����� ��������Ʈ��

    [SerializeField]
    private GameObject berryImage;

    [SerializeField]
    private int startCount;


    Image nowBerryImg;


    static int Prefabcount = 0;
    int prefabnum;



    private void Awake()
    {
        Prefabcount = 0;
    }

    void Start()
    {
        nowBerryImg = berryImage.GetComponent<Image>();

        if (Prefabcount >= 32)
            Prefabcount -= 32;

        prefabnum = Prefabcount;
        Debug.Log(prefabnum);
        Prefabcount++;

        
        berryImageChange();

    }

    void Update()
    {

    }



    //������ ����â �߰� �ٽ� ������ ������ ��������
    public void Explanation() {

        if (berryExpPanel.activeSelf == false)
        {
            berryExpPanel.SetActive(true);
        }
        else
        {
            berryExpPanel.SetActive(false);
        }
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
