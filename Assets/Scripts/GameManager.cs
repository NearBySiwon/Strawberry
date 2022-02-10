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
    internal object count;

    [Header("------------[Check/Settings Panel]")]
    public GameObject Setting;
    public GameObject Check;

    void Awake()
    {

        Application.targetFrameRate = 60;
        berryList = new List<StrawBerry>();
        truck = TruckObj.GetComponent<Truck>();
        target = TruckObj.GetComponent<Transform>();

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
    public void selectTruckPanel()
    {
        if (TruckPanel.activeSelf == false)
        {
            TruckPanel.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            TruckPanel.SetActive(false);
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
        else if (TruckPanel.activeSelf == true)
        {
            TruckPanel.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }
}
