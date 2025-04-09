using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string sceneSwitch;

    public AudioClip buttonSound;

    public AudioClip music;
    private int time = 0;

    private void Awake()
    {
        UnityEngine.Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(music, this.gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //This is a very sloppy way to loop our music track
        //Use a debug log to figure out the eact ticks needed until the song stops because nothing else was working
        if(time > 8800)
        {
            time = 0;
            music.UnloadAudioData();
            AudioSource.PlayClipAtPoint(music, this.gameObject.transform.position);
        }
        else
        {
            time += 1;
        }
    }

    public void QuitGame()
    {
        AudioSource.PlayClipAtPoint(buttonSound, this.gameObject.transform.position);
        Application.Quit();
    }

    public void ChangeScene()
    {
        AudioSource.PlayClipAtPoint(buttonSound, this.gameObject.transform.position);
        SceneManager.LoadScene(sceneSwitch);
    }
}
