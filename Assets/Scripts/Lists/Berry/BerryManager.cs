using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject berryExp;//berryExplanation

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    void Start()
    {
        prefabnum = Prefabcount;
        Prefabcount++;
    }


        void Update()
    {
        
    }


    //������ ������ �ٽ� ������ ������
    public void ActivateExplanation()
    {
        if (berryExp.activeSelf == false)
        {
            berryExp.SetActive(true);
            berryExp.SetActive(true);
        }
        else
        {
            berryExp.SetActive(false);
            berryExp.SetActive(false);
        }

    }


}
