using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBerry : MonoBehaviour
{

    [Header("=====OBJECT=====")]
    [SerializeField]
    private GameObject priceText;
    [SerializeField]
    private GameObject timeText;

    [Header("=====INFO=====")]
    public int[] price;//���׷��̵忡 �ʿ��� ���� �迭
    public float[] time;//���׷��̵忡 �ʿ��� �ð� �迭

    private int index=0;//���� �ε���



    //===================================================================================================================
    void Start()
    {
        
    }

    void Update()
    {
        updateInfo(index);

    }


    //update��ư�� ������
    public void newBerryAdd() 
    {
        //Ÿ�̸Ӱ� 0 �̶�� 
        if (time[index] < 0.9)
        {
            //���ο� ���Ⱑ �߰��ȴ�.
            Debug.Log("���ο� ����!!");

            //�ݾ��� ����������.
            GameManager.instance.coin -= price[index];
            GameManager.instance.ShowCoinText(GameManager.instance.coin);

            //�������̵� ���� ��� -> �� ���� ���׷��̵� �ݾ��� ���δ�.
            index++;
            updateInfo(index);

            //Ÿ�̸Ӱ� ���۵ȴ�.(������ �ϴ� 10��)
            //��ư�� ������ ���Ѵ�.

        }
        else 
        {
            Debug.Log("���ο� ���⸦ ���� ���� �� ��ٸ�����");
        
        }
    }

    public void updateInfo(int index) {

        try
        {
            if (time[index] > 0) { time[index] -= Time.deltaTime; }//�ð��� 0���� ũ�� ���ݾ� ���δ�.

            //���� price�� time text�� ���δ�.
            priceText.GetComponent<Text>().text = price[index].ToString();
            //timeText.GetComponent<Text>().text = string.Format("{0:N2}", time);//!!!!!!!!!!string.Foramt�� ����Ѵ�. ���ڿ� ���� {}�� ���� ������ ���� ������� 0, 1, 2�� ����. �߰����� ������ �ʿ��� ��� �ε��� ������ : �� ���� ������ �����Ѵ�. N�� F�� ����� �Ҽ��� �� ��° ������ ǥ���� �� �ִ�. N2�� ���� �Ҽ��� ��° �ڸ����� ǥ���ϰڴٴ� �ǹ�
            timeText.GetComponent<Text>().text = Mathf.Ceil (time[index]).ToString (); //�����κи� ����Ѵ�. CeilTolnt�Լ��� int������ ��ȯ���ֱ⵵ �Ѵ�.
        }
        catch
        {
            Debug.Log("���� ���� ���� ����");
            //��ư ������ ���ϰ� �ϱ�
        }


    }
}
