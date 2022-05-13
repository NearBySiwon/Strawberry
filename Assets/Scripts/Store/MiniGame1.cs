using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public Scrollbar scrollbar;//��ũ�ѹ�
    public Text count_txt;     //3�� �ؽ�Ʈ
    public Text score_txt;     //����
    public Image quiz_img;     //������� �̹���
    public int quizIndex;      //������� �ε���
    public Image[] answer_img; //������� �̹���(4��)
    public int[] answerIndex;  //������� �ε���(4��)
    public GameObject O;       //O �̹���
    public GameObject X;       //X �̹���
    public Button[] answer_btn;//������� ��ư(4��)
    public Button pause_btn;   //�Ͻ����� ��ư
    public Button exit_btn;    //������ ��ư


    float size;                           //��ũ�ѹ� ������
    int time = 63;                        //��
    int score;                            //����
    List<int> unlockList=new List<int>(); //�رݵ� ���� ��ȣ ����Ʈ

    void Start()
    {
        size = scrollbar.size / 60f;
        for(int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(()=>OnClickAnswerButton(_i));
        }
    }

    void OnEnable()
    {
        StartGame();
    }

    void StartGame()
    {
        //������ ���� ����Ʈ ����
        for (int i = 0; i < 192; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                unlockList.Add(i);
            }
        }

        //3�� ī��Ʈ
        count_txt.gameObject.SetActive(true);
        StartCoroutine(Count3Second());
    }

    IEnumerator Count3Second()
    {
        count_txt.text = (time - 60).ToString();
        yield return new WaitForSeconds(1);
        time -= 1;
        if (time <= 60)
        {
            count_txt.gameObject.SetActive(false);
            StartCoroutine(Timer());
            MakeQuiz();
        }
        else
        {
            StartCoroutine(Count3Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        scrollbar.size -= size;
        time -= 1;
        if (time == 0)
        {
            StopGame();
        }
        else
        {
            StartCoroutine(Timer());
        }
    }

    void MakeQuiz()
    {
        //������� ����� �̹��� ��ġ
        quizIndex = Random.Range(0, unlockList.Count);
        quiz_img.sprite = Globalvariable.instance.berryListAll[quizIndex].GetComponent<Sprite>();

        //������� ����� �̹��� ��ġ
        int randomAnswerIndex = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            if (randomAnswerIndex == i)
            {
                answer_img[i].sprite = quiz_img.sprite;
                answerIndex[i] = quizIndex;
            }
            else
            {
                answerIndex[i] = Random.Range(0, unlockList.Count);
                //�����ε����� �ٸ� ���������̶� �ٸ� �����ȣ ���ö����� ������ȣ�� �̱�
                while (answerIndex[i] == quizIndex)
                {
                    answerIndex[i] = Random.Range(0, unlockList.Count);
                }
                answer_img[i].sprite = Globalvariable.instance.berryListAll[answerIndex[i]].GetComponent<Sprite>();
            }
        }
    }

    public void OnClickAnswerButton(int index)
    {
        //���� : 10�� �߰�, �������� ����
        if (answerIndex[index] == quizIndex)
        {
            O.SetActive(true);
            score += 10;
            score_txt.text = score.ToString()+"��";
        }
        //���� : 10�� �ٱ�
        else
        {
            X.SetActive(true);
            scrollbar.size -= size*10;
            time -= 10;
        }
        if (time > 0)
        {
            Invoke("MakeNextQuiz", 0.3f);
        }
    }

    void MakeNextQuiz()
    {
        O.SetActive(false);
        X.SetActive(false);
        MakeQuiz();
    }

    void StopGame()
    {
        Debug.Log("���� ��");

        //�ð��ʱ�ȭ, ����Ʈ�ʱ�ȭ, ��Ʈȹ��
        time = 63;
        unlockList.Clear();
        DataController.instance.gameData.heart += score / 10;
    }
}
