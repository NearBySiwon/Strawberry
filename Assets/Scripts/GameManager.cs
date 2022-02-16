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
    public bool[] Today;
    public ObjectArray[] Front = new ObjectArray[7];


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
                else if (obj.GetComponent<Weed>() != null)
                {
                    ClickedWeed(obj);
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
    void ClickedWeed(GameObject obj)
    {
        Weed weed = obj.GetComponent<Weed>();
        weed.DeleteWeed();
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

        berry.GetComponent<SpriteRenderer>().sortingOrder = 4;
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

        yield return new WaitForSeconds(0.75f); // 0.75�� �ڿ�

        UpdateBerryCnt();

        yield return new WaitForSeconds(0.25f); // 0.25�� �ڿ�

        farm.isHarvest = false; // ��Ȯ�� ����
        farm.isPlant = false; // ���� ����ش�
        if(!farm.hasWeed) // ���ʰ� ���ٸ�
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ 
        }     
    }
    void UpdateBerryCnt()
    {       
        if(truck.berryCnt < (int)Truck.Count.Max)
        {
            truck.berryCnt += 1;
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
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // ��� ������Ʈ�� collider Ȱ��ȭ
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {           
            if (!farmList[i].isPlant && !farmList[i].hasWeed)
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            if(!berryList[i].hasBug && !farmList[i].hasWeed)
            {
                berryList[i].canGrow = true;
            }
            berryList[i].bug.GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true;
            farmList[i].canGrowWeed = true;
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
    #endregion

    #region �⼮

    public void selectDay1()
    {
        if (Today[0] == false)
        {
            Front[0].Behind[1].SetActive(true);
            Front[0].Behind[2].SetActive(false);
            Front[1].Behind[2].SetActive(true);
            Today[0] = true;
            Today[1] = true;
        }
        else
        {
            Front[0].Behind[1].SetActive(false);
            Front[0].Behind[2].SetActive(true);
            Today[0] = false;
        }
    }
    public void selectDay2()
    {
        if (Today[1] == true)
        {
            Front[1].Behind[1].SetActive(true);
            Front[1].Behind[2].SetActive(false);
            Front[2].Behind[2].SetActive(true);
            Today[2] = true;
        }
        else
        {
            Front[1].Behind[1].SetActive(false);
            Front[1].Behind[2].SetActive(true);
        }
    }
    public void selectDay3()
    {
        if (Today[2] == true)
        {
            Front[2].Behind[1].SetActive(true);
            Front[2].Behind[2].SetActive(false);
            Front[3].Behind[2].SetActive(true);
            Today[3] = true;
        }
        else
        {
            Front[2].Behind[1].SetActive(false);
            Front[2].Behind[2].SetActive(true);
        }
    }
    public void selectDay4()
    {
        if (Today[3] == true)
        {
            Front[3].Behind[1].SetActive(true);
            Front[3].Behind[2].SetActive(false);
            Front[4].Behind[2].SetActive(true);
            Today[4] = true;
        }
        else
        {
            Front[3].Behind[1].SetActive(false);
            Front[3].Behind[2].SetActive(true);
        }
    }
    public void selectDay5()
    {
        if (Today[4] == true)
        {
            Front[4].Behind[1].SetActive(true);
            Front[4].Behind[2].SetActive(false);
            Front[5].Behind[2].SetActive(true);
            Today[5] = true;
        }
        else
        {
            Front[4].Behind[1].SetActive(false);
            Front[4].Behind[2].SetActive(true);
        }
    }
    public void selectDay6()
    {
        if (Today[5] == true)
        {
            Front[5].Behind[1].SetActive(true);
            Front[5].Behind[2].SetActive(false);
            Front[6].Behind[2].SetActive(true);
            Today[6] = true;
        }
        else
        {
            Front[5].Behind[1].SetActive(false);
            Front[5].Behind[2].SetActive(true);
        }
    }
    public void selectDay7()
    {
        if (Today[6] == true)
        {
            Front[6].Behind[1].SetActive(true);
            Front[6].Behind[2].SetActive(false);
        }
        else
        {
            Front[6].Behind[1].SetActive(false);
            Front[6].Behind[2].SetActive(true);
        }
    }

    public void ResetDays()
    {
        //reset��� �߰� ����


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
