using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame; //cloudSave
using Firebase.Auth;    //login
using Newtonsoft.Json; //serialize/deserialize
using System.Text;     //for encoding
using System;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager instance=null;
    public Action OnSaveSucceed;

    FirebaseAuth auth = null; //auth�� instance
    FirebaseUser user = null; //���� ����� ����
    string authCode = "";

    ISavedGameClient SavedGame => PlayGamesPlatform.Instance.SavedGame;
    string fileName = "gameData";
    bool saving;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //�ʱ�ȭ
        if (Application.platform==RuntimePlatform.Android) Init();
    }

    void Init()
    {
        var config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames() //���Ӽ��̺�
            .RequestServerAuthCode(false) //playgames Ŭ���̾�Ʈ ����
            .Build();

        //GPGS�ʱ�ȭ
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        //�����÷��̰��� Ȱ��ȭ
        PlayGamesPlatform.Activate();

        //�������� �α���
        Login();
    }


    //login
    void Login()
    {
        //���� �α���
        Social.localUser.Authenticate((success, error) =>
        {
            if (!success) Debug.Log("�α��� ���� : " + error);
            else
            {
                //firebase ����� ���� ������ ����Ͽ� �÷��̾� ����
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                auth = FirebaseAuth.DefaultInstance;
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("���̾�̽� ���� ��ҵ�");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("���̾�̽� ���� ����: " + task.Exception);
                        return;
                    }
                    user = task.Result;
                    Debug.Log(user.DisplayName+"�� �α���!");
                });
            }
        });
    }

    //logout
    void Logout()
    {
        auth.SignOut();
    }


    //overwrites old file or saves a new one
    public void SaveToCloud()
    {
        if (Application.platform == RuntimePlatform.Android) {
            if (user != null)
            {
                Debug.Log("Ŭ����� ���� �����͸� �����մϴ�...");
                saving = true;
                SavedGame.OpenWithAutomaticConflictResolution(
                    fileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SavedGameOpened);
            }
            else
            {
                Debug.Log("�α����� �Ǿ����� ����");
            }
        }
        else print("�ȵ���̵忡�� �׽�Ʈ ���ּ���");
    }


    //load from cloud
    public void LoadFromCloud()
    {
        if (Application.platform == RuntimePlatform.Android) {
            Debug.Log("Ŭ����κ��� ���� �����͸� �����ɴϴ�...");
            saving = false;
            if (user != null)
            {
                SavedGame.OpenWithAutomaticConflictResolution(
                    fileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SavedGameOpened);
            }
            else
            {
                Debug.Log("�α����� �Ǿ����� ����");
            }
        }
        else print("�ȵ���̵忡�� �׽�Ʈ ���ּ���");
    }


    //�����ϰų� �ε�
    private void SavedGameOpened(SavedGameRequestStatus state, ISavedGameMetadata game)
    {
        //check success
        if (state==SavedGameRequestStatus.Success)
        {
            if (saving)
            {
                //saving
                //read bytes from save
                //gameData to string -> byte[]
                //�̷����ص� �Ǵ��� Ȯ���ʿ�!!!!!
                string saveString=JsonConvert.SerializeObject(DataController.instance.gameData); 
                byte[] data = Encoding.UTF8.GetBytes(saveString);

                //create builder. ..�̰� �� �ؾ��ϴ°ǰ�?
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                SavedGameMetadataUpdate updateMetaData = builder.Build();

                //saving to cloud
                SavedGame.CommitUpdate(game, updateMetaData, data, CheckSaving);
            }
            else 
            {
                //loading
                SavedGame.ReadBinaryData(game, CheckLoading);
            }
        }
        else
        {
            Debug.Log("Error opening game : "+state);
        }
    }


    //callback from SavedGameOpened. Check saving result was successful or not
    private void CheckSaving(SavedGameRequestStatus state, ISavedGameMetadata game)
    {
        Debug.Log("Ŭ���� ���� Ȯ����...");
        if (state == SavedGameRequestStatus.Success)
        {
            Debug.Log("Ŭ���� ���� ����");

            //��¥ ����
            DataController.instance.gameData.cloudSaveTime = System.DateTime.Now;
            //��������
            DataController.instance.SaveData();
            //����â ��¥ ���� - Ŭ���� ���� �� ����Ǿ�� �ϹǷ� �׼����� ����
            OnSaveSucceed();
        }
        else Debug.LogError("Ŭ���� ���� ���� : " + state);
    }


    //callback from SavedGameOpened. Check loading result was successful or not
    private void CheckLoading(SavedGameRequestStatus state, byte[] cloudData)
    {
        Debug.Log("Ŭ���� �ε� Ȯ����...");
        if (state == SavedGameRequestStatus.Success)
        {
            Debug.Log("�ε� ����");
            if (cloudData ==null)
            {
                Debug.Log("�ҷ��� �����Ͱ� �������� ����");
                return;
            }
            else
            {
                //Ŭ���� ������ ���������Ϳ� �ְ� ��������..�̷��� �ص� �ٷ� �ݿ� �Ǵ°��� Ȯ���ؾ� ��
                //byte[] to gameData
                Debug.Log("�����Ͱ�����");
                DataController.instance.gameData = JsonConvert.DeserializeObject<GameData>(Encoding.UTF8.GetString(cloudData));
                DataController.instance.SaveData();
            }
        }
        else
        {
            Debug.Log("�ε� ���� : "+state);
        }
    }

    //public void DeleteCloud()
    //{

    //}
}
