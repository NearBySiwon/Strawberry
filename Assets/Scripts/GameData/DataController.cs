using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataController : MonoBehaviour
{
    public bool isSaveMode;

    //�̱���
    public static DataController instance = null;

    //���ӵ����� 
    string gameDataFileName = "gameData.json";
    //[HideInInspector]
    public GameData gameData;

    private ParticleSystem rainParticle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //�� ��ȯ�� �Ǵ��� �ı����� �ʵ��� ��
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance!=this)
        {
            //�� ��ȯ�� �Ǿ��µ� �ν��Ͻ��� �����ϴ� ���
            //���� ������ �Ѿ�� �ν��Ͻ��� ����ϱ� ���� 
            //���ο� ���� ���ӿ�����Ʈ ����
            Destroy(this.gameObject);
        }
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        LoadData();
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        if (File.Exists(filePath))
        {
            //json���� �ҷ�����
            //Debug.Log(filePath);
            string jsonData = File.ReadAllText(filePath);

            //������ȭ
            gameData =JsonConvert.DeserializeObject<GameData>(jsonData);
            //gameData = JsonUtility.FromJson<GameData>(jsonData);

            // �� ���ӽð� �ε�
            var main = rainParticle.main;
            main.duration = gameData.rainDuration;

            Debug.Log("�����͸� �ε��߽��ϴ�");
        }
        else
        {
            Debug.Log("���ο� ������ ����");
            gameData = new GameData();
            InitData();
            if(isSaveMode) SaveData();
        }

    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        //������ ����ȭ
        string jsonData = JsonConvert.SerializeObject(gameData);
        //string jsonData = JsonUtility.ToJson(gameData);

        //���ÿ� ����
        File.WriteAllText(filePath, jsonData);

        Debug.Log("���� ���� �Ϸ� ��� : "+filePath);
    }

    public void InitData()
    {
        gameData.cloudSaveTime = new System.DateTime();
        // ��ȭ ����
        gameData.heart = 500;
        gameData.coin = 150000;
        gameData.medal = 0;
        // ���� ����
        gameData.unlockBerryCnt = 0;
        gameData.totalHarvBerryCnt = 0;
        gameData.accCoin = 0;
        gameData.accHeart = 0;
        gameData.accAttendance = 0;
        gameData.mgPlayCnt = 0;

        // ���� ���� �ð�


        //Truck
        gameData.truckBerryCnt = 0;

        //Research
        for (int i = 0; i < gameData.researchLevel.Length; i++) 
        {
            gameData.researchLevel[i] = 0;
        }
        
        //BerryFieldData
        for(int i = 0; i < gameData.berryFieldData.Length; i++)
        {
            gameData.berryFieldData[i] = new BerryFieldData();                       
        }
        //�ʱ� ���� ���� ����
        for (int i = 0; i < 192; i++)
        {
            if (Globalvariable.instance.berryListAll[i] == null) continue;

            if (i < 64)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.CLASSIC_FIRST + i * 3));
            else if (i < 128)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.SPECIAL_FIRST + (i - 64) * 5));
            else
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.UNIQUE_FIRST + (i - 128) * 7));
        }
        // �� ���ӽð� �ʱ�ȭ
        var main = rainParticle.main;
        main.duration = 5.0f;

        //���� �Ʒ� ���� �ʿ�
        //isBerryUnlock
        for (int i = 0; i < 192; i++)
        {   gameData.isBerryUnlock[i] = false;   }
        gameData.isBerryUnlock[0] = true;//ù��° �⺻������ ����

        //��������
        for (int i = 0; i < 6; i++) {   gameData.challengeLevel[i] = 0;   }

        //����
        for (int i = 0; i < 7; i++) {   gameData.newsState[i] = 0;  }

        //����
        for (int i = 0; i < 7; i++){   gameData.isCollectionDone[i] = false;   }

        //PTJ
        for (int i = 0; i < 6; i++) { gameData.PTJNum[i] = 0; }

        //����ǥ !!
        for (int i = 0; i < 7; i++) 
        {    gameData.isNewsEM[i] = false;   }
        for (int i = 0; i < 192; i++)
        {    gameData.isBerryEM[i] = false;    }

        gameData.newBerryResearchAble = 0;
        gameData.newBerryBtnState = 0;

        //���԰��ú���
        gameData.isStoreOpend = false;
        for (int i = 0; i < 4; i++) { gameData.highScore[i] = 0; }
    }
    void OnApplicationQuit()
    {
        //��������� ����
        if(isSaveMode)SaveData();
    }
    
}
