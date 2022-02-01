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

    private void Start()
    {
        for (int i = 0; i < 7; i++) {
            AddElement();
        }
    }


    public void AddElement() {

        var instance=Instantiate(_elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(_content);//�θ� content�� �Ѵ�. �� ������ ����.
        
        
    }
}
