using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBerry : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradePrice;
    [SerializeField]
    private GameObject upgradeTimer;

    
    [Header("=====INFO=====")]
    public int[] upgradePrice_;//���׷��̵忡 �ʿ��� ���� �迭
    public int[] upgradeTime_;//���׷��̵忡 �ʿ��� �ð� �迭


    private int berryUpgradeLevel=0;

    void Start()
    {
        updateInfo(berryUpgradeLevel);
    }


    void Update()
    {
        
    }

    public void newBerryAdd() 
    {
        //Ÿ�̸Ӱ� 0 �̶�� 
        if (upgradeTime_[0] == 0)
        {
            //���ο� ���Ⱑ �߰��ȴ�.
            Debug.Log("���ο� ����!!");

            //�ݾ��� ����������.(������ �ϴ� �ϳ��� ����)
            GameManager.instance.coin -= upgradePrice_[0];
            GameManager.instance.ShowCoinText(GameManager.instance.coin);

            //�������̵� ���� ��� -> �� ���� ���׷��̵� �ݾ��� ���δ�.
            berryUpgradeLevel++;
            updateInfo(berryUpgradeLevel);

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
            upgradePrice.GetComponent<Text>().text = upgradePrice_[index].ToString();
            upgradeTimer.GetComponent<Text>().text = upgradeTime_[index].ToString();
        }
        catch{
            Debug.Log("���� ���� ���� ����");
            //��ư ������ ���ϰ� �ϱ�
        }
    }
}
