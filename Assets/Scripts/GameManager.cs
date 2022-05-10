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

    [Header("[ Object ]")]
    public GameObject stemPrefab; // ������
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
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
    
    //���� ����â
    public GameObject berryExp;

    //���ο����================================
    [Header("======[ NEW BERRY ]======")]
    [Header("[ OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;
    public GameObject stopBtn_newBerry;

    public GameObject newBerryTimeReuce;
    public GameObject newBerryAcheive;

    [Header("[ SPRITE ]")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;

    [Header("[ NEW BERRY INFO ]")]
    public float[] time_newBerry;
    public int[] price_newBerry;

    private bool isStart_newBerry = false; //������ �����°�
    private int newBerryResearchIndex;
   //===========================================

   [Header("[ Check/Settings Panel ]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;
    public bool isblackPanelOn = false;

    [Header("[ Check/Day List ]")]
    public GameObject AttendanceCheck;
    public string url = "";

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public GameObject NoCoinPanel;
    public GameObject NoHeartPanel;
    public GameObject BP;



    #endregion

    #region �⺻
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this; // ���� �Ŵ����� �̱��� ����ȭ >> GameManager.instance.~~ �� ȣ��

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //�⼮ ���� ȣ��.
        StartCoroutine(WebCheck());
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        CheckTime();

        //SetBerryPrice();
        InitDataInGM();       
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
        //���ο� ���� ����
        newBerryResearchIndex = DataController.instance.gameData.newBerryResearch; //���� �ε���
        updateInfo(newBerryResearchIndex);

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
        ShowCoinText(CoinText, DataController.instance.gameData.coin); // Ʈ������ ��Ÿ�� �� ���̾����� �Ű������� �ް� �����߾�� - �����
        HeartText.text = DataController.instance.gameData.heart.ToString();
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
            DataController.instance.gameData.researchLevel[2] * Globalvariable.instance.coeffi);
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
        stem.instantBerry.Explosion(pos, target.position, 0.5f);
        stem.instantBerry.GetComponent<SpriteRenderer>().sortingOrder = 4;

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

        stem.gameObject.SetActive(false);
        farm.isHarvest = false; // ��Ȯ�� ����              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !BP.activeSelf) // ���ʰ� ���ٸ�
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ 
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < TRUCK_CNT_LEVEL_MAX)
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.instantBerry.berryPrice;
        }
    }
    #endregion

    #region ��ȭ
    
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
        DataController.instance.gameData.coin += cost; // ���� ���� +
        DataController.instance.gameData.accCoin += cost; // ���� ���� +
    }

    public void UseCoin(int cost) // ���� ��� �Լ� (���̳ʽ� ���� ����)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
            DataController.instance.gameData.coin -= cost;
        else
        {
            //��� �г� ����
            GameManager.instance.DisableObjColliderAll();
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            BP.SetActive(true);
            NoCoinPanel.SetActive(true);
            NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetHeart(int cost) // ��Ʈ ȹ�� �Լ�
    {
        DataController.instance.gameData.heart += cost; // ���� ��Ʈ +
        DataController.instance.gameData.accHeart += cost; // ���� ��Ʈ +
    }

    public void UseHeart(int cost) // ��Ʈ ȹ�� �Լ� (���̳ʽ� ���� ����)
    {
        int myHeart = DataController.instance.gameData.heart;
        if (myHeart >= cost)
            DataController.instance.gameData.heart -= cost;
        else
        {
            //��� �г� ����
            GameManager.instance.DisableObjColliderAll();
            panelHearText.text = DataController.instance.gameData.heart.ToString()+"��";
            BP.SetActive(true);
            NoHeartPanel.SetActive(true);
            NoHeartPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }
    #endregion

    #region �ݶ��̴�
    public void DisableObjColliderAll() // ��� ������Ʈ�� collider ��Ȱ��ȭ
    {
        BoxCollider2D coll;
        isblackPanelOn = true;
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
        isblackPanelOn = false;
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
    public void workingApply(List<Sprite> workingList)
    {
        for (int i = 0; i < 3; i++)//ĭ 3��
        {
            try
            {
                if (workingList[i] == null) { working[i].SetActive(false); } //�˹����� ��� ������ ��Ȱ��ȭ
                else //�˹��� ������
                {
                    working[i].SetActive(true); //Ȱ��ȭ
                    working[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = workingList[i];//�� �̹��� �ֱ�
                }
            }
            catch { Debug.Log("ERRORORRORO"); }
        }
    }
    //PTJ��� ����ߴ���
    public void workingCount(int employCount) 
    { workingCountText.GetComponent<Text>().text = employCount.ToString(); }

    #endregion

    #region New Berry Add

    //NewBerryAdd State = beforeStart, useHeart, Start, Timer, Done, Achieve ->���� ���� ȹ�� ���
    //
    //���ο� ���� �߰�
    public void updateInfo(int index)
    {
        //�����ϰ��ִ� Ÿ�̸� + ������ ����� ���δ�.
        try
        {
            if (isStart_newBerry == true)
            {
                if (time_newBerry[index] > 0) //�ð��� 0���� ũ�� 1�ʾ� ����
                {
                    time_newBerry[index] -= Time.deltaTime;
                    startBtn_newBerry.GetComponent<Image>().sprite = ingImg;//������ �̹���
                }
                else
                {  startBtn_newBerry.GetComponent<Image>().sprite = doneImg; }//�Ϸ� �̹���
            }

            //���� price�� time text�� ���δ�.
            priceText_newBerry.GetComponent<Text>().text = price_newBerry[index].ToString();
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry[index]));
        }
        catch
        {
            priceText_newBerry.GetComponent<Text>().text = "no berry";
            timeText_newBerry.GetComponent<Text>().text = "no berry";
        }
    }



    //���ο� ���� ��ư�� ������
    public void newBerryAdd()
    {
        //�ð�, �� ������ ������ ����ȴ�.
        try
        {
            if (time_newBerry[newBerryResearchIndex] == null) { Debug.Log("test"); }
            if (startBtn_newBerry.GetComponent<Image>().sprite == startImg) // ���� ��ư Ŭ��
            {
                Time.timeScale = 0;//�����ϱ� ������ �ð� ����
                newBerryTimeReuce.SetActive(true);//��Ʈ�� �ð� ���̰ڳĴ� �г� ����
            }
            else { Time.timeScale = 1; }


            //Ÿ�̸Ӱ� 0 �̶�� (=Ÿ�̸� ������ ���� ���� �� ����)
            if (time_newBerry[newBerryResearchIndex] < 0.9)
            {

                int newBerryIndex = 0;
                while (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == true)
                {
                    //���� ���� ���� ã�Ƽ� true�� ����
                    //���� �����߿� �������� �ϳ� ����(���Ƿ� 0~9)
                    newBerryIndex = UnityEngine.Random.Range(0, 9);
                }
                //���ο� ���Ⱑ �߰��ȴ�.
                DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;
                //����ǥ ǥ��
                DataController.instance.gameData.isBerryEM[newBerryIndex] = true;

                //���� ���� ȿ����(¥��)
                AudioManager.instance.TadaAudioPlay();

                //���� ���� ����â
                newBerryAcheive.SetActive(true);
                newBerryAcheive.transform.GetChild(0).GetComponent<Image>().sprite
                    = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
                newBerryAcheive.transform.GetChild(1).GetComponent<Text>().text
                    = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<Berry>().berryName;



                //���� �Һ�
                UseCoin(price_newBerry[newBerryResearchIndex]);

                //���׷��̵� ���� ��� -> �� ���� ���׷��̵� ����, �ݾ� ���δ�
                DataController.instance.gameData.newBerryResearch++;
                updateInfo(newBerryResearchIndex);

                //���۹�ư���� ����
                startBtn_newBerry.GetComponent<Image>().sprite = startImg;
                isStart_newBerry = false;
            }
            //Ÿ�̸� 0�� �ƴ϶�� Ÿ�̸Ӹ� �����Ѵ�.
            else
            {  isStart_newBerry = true;  }

        }
        catch 
        { 
            priceText_newBerry.GetComponent<Text>().text = "no berry";
            timeText_newBerry.GetComponent<Text>().text = "no berry";
        }
    }

    #endregion

    #region Explanation
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //����â ����
                berryExp.SetActive(true);

                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;

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

    #endregion

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