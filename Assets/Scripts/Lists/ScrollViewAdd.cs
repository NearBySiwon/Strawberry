using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject _elementPrefab = null;

    [SerializeField]
    private Transform _content = null;

    int count=0;

    public void AddElement() {

        var instance=Instantiate(_elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(_content);//�θ� content�� �Ѵ�. �� ������ ����.
        
        count++;
    }
}
