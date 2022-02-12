using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//������
    
    [SerializeField]
    private Transform content = null;//�������� �� ����Ʈ

    [SerializeField]
    private int count=0;//������ ����


    private void Start()
    {
        for (int i = 0; i < count; i++) {
            AddElement();
        }
    }

    public void AddElement() 
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
