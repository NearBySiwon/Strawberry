using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Stem : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 stemPos;

    //
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
    public GameObject berryPrefabNow;
    public Berry instantBerry;
    public Bug stemBug;

    public int stemIdx;
    public int seedAnimLevel; // ���� ���� �����ؼ� ���� �����Ϳ� �Ѱ������

    public float randomTime = 0f;
    public int[] berryRankProb = { 50, 35, 15 }; //���� �� �ű�

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];
    }
    private void OnEnable()
    {
        stemPos = transform.position;
        randomTime = Random.Range(7.0f, 18.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        // ���� �ʱ�ȭ
        canGrow = true;
        createTime = 0f;
        randomTime = 0f;

        // ���� Ʈ������ �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;
        berryPrefabNow = null;
        instantBerry = null;
    }
    void Update() // �ð��� ���� ���� ����
    {
        if (canGrow)
        {
            createTime += Time.deltaTime;
            if (randomTime <= createTime)
            {
                stemBug.GenerateBug();
                randomTime = 200f;
            }
            if (5.0f <= createTime && createTime < 10.0f)
            {
                if (seedAnimLevel == 1) return;
                SetAnim(1);
            }
            else if (10.0f <= createTime && createTime < 15.0f)
            {
                if (seedAnimLevel == 2) return;

                SetAnim(2);
            }
            else if (15.0f <= createTime && createTime < 20.0f)
            {
                if (seedAnimLevel == 3) return;
                SetAnim(3);
            }
            else if (createTime >= 20.0f)
            {
                SetAnim(4);
                canGrow = false;
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // ���� �ݶ��̴� �ٽ� Ȱ��ȭ
            }
        }
    }
    public void SetAnim(int level)
    {
        this.seedAnimLevel = level;
        anim.SetInteger("Seed", seedAnimLevel);

        if (this.seedAnimLevel == 0)
        {
            transform.position = new Vector2(stemPos.x, stemPos.y + 0.05f);
        }
        else if (this.seedAnimLevel == 1)
        {
            sprite.sortingOrder = 0;
            transform.position = new Vector2(stemPos.x - 0.12f, stemPos.y + 0.26f);
        }
        else if (this.seedAnimLevel == 2)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.32f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.1f);
        }
        else if (this.seedAnimLevel == 3)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.35f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
        }
        else if (this.seedAnimLevel == 4)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.35f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
        }
    }

    void DefineBerry() // ���� Ȯ�������� ������ ���� ����
    {
        int cumulative = 0, probSum = 0;
        int len = GameManager.instance.berryPrefabListUnlock.Count;

        for (int i = 0; i < len; i++)
        {
            probSum += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb; // �رݵ� ���⿡�� ������ �߻�Ȯ���� ������
        }
        int berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            cumulative += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb;
            if (berryRandomChance <= cumulative)
            {
                berryPrefabNow = GameManager.instance.berryPrefabListUnlock[i];
                break;
            }
        }
    }
    void MakeBerry() // ���� ����
    {
        // �ε�ɶ� isPlant�� ���� ����ް� or instantBerry�� null�� �ƴϸ� ����
        GameObject instantBerryObj = Instantiate(berryPrefabNow, this.transform);
        instantBerryObj.name = berryPrefabNow.name;

        instantBerry = instantBerryObj.GetComponent<Berry>();
        instantBerry.gameObject.SetActive(false);
    }
}
