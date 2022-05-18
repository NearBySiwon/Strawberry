using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    [Serializable]
    public class NewsStruct
    {
        public string Title;//����
        public string Exp;//���� ����
        public int Price;
        public NewsStruct(string Title, string Exp,int Price)
        {
            this.Title = Title;
            this.Exp = Exp;
            this.Price = Price;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    NewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject TitleText;
    [SerializeField]
    private GameObject CountText;
    [SerializeField]
    private GameObject Lock;//���� ���
    [SerializeField]
    private GameObject Unlockable;//���� ��� ���� ����


    [Header("==========�г�==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;



    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    GameObject newsExp;
    GameObject newsContent;
    GameObject YNPanel;
    //=======================================================================================================================
    //=======================================================================================================================
    private void Start()
    {
        newsExp = GameObject.FindGameObjectWithTag("NewsExplanation").transform.GetChild(0).gameObject;
        newsContent = GameObject.FindGameObjectWithTag("NewsContent");
        YNPanel= GameObject.FindGameObjectWithTag("YNPanel");
        //�޴�
        GameManager.instance.ShowMedalText();

        //�����յ鿡�� ���� ��ȣ ���̱�
        prefabnum = Prefabcount;
        Prefabcount++;

        //����
        TitleText.GetComponent<Text>().text = Info[prefabnum].Title;
        //���� ����
        CountText.GetComponent<Text>().text = "0" + (prefabnum + 1);

        //���� ���� ������Ʈ
        InfoUpdate();
    }
    //==================================================================================================================
    //==================================================================================================================

    public void InfoUpdate()
    {
        //���� ���¿� ���� lock, unlockable,unlock���� ���̱� 
        switch (DataController.instance.gameData.newsState[prefabnum]) 
        {
            case 0://LOCK
                Lock.SetActive(true);
                Unlockable.SetActive(false);
                break;
            case 1://UNLOCK ABLE
                Lock.SetActive(false);
                Unlockable.SetActive(true);
                break;
            case 2://UNLOCK
                Lock.SetActive(false);
                Unlockable.SetActive(false);
                break;
        }

    }


    public void NewsButton()
    {
        switch (DataController.instance.gameData.newsState[prefabnum])
        {
            case 0://LOCK
                //lock���´� ���� �� ����.
                break;
            case 1://UNLOCK ABLE
                NewsUnlock();

                break;
            case 2://UNLOCK
                //���� â�� ����.
                Explantion();
                break;
        }

        InfoUpdate();
    }

    //���� �ر�?
    public void NewsUnlock() 
    {
        if (DataController.instance.gameData.medal >= Info[prefabnum].Price)
        {
            GameManager.instance.UseMedal(Info[prefabnum].Price);//�޴� �Һ�
            
            //unlock���·� ����
            Unlockable.SetActive(false);
            DataController.instance.gameData.newsState[prefabnum] = 2;


            //���� ���� ���� �ִٸ�
            if (prefabnum + 1 != newsContent.transform.GetChildCount())
            {
                DataController.instance.gameData.newsState[prefabnum + 1] = 1;//������ ��� ���� �����ϰ�

                newsContent.transform.GetChild(prefabnum + 1).//������ ã�Ƽ�
                    transform.GetChild(4).gameObject.SetActive(true);//������� �̹����� ����
                newsContent.transform.GetChild(prefabnum + 1).
                    transform.GetChild(3).gameObject.SetActive(false);//����̹��� �����
            }
            //���� ȹ��??
            int RandomNum = UnityEngine.Random.Range(0, 100);
            if (RandomNum <= Info[prefabnum].Price * 10) //price*10%Ȯ����
            {
                //���� ȹ��
                GameManager.instance.newsBerry();
            }


            //�ȳ�â
            /*
            BP.SetActive(true);
            confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ �رݵǾ����!";
            confirmPanel.GetComponent<PanelAnimation>().OpenScale();
            */

        }
        else
        {
            /*
            //�޴��� ������ ��
            warnningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ �����ؿ�!";
            BP.SetActive(true);
            warnningPanel.GetComponent<PanelAnimation>().OpenScale();
            */
        }

    }

    //���� ����â
    private void Explantion()
    {
        //����â�� ����.
        newsExp.SetActive(true);
        //������ ä���.
        newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text
            = Info[prefabnum].Title;//����
        newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text
            = Info[prefabnum].Exp;//����

    }

}
