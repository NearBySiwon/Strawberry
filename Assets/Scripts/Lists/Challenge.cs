using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;    //����
        public int rewardMedal; //�޴� ����
        public int[] rewardHeart = new int[100]; //��Ʈ ����
        public int[] clearCriterion = new int[100];  //�޼� ����

        public ChallengeNewsStruct(string Title, int rewardMedal, int[] rewardHeart,int[] clearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.clearCriterion = clearCriterion;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    public GameObject levelText;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject achieveCondition; //�������� �޼� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject nowCondition; //���� �������� �޼� ��ġ �ؽ�Ʈ
    [SerializeField]
    private GameObject Button;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge;

    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;


    [Header("==========�г�==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;


    static int Prefabcount = 0; //�߰� �� Prefab ��
    int prefabnum; //�ڽ��� ���° Prefab����


    private int realLevel;//�� ������ ������ ������ �� �����Ŵ�.

    private int[] ChallengeValue = new int[6];

    //=======================================================================================================================
    //=======================================================================================================================
    
    private void Start()
    {
        GameManager.instance.ShowMedalText();//���� �޴��� ���δ�.
        InfoInit();
    }

    private void Update()
    {

        ChallengeValue[0] = DataController.instance.gameData.unlockBerryCnt;
        ChallengeValue[1] = DataController.instance.gameData.totalHarvBerryCnt;
        ChallengeValue[2] = DataController.instance.gameData.accCoin;
        ChallengeValue[3] = DataController.instance.gameData.accHeart;
        ChallengeValue[4] = DataController.instance.gameData.accAttendance;
        ChallengeValue[5] = DataController.instance.gameData.mgPlayCnt;

            
            
        //������ �޾Ҵٸ� �� �����̿��� �Ѵ�.
        //realLevel = ChallengeValue[prefabnum] / 30;
        //if (realLevel %30== 0) { realLevel--; }

        InfoUpdate();
    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6)
        { Prefabcount = 0; }
        prefabnum = Prefabcount;

        Prefabcount++;


        Info[prefabnum].rewardHeart = new int[100];
        Info[prefabnum].clearCriterion = new int[100];

        titleText.GetComponent<Text>().text = Info[prefabnum].Title;//����ǥ��


        //���� ����
        Info[prefabnum].rewardMedal = 1; // 1 ����
        
        for (int i=0; i<100; i++) // 1~100����
        {
            Info[prefabnum].rewardHeart[i] = i * 5; // ����x5 ��Ʈ
        }

        switch (prefabnum)
        {
            case 0: // ���� ����
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 10;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 20;
                }
                break;
            case 1: // ���� ��Ȯ
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 100;
                    else if (i == 1)
                        Info[prefabnum].clearCriterion[i] = 200;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2];
                }
                break;
            case 2: // ���� ����
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 1000;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 3;
                }
                break;
            case 3: // ���� ��Ʈ
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 100;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 3;
                }
                break;
            case 4: // ���� �⼮
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 10;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 20;
                }
                break;
            case 5: // �̴ϰ��� �÷���
                for (int i = 0; i < 100; i++) // 1~100����
                {
                    if (i == 0)
                        Info[prefabnum].clearCriterion[i] = 10;
                    else if (i == 1)
                        Info[prefabnum].clearCriterion[i] = 20;
                    else
                        Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2];
                }
                break;
        }
        Debug.Log(Info[prefabnum].clearCriterion[0]);
    }

    public void InfoUpdate() {
        // titleText.GetComponent<Text>().text = Info[prefabnum].Title + " " + ChallengeValue[prefabnum];//����+���� ��
        levelText.GetComponent<Text>().text ="Lv."+ (1+DataController.instance.gameData.challengeLevel[prefabnum]).ToString();  //����

        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]].ToString();   //�������� ������ �޼� ���� ����

        if (realLevel <= DataController.instance.gameData.challengeLevel[prefabnum])
        {
            //�������� ������ ����
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]]) / Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]];
            //�������� ������ ���簪
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]]).ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]])
            {
                //���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]].ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
            }
        }
        else
        { 
            //���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[DataController.instance.gameData.challengeLevel[prefabnum]].ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
        }

    }
    //==================================================================================================================
    //==================================================================================================================


    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() {

        if (ChallengeValue[prefabnum] / 30 == DataController.instance.gameData.challengeLevel[prefabnum] + 1
            || realLevel > DataController.instance.gameData.challengeLevel[prefabnum])
        {
            AudioManager.instance.Cute1AudioPlay();
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //�޴� ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart[DataController.instance.gameData.challengeLevel[prefabnum]]); //��Ʈ ���� ȹ��

            Button.GetComponent<Image>().sprite = IngButton; //�������� ��ư �̹��� ����

            DataController.instance.gameData.challengeLevel[prefabnum]++; //��������

        }

    }


}
