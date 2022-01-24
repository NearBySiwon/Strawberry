using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Object ]")]
    public GameObject Strawberry;
    public GameObject Farm;
    public List<Farm> FarmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    
    public Transform StrawberryGroup;
    public List<StrawBerry> strawPool;
    
    int poolSize = 16;
    public int poolCursor;

    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        strawPool = new List<StrawBerry>();
        
        for (int i = 0; i < poolSize; i++) // ������Ʈ Ǯ������ �̸� ���� ����
        {
            MakeStrawBerry();
        }
    }    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư����
        {           
            StrawBerry stb = GetStrawBerry(); // ���⸦ Ǯ������ �����´�
            if(stb != null)
            {
                GameObject farmObj = ClickObj(); // Ŭ���� ������Ʈ�� �����´�
                if(farmObj != null && !farmObj.GetComponent<Farm>().isPlant) // ���� Ŭ�� �Ǿ����� �翡 ���Ⱑ �ɾ��� ���� �ʴٸ�
                {
                    PlantStrawBerry(stb, farmObj); // �ɴ´�
                    farmObj.GetComponent<Farm>().isPlant = true; // üũ ���� ����
                }                                  
            }                
        }
        if(Input.GetMouseButtonDown(1)) // ������ ���콺 ��ư����
        {
            Harvest(); // ��Ȯ
        }
    }
    void MakeStrawBerry() // ���� ����
    {
        GameObject instantStrawBerryObj = Instantiate(Strawberry, StrawberryGroup);
        instantStrawBerryObj.name = "StrawBerry " + strawPool.Count;
        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();

        instantStrawBerry.gameObject.SetActive(false);
        strawPool.Add(instantStrawBerry);       
    }
    StrawBerry GetStrawBerry()
    {
        poolCursor = 0;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (!strawPool[poolCursor].gameObject.activeSelf)
            {
                return strawPool[poolCursor]; // ��Ȱ��ȭ�� ���� ��ȯ
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
        return null; // ���Ⱑ 16�� ���� á�ٸ� null ��ȯ
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        stb.gameObject.SetActive(true); // ���� Ȱ��ȭ      
        stb.transform.position = obj.transform.position; // ���� Transform�� ���⸦ �ɴ´�
    }
    void Harvest()
    {
        poolCursor = 0;
        Vector2 pos;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (strawPool[poolCursor].gameObject.activeSelf && !strawPool[poolCursor].canGrow) // ���Ⱑ Ȱ��ȭ �� ���� && �� �ڶ� ������
            {
                StrawBerry stb = strawPool[poolCursor];

                stb.SetAnim(5); // ��Ȯ �̹����� ����
                pos = stb.transform.position;
                stb.Explosion(pos, target.position, 0.5f); // DOTWeen ȿ�� ����
                FarmList[poolCursor].isPlant = false; // ���� ����ش�
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
    }
    GameObject ClickObj() // Ŭ������ ������Ʈ�� ��ȯ
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if(hit.collider == null) return null;

        return hit.collider.gameObject;
    }
}
