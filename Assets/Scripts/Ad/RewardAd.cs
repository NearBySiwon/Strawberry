using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button showAdButton;
    [SerializeField] string androidUnitId = "Rewarded_Android";
    //[SerializeField] string iOSUnitId = "Rewarded_iOS";
    string adUnitId = null; //This will remain null for unsupported platforms

    void Awake()
    {
        showAdButton.onClick.AddListener(ShowAd);
    }

    void Start()
    {
#if UNITY_IOS
        //adUnitId = iOSUnitId;
#elif UNITY_ANDROID
        adUnitId = androidUnitId;
#endif
        //�ȵ�, ios ���� �÷����̸� ��ư �ȴ����� ó��
        showAdButton.interactable = false;

        StartCoroutine(LoadAd());
    }


    //Load content to the Ad Unit
    IEnumerator LoadAd()
    {
        WaitForSeconds wait = new WaitForSeconds(.5f);
        while (!Advertisement.isInitialized)
        {
            //���� �ε��� �˾� ����
            yield return wait;
        }
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("������ ���� �ε� �Ϸ�");
        showAdButton.interactable = true;
    }

    public void ShowAd()
    {
        showAdButton.interactable = false;
        Advertisement.Show(adUnitId,this);

        //�ӽ���ġ : UnityAds 4.0���� - �ݹ鵿��x ���۳ѵ� ..........�� Ȯ���ϰ� ������ �ø�������
        showAdButton.interactable = true;
        GameManager.instance.GetCoin(DataController.instance.gameData.truckCoin * 3);
        Debug.Log("����ȹ��");
        DataController.instance.gameData.truckBerryCnt = 0;
        DataController.instance.gameData.truckCoin = 0;
        StartCoroutine(LoadAd());//����ε�
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"���� �ε� ����: {error}-{message}");
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId == placementId && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            //������ ���� - �ٸ����� �߰��Ǹ� �ڵ� �����ؾ� ��
            showAdButton.interactable = true;
            GameManager.instance.GetCoin(DataController.instance.gameData.truckCoin*3);
            Debug.Log("����ȹ��");
            DataController.instance.gameData.truckBerryCnt = 0;
            DataController.instance.gameData.truckCoin = 0;
            StartCoroutine(LoadAd());//����ε�
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"���� �����ֱ� Error : {error}-{message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }

    /*void OnDestroy()
    {
        showAdButton.onClick.RemoveAllListeners();
    }*/
}
