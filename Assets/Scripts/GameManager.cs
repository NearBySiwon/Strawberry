using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[System.Serializable]
public class ObjectArray //GameObject�� ������ �迭 ����� ���� MonoBehaviour �ܺο��� ����
{
    public GameObject[] Behind = new GameObject[3];
}

public class GameManager : MonoBehaviour
{
    #region �ν�����
    public static GameManager instance;

    [Header("------------[ Money ]")]
    //[SerializeField] public int coin;
    //[SerializeField] public int heart;
    public Text CoinText;
    public Text HeartText;   
    public int[,] BerryPrice = new int[3, 32];

    [Header("------------[ Object ]")]
    public GameObject stemPrefab; // ������
    public GameObject bugPrefab;
      
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();
   
    public List<GameObject> berryPrefabListAll = new List<GameObject>();   
    public List<GameObject> berryPrefabListUnlock = new List<GameObject>();
    public List<GameObject> berryPrefabListlock = new List<GameObject>();

    [Header("------------[Truck List]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;        
    Transform target;
    
    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;
    public GameObject PanelBlack;
    public GameObject panelBlack_Exp;
    internal object count;
    public GameObject[] working;
    //���ο���� ����========================================
    [Header("OBJECT")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;
    [Header("INFO")]
    public float[] time_newBerry;
    public int[] price_newBerry;
    [Header("SPRITE")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;

    private int index_newBerry = 0;//���� �ε���
    private bool isStart_newBerry = false;//������ �����°�

    [Header("------------[Check/Settings Panel]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;

    [Header("------------[Check/Day List]")]
    public bool[] Today;
    public ObjectArray[] Front = new ObjectArray[7];

    #endregion

    #region �⺻
    void Awake()
    {
        Attendance();

            Application.targetFrameRate = 60;
        instance = this; // ���� �Ŵ����� �̱��� ����ȭ >> GameManager.instance.~~ �� ȣ��                               
        target = TruckObj.GetComponent<Transform>();
        
        SetBerryPrice();
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
            if ((0 < creatTimeTemp && creatTimeTemp < 20) || DataController.instance.gameData.berryFieldData[i].hasWeed)
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
                if (obj.GetComponent<Farm>() != null)
                {
                    ClickedFarm(obj);
                }
                else if (obj.GetComponent<Bug>() != null)
                {
                    ClickedBug(obj);
                }
                else if (obj.GetComponent<Weed>() != null)
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
        ShowCoinText();
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
        DataController.instance.gameData.berryCnt = 0;
    }
    /*void MakeStemAndBug() // �ٱ� ����
    {
        GameObject instantStemObj = Instantiate(stemPrefab, stemGroup);
        instantStemObj.name = "stem " + stemList.Count;

        Stem instantStem = instantStemObj.GetComponent<Stem>();
        instantStem.stemIdx = stemList.Count;

        instantStem.gameObject.SetActive(false);
        stemList.Add(instantStem);

        GameObject instantBugObj = Instantiate(bugPrefab, instantStemObj.transform);
        instantBugObj.name = "Bug " + bugList.Count;

        Bug instantBug = instantBugObj.GetComponent<Bug>();
        instantBug.bugIdx = bugList.Count;

        instantBug.gameObject.SetActive(false); // ����
        bugList.Add(instantBug);
    }*/
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

        UpdateBerryCnt();
        stem.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f); // 0.25�� �ڿ�

        DataController.instance.gameData.berryFieldData[farm.farmIdx].isHarvest = false; // ��Ȯ�� ����              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed) // ���ʰ� ���ٸ�
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ 
        }     
    }
    void UpdateBerryCnt()
    {       
        if(DataController.instance.gameData.berryCnt < 48)
        {
            DataController.instance.gameData.berryCnt += 1;
        }
    }

    #endregion

    #region ��ȭ
    void SetBerryPrice()
    {
        BerryPrice[0, 0] = 10; // Ŭ������ 0�� ����

        for (int i = 0; i < 32; i++)
        {
            if (i != 0)
                BerryPrice[0, i] = BerryPrice[0, i - 1] + 5; // Ŭ���� ���Ⱚ ���� (1������)
            BerryPrice[1, i] = BerryPrice[0, i] * 2;
            BerryPrice[2, i] = BerryPrice[0, i] * 3;
        }

        Debug.Log("���Ⱑġ : " + BerryPrice[0, 0] + " " + BerryPrice[1, 0] + " " + BerryPrice[2, 0]);
    }

    public void ShowCoinText()
    {
        int show = DataController.instance.gameData.coin;
        if (show <= 9999)           // 0~9999���� A
        {
            CoinText.text = show.ToString() + " A";
        }
        else if (show <= 9999999)   // 10000~9999999(=9999B)���� B
        {
            show /= 1000;
            CoinText.text = show.ToString() + " B";
        }
        else                        // �� �� C (�ִ� 2100C)
        {
            show /= 1000000;
            CoinText.text = show.ToString() + " C";
        }
    }
    #endregion

    #region �ݶ��̴�
    public void DisableObjColliderAll() // ��� ������Ʈ�� collider ��Ȱ��ȭ
    {
        BoxCollider2D coll;
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
        for (int i = 0; i < farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // ���ʰ� ���� ���� ���� ColliderȰ��ȭ
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            /*if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed) // (4)�� ��Ȳ, �� ������ ���� �� �� ���� ��
            {
                DataController.instance.gameData.berryFieldData[i].canGrow = true;
            }*/
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // ������ Collider Ȱ��ȭ
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region ����Ʈ
    //Ȱ��ȭ ��Ȱ��ȭ
    public void turnOff(GameObject Obj)
    {
        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
    }
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
            catch{ Debug.Log("error test"); }
        }
    }
    public void updateInfo(int index)
    {

        try
        {
            if (isStart_newBerry == true)
            {
                if (GameManager.instance.time_newBerry[index] > 0) //�ð��� 0���� ũ�� 1�ʾ� ����
                {
                    GameManager.instance.time_newBerry[index] -= Time.deltaTime;
                    startBtn_newBerry.GetComponent<Image>().sprite = ingImg;
                }
                else
                { startBtn_newBerry.GetComponent<Image>().sprite = doneImg; }


            }
            //���� price�� time text�� ���δ�.
            priceText_newBerry.GetComponent<Text>().text = price_newBerry[index].ToString();
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(GameManager.instance.time_newBerry[index])); //�����κи� ����Ѵ�.

        }
        catch
        {            Debug.Log("���� ���� ���� ����");        }
    }
    //��ư�� ������
    public void newBerryAdd()
    {
        //Ÿ�̸Ӱ� 0 �̶�� 
        if (GameManager.instance.time_newBerry[index_newBerry] < 0.9)
        {
            //���ο� ���Ⱑ �߰��ȴ�.
            Debug.Log("���ο� ����!!");


            //�ݾ��� ����������.
            //GameManager.instance.coin -= price_newBerry[index_newBerry];
            DataController.instance.gameData.coin-= price_newBerry[index_newBerry];
            //GameManager.instance.ShowCoinText(GameManager.instance.coin);
            ShowCoinText();

            //�������̵� ���� ��� -> �� ���� ���׷��̵� �ݾ��� ���δ�.
            index_newBerry++;
            updateInfo(index_newBerry);

            //���۹�ư���� ����
            startBtn_newBerry.GetComponent<Image>().sprite = startImg;
            isStart_newBerry = false;
        }
        else
        {
            Debug.Log("���ο� ���⸦ ���� ���� �� ��ٸ�����");
            isStart_newBerry = true;
        }
    }
    public string TimeForm(int time)
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
    public void Attendance()
    {
        DateTime today = DateTime.Now;
        DateTime lastday = DataController.instance.gameData.Lastday; //���� ��¥ �޾ƿ���
        bool isAttendance = DataController.instance.gameData.attendance;
        int days = DataController.instance.gameData.days;
        int weeks;

        // days = 6; �⼮ ��� �׽�Ʈ ��. ���� ��¥ ����.

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
                days += 1;
                weeks = days;
            }
            else
            {
                weeks = days;
                //Week 1 �� �ؽ�Ʈ ����
            }

            if (DateTime.Compare(today, lastday) > 0) //���� ��¥�� ������ �⼮ ��¥���� �̷���
            {
                //days�� �´� ��ư Ȱ��ȭ
                //
                switch (weeks)
                {
                    case 1:
                        selectDay1();
                        break;
                    case 2:
                        selectDay2();
                        break;
                    case 3:
                        selectDay3();
                        break;
                    case 4:
                        selectDay4();
                        break;
                    case 5:
                        selectDay5();
                        break;
                    case 6:
                        selectDay6();
                        break;
                    case 7:
                        selectDay7();
                        break;
                }
            }
            else // ���Ÿ�
            {
                //days 1���� �ʱ�ȭ �� day1��ư Ȱ��ȭ, week 1�� ����.
                DataController.instance.gameData.days = 0;
                selectDay1();
                //week 1 �ؽ�Ʈ ����.
            }
        }
    }

    public void selectDay1()
    {
        Front[0].Behind[2].SetActive(true);
    }
    public void selectDay2()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[2].SetActive(true);
    }
    public void selectDay3()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[2].SetActive(true);
    }
    public void selectDay4()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[2].SetActive(true);
    }
    public void selectDay5()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[2].SetActive(true);
    }
    public void selectDay6()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[1].SetActive(true);
        Front[5].Behind[2].SetActive(true);
    }
    public void selectDay7()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[1].SetActive(true);
        Front[5].Behind[1].SetActive(true);
        Front[6].Behind[2].SetActive(true);
    }


    public void clickDay1()
    {
        Front[0].Behind[1].SetActive(true);
        Front[0].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay2()
    {
        Front[1].Behind[1].SetActive(true);
        Front[1].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay3()
    {
        Front[2].Behind[1].SetActive(true);
        Front[2].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay4()
    {
        Front[3].Behind[1].SetActive(true);
        Front[3].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay5()
    {
        Front[4].Behind[1].SetActive(true);
        Front[4].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay6()
    {
        Front[5].Behind[1].SetActive(true);
        Front[5].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay7()
    {
        Front[6].Behind[1].SetActive(true);
        Front[6].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }


    public void CheckTime()
    {
        //�÷��� ���� ������ �Ѿ ��� �⼮ ����
        // �����ð� ���ϱ�.
        DateTime target = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        target = target.AddDays(1);
        // �����ð� - ����ð�
        TimeSpan ts = target - DateTime.Now;
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
