using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//������
    
    [SerializeField]
    private Transform content1 = null;//�������� �� ����Ʈ
    [SerializeField]
    private Transform content2 = null;

    [SerializeField]
    private int count=0;//������ ����

    [SerializeField]
    private bool isBerry;//���� ����Ʈ�ΰ�?

    private void Start()
    {
        if (isBerry == true)
        {
            for (int i = 0; i < count/2; i++)
            {
                AddElement(content1);
                
            }
            for (int i = 0; i < count / 2; i++)
            {
                AddElement(content2);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                AddElement(content1);
            }
        }
    }

    public void AddElement(Transform content) 
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
