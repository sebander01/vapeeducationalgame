using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiz", menuName = "Quiz/NewQuiz")]
public class Quiz : ScriptableObject
{
    [Header("Question Varients")]
    public string QuestionV1;
    public string QuestionV2;
    public string QuestionV3;

    [Header("Possible Answers")]
    public string fakeAnswer1;
    public string fakeAnswer2;
    public string answer;

    public string answerExplanation;

    public string WorkingQuestion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateQuiz()
    {
        //Generate a random number
        int randomNum = Random.Range(0, 3);
        if (randomNum == 0)
        {
            WorkingQuestion = QuestionV1;
        }
        else if (randomNum == 1)
        {
            WorkingQuestion = QuestionV2;
        }
        else if(randomNum == 2)
        {
            WorkingQuestion = QuestionV3;
        }
        else if(randomNum == 3)
        {
            Debug.Log("It's 3");
        }
    }
}
