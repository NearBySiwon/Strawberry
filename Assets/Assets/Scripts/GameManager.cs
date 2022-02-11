using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Money ]")]
    [SerializeField] public int coin;
    [SerializeField] int heart;
    public Text CoinText;
    public Text HeartText;

    [Header("------------[ Object ]")]
    public GameObject berryPrefab; // ������
    public Truck truck;
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]

    public Transform berryGroup;
    public List<StrawBerry> berryList;

    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]   

    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;
    public GameObject PanelBlack;
    internal object count;

    [Header("------------[Check/Settings Panel]")]
    public GameObject Setting;
    public GameObject Check;

    [Header("------------[CheckList]")]
    public bool Day1, Day2, Day3, Day4, Day5, Day6, Day7;
    public GameObject Day1_1, Day1_2, Day1_3, Day2_1, Day2_2, Day2_3, Day3_1, Day3_2, Day3_3, Day4_1, Day4_2, Day4_3
        , Day5_1, Day5_2, Day5_3, Day6_1, Day6_2, Day6_3, Day7_1, Day7_2, Day7_3;



    void Awake()
    {

        Application.targetFrameRate = 60;
        berryList = new List<StrawBerry>();


        for (int i = 0; i < 16; i++) // ������Ʈ Ǯ������ �̸� ���� ����
        {
            MakeStrawBerry();
        }

        // �ӽ� ��ȭ ����
        coin = 100000;
        heart = 300;
        CoinText.text = coin.ToString() + " A";
        HeartText.text = heart.ToString();
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
                else if (obj.GetComponent<Truck>() != null)
                {
                    ClickedTruck();
                }
            }
        }
    }
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
        stb.transform.position = obj.transform.position; ; // ���� Transform�� ���⸦ �ɴ´�
        stb.gameObject.SetActive(true); // ���� Ȱ��ȭ              
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
    public void DisableObjColliderAll() // ��� ������Ʈ�� collider ��Ȱ��ȭ
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            berryList[i].canGrow = false;
        }
    }
    public void EnableObjColliderAll() // ��� ������Ʈ�� collider Ȱ��ȭ
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = true;
            berryList[i].canGrow = true;
        }
    }

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

    public void selectPTJList()
    {
        if (PartTimeList.activeSelf == false)
        {
            PartTimeList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            PartTimeList.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }
    public void selectSearchList()
    {
        if (ResearchList.activeSelf == false)
        {
            ResearchList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            ResearchList.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }
    public void selectBerryList()
    {
        if (BerryList.activeSelf == false)
        {
            BerryList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            BerryList.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }
    public void selectSettingPanel()
    {
        if (Setting.activeSelf == false)
        {
            Setting.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            Setting.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }
    public void selectCheckPanel()
    {
        if (Check.activeSelf == false)
        {
            Check.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            Check.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }


    public void selectPanelBlack() // ����â Ŭ���� UI ����
    {
        if (Setting.activeSelf == true)
        {
            Setting.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (Check.activeSelf == true)
        {
            Check.SetActive(false);
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
    }

    public void selectDay1()
    {
        if (Day1 == false)
        {
            Day1_2.SetActive(true);
            Day1_3.SetActive(false);
            Day1 = true;
            Day2_3.SetActive(true);
        }
    }

    public void selectDay2()
    {
        if (Day1 == true)
        {
            Day2_2.SetActive(true);
            Day2_3.SetActive(false);
            Day2 = true;
            Day3_3.SetActive(true);
        }
    }

    public void selectDay3()
    {
        if (Day2 == true)
        {
            Day3_2.SetActive(true);
            Day3_3.SetActive(false);
            Day3 = true;
            Day4_3.SetActive(true);

        }
    }

    public void selectDay4()
    {
        if (Day3 == true)
        {
            Day4_2.SetActive(true);
            Day4_3.SetActive(false);
            Day4 = true;
            Day5_3.SetActive(true);

        }
    }

    public void selectDay5()
    {
        if (Day4 == true)
        {
            Day5_2.SetActive(true);
            Day5_3.SetActive(false);
            Day5 = true;
            Day6_3.SetActive(true);

        }
    }

    public void selectDay6()
    {
        if (Day5 == true)
        {
            Day6_2.SetActive(true);
            Day6_3.SetActive(false);
            Day6 = true;
            Day7_3.SetActive(true);

        }
    }

    public void selectDay7()
    {
        if (Day6 == true)
        {
            Day7_2.SetActive(true);
            Day7_3.SetActive(false);
            Day7 = true;
        }
    }

}
