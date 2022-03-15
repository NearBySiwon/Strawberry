using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{
    public int weedIdx;

    public float weedProb = 20f; // �ű�
    public float xPos = 0f;   // �ű�   
    public int weedSpriteNum; // �ű�
    public bool isWeedEnable; // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������

    private Farm farm;
    private Stem stem;
    private Animator anim;
    private BoxCollider2D farmColl;
    void Awake()
    {        
        anim = GetComponent<Animator>();
        farm = transform.parent.gameObject.GetComponent<Farm>();

        stem = GameManager.instance.stemList[farm.farmIdx];
        farmColl = farm.GetComponent<BoxCollider2D>();       
    }
    void OnEnable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = true;
        DataController.instance.gameData.berryFieldData[weedIdx].weedSpriteNum = Random.Range(0, 3);
        anim.SetInteger("Generate", weedSpriteNum);
    }
    void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = false;
    }
    
    public void GenerateWeed() // ���� ����
    {
        float prob = Random.Range(0, 100);
        
        //scale = Random.Range(1.3f, 1.8f);
        if (prob < DataController.instance.gameData.berryFieldData[weedIdx].weedProb)
        {
            this.gameObject.SetActive(true); // �� �ڽ�(����)�� Ȱ��ȭ

            farmColl.enabled = false; // ���� �ݶ��̴� ��Ȱ��ȭ
            DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = true; // ���ʺ������θ� Ȯ���ϴ� ����
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = false; // ������ ���� ����

            xPos = Random.Range(-0.35f, 0.35f); // ���� X���� ������ ��ġ�� ���� ����
            transform.position = new Vector2(farm.transform.position.x + xPos, farm.transform.position.y + 0.07f);            
        }
    }
    public void DeleteWeed()
    {
        anim.SetTrigger("Delete");

        StartCoroutine(DisableWeed(0.25f)); // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
    }
    IEnumerator DisableWeed(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false); // ���� ��Ȱ��ȭ

        float creatTime = DataController.instance.gameData.berryFieldData[weedIdx].createTime; // ���Ⱑ ������ �ð����� ����
        if (creatTime == 0f || creatTime >= 20f) // �� ���̰ų� ���Ⱑ ��Ȯ������ ���¶��
        {
            farmColl.enabled = true; // ���� Collider�� �Ҵ�.
        }
        else // �ƴ϶��
        {
            farmColl.enabled = false; // ����.
        }
        DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = false; // ���� ���ŵ�
        if (!DataController.instance.gameData.berryFieldData[weedIdx].hasBug) // ������ ���ٸ�
        {
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = true; // ����� �ٽ� �ڶ� �� �ִ�.
        }
    }
}
