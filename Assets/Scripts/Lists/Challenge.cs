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
        public int clearCriterion;  //�޼� ����

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
        if (Prefabcount >= 6){ Prefabcount = 0; }
        prefabnum = Prefabcount;
        Prefabcount++;

        //����ǥ��
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;

        //���� ����
        Info[prefabnum].rewardMedal = 1; // 1 ����
        Info[prefabnum].rewardHeart = DataController.instance.gameData.challengeLevel[prefabnum] * 5;//����X5 ��Ʈ


        //���� ���� ����
        switch (prefabnum)
        {
            case 0: // ���� ����
                

                break;
            case 1: // ���� ��Ȯ
                
                break;
            case 2: // ���� ����
                
                break;
            case 3: // ���� ��Ʈ
                
                break;
            case 4: // ���� �⼮
                
                break;
            case 5: // �̴ϰ��� �÷���
                
                break;
        }
    }

    public void InfoUpdate() {
        // titleText.GetComponent<Text>().text = Info[prefabnum].Title + " " + ChallengeValue[prefabnum];//����+���� ��
        levelText.GetComponent<Text>().text ="Lv."+ DataController.instance.gameData.challengeLevel[prefabnum].ToString();  //����

        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion.ToString();   //�������� ������ �޼� ���� ����

        if (realLevel <= DataController.instance.gameData.challengeLevel[prefabnum])
        {
            //�������� ������ ����
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion) / Info[prefabnum].clearCriterion;
            //�������� ������ ���簪
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion).ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion)
            {
                //���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion.ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
            }
        }
        else
        { 
            //���� �� ���·�
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion.ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //�������� ��ư �̹��� ����
        }

    }
    //==================================================================================================================
    //==================================================================================================================


    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() {

        if (ChallengeValue[prefabnum] / Info[prefabnum].clearCriterion == 
            DataController.instance.gameData.challengeLevel[prefabnum] + 1
            || realLevel > DataController.instance.gameData.challengeLevel[prefabnum])
        {
            AudioManager.instance.RewardAudioPlay();
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //�޴� ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //��Ʈ ���� ȹ��

            if (DataController.instance.gameData.challengeLevel[prefabnum] < 100)
            {
                Button.GetComponent<Image>().sprite = IngButton; //�������� ��ư �̹��� ����
                DataController.instance.gameData.challengeLevel[prefabnum]++; //��������
            }

        }

    }


}
