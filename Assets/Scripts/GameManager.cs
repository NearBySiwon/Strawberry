using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;



public class GameManager : MonoBehaviour
{
    #region �ν�����
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    public Text MedalText;

    [Header("[ Object ]")]
    public GameObject stemPrefab; // ������
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
    public List<GameObject> stemObjList = new List<GameObject>();
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;
    public Text truckCoinBonusText;
    public int bonusTruckCoin;

    public const int TRUCK_CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int TRUCK_CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int TRUCK_CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int TRUCK_CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;


    [Header("[ PartTime/Search/Berry List ]")]
    //PTJ �˹�
    public GameObject workingCountText;//��� ���� ���� ��
    public GameObject[] working;//��� ���� ���� ����Ʈ ��ܿ�
    [NonSerialized]
    public List<int> workingID = new List<int>();//���� ���ϰ� �ִ� �˹ٻ� Id
    public GameObject PTJList;



    static List<Sprite> workingList = new List<Sprite>();
    [Header("==========PTJ Warning Panel===========")]
    public GameObject warningBlackPanel;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;

    //NEWS
    [NonSerialized]
    public int NewsPrefabNum;
    //���ο����================================
    [Header("[ OBJECT ]")]
    [Header("[ ==============NEW BERRY============== ]")]

    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;

    public GameObject TimeReuce_newBerry;
    public GameObject TimeReduceBlackPanel_newBerry;
    public GameObject TimeReducePanel_newBerry;
    public Text TimeReduceText_newBerry;
    public GameObject AcheivePanel_newBerry;


    public GameObject NoPanel_newBerry;
    public GameObject BlackPanel_newBerry;

    


    [Header("[ SPRITE ]")]
    public Sprite StartImg;
    public Sprite DoneImg;
    public Sprite IngImg;
    public SpriteRenderer[] stemLevelSprites;

    private float time_newBerry;
    private int price_newBerry;

    private string BtnState;//���� ��ư ����
    private int newBerryIndex;//�̹��� ���ߵǴ� ���� �ѹ�
    public GameObject Global;
    //===========================================

    [Header("[ Check/Settings Panel ]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;
    

    [Header("[ Check/Day List ]")]
    public GameObject AttendanceCheck;
    public string url = "";

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public GameObject NoCoinPanel;
    public GameObject NoHeartPanel;
    public GameObject BP;

    


    [Header("[ Game Flag ]")]
    public bool isGameRunning;
    public bool isBlackPanelOn = false;
    private int coinUpdate;

    #endregion

    #region �⺻
    void Start()
    {

        Application.targetFrameRate = 60;
        instance = this; // ���� �Ŵ����� �̱��� ����ȭ >> GameManager.instance.~~ �� ȣ��



        target = TruckObj.GetComponent<Transform>();
        
        //for(int i = 0; i < )
        //�⼮ ���� ȣ��.
        StartCoroutine(WebCheck());
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        CheckTime();


        InitDataInGM();

        //TimerStart += Instance_TimerStart;

        //DisableObjColliderAll();       
       
        isGameRunning = true;


        //NEW BERRY
        NewBerryUpdate();

        ShowCoinText(CoinText, DataController.instance.gameData.coin);
        HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    public void GameStart()
    {
        isGameRunning = true;
    }

    void InitDataInGM()
    {
        for (int i = 0; i < 16; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].isStemEnable)
            {
                stemList[i].gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isWeedEnable)
            {
                farmList[i].weed.gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isBugEnable)
            {
                bugList[i].gameObject.SetActive(true);
            }
            float creatTimeTemp = DataController.instance.gameData.berryFieldData[i].createTime;
            if ((0 < creatTimeTemp && creatTimeTemp < DataController.instance.gameData.stemLevel[4]) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
        //PTJ
        workinCountApply();
        workingCountText.GetComponent<Text>().text = DataController.instance.gameData.PTJCount.ToString();


        //NEW BERRY ����
        //�̺κ� �ð����̸� ���� ������. �ϴ� ������ �ð������ϱ� �̷��� �ϰ� ���߿� ���ִ°� ������ �����ּ���
        switch (DataController.instance.gameData.newBerryBtnState) { 
            case 0:BtnState = "start"; startBtn_newBerry.GetComponent<Image>().sprite = StartImg; break;
            case 1: BtnState = "ing"; startBtn_newBerry.GetComponent<Image>().sprite = IngImg; break;
            case 2: BtnState = "done"; startBtn_newBerry.GetComponent<Image>().sprite = DoneImg; break;
        }

        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư����
        {
            GameObject obj = ClickObj(); // Ŭ������ ������ �����´�
            if (obj != null)
            {

                if (obj.CompareTag("Farm"))
                {
                    ClickedFarm(obj);
                }
                else if (obj.CompareTag("Bug"))
                {
                    ClickedBug(obj);
                }
                else if (obj.CompareTag("Weed"))
                {
                    ClickedWeed(obj);
                }
            }
        }

        //������ �ڷΰ��� ��ư ������ ��/�����Ϳ���ESC��ư ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataController.instance.SaveData();
            Application.Quit();
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        //ShowCoinText(CoinText, DataController.instance.gameData.coin); // Ʈ������ ��Ÿ�� �� ���̾����� �Ű������� �ް� �����߾�� - �����
        //HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    #endregion

    #region �����
    void ClickedFarm(GameObject obj)
    {

        Farm farm = obj.GetComponent<Farm>();

        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant)
        {
            Stem st = GetStem(farm.farmIdx);
            if (st != null)
            {
                PlantStrawBerry(st, obj); // �ɴ´�
                AudioManager.instance.SowAudioPlay();
                DataController.instance
                    .gameData.berryFieldData[farm.farmIdx].isPlant = true; // üũ ���� ����
            }
        }
        else
        {
            if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].canGrow)
            {
                Harvest(stemList[farm.farmIdx]); // ��Ȯ
            }
        }
    }
    void ClickedBug(GameObject obj)
    {
        Bug bug = obj.GetComponent<Bug>();
        bug.DieBug();
    }
    void ClickedWeed(GameObject obj)
    {
        Weed weed = obj.GetComponent<Weed>();
        weed.DeleteWeed();
    }
    public void ClickedTruck()
    {
        bonusTruckCoin = (int)(DataController.instance.gameData.truckCoin *
            DataController.instance.gameData.researchLevel[2] * Globalvariable.instance.getEffi());
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);
        ShowCoinText(truckCoinBonusText, bonusTruckCoin);
    }

    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    public void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // ���� Transform�� ���⸦ �ɴ´�
        stem.gameObject.SetActive(true); // ���� Ȱ��ȭ              
        coll.enabled = false; // ���� �ݶ��̴��� ��Ȱ��ȭ (���ʿ� �浹 ����)
    }
    public void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;

        AudioManager.instance.HarvestAudioPlay();//���� ��Ȯ�Ҷ� ȿ����
        farm.isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.getInstantBerryObj().GetComponent<Berry>().Explosion(pos, target.position, 0.5f);
        //stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 3;

        StartCoroutine(HarvestRoutine(farm, stem)); // �������� ���Ⱑ �ɾ����� ������ ����

    }
    GameObject ClickObj() // Ŭ������ ������Ʈ�� ��ȯ
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm, Stem stem)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // ���� ��� ��Ȱ��ȭ

        yield return new WaitForSeconds(0.75f); // 0.75�� �ڿ�

        UpdateTruckState(stem);

        DataController.instance.gameData.totalHarvBerryCnt++; // ��Ȯ�� ������ �� ���� ������Ʈ            
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // ���� ����ش�

        //�ٱ⿡ ���̵� �ƿ� ����
        Animator anim = stemObjList[stem.stemIdx].GetComponent<Animator>();
        anim.SetInteger("Seed", 5);

        yield return new WaitForSeconds(0.3f); // 0.3�� �ڿ�

        stem.gameObject.SetActive(false);

        farm.isHarvest = false; // ��Ȯ�� ����              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !isBlackPanelOn) // ���ʰ� ���ٸ�
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ 
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < TRUCK_CNT_LEVEL_MAX)
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.getInstantBerryObj().GetComponent<Berry>().berryPrice;
        }
    }
    #endregion

    #region ��ȭ

    IEnumerator CountAnimation(Text characters, float target, float current) //��ȭ ���� �ִϸ��̼�

    {
        float duration = 1.0f; // ī���ÿ� �ɸ��� �ð� ����. 

        float offset = (target - current) / duration;

        while (current < target)

        {

            current += offset * Time.deltaTime;

            if (characters == CoinText)
            {
                if ((int)current <= 9999)           // 0~9999���� A
                {
                    characters.text = ((int)current).ToString() + "A";
                }
                else if ((int)current <= 9999999)   // 10000~9999999(=9999B)���� B
                {
                    current /= (float)1000;
                    characters.text = ((int)current).ToString() + "B";
                }
                else                        // �� �� C (�ִ� 2100C)
                {
                    current /= (float)1000000;
                    characters.text = ((int)current).ToString() + "C";
                }
            }
            else
                characters.text = ((int)current).ToString();

            yield return null;

        }

        current = target;
        int num = (int)current;

        if (characters == CoinText)
        {
            if (num <= 9999)           // 0~9999���� A
            {
                characters.text = num.ToString() + "A";
            }
            else if (num <= 9999999)   // 10000~9999999(=9999B)���� B
            {
                current /= 1000;
                characters.text = num.ToString() + "B";
            }
            else                        // �� �� C (�ִ� 2100C)
            {
                current /= 1000000;
                characters.text = num.ToString() + "C";
            }
        }
        else
            characters.text = num.ToString();
    }

    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999���� A
        {
            coinText.text = coin.ToString() + "A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)���� B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + "B";
        }
        else                        // �� �� C (�ִ� 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + "C";
        }
    }

    public void GetCoin(int cost) // ���� ȹ�� �Լ�
    {
        int current, acc;
        current = DataController.instance.gameData.coin;
        DataController.instance.gameData.coin += cost; // ���� ���� +
        acc = DataController.instance.gameData.coin;
        StartCoroutine(CountAnimation(CoinText, acc, current));
        DataController.instance.gameData.accCoin += cost; // ���� ���� +
    }

    public void UseCoin(int cost) // ���� ��� �Լ� (���̳ʽ� ���� ����)
    {
        int mycoin = DataController.instance.gameData.coin;
        int mycoinacc;
        if (mycoin >= cost)
        {
            DataController.instance.gameData.coin -= cost;
            mycoinacc= DataController.instance.gameData.coin;
            StartCoroutine(CountAnimation(CoinText, mycoinacc, mycoin));
        }
        else
        {
            //��� �г� ����
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            BP.SetActive(true);
            NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetHeart(int cost) // ��Ʈ ȹ�� �Լ�
    {
        int current, acc;
        current = DataController.instance.gameData.heart;
        DataController.instance.gameData.heart += cost; // ���� ��Ʈ +
        acc = DataController.instance.gameData.heart;
        StartCoroutine(CountAnimation(HeartText, acc, current));
        DataController.instance.gameData.accHeart += cost; // ���� ��Ʈ +
    }

    public void UseHeart(int cost) // ��Ʈ ȹ�� �Լ� (���̳ʽ� ���� ����)
    {
        int myHeart = DataController.instance.gameData.heart;
        int myHeartacc;
        if (myHeart >= cost)
        {
            DataController.instance.gameData.heart -= cost;
            myHeartacc= DataController.instance.gameData.heart;
            StartCoroutine(CountAnimation(HeartText, myHeartacc, myHeart));
        }
        else
        {
            //��� �г� ����
            panelHearText.text = DataController.instance.gameData.heart.ToString() + "��";
            BP.SetActive(true);
            NoHeartPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetMedal(int cost)
    {
        DataController.instance.gameData.medal += cost;
        ShowMedalText();
    }

    public void UseMedal(int cost)
    {
        int myMedal = DataController.instance.gameData.medal;
        if (myMedal >= cost)
        {
            DataController.instance.gameData.medal -= cost;
            ShowMedalText();
        }
        else
        {
            //�޴��� ���ڸ��� �ߴ� ���
        }
    }
    public void ShowMedalText()
    {
        MedalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();
    }
    #endregion

    #region �ݶ��̴�
    public void DisableObjColliderAll() // ��� ������Ʈ�� collider ��Ȱ��ȭ
    {
        BoxCollider2D coll;
        isBlackPanelOn = true;
        Debug.Log(GameManager.instance.isBlackPanelOn);
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            //stemList[i].canGrow = false;
            bugList[i].GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            // Weed�� Collider ����
            //farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // ��� ������Ʈ�� collider Ȱ��ȭ
    {
        BoxCollider2D coll;
        isBlackPanelOn = false;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // ���ʰ� ���� ���� �� ���� ColliderȰ��ȭ
            {
                coll.enabled = true;
            }
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4]) // (4)�� ��Ȳ, �� ������ ���� �� �� ���� �� �� �ڶ� ������� �ݶ��̴��� ���ش�.
            {
                coll.enabled = true;
            }
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // ������ Collider Ȱ��ȭ
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region ����Ʈ

    #region PTJ
    
    public void workinCountApply()
    {

        //���� �˹����� �˹ٻ����� ���� ���Ƚ��
        //������â?
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (working[i].activeSelf == true)
                {
                    working[i].transform.GetChild(1).transform.GetComponent<Text>().text
                        = DataController.instance.gameData.PTJNum[workingID[i]].ToString();
                    if (DataController.instance.gameData.PTJNum[workingID[i]] == 0)
                    { working[i].SetActive(false); }
                }
            }
            catch { }
        }
    }


    //��� ��ư Ŭ����
    public void PTJEmployButtonClick(int prefabNum) {
        //ȿ����
        AudioManager.instance.Cute1AudioPlay();

        //������� �ƴ� �����̴�
        if (DataController.instance.gameData.PTJNum[prefabNum] == 0)
        {
            if (DataController.instance.gameData.PTJCount < 3)
            {
                int cost = PTJ.instance.Info[prefabNum].Price * DataController.instance.gameData.PTJSelectNum[1];
                if (cost <= DataController.instance.gameData.coin)
                {
                    int ID = DataController.instance.gameData.PTJSelectNum[0];
                    //HIRE

                    //���λ��
                    //UseCoin(Info[ID].Price);

                    //���
                    DataController.instance.gameData.PTJNum[prefabNum] = DataController.instance.gameData.PTJSelectNum[1];

                    //������� �˹ٻ� �� ����
                    DataController.instance.gameData.PTJCount++;
                }
                else 
                {
                    //ȿ����
                    AudioManager.instance.Cute4AudioPlay();
                    //��ȭ ���� ��� �г�
                    ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                    warningBlackPanel.SetActive(true);
                }
            }
            else 
            {
                //ȿ����
                AudioManager.instance.Cute4AudioPlay();
                //3���̻� ������̶�� ��� �г� ����
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "��� ������ �˹� ����\n�Ѿ���!";
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                warningBlackPanel.SetActive(true);
            }
        }
        //������� �����̴�
        else
        {
            //FIRE
            //Ȯ��â����
            HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            warningBlackPanel.SetActive(true);
        }
        

    }

    public void Fire() 
    {
        int ID = DataController.instance.gameData.PTJSelectNum[0];
        //��� ����
        DataController.instance.gameData.PTJNum[ID] = 0;
        //��� ���� �˹ٻ� �� ����
        //PTJ���� ������

        //Ȯ��â������
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();
        warningBlackPanel.SetActive(false);
    }
    #endregion

    #region New Berry Add
    public void NewBerryUpdate()
    {
        //���� �� ���� ���� ����
        
        if (isNewBerryAble() == true)
        {
            //�������Ⱑ ��������.->�ð�,���� ��������.
            price_newBerry = 100 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
            selectBerry();

            //���� �ð� ���� �̰����� ?
            priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
            timeText_newBerry.GetComponent<Text>().text = "??:??";


            //���� ���� �����
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true);}
    }

    private bool isNewBerryAble()
    {

        //���� �����⸦ ���� �� �� �ֳ�?
        switch (DataController.instance.gameData.newBerryResearchAble)
        {
            case 0://classic ���߰���
                if (BerryCount("classic",false) == BerryCount("classic", true)) 
                { return false; }
                break;
            case 1://classic, special ���߰���
                if (BerryCount("classic",false) + BerryCount("special", false) == 
                    BerryCount("classic", true) + BerryCount("special", true)) 
                { return false; }
                break;
            case 2: //classic, special, unique ���߰���
                if (BerryCount("classic", false) + BerryCount("special", false) + BerryCount("unique", false) == 
                    BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true))
                { return false; }
                break;
        }
        return true;
    }


    //isUnlock-> false=���� ���� �����ϴ� ���� �������� ��ȯ / true=���� unlock�� ���� �������� ��ȯ�Ѵ�.
    private int BerryCount(string berryClssify, bool isUnlock)
    {
        int countIsExsist = 0;
        int countIsUnlock = 0;
        switch (berryClssify)
        {
            case "classic":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().classicBerryList.Count; i++)
                {
                    if (DataController.instance.gameData.isBerryUnlock[i]==true) { countIsUnlock++; }
                    if (Global.GetComponent<Globalvariable>().classicBerryList[i] == true) { countIsExsist++; } 
                }
                break;

            case "special":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().specialBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64+64; i++)
                {if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().uniqueBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().uniqueBerryList[i] == true) { countIsExsist++; } }
                for (int i = 128; i < 128+64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;
            //default:Debug.Log("�߸��� �� �޾Ҵ�");break;
        }


        if (isUnlock == true)
        { return countIsUnlock; }
        else { return countIsExsist; }
    }



    //���ο� ���� ���� ��ư ������
    public void NewBerryButton()
    {

        switch (BtnState)
        {
            case "start":
                //�̹� ������ ���߿� �ʿ��� ���ݰ� �ð�
                priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
                timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));

                if (DataController.instance.gameData.coin >= price_newBerry)
                {
                    //���Һ�
                    UseCoin(price_newBerry);

                    //��ư���� ing��
                    DataController.instance.gameData.newBerryBtnState = 1;
                    //Ÿ�̸� ����
                    StartCoroutine("Timer");

                    //�ð� ���ҿ��� ���� �г��� ����.
                    TimeReduceBlackPanel_newBerry.SetActive(true); //�ÿ� �ǵ帲
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale(); //�ÿ� �ǵ帲
                    TimeReduceText_newBerry.GetComponent<Text>().text //�ÿ� �ǵ帲
                        = "��Ʈ 10���� �ð��� 10������ ���̽ðڽ��ϱ�?\n";//������ 1��. �ӽ�
                }
                else
                {
                    NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                }
                break;
            case "ing":
                //�ð� ���ҿ��� ���� �г��� ����.
                TimeReduceBlackPanel_newBerry.SetActive(true);
                TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale();
                TimeReduceText_newBerry.GetComponent<Text>().text
                    = "��Ʈ 10���� �ð��� 10������ ���̽ðڽ��ϱ�?\n";//������ 1��. �ӽ�
                break;
            case "done": /*���� ����*/
                GetNewBerry();
                break;
                //default:Debug.Log("NowButton�̸��� �߸�����ϴ�."); break;
        }
        
    }

    //TimeReucePanel_newBerry
    //��Ʈ �Ἥ �ð��� ���ϰǰ���? �г�
    public void TimeReduce(bool isTimeReduce)
    {
        if (DataController.instance.gameData.heart >= 10)
        {
            //��Ʈ �Ἥ �ð��� ���ϰŸ�
            if (isTimeReduce == true)
            {
                //�ð��� 10���� �ٿ��ش�.
                time_newBerry = 1;
                timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));
                //��Ʈ�� �Һ��Ѵ�.
                UseHeart(10);
            }
        }
        else 
        { NoHeartPanel.GetComponent<PanelAnimation>().OpenScale(); }
        //â ����
        TimeReduceBlackPanel_newBerry.SetActive(false);
        TimeReducePanel_newBerry.GetComponent<PanelAnimation>().CloseScale();

    }

    IEnumerator Timer()
    {
        //1�ʾ� ����
        yield return new WaitForSeconds(1f);
        time_newBerry--;
        //�����ϴ� �ð� ���̱�
        timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));
        
        //Ÿ�̸� ������
        if (time_newBerry < 0.1f) 
        {
            DataController.instance.gameData.newBerryBtnState =2;//Done���·�
            StopCoroutine("Timer");  
        }
        else { StartCoroutine("Timer"); }
        
    }


    private void selectBerry() 
    {
        newBerryIndex = 0;
        while (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == true
            || Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex] == null)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0: newBerryIndex = UnityEngine.Random.Range(1, 64);
                    time_newBerry = 10;
                    break;
                case 1: newBerryIndex = berryPercantage(128);
                    break;
                case 2: newBerryIndex = berryPercantage(192); 
                    break;
            }
            //���� unlock���� ���� ���� �߿��� �����ϴ� ������ ����
        }
    }


    private void GetNewBerry()
    {

        //���ο� ���Ⱑ �߰��ȴ�.
        DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;
        DataController.instance.gameData.unlockBerryCnt++;

        //����ǥ ǥ��
        DataController.instance.gameData.isBerryEM[newBerryIndex] = true;

        //���� ���� ȿ����(¥��)
        AudioManager.instance.TadaAudioPlay();

        //���� ���� ����â
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().sprite
            = Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;

        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Text>().text
            = Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex].GetComponent<Berry>().berryName;

        //����â ����
        BlackPanel_newBerry.SetActive(true);


        DataController.instance.gameData.newBerryBtnState = 0;

        NewBerryUpdate();

    }
    
    private int berryPercantage(int endIndex) 
    { 
        int randomNum=0;
        int newBerryIndex = 0;

        //RANDOM NUM -> classic(45)=0~44  special(35)=45~79  unique(20)=80~101
        if (endIndex == 128) { randomNum = UnityEngine.Random.Range(0, 80); }//���� Ŭ�����̶� ����ȸ� �����ϸ�
        else if (endIndex == 192) { randomNum = UnityEngine.Random.Range(0, 100 + 1); }//���� ���δ� �����ϸ�


        //if (berryCountComparision() == 3)
        //{
        if (randomNum < 45) { newBerryIndex = UnityEngine.Random.Range(0, 64); time_newBerry = 10; }//classic
        else if (randomNum < 80) { newBerryIndex = UnityEngine.Random.Range(64, 128); time_newBerry = 20; }//special
        else if (randomNum <= 100) { newBerryIndex = UnityEngine.Random.Range(128, 192); time_newBerry = 30; }//unique
        //}

        return newBerryIndex;
    }
    private int berryCountComparision() //<-����.
    {

        int classicCnt = BerryCount("classic", true);
        int specialCnt = BerryCount("special", true);
        int uniqueCnt = BerryCount("unique", true);

        if (Mathf.Abs(classicCnt - specialCnt) > 10 ||
            Mathf.Abs(classicCnt - uniqueCnt) > 10 ||
            Math.Abs(specialCnt - uniqueCnt) > 10)
        { return Mathf.Min(classicCnt, specialCnt, uniqueCnt); }
        else 
        { return 3; }// �����ϰ� �������� �����Ƿ� ������������
    }


    public void newsBerry()
    {
        if (isNewBerryAble())
        {
            selectBerry();//������ �����̶� ���� �ر� ���ÿ� �ߴµ� ���� ���� �������� �ϸ� ����������� ����
            
            //���ο� ���Ⱑ �߰��ȴ�.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;
            //����ǥ ǥ��
            DataController.instance.gameData.isBerryEM[newBerryIndex] = true;

            //���� ���� ȿ����(¥��)
            AudioManager.instance.TadaAudioPlay();

            //���� ���� ����â
            /*
            AcheivePanel_newBerry.SetActive(true);
            AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().sprite
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
            AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Text>().text
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<Berry>().berryName;
            */
            //����â ����
            //BlackPanel_newBerry.SetActive(true);

        }
        else { Debug.Log("���̻� ���߰����� ���� ����. ��"); }
    
    }
    
    #endregion

    #region Explanation
    /*
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //����â ����
                berryExp_BlackPanel.SetActive(true); //�ÿ� �ǵ帲
                berryExp_Panel.GetComponent<PanelAnimation>().OpenScale(); //�ÿ� �ǵ帲

                //GameObject berryExpImage = berryExp.transform.GetChild(1).GetChild(1).gameObject; //�ÿ� �ǵ帲
                //GameObject berryExpName = berryExp.transform.GetChild(1).GetChild(2).gameObject; //�ÿ� �ǵ帲
                //GameObject berryExpTxt = berryExp.transform.GetChild(1).GetChild(3).gameObject; //�ÿ� �ǵ帲


                //Explanation ������ ä���.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//�̹��� ����

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//�̸� ����

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryExplain;//���� ����    
            }
        }
        catch
        {
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }
    }
    */
    #endregion

    public void NewsUnlock() 
    {

        News.instance.NewsUnlock(NewsPrefabNum);
    }

    #region ��Ÿ
    //Ȱ��ȭ ��Ȱ��ȭ�� â ���� �Ѱ�
    public void turnOff(GameObject Obj)
    {

        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
  
    }

    public string TimeForm(int time)//�ʴ��� �ð��� ��:�ʷ� ����
    {
        int M = 0, S = 0;//M,S ����
        string Minutes, Seconds;//M,S �ؽ�Ʈ �����

        M = (time / 60);
        S = (time % 60);


        //M,S����
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S�� 10�̸��̸� 01, 02... ������ ǥ��
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S�� 0�̸� 00���� ǥ���Ѵ�.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #endregion

    #region �⼮

    //���ͳ� �ð� ��������.

    IEnumerator WebCheck() 
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date);
                DataController.instance.gameData.Today = dateTime;
            }
        }
    }

    //���� üũ �� ��������

    public void CheckTime()
    {
        //�÷��� ���� ������ �Ѿ ��� �⼮ ����
        // �����ð� ���ϱ�.
        DateTime target = new DateTime(DataController.instance.gameData.Today.Year, DataController.instance.gameData.Today.Month, DataController.instance.gameData.Today.Day);
        target = target.AddDays(1);
        // �����ð� - ����ð�
        TimeSpan ts = target - DataController.instance.gameData.Today;
        // �����ð� ��ŭ ��� �� OnTimePass �Լ� ȣ��.
        Invoke("OnTimePass", (float)ts.TotalSeconds);
    }

    public void OnTimePass()
    {
        //��������
        DataController.instance.gameData.attendance = false;
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
    }

    #endregion

    #region ���� �޴�
    public void OnclickStart()
    {
    }

    public void OnclickOption()
    {

    }

    public void OnclickQuit()
    {

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion

}