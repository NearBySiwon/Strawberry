using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class berry_add : MonoBehaviour
{
    [SerializeField]
    private GameObject berryPrefab = null;

    [SerializeField]
    private Transform _content = null;


    private void Start()
    {
        for (int i = 0; i < 32; i++) {
            AddElement();
        }
    }
    public void AddElement()
    {

        var instance = Instantiate(berryPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(_content);//�θ� content�� �Ѵ�. �� ������ ����.


    }
}
