using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public GameObject GameoverUi;
    public RectTransform board;
    public CanvasGroup homeButton;
    public CanvasGroup retryButton;
    public CanvasGroup bg;
    private Vector3 originalPos;

    public TextMeshProUGUI tmpText;
    [TextArea]
    public string fullText = "Game\nOver";
    public float typingSpeed = 0.05f;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        tmpText.text = "";
        originalPos = board.anchoredPosition;
        GameoverUi.SetActive(false);
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void RetryBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowGameOver()
    {
        GameoverUi.SetActive(true);
        board.anchoredPosition = new Vector2(0, 1000); // 화면 위에서 시작
        homeButton.alpha = 0;
        retryButton.alpha = 0;
        bg.alpha = 0;

        bg.DOFade(1, 0.3f).OnComplete(() =>
        {
            board.DOAnchorPos(originalPos, 0.6f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                StartCoroutine(StartTyping());

            });
        });

    }


    IEnumerator StartTyping()
    {
        StartCoroutine(TypeText());

        yield return new WaitForSeconds(0.6f);
        homeButton.DOFade(1, 0.3f);
        retryButton.DOFade(1, 0.3f);

    }

    private IEnumerator TypeText()
    {
        tmpText.text = ""; // 초기화

        foreach (char c in fullText)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
}
