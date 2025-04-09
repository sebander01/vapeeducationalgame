using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManage : MonoBehaviour
{
    //Holds the question
    public GameObject question;
    public GameObject A1;
    public GameObject A2;
    public GameObject A3;
    public GameObject Explanation;

    //Holds the answer
    public string answer;
    public string fakeAnswer1;
    public string fakeAnswer2;

    //Question Canvas
    public GameObject QuestionUI;
    public GameObject WrongAnswerUI;

    //Quiz buttons
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button bExit;

    //List of questions
    public List<Quiz> quizList;
    public string explanation;

    public List<GameObject> obsticle;
    private bool canPress = true;

    //Sound
    public AudioClip rightAudio;
    public AudioClip wrongAudio;
    public AudioClip buttonAudio;
    public AudioClip factPrompt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextQuestion()
    {
        AudioSource.PlayClipAtPoint(factPrompt, GameObject.Find("Player").gameObject.transform.position);
        canPress = true;
        //Open quiz ui
        QuestionUI.SetActive(true);
        //Find the quiz at the top of the list and find it's answer
        //Generate the question
        quizList[0].GenerateQuiz();
        //Get the working quesiton
        question.GetComponent<TextMeshProUGUI>().text = quizList[0].WorkingQuestion;
        //Get the answer
        answer = quizList[0].answer;
        fakeAnswer1 = quizList[0].fakeAnswer1;
        fakeAnswer2 = quizList[0].fakeAnswer2;
        explanation = quizList[0].answerExplanation;

        try
        {
            quizList.Remove(quizList[0]);
        }
        catch
        {
            quizList.Clear();
        }
            //Random number
            int randomNum = Random.Range(0, 3);
            int num = 0;
            num = randomNum;

            try{
                Button1.onClick.RemoveAllListeners();
                Button2.onClick.RemoveAllListeners();
                Button3.onClick.RemoveAllListeners();
            }
            catch
            {
                Debug.Log("No Listeners");
            }
            //Add a random listener
            if(num == 0)
            {
                Debug.Log(num);
                A1.GetComponent<TextMeshProUGUI>().text = answer;
                A2.GetComponent<TextMeshProUGUI>().text = fakeAnswer1;
                A3.GetComponent<TextMeshProUGUI>().text = fakeAnswer2;
                Button1.onClick.AddListener(RightAnswer);
                Button2.onClick.AddListener(WrongAnswer);
                Button3.onClick.AddListener(WrongAnswer);
            }
            else if (num == 1)
            {
                Debug.Log(num);
                A1.GetComponent<TextMeshProUGUI>().text = fakeAnswer1;
                A2.GetComponent<TextMeshProUGUI>().text = answer;
                A3.GetComponent<TextMeshProUGUI>().text = fakeAnswer2;
                Button1.onClick.AddListener(WrongAnswer);
                Button2.onClick.AddListener(RightAnswer);
                Button3.onClick.AddListener(WrongAnswer);
            }
            else if (num == 2)
            {
                Debug.Log(num);
                A1.GetComponent<TextMeshProUGUI>().text = fakeAnswer1;
                A2.GetComponent<TextMeshProUGUI>().text = fakeAnswer2;
                A3.GetComponent<TextMeshProUGUI>().text = answer;
                Button1.onClick.AddListener(WrongAnswer);
                Button2.onClick.AddListener(WrongAnswer);
                Button3.onClick.AddListener(RightAnswer);
            }
            else
            {
                Debug.Log("There was a mistake here");
            }
    }

    public void RightAnswer()
    {
        if(canPress)
        {
            canPress = false;
            QuestionUI.SetActive(false);
            AudioSource.PlayClipAtPoint(rightAudio, GameObject.Find("Player").gameObject.transform.position);
            //Start time
            GameObject.Find("Player").GetComponent<PlayerManager>().timeTicks = true;
            GameObject.Find("Player").GetComponent<PlayerManager>().StopandStartTime();

            //Disable obsticle
            obsticle[0].SetActive(false);
            obsticle.Remove(obsticle[0]);
            //Boost the player speed
            GameObject.Find("Player").GetComponent<PlayerManager>().speedBoost += (float)0.5;
            Debug.Log("Winner");
            GameObject.Find("Player").GetComponent<PlayerManager>().allowMovement = true;
        }
    }

    public void WrongAnswer()
    {
        if(canPress)
        {
            Debug.Log("Loser");
            canPress = false;
            AudioSource.PlayClipAtPoint(wrongAudio, GameObject.Find("Player").gameObject.transform.position);
            //Stop Time
            GameObject.Find("Player").GetComponent<PlayerManager>().timeTicks = false;
            GameObject.Find("Player").GetComponent<PlayerManager>().StopandStartTime();
            QuestionUI.SetActive(false);
            WrongAnswerUI.SetActive(true);
            Explanation.GetComponent<TextMeshProUGUI>().text = explanation;
        }
    }

    public void ExitQuiz()
    {
        //Start Time
        AudioSource.PlayClipAtPoint(buttonAudio, GameObject.Find("Player").gameObject.transform.position);
        GameObject.Find("Player").GetComponent<PlayerManager>().StopandStartTime();
        GameObject tempName = obsticle[0];
        obsticle[0].SetActive(false);
        obsticle.Remove(obsticle[0]);
        tempName.SetActive(true);
        WrongAnswerUI.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerManager>().allowMovement = true;
    }
}
