using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class berry_add : MonoBehaviour
{
    [SerializeField]
    private GameObject berryPrefab = null;

    [SerializeField]
    private Transform _content = null;

    public int count = 0;

    public void AddElement()
    {

        var instance = Instantiate(berryPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(_content);//�θ� content�� �Ѵ�. �� ������ ����.

        count++;
    }
}
