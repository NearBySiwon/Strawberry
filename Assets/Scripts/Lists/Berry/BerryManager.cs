using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] berryImg;//��� ���� �̹��� �ҽ���. ��������Ʈ��

    [SerializeField]
    private GameObject berryImage;

    GameObject test;




    static int Prefabcount = 0;
    int prefabnum;



    private void Awake()
    {
        Prefabcount = 0;
    }

    void Start()
    {

        test = GameObject.Find("test_special");
        Debug.Log("test=" + test);


        if (Prefabcount >= 32)
            Prefabcount -= 32;

        prefabnum = Prefabcount;
        //Debug.Log(prefabnum);
        Prefabcount++;


        berryImageChange();

    }

    void Update()
    {

    }



    //������ ����â �߰� �ٽ� ������ ������ ��������
    public void Explanation()
    {

        if (test.transform.GetChild(0).transform.gameObject.activeSelf == true)
        {
            test.transform.GetChild(0).transform.gameObject.SetActive(false);
        }
        else
        {
            test.transform.GetChild(0).transform.gameObject.SetActive(true);
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
