using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;

    public AudioSource bgSound;
    public AudioClip[] bglist;//����� ����Ʈ
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

    //���� ���� ����� ����
    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1) {
        for (int i = 0; i < bglist.Length; i++) {
            if (arg0.name == bglist[i].name)//���̸��� ���� ����� Ʋ��
            {
                BgSoundPlay(bglist[i]);//���
            }
        }
    
    }
    //ȿ����
    public void SFXPlay(string sfxName,AudioClip clip) {

        GameObject go = new GameObject(sfxName+"Sound");//�ش��̸��� ���� ������Ʈ�� �Ҹ� �������� �����ȴ�.
        
        //����� ���
        AudioSource audioSource = go.AddComponent<AudioSource>();//�� ������Ʈ�� ����� �ҽ� ������Ʈ �߰�
        audioSource.clip = clip;//����� �ҽ� Ŭ�� �߰�
        audioSource.Play();//���

        Destroy(go, clip.length);//ȿ���� ��� ��(clip.length �ð� ������) �ı�
    
    }
    /*�ٸ���ũ��Ʈ�� ����
    public AudioClip clip;
    AudioManager.instance.SFXPlay("Hook",clip);
    */

    public void BgSoundPlay(AudioClip clip) {
        if (isPlayAudio == true)
        {
            bgSound.clip = clip;
            bgSound.loop = false;
            bgSound.volume = 0.1f;
            bgSound.Play();
        }
    }
}
