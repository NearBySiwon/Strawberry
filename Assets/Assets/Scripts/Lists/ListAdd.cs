using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    public GameObject elementPrefab = null;//������
    

    [SerializeField]
    private Transform content = null;//�������� �� ����Ʈ

    public int count = 0;//���� ������ ���� �˱�����


    public static ListAdd insatnce;
    public void Awake()
    {
        insatnce = this;
    }

    private void Start()
    {
        for (int i = 0; i < 7; i++) {
            AddElement();
        }
    }

    public void AddElement() 
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
