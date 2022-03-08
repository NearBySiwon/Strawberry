using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Berry : MonoBehaviour
{         
    //���⿡ public���� ���� ���� ����.
    public string berryName;
    public int berrykindProb;
    
  
    void Awake()
    {         
    }
    private void OnEnable()
    {
            
    }
    private void OnDisable()
    {     
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;        
    }  
    public void Explosion(Vector2 from, Vector2 to, float exploRange) // DOTWeen ȿ��
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * exploRange, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { Destroy(gameObject); });
    }   
}
