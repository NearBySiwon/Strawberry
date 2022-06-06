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
        public int rewardHeart; //��Ʈ ����
        public int[] clearCriterion=new int[100];  //�޼� ����
        public int accClearCriterion;

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart,int[] clearCriterion,int accClearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.clearCriterion = clearCriterion;
            this.accClearCriterion = accClearCriterion;
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

    [Header("==========Animation==========")]
    public GameObject heartMover;
    public GameObject medalMover;


    static int Prefabcount = 0; //�߰� �� Prefab ��
    int prefabnum; //�ڽ��� ���° Prefab����


    private int realLevel;//�� ������ ������ ������ �� �����Ŵ�.

    private int[] ChallengeValue = new int[6];//������ ��

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

            
            
        //������ ����� �� �޾Ҵٸ� �� �����̿��� �Ѵ�.
        //realLevel = ChallengeValue[prefabnum] / Info[prefabnum].clearCriterion;
        //if (realLevel % Info[prefabnum].clearCriterion == 0) { realLevel--; }

        InfoUpdate();
    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() 
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6){ Prefabcount = 0; }
        prefabnum = Prefabcount;
        Prefabcount++;


        //����ǥ��
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;


        //���� ����
        Info[prefabnum].rewardMedal = 1; // 1 ����
        Info[prefabnum].rewardHeart = DataController.instance.gameData.challengeLevel[prefabnum] * 5;//����X5 ��Ʈ


        //���� ���� ����
        Info[prefabnum].accClearCriterion = 0;
        Info[prefabnum].clearCriterion = new int[100];

        int level = DataController.instance.gameData.challengeLevel[prefabnum];
        switch (prefabnum)
        {
            case 0: // ���� ����
            case 4: // ���� �⼮
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 0; i < 100; i++)
                {
                    
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 20 * i;

                    if (ChallengeValue[prefabnum]> 
                        Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    { 
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];
                        realLevel = i;
                    }
                    
                }
                break;

            case 1: // ���� ��Ȯ
                for (int i = 0; i < 100; i++)
                {
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 100; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 200; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }
                    Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//���� ���� ����
                }
                break;


            case 2: // ���� ����
                Info[prefabnum].clearCriterion[0] = 1000;
                for (int i = 0; i < 100; i++)
                { 
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, level);
                    Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//���� ���� ����
                }
                break;


            case 3: // ���� ��Ʈ
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 0; i < 100; i++)
                { 
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, level);
                    Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//���� ���� ����
                }
                break;


            case 5: //�̴ϰ��� �÷���
                for (int i = 0; i < 100; i++)
                {
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 10; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 20; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }
                    Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//���� ���� ����
                }
                break;

        }

    }

    public void InfoUpdate() {
        int level = DataController.instance.gameData.challengeLevel[prefabnum];

        //realLevel===========
   



        //text ����===========
        levelText.GetComponent<Text>().text ="Lv."+ level.ToString();  //����
        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[level].ToString();   //�������� ������ �޼� ���� ����

        //������===============
        if (realLevel <= level)
        {
            //�������� ������ ����
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[level]) / Info[prefabnum].clearCriterion[level];
            //�������� ������ ���簪
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[level]).ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion[level])
            {
                //���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[level].ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
            }
        }
        else
        { 
            //���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[level].ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
        }

    }
    //==================================================================================================================
    //==================================================================================================================


    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() {

        int level = DataController.instance.gameData.challengeLevel[prefabnum];

        //�������� �޼��ߴ��� Ȯ��
        if (ChallengeValue[prefabnum] / Info[prefabnum].clearCriterion[level] == level + 1 || realLevel > level)
        {
            AudioManager.instance.RewardAudioPlay();
            heartMover.GetComponent<HeartMover>().HeartChMover(120);
            medalMover.GetComponent<HeartMover>().BadgeMover(120);
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //�޴� ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //��Ʈ ���� ȹ��

            if (level < 100)
            {
                Button.GetComponent<Image>().sprite = IngButton; //�������� ��ư �̹��� ����
                DataController.instance.gameData.challengeLevel[prefabnum]++; //��������
            }

        }

    }


}
