using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Object ]")]
    public GameObject Strawberry; // ������
    public GameObject Farm; // ������
    
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    
    public Transform berryGroup;
    public List<StrawBerry> strawList;
  
    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]

    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;

    
    void Awake()
    {
        Application.targetFrameRate = 60;
        strawList = new List<StrawBerry>();
        
        for (int i = 0; i < 16; i++) // ������Ʈ Ǯ������ �̸� ���� ����
        {
            MakeStrawBerry();            
        }
    }    
    void Update()
    {
        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư����
        {
            GameObject obj = ClickObj(); // Ŭ������ ������ �����´�
            if(obj != null)
            {
                if(obj.GetComponent<Farm>() != null)
                {
                    Farm farm = obj.GetComponent<Farm>();
                    if (!farm.isPlant)
                    {
                        StrawBerry stb = GetStrawBerry();
                        if (stb != null)
                        {
                            PlantStrawBerry(stb, obj); // �ɴ´�
                            stb.farmIdx = farm.farmIdx; // ����� ���� ����
                            farm.berryIdx = stb.berryIdx; // ��� ���⸦ ����

                            farm.GetComponent<Farm>().isPlant = true; // üũ ���� ����
                        }
                    }
                    else
                    {
                        if(!strawList[farm.berryIdx].canGrow)
                        {
                            Harvest(strawList[farm.berryIdx]); // ��Ȯ
                        }
                    }
                }                
            }
        }       
    }
    void MakeStrawBerry() // ���� ����
    {
        GameObject instantStrawBerryObj = Instantiate(Strawberry, berryGroup);
        instantStrawBerryObj.name = "StrawBerry " + strawList.Count;

        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();
        instantStrawBerry.berryIdx = strawList.Count;

        instantStrawBerry.gameObject.SetActive(false);
        strawList.Add(instantStrawBerry);       
    }    
    StrawBerry GetStrawBerry()
    {       
        for (int i = 0; i < strawList.Count; i++)
        {
            if (!strawList[i].gameObject.activeSelf)
            {
                return strawList[i]; // ��Ȱ��ȭ�� ���� ��ȯ
            }            
        }
        return null; // ���Ⱑ 16�� ���� á�ٸ� null ��ȯ
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        stb.gameObject.SetActive(true); // ���� Ȱ��ȭ      
        stb.transform.position = obj.transform.position; // ���� Transform�� ���⸦ �ɴ´�
    }
    void Harvest(StrawBerry berry)
    {       
        Vector2 pos;
        Farm farm = farmList[berry.farmIdx];

        berry.SetAnim(5); // ��Ȯ �̹����� ����
        pos = berry.transform.position;
        berry.Explosion(pos, target.position, 0.5f); // DOTWeen ȿ�� ����
        farm.isPlant = false; // ���� ����ش�

        StartCoroutine(HarvestRoutine(farm)); // �������� ���Ⱑ �ɾ����� ������ ����
    }
    GameObject ClickObj() // Ŭ������ ������Ʈ�� ��ȯ
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if(hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // ���� ��� ��Ȱ��ȭ

        yield return new WaitForSeconds(0.5f); // 0.5�� �ڿ�

        farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ
    }


    //����Ʈ Ȱ��ȭ ��Ȱ��ȭ===========================================================================================
    //�ߺ�.... �����ʿ� ����
    public void selectPTJList()
    {
        if (PartTimeList.activeSelf == false)
            PartTimeList.SetActive(true);
        else
            PartTimeList.SetActive(false);

    }
    public void selectSearchList()
    {
        if (ResearchList.activeSelf == false)
            ResearchList.SetActive(true);
        else
            ResearchList.SetActive(false);
    }

    public void selectBerryList()
    {
        if (BerryList.activeSelf == false)
            BerryList.SetActive(true);
        else
            BerryList.SetActive(false);
    }
}
