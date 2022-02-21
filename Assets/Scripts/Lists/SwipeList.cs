using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeList : MonoBehaviour
{
	[Header("========Swipe List=======")]
	[SerializeField]
	private Scrollbar scrollBar;                    // Scrollbar�� ��ġ�� �������� ���� ������ �˻�
	[SerializeField]
	private float swipeTime = 0.2f;         // �������� Swipe �Ǵ� �ð�
	[SerializeField]
	private float swipeDistance = 10.0f;        // �������� Swipe�Ǳ� ���� �������� �ϴ� �ּ� �Ÿ�
	[SerializeField]
	private float swipeDistance_scrollBar = 0.01f;


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

	private void Start()
	{
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

		//��ũ�ѹ� ��ġ 0Ȥ�� 1�� ����
		//if (scrollBar.value < 0.5f) { scrollBar.value = 0f; }
		//else { scrollBar.value = 1f; }
		Debug.Log(currentPage);
	}

	public void swipeButton(int value) {
		switch (value) {

			case 0: 
				// ���� �������� ���� ���̸� ����
				if (currentPage == 0) return;
				// �������� �̵�. ���� �������� 1 ����
				currentPage--; 
				break;

			case 1: 
				// ���� �������� ������ ���̸� ����
				if (currentPage == maxPage - 1) return;

				// ���������� �̵�. ���� �������� 1 ����
				currentPage++; 
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
			// ��ũ�� ���� ����
			startScroll = scrollBar.value;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			endTouchX = Input.mousePosition.x;
			//��ũ�� ���� ����
			endScroll = scrollBar.value;

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
				// ��ũ�� ���� ����
				startScroll = scrollBar.value;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				endTouchX = touch.position.x;
				//��ũ�� ���� ����
				endScroll = scrollBar.value;

				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{

		// �ʹ� ���� �Ÿ��� �������� ���� Swipe �ȵȴ�.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance || Mathf.Abs(startScroll - endScroll) < swipeDistance_scrollBar)
		{
			// ���� �������� Swipe�ؼ� ���ư���
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}

		


		// Swipe ����
		bool isLeft = startTouchX < endTouchX ? true : false;
		bool isLeft2 = startScroll < endScroll ? true : false;

		// �̵� ������ ������ ��
		if (isLeft == true || isLeft2==false)
		{
			// ���� �������� ���� ���̸� ����
			if (currentPage == 0) return;

			// �������� �̵��� ���� ���� �������� 1 ����
			currentPage--;
		}
		// �̵� ������ �������� ��
		else if(isLeft==false||isLeft2==true)
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


}


