using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text, Image ����UI���� ���� ���� ����Ҽ� �ְԵ˴ϴ�

public class Attandance : MonoBehaviour
{
   
    public Image TestImage; //������ �����ϴ� �̹���
    public Sprite TestSprite; //�ٲ���� �̹���

    public void ChangeImage()
    {
        TestImage.sprite = TestSprite; //TestImage�� SourceImage�� TestSprite�� �����ϴ� �̹����� �ٲپ��ݴϴ�
    }
    

}

