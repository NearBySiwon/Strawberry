using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //9:16 �����ػ� �����

    void Awake()
    {
        /*
        //Default �ػ� ����
        float FixedAspectRatio = 9f / 16f;

        //���� �ػ��� ����
        float CurrentAspectRatio = (float)Screen.width / (float)Screen.height;

        //���� �ػ� ���� ������ �� �� ���
        if (CurrentAspectRatio > FixedAspectRatio)
        {
            thisCanvas.matchWidthOrHeight = 0;
        }
        //���� �ػ��� ���� ������ �� �� ���
        else if (CurrentAspectRatio < FixedAspectRatio)
        {
            thisCanvas.matchWidthOrHeight = 1;
        }
        */

        /*
        Camera camera = GetComponent<Camera>(); //ī�޶� ������Ʈ ������
        Rect rect = camera.rect; //ī�޶� Viewport rect �κ� ������

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // ����, ���� �����ϰ� ���� ����
        float scalewidth = 1f / scaleheight; //���� ���� 1�� �������� ��

        if (scaleheight < 1) //10:16�̸� 1���� ũ�� 8:16�̸� 1���� �۴�.
        {
            //1���� ������ 8:16 �� �Ʒ� ������ ���´�. y�� ���� ����
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            //�ݴ�� 1���� ũ�� 9:16. x �� ����
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect; //������ ����
    }
        */
    }
}
