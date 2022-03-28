using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd1 : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//������

    [SerializeField]
    private Transform content1 = null;//�������� �� ����Ʈ
    [SerializeField]
    private Transform content2 = null;
    public Transform content3 = null;
    public Transform content4 = null;




    //===================================================================================
    private void Start()
    {
        for (int t = 0; t < 3; t++)
        {
            for (int i = 0; i < 16; i++)
            {
                AddElement(content1);
            }
            for (int i = 0; i < 16; i++)
            {
                AddElement(content2);
            }
            for (int i = 0; i < 16; i++)
            {
                AddElement(content3);
            }
            for (int i = 0; i < 16; i++)
            {
                AddElement(content4);
            }
        }

    }

    public void AddElement(Transform content)
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
