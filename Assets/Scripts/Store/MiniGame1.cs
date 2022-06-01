using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    [Header("Game")]
    public Scrollbar scrollbar;//��ũ�ѹ�
    public Text score_txt;     //����
    public Image quiz_img;     //������� �̹���
    public int quizIndex;      //������� �ε���
    public Button[] answer_btn;//������� ��ư(4��)
    public Image[] answer_img; //������� �̹���(4��)
    public int[] answerIndex;  //������� �ε���(4��)
    public GameObject O;       //O �̹���
    public GameObject X;       //X �̹���


    [Header("UI")]
    public GameObject countImgs;//ī��Ʈ �̹���
    public Button pause_btn;   //�Ͻ����� ��ư
    public Button exit_btn;    //������ ��ư
    public GameObject resultPanel;//����г�
    public Text result_txt;    //��� �ؽ�Ʈ

    float size;                         //��ũ�ѹ� ������
    int time;                           //��
    int score;                          //����
    List<int> unlockList=new List<int>(); //�رݵ� ���� ��ȣ ����Ʈ

    Globalvariable global;
    void Start()
    {
        size = scrollbar.size / 60f;
        for(int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(()=>OnClickAnswerButton(_i));
        }
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
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
        scrollbar.size = 1;
        score = 0;
        time = 64;
        score_txt.text = score.ToString() + "��";

        //4�� ī��Ʈ
        StartCoroutine(Count4Second());
    }

    IEnumerator Count4Second()
    {
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(false);
        time -= 1;
        if (time <= 60)
        {
            quiz_img.gameObject.SetActive(true);
            for(int i = 0; i < 4; i++){ 
                answer_img[i].gameObject.SetActive(true);
                answer_btn[i].enabled = true;
            }
            StartCoroutine(Timer());
            MakeQuiz();
        }
        else
        {
            StartCoroutine(Count4Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        scrollbar.size -= size;
        time -= 1;
        if (time <= 0)
        {
            FinishGame();
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
        quiz_img.sprite = global.berryListAll[quizIndex].GetComponent<SpriteRenderer>().sprite;

        //������ ������� �ε���(0~4)�� ������� ��ġ
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
                //�����ε����� �ٸ� ���������̶� �ٸ� �����ȣ ���ö����� ������ȣ�� �̾Ƽ� ������⿡ ��ġ
                answerIndex[i] = Random.Range(0, unlockList.Count);
                while (CheckIndex(i))
                {
                    answerIndex[i] = Random.Range(0, unlockList.Count);
                }
                answer_img[i].sprite = global.berryListAll[answerIndex[i]].GetComponent<SpriteRenderer>().sprite;
            }
        }

        bool CheckIndex(int idx)
        {
            if (answerIndex[idx] == quizIndex) return true;
            for(int i = 0; i < 4; i++)
            {
                if (i == idx) continue;
                if (answerIndex[idx] == answerIndex[i]) return true;
            }
            return false;
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

    void FinishGame()
    {
        //���� �Ⱥ��̰�
        quiz_img.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++) { 
            answer_img[i].gameObject.SetActive(false);
            answer_btn[i].enabled=false;
        }
        O.SetActive(false);
        X.SetActive(false);

        //�ְ��� ����
        if (DataController.instance.gameData.highScore[0] < score)
        {
            DataController.instance.gameData.highScore[0] = score;
        }

        //����г�
        resultPanel.SetActive(true);
        result_txt.text = "�ְ��� : " + DataController.instance.gameData.highScore[0] + "\n�������� : " + score;

        //��Ʈ����
        DataController.instance.gameData.heart += score / 10;

        //�̴ϰ��� �÷��� Ƚ�� ����
        DataController.instance.gameData.mgPlayCnt++;
    }

    public void ReStart()
    {
        score = 0;
        time = 64;
        unlockList.Clear();
        StartGame();
    }

    public void OnClickPauseButton()
    {
        Time.timeScale = 0;
    }

    public void OnClickKeepGoingButton()
    {
        Time.timeScale = 1;
    }
}
