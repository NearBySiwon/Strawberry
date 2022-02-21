using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeLayer : MonoBehaviour
{

    [Header("====Berry List Buttons====")]
    [SerializeField]
    private GameObject[] tagButtons;
    public Sprite[] tagButtons_Image;
    public Sprite[] tagButtons_selectImage;

    [Header("====ScrollView====")]
    //���� ������Ʈ �θ� ������Ʈ
    [SerializeField]
    private GameObject content1;
    [SerializeField]
    private GameObject content2;

    //��Ʈ�ѹ�
    //[SerializeField]
    //private GameObject scrollBar;


    private GameObject[] target_berry;

    [Header("========Swipe List=======")]
    [SerializeField]
    private Scrollbar scrollBar;                    // Scrollbar�� ��ġ�� �������� ���� ������ �˻�
    [SerializeField]
    private float swipeTime = 0.1f;         // �������� Swipe �Ǵ� �ð�
    [SerializeField]
    private float swipeDistance = 1.0f;        // �������� Swipe�Ǳ� ���� �������� �ϴ� �ּ� �Ÿ�



    private float[] scrollPageValues;           // �� �������� ��ġ �� [0.0 - 1.0]
    private float valueDistance = 0;            // �� ������ ������ �Ÿ�
    private int currentPage = 0;            // ���� ������
    private int maxPage = 2;                // �ִ� ������ 2�� ����

    private float startTouchX;              // ��ġ ���� ��ġ
    private float endTouchX;                    // ��ġ ���� ��ġ

    private float startScroll;
    private float endScroll;

    private bool isSwipeMode = false;       // ���� Swipe�� �ǰ� �ִ��� üũ

    private void Awake()
    {
        // ��ũ�� �Ǵ� �������� �� value ���� �����ϴ� �迭 �޸� �Ҵ�
        scrollPageValues = new float[2];

        // ��ũ�� �Ǵ� ������ ������ �Ÿ�
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

    }
    void Start()
    {
        //ó������ berry classic
        selectBerryTag("berry_classic");

        // ó�� ������ �� 0�� ������ ���δ�.
        SetScrollBarValue(0);
    }


	public void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value = scrollPageValues[index];
	}

	private void Update()
	{
		UpdateInput();

	}

	public void swipeButton(int value)
	{
		switch (value)
		{

			case 0:
				// ���� �������� ���� ���̸� ����
				if (currentPage == 0) return;
				// �������� �̵�. ���� �������� 1 ����
				currentPage--;
				SetScrollBarValue(0);

				break;

			case 1:
				// ���� �������� ������ ���̸� ����
				if (currentPage == maxPage - 1) return;
				// ���������� �̵�. ���� �������� 1 ����
				currentPage++;
				SetScrollBarValue(1);
				break;

		}

	}

	private void UpdateInput()
	{
		// ���� Swipe�� �������̸� ��ġ �Ұ�
		if (isSwipeMode == true) return;

		#if UNITY_EDITOR
		// ���콺 ���� ��ư�� ������ �� 1ȸ
		if (Input.GetMouseButtonDown(0))
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			startTouchX = Input.mousePosition.x;

		}
		else if (Input.GetMouseButtonUp(0))
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			endTouchX = Input.mousePosition.x;


			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				startTouchX = touch.position.x;

			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				endTouchX = touch.position.x;


				UpdateSwipe();
			}
		}
		#endif


	}

	private void UpdateSwipe()
	{

		// �ʹ� ���� �Ÿ��� �������� ���� Swipe �ȵȴ�.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
		{
			// ���� �������� Swipe�ؼ� ���ư���
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
			Debug.Log("swipe");
		}



		// Swipe ����
		bool isLeft = startTouchX < endTouchX ? true : false;

		// �̵� ������ ������ ��
		if (isLeft == true)
		{
			// ���� �������� ���� ���̸� ����
			if (currentPage == 0) return;

			// �������� �̵��� ���� ���� �������� 1 ����
			currentPage--;
		}
		// �̵� ������ �������� ��
		else if (isLeft == false)
		{
			// ���� �������� ������ ���̸� ����
			if (currentPage == maxPage - 1) return;

			// ���������� �̵��� ���� ���� �������� 1 ����
			currentPage++;
		}


		// currentIndex��° �������� Swipe�ؼ� �̵�
		StartCoroutine(OnSwipeOneStep(currentPage));

	}


	// �������� �� �� ������ �ѱ�� Swipe ȿ�� ���
	private IEnumerator OnSwipeOneStep(int index)
	{
		float start = scrollBar.value;
		float current = 0;
		float percent = 0;

		isSwipeMode = true;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

			yield return null;
		}

		isSwipeMode = false;
	}

	public void TagImageChange(int index) {

        //��ư ��������Ʈ �� �ȴ����ŷ�
        for (int i = 0; i < tagButtons_Image.Length; i++) {
            tagButtons[i].GetComponent<Image>().sprite = tagButtons_Image[i];
        }

        //�ش� ��ư ��������Ʈ�� �����ŷ�
        tagButtons[index].GetComponent<Image>().sprite = tagButtons_selectImage[index];
    
    }

    //��ư�� ������ �� �ش� �з��� ���⸦ ���δ�.name=tag�̸�
    public void selectBerryTag(string name)
    {

        //���� ������ ���̰� Ȱ��ȭ
        Active(name);
        
        //�ٸ� ������ �Ⱥ��̰� ��Ȱ��ȭ
        switch (name) {
            case "berry_classic": inActive("berry_special"); inActive("berry_unique"); break;
            case "berry_special": inActive("berry_classic"); inActive("berry_unique"); break;
            case "berry_unique": inActive("berry_special"); inActive("berry_classic"); break;
        }


		//horizontal scrollbar ó������ ������
		//scrollBar.transform.GetComponent<Scrollbar>().value = 0;
		SetScrollBarValue(0);

	}

    public void inActive(string name) {

        //�ش� �±׸� ���� ������Ʈ�� ã�´�.
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //�� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(false);
        }

    }

    public void Active(string name)
    {
        //��� ���� ������Ʈ Ȱ��ȭ
        int iCount = content1.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = content1.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
        int iCount2 = content2.transform.childCount;
        for (int i = 0; i < iCount2; i++)
        {
            Transform trChild2 = content2.transform.GetChild(i);
            trChild2.gameObject.SetActive(true);
        }


        //�ش� ������ ���̰� Ȱ��ȭ
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //�� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(true);
        }

    }


}
