using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public GameObject LifeObj;
    public TMP_Text ScoreTxt;
    public TMP_Text BestScoreTxt;
    public GameObject failImg;
    public GameObject successImg;
    public delegate void OnPlay(bool isPlay);
    public OnPlay onPlay;

    public float gameSpeed = 2;
    float initSpeed = -1;
    public bool isPlay = false;
    public GameObject playBtn;
    public int Life = 5;
    public int score = 0;
    public List<int> ClearScoreList;
    int curLevel = 0;
    IEnumerator AddScore()
    {
        while(isPlay)
        {
            score++;
            ScoreTxt.text = score.ToString();
            if(score >= ClearScoreList[curLevel])
            {
                curLevel++;
                gameSpeed++;
                if (curLevel >= ClearScoreList.Count)
                    Clear();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    void Clear()
    {
        playBtn.SetActive(true);
        isPlay = false;
        failImg.SetActive(false);
        successImg.SetActive(true);
        BestScoreTxt.gameObject.SetActive(true);

        onPlay.Invoke(isPlay);
        StopCoroutine(AddScore());
        if (PlayerPrefs.GetInt("BestScore", 0) < score)
            PlayerPrefs.SetInt("BestScore", score);
        BestScoreTxt.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    public void PlayBtn()
    {
        BestScoreTxt.text = "0";
        Life = 5;
        score = 0;
        curLevel = 0;
        if (initSpeed == -1)
            initSpeed = gameSpeed;
        gameSpeed = initSpeed;
        for (int i = 0; i < 5; i++)
            LifeObj.transform.GetChild(i).gameObject.SetActive(true);
        playBtn.SetActive(false);
        failImg.SetActive(false);
        successImg.SetActive(false);
        BestScoreTxt.gameObject.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);
        StartCoroutine(AddScore()) ;
    }

    public void GameOver()
    {
        playBtn.SetActive(true);
        isPlay = false;
        failImg.SetActive(true);
        successImg.SetActive(false);
        BestScoreTxt.gameObject.SetActive(true);

        onPlay.Invoke(isPlay);
        StopCoroutine(AddScore());
        if (PlayerPrefs.GetInt("BestScore", 0) < score)
            PlayerPrefs.SetInt("BestScore", score);
        BestScoreTxt.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    public void Collision()
    {
        Life--;
        LifeObj.transform.GetChild(Life).gameObject.SetActive(false);
        if (Life == 0)
            GameOver();
    }
}
