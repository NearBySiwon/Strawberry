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

    GameObject berryExp;


    static int Prefabcount = 0;
    int prefabnum;



    void Start()
    {

        berryExp = GameObject.Find("berryExplanation");



        //�����յ鿡�� ��ȣ�� �ٿ� ����
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

        if (berryExp.transform.GetChild(0).transform.gameObject.activeSelf == true)
        {
            berryExp.transform.GetChild(0).transform.gameObject.SetActive(false);
        }
        else
        {
            berryExp.transform.GetChild(0).transform.gameObject.SetActive(true);
        }

    }

    public void OffExplanation() 
    { 
    

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
