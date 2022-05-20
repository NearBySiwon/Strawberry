using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;

    [Header("object")]
    //�Ҹ� ���� �����̴�
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//����� �ͼ�
    public AudioSource bgSound;//����� �Ŵ���

    //����� �����(����� �̸��� ���̸��� ������� �÷��� ���ݴϴ�)
    public AudioClip[] bglist;

    //ȿ���� �����
    public AudioClip SFXclip;
    public AudioClip SowClip;
    public AudioClip HarvestClip;
    public AudioClip Cute1Clip;
    public AudioClip BlopClip;
    public AudioClip Cute2Clip;
    public AudioClip Cute4Clip;
    public AudioClip TadaClip;
    public AudioClip RainClip;
    public AudioClip RightClip;
    public AudioClip WrongClip;
    public AudioClip DoorOpenClip;
    public AudioClip DoorCloseClip;


    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;//�� ���۽� ����
        }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = 0.5f;
        SFXSoundSlider.GetComponent<Slider>().value = 0.5f;
        BGSoundVolume();
        SFXVolume();
    }
    //���� ���� ����� ����
    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1) {
        for (int i = 0; i < bglist.Length; i++) {
            if (arg0.name == bglist[i].name)//���̸��� ���� ����� Ʋ��
            {
                BgSoundPlay(bglist[i]);//���
            }
        }
    
    }


    public void BGSoundVolume() {

        //����� ������ȭ
        if (BGSoundSlider.GetComponent<Slider>().value == 0)
        { mixer.SetFloat("BGSoundVolume", -80); }
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20);

    }

    public void SFXVolume() {
        //ȿ���� ������ȭ
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) 
        { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

    }
    //ȿ����
    //�̰� �̿ϼ�
    public void SFXPlay(string sfxName,AudioClip clip) {

        GameObject go = new GameObject(sfxName+"Sound");//�ش��̸��� ���� ������Ʈ�� �Ҹ� �������� �����ȴ�.
        
        //����� ���
        AudioSource audioSource = go.AddComponent<AudioSource>();//�� ������Ʈ�� ����� �ҽ� ������Ʈ �߰�
        audioSource.clip = clip;//����� �ҽ� Ŭ�� �߰�
        audioSource.loop = false;
        audioSource.Play();//���

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        Destroy(go, clip.length);//ȿ���� ��� ��(clip.length �ð� ������) �ı�
    
    }



    //��� �ٸ� ��ũ��Ʈ�����ϸ� ���ϼ��ֱ��ѵ� �ʹ� �л�ɱ�� �̷���
    public void SFXAudioPlay() //��ư ȿ����
    { SFXPlay("ButtonSFX", SFXclip); }
    public void HarvestAudioPlay() //��Ȯ ȿ����
    { SFXPlay("HarvestSFX", HarvestClip); }
    public void TadaAudioPlay() 
    { SFXPlay("TadaSFX", TadaClip); }

    //���� ����� �ִ� �Լ�
    public void SFXPlayButton(AudioClip clip) 
    {
        SFXPlay("", clip);
    }

    public void BgSoundPlay(AudioClip clip) {
        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = false;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }
}
