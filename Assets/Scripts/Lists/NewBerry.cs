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
    [SerializeField]
    private GameObject startBtn;

    [Header("=====INFO=====")]
    public int[] price;//���׷��̵忡 �ʿ��� ���� �迭
    public float[] time;//���׷��̵忡 �ʿ��� �ð� �迭

    [Header("=====SPRITE=====")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;


    private int index=0;//���� �ε���
    private bool isStart = false;//������ �����°�


    //===================================================================================================================
    void Start()
    {
    }

    void Update()
    {
        updateInfo(index);
        Debug.Log("isStart="+isStart);
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

            //���۹�ư���� ����
            startBtn.GetComponent<Image>().sprite = startImg;
            isStart = false;
        }
        else 
        {
            Debug.Log("���ο� ���⸦ ���� ���� �� ��ٸ�����");
            isStart = true;
        }
    }

    public void updateInfo(int index) {

        try
        {
            if (isStart == true)
            {
                if (time[index] > 0) //�ð��� 0���� ũ�� 1�ʾ� ����
                { 
                    time[index] -= Time.deltaTime; 
                    startBtn.GetComponent<Image>().sprite = ingImg; 
                }
                else 
                { startBtn.GetComponent<Image>().sprite = doneImg; }

                
            }
            //���� price�� time text�� ���δ�.
            priceText.GetComponent<Text>().text = price[index].ToString();
            timeText.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time[index])); //�����κи� ����Ѵ�.

        }
        catch
        {
            Debug.Log("���� ���� ���� ����");
            //��ư ������ ���ϰ� �ϱ�
        }


    }






    public string TimeForm(int time)
    {
        int M=0, S=0;//M,S ����
        string Minutes, Seconds;//M,S �ؽ�Ʈ �����

        M = (time / 60); 
        S = (time % 60);


        //M,S����
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S�� 10�̸��̸� 01, 02... ������ ǥ��
        if (M < 10 && M>0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S>0) { Seconds = "0" + S.ToString(); }

        //M,S�� 0�̸� 00���� ǥ���Ѵ�.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
}
