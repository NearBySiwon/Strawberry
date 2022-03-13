using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeNews : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;
        public int[] reward;
        public bool isDone_c;
        public bool isUnlock_n;
        public string Exp_n;
        public int Count_c;


        public ChallengeNewsStruct(string Title, int[] reward, bool isDone_c, bool isUnlock_n, string Exp_n, int Count_c)
        {
            this.Title = Title;
            this.reward = reward;
            this.isDone_c = isDone_c;
            this.isUnlock_n = isUnlock_n;
            this.Exp_n = Exp_n;
            this.Count_c = Count_c;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject countText_News;
    [SerializeField]
    private GameObject lock_News;
    [SerializeField]
    private GameObject nowText_Challenge;
    [SerializeField]
    private GameObject Btn_Challenge;
    


    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite[] BtnImage_Challenge;

    [Header("==========��Ÿ==========")]
    [SerializeField]
    private bool isChallenge;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;


    private void Start()
    {
        InfoUpdate();
    }
    public void InfoUpdate() {

        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 7)
        { Prefabcount =0; }
        prefabnum = Prefabcount;



        //����ǥ��
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;


        if (isChallenge == true && Info[prefabnum].isDone_c == false) 
        {
            Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];
        }
        else if (Info[prefabnum].isUnlock_n == false)
        {
            lock_News.SetActive(true);
        }



        Prefabcount++;

    }
}
