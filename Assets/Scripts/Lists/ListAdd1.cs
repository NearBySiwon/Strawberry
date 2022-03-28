using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd1 : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//������

    [SerializeField]
    private Transform[] content = null;//�������� �� ����Ʈ




    //===================================================================================
    private void Start()
    {
        for (int t = 0; t < 3; t++)//classic, special unique 3��
        {
            for (int j = 0; j < content.Length; j++)//layer ������ŭ
            {
                for (int i = 0; i < 16; i++)//content ������ŭ
                {
                    AddElement(content[j]);
                }
            }

        }

    }

    public void AddElement(Transform content)
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
