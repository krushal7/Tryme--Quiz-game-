using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
#pragma warning disable 649
   
    [SerializeField] private QuizGameUI quizGameUI;
    
    [SerializeField] private List<QuizDataScriptable> quizDataList;
    [SerializeField] private float timeLimit = 180f;
    
#pragma warning restore 649

    private string currentCategory = "";
    private int correctAnswerCount = 0;
    //questions data
    private List<Question> questions;
    
    private Question selectedQuetion = new Question();
    private int gameScore;
    private int lifesRemaining;
    private float currentTime;
    private QuizDataScriptable dataScriptable;

    private GameStatus gameStatus = GameStatus.NEXT;

    public GameStatus GameStatus { get { return gameStatus; } }
   

    public List<QuizDataScriptable> QuizData { get => quizDataList; }
    public void StartGame(int categoryIndex, string category)
    {
        currentCategory = category;
        correctAnswerCount = 0;
        gameScore = 0; //game score to 0
        lifesRemaining = 3;
        currentTime = timeLimit;
        questions = new List<Question>();
        dataScriptable = quizDataList[categoryIndex];
        questions.AddRange(dataScriptable.questions);
        //select the question
        SelectQuestion();
        gameStatus = GameStatus.PLAYING;
        quizGameUI.ScoreText.text = "Score: " + gameScore;

    }


    
    private void SelectQuestion()
    {
        
        int val = UnityEngine.Random.Range(0, questions.Count);
        
        selectedQuetion = questions[val];
        //send the question to quizGameUI
        quizGameUI.SetQuestion(selectedQuetion);

        questions.RemoveAt(val);
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);                       
        quizGameUI.TimerText.text = time.ToString("mm':'ss");   //convert time to Time format

        if (currentTime <= 0)
        {
            //Game Over
            GameEnd();
        }
    }


    /// <param name="selectedOption">answer string</param>
    
    public bool Answer(string selectedOption)
    {
        
        bool correct = false;
        
        if (selectedQuetion.correctAns == selectedOption)
        {
            //Yes, Ans is correct
            correctAnswerCount++;
            correct = true;
            gameScore += 50;
            quizGameUI.ScoreText.text = "Score:" + gameScore;
        }
        else
        {
            //No, Ans is wrong
            //Reduce Life
            lifesRemaining--;
            quizGameUI.ReduceLife(lifesRemaining);

            if (lifesRemaining == 0)
            {
                GameEnd();
            }
        }

        if (gameStatus == GameStatus.PLAYING)
        {
            if (questions.Count > 0)
            {
                
                Invoke("SelectQuestion", 0.4f);
            }
            else
            {
                GameEnd();
            }
        }
        
        return correct;
    }

    private void GameEnd()
    {
        gameStatus = GameStatus.NEXT;
        quizGameUI.GameOverPanel.SetActive(true);
        quizGameUI.GameOverScoreText.text = "Total Score: " + gameScore;
         
    }
}


[System.Serializable]
public class Question
{
    public string questionInfo;         
    public QuestionType questionType;   
    public Sprite questionImage;        
    public List<string> options;        
    public string correctAns;           
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE
}

[SerializeField]
public enum GameStatus
{
    PLAYING,
    NEXT
}