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


    //==========prefab num===========
    static int Prefabcount = 0; //�߰� �� Prefab ��
    int prefabnum; //�ڽ��� ���° Prefab����


    //==========�������� ��==========
    private int LevelReal;//�� ������ ������ ������ �� �����Ŵ�.
    private int LevelNow;//���� �� ������ �ִ�.

    private int[] ChallengeValue = new int[6];//������ ��

    private int ValueNow;//�̹� ���������� ��(0���� ���ŵ�)
    private int ValueReal;
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

        LevelNow= DataController.instance.gameData.challengeLevel[prefabnum];//�̰Ŵ� ���� ������

        //������ ����� �� �޾Ҵٸ� �� �����̿��� �Ѵ�.
        //LevelReal = ValueReal[prefabnum] / Info[prefabnum].clearCriterion;
        //if (LevelReal % Info[prefabnum].clearCriterion == 0) { LevelReal--; }

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

    }


    private void ChallengeSet() 
    {
        //Level Real���� ȹ��
        //acc Clear Criterion���� ȹ��

        //Clear Criterion����
        //Value Now ����
        //Value Real ����


        //���� ���� ����
        Info[prefabnum].accClearCriterion = 0;
        Info[prefabnum].clearCriterion = new int[100];

        switch (prefabnum)
        {
            case 0: // ���� ����
            case 4: // ���� �⼮
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 20 * i;


                    
                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion���� ȹ��
                        LevelReal = i;//Level Real���� ȹ��
                    }
                }

                break;

            case 1: // ���� ��Ȯ
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion ����
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 100; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 200; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion���� ȹ��
                        LevelReal = i;//Level Real���� ȹ��
                    }
                }
                break;


            case 2: // ���� ����
                Info[prefabnum].clearCriterion[0] = 1000;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, LevelNow);



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion���� ȹ��
                        LevelReal = i;//Level Real���� ȹ��
                    }
                }
                break;


            case 3: // ���� ��Ʈ
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, LevelNow);



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion���� ȹ��
                        LevelReal = i;//Level Real���� ȹ��
                    }
                }
                break;


            case 5: //�̴ϰ��� �÷���
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion ����
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 10; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 20; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion���� ȹ��
                        LevelReal = i;//Level Real���� ȹ��
                    }
                }
                break;

        }


        if (LevelNow < LevelReal)//���� ���� �� ���� ���� �����̸�
        {
            ValueNow = Info[prefabnum].clearCriterion[LevelNow];//���� ������ �ִ� �޼� ���� ValueNow.
        }
        else //���� �ִ�� ���� �����̸�
        {
            ValueNow = ChallengeValue[prefabnum] - Info[prefabnum].accClearCriterion;
        }



    }
    public void InfoUpdate() {

        ChallengeSet();

        //text ����=========== update���� ���ͤ�
        levelText.GetComponent<Text>().text ="Lv."+ LevelNow.ToString();  //����
        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[LevelNow].ToString();   //�������� ������ �޼� ���� ����

        //������===============
        if (LevelReal > LevelNow || ValueNow == Info[prefabnum].clearCriterion[LevelNow])
            //���� ���� �� ������  ||  �������� �޼�
        {
            //�������� ������ == ���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            //�������� ������ ���簪 == �ִ�
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString(); ;
            //�������� ��ư �̹��� == Done
            Button.GetComponent<Image>().sprite = DoneButton;


        }
        else //���� �ִ��� �� ����
        {
            //�������� ������ == ValueNow ��ŭ ����
            Gauge.GetComponent<Image>().fillAmount = (float)(ValueNow) / Info[prefabnum].clearCriterion[LevelNow];
            //�������� ������ ���簪 == ValueNow
            nowCondition.GetComponent<Text>().text = ValueNow.ToString();

        }

        /*
        if (LevelReal <= LevelNow)
        {
            //�������� ������ == ����
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[LevelNow]) / Info[prefabnum].clearCriterion[LevelNow];
            //�������� ������ ���簪 == ValueNow
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[LevelNow]).ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion[LevelNow])
            {
                //���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� Done
            }
        }
        else
        { 
            //���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� Done
        }
        */
    }
    //==================================================================================================================
    //==================================================================================================================


    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() 
    {

        //�������� �޼��ߴ��� Ȯ��
        if (LevelReal > LevelNow || ValueNow == Info[prefabnum].clearCriterion[LevelNow])
            //���� ���� �� ������  ||  �������� �޼�
        {
            AudioManager.instance.RewardAudioPlay();
            heartMover.GetComponent<HeartMover>().HeartChMover(120);
            medalMover.GetComponent<HeartMover>().BadgeMover(120);
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //�޴� ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //��Ʈ ���� ȹ��

            if (LevelNow < 100)
            {
                if (LevelReal == LevelNow) { LevelNow++; }//���� ���� �ִ�� ���� �����̸� �� ������ ������
                Button.GetComponent<Image>().sprite = IngButton; //�������� ��ư �̹��� ����
                DataController.instance.gameData.challengeLevel[prefabnum]++; //LevelReal���� == ��������
                
            }
        }
    }



}
