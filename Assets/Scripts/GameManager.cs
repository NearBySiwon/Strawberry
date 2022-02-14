using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region �ν�����
    public static GameManager instance;

    [Header("------------[ Money ]")]
    [SerializeField] public int coin;
    [SerializeField] public int heart;
    public Text CoinText;
    public Text HeartText;
    //public int[] berryPrice = { 10, 20, 30, 40 };
    public int[,] BerryPrice = new int[3, 32];

    [Header("------------[ Object ]")]
    public GameObject berryPrefab; // ������    
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    public Transform berryGroup;
    public List<StrawBerry> berryList;

    [Header("------------[Truck List]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Truck truck;
    Transform target;

    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;
    public GameObject PanelBlack;
    public GameObject panelBlack_Exp;
    public GameObject berryExpPanel;
    internal object count;

    [Header("------------[Check/Settings Panel]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;

    [Header("------------[Check/Day List]")]
    public bool Day1, Day2, Day3, Day4, Day5, Day6, Day7; //public bool[] Today; ��� �迭�� ó���ϸ� ���� (�쿬)
    public GameObject Day1_1, Day1_2, Day1_3, Day2_1, Day2_2, Day2_3, Day3_1, Day3_2, Day3_3, 
        Day4_1, Day4_2, Day4_3, Day5_1, Day5_2, Day5_3, Day6_1, Day6_2, Day6_3, Day7_1, Day7_2, Day7_3; // ��׵��� (�쿬)
    #endregion

    #region �⺻
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this; // ���� �Ŵ����� �̱��� ����ȭ >> Ÿ ��ũ��Ʈ���� GameManager�� ������Ʈ ���� �����ø�
                         // ���� ��ũ��Ʈ ���� ���ӸŴ��� �Ҵ� ���ص� GameManager.instance.~~ �� ȣ���Ͻø� �ſ�!! 
        berryList = new List<StrawBerry>();
        truck = TruckObj.GetComponent<Truck>();
        target = TruckObj.GetComponent<Transform>();

        for (int i = 0; i < 16; i++) // ������Ʈ Ǯ������ �̸� ���� ����
        {
            MakeStrawBerry();
        }

        // �ӽ� ��ȭ ����
        coin = 10000;
        heart = 300;
        ShowCoinText(coin);
        //CoinText.text = coin.ToString() + " A";
        HeartText.text = heart.ToString();

        SetBerryPrice();
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
            }
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        ShowCoinText(coin);
        HeartText.text = heart.ToString();
    }
    #endregion

    #region �����
    void ClickedFarm(GameObject obj)
    {
        Farm farm = obj.GetComponent<Farm>();
        if (!farm.isPlant)
        {
            StrawBerry stb = GetStrawBerry(farm.farmIdx);
            if (stb != null)
            {
                PlantStrawBerry(stb, obj); // �ɴ´�                            
                farm.GetComponent<Farm>().isPlant = true; // üũ ���� ����
            }
        }
        else
        {
            if (!berryList[farm.farmIdx].canGrow)
            {
                Harvest(berryList[farm.farmIdx]); // ��Ȯ
            }
        }
    }
    void ClickedBug(GameObject obj)
    {
        Bug bug = obj.GetComponent<Bug>();
        bug.DieBug();
    }
    public void ClickedTruck()
    {
        truck.berryCnt = 0;
    }
    void MakeStrawBerry() // ���� ����
    {
        GameObject instantStrawBerryObj = Instantiate(berryPrefab, berryGroup);
        instantStrawBerryObj.name = "Berry " + berryList.Count;

        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();
        instantStrawBerry.berryIdx = berryList.Count;

        instantStrawBerry.gameObject.SetActive(false);
        berryList.Add(instantStrawBerry);
    }
    StrawBerry GetStrawBerry(int idx)
    {
        if (farmList[idx].isPlant) return null;

        return berryList[idx];
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        stb.transform.position = obj.transform.position; ; // ���� Transform�� ���⸦ �ɴ´�
        stb.gameObject.SetActive(true); // ���� Ȱ��ȭ              
        coll.enabled = false; // ���� �ݶ��̴��� ��Ȱ��ȭ (���ʿ� �浹 ����)
    }
    void Harvest(StrawBerry berry)
    {
        Farm farm = farmList[berry.berryIdx];
        if (farm.isHarvest) return;

        farm.isHarvest = true;
        Vector2 pos;

        berry.SetAnim(5); // ��Ȯ �̹����� ����
        pos = berry.transform.position;
        berry.Explosion(pos, target.position, 0.5f); // DOTWeen ȿ�� ����

        StartCoroutine(HarvestRoutine(farm)); // �������� ���Ⱑ �ɾ����� ������ ����
    }
    GameObject ClickObj() // Ŭ������ ������Ʈ�� ��ȯ
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // ���� ��� ��Ȱ��ȭ

        yield return new WaitForSeconds(0.65f); // 0.65�� �ڿ�

        UpdateBerryCnt(berryList[farm.farmIdx]);

        yield return new WaitForSeconds(0.25f); // 0.25�� �ڿ�

        farm.isHarvest = false; // ��Ȯ�� ����
        farm.isPlant = false; // ���� ����ش�
        farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ      
    }
    void UpdateBerryCnt(StrawBerry berry)
    {
        truck.berryCnt += berry.route + 1;
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

    public void ShowCoinText(int coin)
    {
        int show = coin;
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
            berryList[i].canGrow = false;
            berryList[i].bug.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    public void EnableObjColliderAll() // ��� ������Ʈ�� collider Ȱ��ȭ
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            if(!farmList[i].isPlant)
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            if(!berryList[i].hasBug)
            {
                berryList[i].canGrow = true;
            }
            berryList[i].bug.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    #endregion

    #region ����Ʈ
    //����Ʈ Ȱ��ȭ ��Ȱ��ȭ===========================================================================================
    //�ߺ�.... �����ʿ� ����

    public void Xbutton()
    {
        //X��ư -> �θ� ������Ʈ ��Ȱ��ȭ, �� �г� ��Ȱ��ȭ
        //X.transform.parent.gameObject.SetActive(false);
    }

    public void ListButton()
    {
        //����Ʈ ��ư -> �ڽ� ������Ʈ Ȱ��ȭ, ���г� Ȱ��ȭ
        //L.transform.GetChild(0).gameObject.SetActive(true);

    }

    public void selectPTJList() // ������ �г��� Ŭ�� ���� �� �г� Ȱ��ȭ
    {
        if (PartTimeList.activeSelf == false)
        {
            PartTimeList.SetActive(true);
            PanelBlack.SetActive(true);
        }        
    }
    public void selectSearchList()
    {
        if (ResearchList.activeSelf == false)
        {
            ResearchList.SetActive(true);
            PanelBlack.SetActive(true);
        }       
    }
    public void selectBerryList()
    {
        if (BerryList.activeSelf == false)
        {
            BerryList.SetActive(true);
            PanelBlack.SetActive(true);
        }       
    }
    #endregion

    #region �г�

    public void selectSettingPanel()
    {
        if (SettingsPanel.activeSelf == false)
        {
            SettingsPanel.SetActive(true);
            PanelBlack.SetActive(true);
        }       
    }
    public void selectCheckPanel()
    {
        if (CheckPanel.activeSelf == false)
        {
            CheckPanel.SetActive(true);
            PanelBlack.SetActive(true);
        }       
    }
    public void selectTruckPanel()
    {
        if (TruckPanel.activeSelf == false)
        {
            TruckPanel.SetActive(true);
            PanelBlack.SetActive(true);
        }       
    }

    public void selectPanelBlack() // ����â Ŭ���� UI ����
    {
        if (SettingsPanel.activeSelf == true)
        {
            SettingsPanel.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (CheckPanel.activeSelf == true)
        {
            CheckPanel.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (PartTimeList.activeSelf == true)
        {
            PartTimeList.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (ResearchList.activeSelf == true)
        {
            ResearchList.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (BerryList.activeSelf == true)
        {
            BerryList.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (TruckPanel.activeSelf == true)
        {
            TruckPanel.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }

    #endregion

    #region �⼮
    public void selectDay1()
    {
        if (Day1 == false)
        {
            Day1_2.SetActive(true);
            Day1_3.SetActive(false);
            Day2_3.SetActive(true);
            Day1 = true;
            Day2 = true;
        }
        else
        {
            Day1_2.SetActive(false);
            Day1_3.SetActive(true);
            Day1 = false;
        }
    }
    public void selectDay2()
    {
        if (Day2 == true)
        {
            Day2_2.SetActive(true);
            Day2_3.SetActive(false);
            Day3_3.SetActive(true);
            Day3 = true;
        }
        else
        {
            Day2_2.SetActive(false);
            Day2_3.SetActive(true);
        }
    }
    public void selectDay3()
    {
        if (Day3 == true)
        {
            Day3_2.SetActive(true);
            Day3_3.SetActive(false);
            Day4_3.SetActive(true);
            Day4 = true;
        }
        else
        {
            Day3_2.SetActive(false);
            Day3_3.SetActive(true);
        }
    }
    public void selectDay4()
    {
        if (Day4 == true)
        {
            Day4_2.SetActive(true);
            Day4_3.SetActive(false);
            Day5_3.SetActive(true);
            Day5 = true;
        }
        else
        {
            Day4_2.SetActive(false);
            Day4_3.SetActive(true);
        }
    }
    public void selectDay5()
    {
        if (Day5 == true)
        {
            Day5_2.SetActive(true);
            Day5_3.SetActive(false);
            Day6_3.SetActive(true);
            Day6 = true;
        }
        else
        {
            Day5_2.SetActive(false);
            Day5_3.SetActive(true);
        }
    }
    public void selectDay6()
    {
        if (Day6 == true)
        {
            Day6_2.SetActive(true);
            Day6_3.SetActive(false);
            Day7_3.SetActive(true);
            Day7 = true;
        }
        else
        {
            Day6_2.SetActive(false);
            Day6_3.SetActive(true);
        }
    }
    public void selectDay7()
    {
        if (Day7 == true)
        {
            Day7_2.SetActive(true);
            Day7_3.SetActive(false);
        }
        else
        {
            Day2_2.SetActive(false);
            Day2_3.SetActive(true);
        }
    }

    public void resetDay()
    {
        //reset��� �߰� ����


    }

    #endregion
}
