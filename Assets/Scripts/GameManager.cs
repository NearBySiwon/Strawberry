using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;

[System.Serializable]
public class ObjectArray //GameObject�� ������ �迭 ����� ���� MonoBehaviour �ܺο��� ����
{
    public GameObject[] Behind = new GameObject[3];
}

public class GameManager : MonoBehaviour
{
    #region �ν�����
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    // public int[] BerryPrice = new int[192];

    [Header("[ Object ]")]
    public GameObject stemPrefab; // ������
    public GameObject bugPrefab;

    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;

    public const float STEM_LEVEL_0 = Globalvariable.STEM_LEVEL_0;
    public const float STEM_LEVEL_1 = Globalvariable.STEM_LEVEL_1;
    public const float STEM_LEVEL_2 = Globalvariable.STEM_LEVEL_2;
    public const float STEM_LEVEL_3 = Globalvariable.STEM_LEVEL_3;
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;

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

    //���ο����
    [Header("[ OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;
    [Header("[ INFO ]")]
    public float[] time_newBerry;
    public int[] price_newBerry;
    [Header("[ SPRITE ]")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;

    private int index_newBerry = 0; //���� �ε���
    private bool isStart_newBerry = false; //������ �����°�

    [Header("[ Check/Settings Panel ]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;
    public bool isblackPanelOn = false;

    [Header("[ Check/Day List ]")]
    public bool[] Today;
    public ObjectArray[] Front = new ObjectArray[7];
    public string url = "";

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
        Attendance();
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
            if ((0 < creatTimeTemp && creatTimeTemp < STEM_LEVEL_MAX) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
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

        updateInfo(index_newBerry);
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
                DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant = true; // üũ ���� ����
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
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);               
    }
    
    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // ���� Transform�� ���⸦ �ɴ´�
        stem.gameObject.SetActive(true); // ���� Ȱ��ȭ              
        coll.enabled = false; // ���� �ݶ��̴��� ��Ȱ��ȭ (���ʿ� �浹 ����)
    }
    void Harvest(Stem stem)
    {
        AudioManager.instance.HarvestAudioPlay();//���� ��Ȯ�Ҷ� ȿ����

        Farm farm = farmList[stem.stemIdx];
        if (DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest) return;

        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // ���� ����ش�
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest = true;
        Vector2 pos = stem.transform.position; ;
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
        stem.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f); // 0.25�� �ڿ�

        DataController.instance.gameData.berryFieldData[farm.farmIdx].isHarvest = false; // ��Ȯ�� ����              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed) // ���ʰ� ���ٸ�
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
    /*void SetBerryPrice()
    {
        BerryPrice[0] = 10;     // Ŭ������ 0�� ����
        BerryPrice[64] = 20;    // ������� 0�� ����
        BerryPrice[128] = 30;   // ����ũ�� 0�� ����

        for (int i = 0; i < 192; i++)
        {
            if (i < 64)
                BerryPrice[i] = BerryPrice[0] + i * 3;
            else if (i < 128)
                BerryPrice[i] = BerryPrice[64] + (i - 64) * 5;
            else
                BerryPrice[i] = BerryPrice[128] + (i - 128) * 7;
        }

        //Debug.Log("���Ⱑġ ���� �Ϸ�");
        //Debug.Log(BerryPrice[0] + " " + BerryPrice[64] + " " + BerryPrice[128] + " ");
        //Debug.Log(BerryPrice[9] + " " + BerryPrice[73] + " " + BerryPrice[137] + " ");
    }*/

    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999���� A
        {
            coinText.text = coin.ToString() + " A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)���� B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + " B";
        }
        else                        // �� �� C (�ִ� 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + " C";
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
            Debug.Log("������ �����ؿ�!"); //��� �г� ����
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
            Debug.Log("��Ʈ�� �����ؿ�!"); //��� �г� ����
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
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= STEM_LEVEL_MAX) // (4)�� ��Ȳ, �� ������ ���� �� �� ���� �� �� �ڶ� ������� �ݶ��̴��� ���ش�.
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
    //Ȱ��ȭ ��Ȱ��ȭ�� â ���� �Ѱ�
    public void turnOff(GameObject Obj)
    {
        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
    }

    #region PTJ

    //PTJ
    public void workingApply(List<Sprite> workingList)
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (workingList[i] == null) { working[i].SetActive(false); }
                else {
                    working[i].SetActive(true);
                    working[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = workingList[i];
                }
            }
            catch {}//���� ����
        }
    }
    //PTJ��� ����ߴ���
    public void workingCount(int employCount) { workingCountText.GetComponent<Text>().text = employCount.ToString() + "/3"; }
    #endregion

    #region New Berry Add
    //���ο� ���� ���� ����
    public void updateInfo(int index)
    {

        try
        {
            if (isStart_newBerry == true)
            {
                if (time_newBerry[index] > 0) //�ð��� 0���� ũ�� 1�ʾ� ����
                {
                    time_newBerry[index] -= Time.deltaTime;
                    startBtn_newBerry.GetComponent<Image>().sprite = ingImg;
                }
                else
                { startBtn_newBerry.GetComponent<Image>().sprite = doneImg; }


            }
            //���� price�� time text�� ���δ�.
            priceText_newBerry.GetComponent<Text>().text = price_newBerry[index].ToString();
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry[index])); //�����κи� ����Ѵ�.

        }
        catch
        { Debug.Log("���� ���� ���� ����"); }
    }
    //���ο� ���� ��ư�� ������
    public void newBerryAdd()
    {
        //Ÿ�̸Ӱ� 0 �̶�� 
        if (time_newBerry[index_newBerry] < 0.9)
        {
            int newBerryIndex=0;
            //���� �����߿� �������� �ϳ� ����(���Ƿ� 0~10)
            while (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == true) 
            {
                newBerryIndex = UnityEngine.Random.Range(0, 11);
            }
            //���ο� ���Ⱑ �߰��ȴ�.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;


            //���� �Һ�
            UseCoin(price_newBerry[index_newBerry]);
            //ShowCoinText(CoinText, DataController.instance.gameData.coin); // Ʈ������ ��Ÿ�� �� ���̾����� �Ű������� �ް� �����߾�� - ����� 

            //�������̵� ���� ��� -> �� ���� ���׷��̵� �ݾ��� ���δ�.
            index_newBerry++;
            updateInfo(index_newBerry);

            //���۹�ư���� ����
            startBtn_newBerry.GetComponent<Image>().sprite = startImg;
            isStart_newBerry = false;
        }
        else
        {
            isStart_newBerry = true;
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

    #region �⼮

    #region �⼮ ���� ���

    IEnumerator WebCheck() //���ͳ� �ð� ��������.
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

    public void Attendance()
    {
        DateTime today = DataController.instance.gameData.Today;
        DateTime lastday = DataController.instance.gameData.Lastday; //���� ��¥ �޾ƿ���
        bool isAttendance = DataController.instance.gameData.attendance; //�⼮ ���� �Ǵ� bool ��
        int days = DataController.instance.gameData.days; // �⼮ ���� ��¥.
        int weeks; //���� ����
        //lastday = DateTime.Parse("2022-03-12"); //�׽�Ʈ��
        //days = 6; //�׽�Ʈ ��.

        TimeSpan ts = today - lastday; //��¥ ���� ���
        int DaysCompare = ts.Days; //Days ������ ����.

        if (isAttendance == false)
        {
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //�������� Week �ؽ�Ʈ ���� ���� �߰�����.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else if (days == 0)
            {
                weeks = days;
                //Week 1 �� �ؽ�Ʈ ����
            }
            else
            {
                weeks = days;
                //Week 1 �� �ؽ�Ʈ ����
            }
            if (DaysCompare==1) //���� ��¥�� ������ �⼮ ��¥���� �Ϸ� �̷���
            {
                //days�� �´� ��ư Ȱ��ȭ
                switch (weeks)
                {
                    case 0:
                        selectDay(weeks);
                        break;
                    case 1:
                        selectDay(weeks);
                        break;
                    case 2:
                        selectDay(weeks);
                        break;
                    case 3:
                        selectDay(weeks);
                        break;
                    case 4:
                        selectDay(weeks);
                        break;
                    case 5:
                        selectDay(weeks);
                        break;
                    case 6:
                        selectDay(weeks);
                        break;
                }
            }
            else if (DateTime.Compare(today, lastday) < 0) //������ ������ ���� ������, ���� ������
            {
                //days 1���� �ʱ�ȭ �� day1��ư Ȱ��ȭ, week 1�� ����.
                DataController.instance.gameData.days = 0;
                selectDay(0);
                //week 1 �ؽ�Ʈ ����.
            }
            else //�����⼮�� �ƴѰ��
            {
                //days 1���� �ʱ�ȭ �� day1��ư Ȱ��ȭ, week 1�� ����.
                DataController.instance.gameData.days = 0;
                selectDay(0);
                //week 1 �ؽ�Ʈ ����.
            }
        }
        else //�⼮�� �̹� �� ���´�
        {
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //�������� Week �ؽ�Ʈ ���� ���� �߰�����.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else
            {
                weeks = days;
                //Week 1 �� �ؽ�Ʈ ����
            }
            for (int i = 0; i < weeks; i++) //�⼮�Ϸ� ��ư Ȱ��ȭ
            {
                Front[i].Behind[1].SetActive(true);
            }
        }
    }

    #endregion

    #region ��¥ ����

    public void selectDay(int day)
    {
        if (day != 0)
        {
            for (int i = 0; i < day; i++)
            {
                Front[i].Behind[1].SetActive(true);
            }
        }
        Front[day].Behind[2].SetActive(true);
    }

    #endregion

    #region �⼮ ���� ����

    public void AttandanceSave(int number)
    {
        //�⼮ ���� ����.
        Front[number].Behind[1].SetActive(true);
        Front[number].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DataController.instance.gameData.Today;
    }

    #endregion

    #region �⼮ ��������Ʈ Ŭ��

    public void clickDay1()
    {
        AttandanceSave(0);
    }
    public void clickDay2()
    {
        AttandanceSave(1);
    }
    public void clickDay3()
    {
        AttandanceSave(2);
    }
    public void clickDay4()
    {
        AttandanceSave(3);
    }
    public void clickDay5()
    {
        AttandanceSave(4);
    }
    public void clickDay6()
    {
        AttandanceSave(5);
    }
    public void clickDay7()
    {
        AttandanceSave(6);
    }

    #endregion

    #region ���� üũ �� ��������

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
        Attendance();
    }

    #endregion

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
