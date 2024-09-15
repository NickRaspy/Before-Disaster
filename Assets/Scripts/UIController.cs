using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] private Text debugText;

    [SerializeField] private GameObject block;
    [Header("Main")]
    [SerializeField] private Text gameTimerText;
    [SerializeField] private Text interactionLeftText;

    [Header("Finish Screen")]
    [SerializeField] private GameObject finishGameScreen;
    [SerializeField] private Text finishText;
    public List<FinishText> finishTexts = new();

    [Header("Action")]
    [SerializeField] private Text interactableText;
    [SerializeField] private Text warnText;
    [SerializeField] private string warnTextAnimationState;
    [SerializeField] private Slider actionTimer;


    private GameTimer gameTimer;
    private void Start()
    {
        gameTimer = new(gameTimerText);

        gameManager.UIController = this;
        gameManager.gameTimer = gameTimer;
    }

    public void BlockEnable() => block.SetActive(true);
    public void ChangeInteractableText(string text) => interactableText.text = text;
    public void WarnTextAppear()
    {
        if (warnText.GetComponent<Animator>() == null) StartCoroutine(ShowObjectOnTime(warnText.gameObject));
        else
        {
            warnText.GetComponent<Animator>().Play(warnTextAnimationState);
        }
    }
    public void ChangeInteractionLeftText(string text) => interactionLeftText.text = text;
    public void ShowFinishScreen() => finishGameScreen.SetActive(true);
    public void SelectFinishText(float percentage)
    {
        string text = string.Empty;
        List<float> percentages = new();
        foreach (FinishText finishText in finishTexts)
        {
            if(percentage <= finishText.requiredPercentage)
            {
                text = finishText.text;
                break;
            }
            percentages.Add(finishText.requiredPercentage);
        }
        if (text == string.Empty)
        {
            float maxPercentage = percentages.Max();
            Debug.Log(maxPercentage);
            text = finishTexts.Find(x => x.requiredPercentage == maxPercentage).text;
        }
        finishText.text = text;
    }
    IEnumerator ShowObjectOnTime(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(3f);
        go.SetActive(false);
    }
    public void SetDebugText(string text) => debugText.text = text;
}
[Serializable]
public class FinishText
{
    public string text;
    [Range(0f,1f)] public float requiredPercentage;
}