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
        public string Title;//����
        public int rewardMedal;//�޴� ����
        public int rewardHeart;//��Ʈ ����
        public int clearCriterion;//�޼� ����

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart,int clearCriterion)
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
    private GameObject achieveCondition;//�������� �޼� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject nowCondition;//���� �������� �޼� ��ġ �ؽ�Ʈ
    [SerializeField]
    private GameObject Button;
    public GameObject bangMark;

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


    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;


    private int realLevel;//�� ������ ������ ������ �� �����Ŵ�.

    private int[] ChallengeValue = new int[6];
    private int[] ChallengeValue_ = new int[6];
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


        //0~29,0
        ChallengeValue_[prefabnum] = ChallengeValue[prefabnum] % 30;
            
            
        //������ �޾Ҵٸ� �� �����̿��� �Ѵ�.
        realLevel = ChallengeValue[prefabnum] / 30;
        if (realLevel %30== 0) { realLevel--; }

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

        titleText.GetComponent<Text>().text = Info[prefabnum].Title;//����ǥ��



        //���� ����
        Info[prefabnum].rewardMedal = 1;
        Info[prefabnum].clearCriterion = 30;// �ϴ� 30���� ����
        //�ӽ�
        switch (prefabnum) 
        {
            case 0: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion; break;
            case 1: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/5; break;
            case 2: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/50; break;
            case 3: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/30; break;
            case 4: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/6; break;
            case 5: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/10; break;
        }

    }

    public void InfoUpdate() {

        titleText.GetComponent<Text>().text = Info[prefabnum].Title + " " + ChallengeValue[prefabnum];//����+���� ��
        levelText.GetComponent<Text>().text ="Lv."+ (1+DataController.instance.gameData.challengeLevel[prefabnum]).ToString();//����
                                                                                                                                //�������� ������ �޼� ���� ����
        achieveCondition.GetComponent<Text>().text
            = "/" + Info[prefabnum].clearCriterion.ToString();

        if (realLevel <= DataController.instance.gameData.challengeLevel[prefabnum])
        {
            //�������� ������ ����
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion) / Info[prefabnum].clearCriterion;
            //�������� ������ ���簪
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion).ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] / 30 == DataController.instance.gameData.challengeLevel[prefabnum] + 1)
            {
                //���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = "30";

                Button.GetComponent<Image>().sprite = DoneButton;//�������� ��ư �̹��� ����
                bangMark.SetActive(true);//����ǥ!! ���� (ȹ�� ������ �������� �ִ�)
            }
        }
        else
        { 
            //���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = "30";

            Button.GetComponent<Image>().sprite = DoneButton;//�������� ��ư �̹��� ����
            bangMark.SetActive(true);//����ǥ!! ���� (ȹ�� ������ �������� �ִ�)
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
            //�޴� ���� ȹ��
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            //��Ʈ ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);

            Button.GetComponent<Image>().sprite = IngButton;//�������� ��ư �̹��� ����
            bangMark.SetActive(false);//����ǥ �����

            DataController.instance.gameData.challengeLevel[prefabnum]++;//��������

        }

    }


}
