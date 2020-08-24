using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Crosstales.RTVoice;
public class GamManager : MonoBehaviour
{
    public Transform UnArrangedParent;
    public Transform ArrangedParent;
    public int currentWordNumber = 0;
    public int currentLevel = 0;
    public GameObject WinPanel;
    public GameObject GameOverPanel;
    float tempTime = 0.0f;
    public Image CurrentImage;
    public Sprite[] LevelsImages;
    public int maxLevel = 0;
    /*****************************************************************/
    public GameObject[] WordslevelsPanel;
    /*****************************************************************/
    public Text timerText;
    public Text winnerText;
    public string[] finalSentens;
    public Text scoreText;
    public int score = 0;
    /*
<<<<<<< Updated upstream

=======*/
    public CanvasGroup _MianBoard;
    /*****************************************************************/
    public AudioSource audioSource;
    public AudioClip clickClip;
    public AudioClip winClip;
    public AudioClip LoseClip;
    public AudioClip WrongWordClicked;
    public float timeLeft = 30.0f;
    /*****************************************************************/
    public enum SceneType
    {
        Quiz,
        MainDrag,
        None,
    }
    [Header("Which Scene you are in now ? ")]
    public SceneType sceneType;
    [Header("Dont need to assign it if you are not in main Drag Scene")]
    public Sprite[] MainDragImagesLayers;
    public int MainDragSceneCounter = 0;
    [Header("Layers Images from first to the last 0:n   ")]
    [Header("Dont need to assign it if you are not in main Drag Scene")]
    public GameObject[] ImagesLayers;
    public Text currentDragItemName_text;
    public GameObject MainDragScene_WinPanel;
    public Transform itemsParen;
    public GameObject VocuplaryDonePanel;
    /*****************************************************************/
    public GameObject ThankyouPanel;
    public Text scoreThankYoupanel;
    public AudioClip completeThePuzzle;
    public static GamManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public int curentMainDragCounter = 0;
    public void SpeakNow(string txt)
    {
        Speaker.Speak(txt);
    }
    public void ChangeCurrnetImageOnly()
    {
        ImagesLayers[MainDragSceneCounter].SetActive(true);
        ImagesLayers[MainDragSceneCounter].GetComponent<Image>().sprite = MainDragImagesLayers[MainDragSceneCounter];
        SpeakNow(MainDragImagesLayers[MainDragSceneCounter].name);
        //our tag that tell us this this the current snap zone ( MainBoard )
        //agesLayers[MainDragSceneCounter].gameObject.tag = "Untagged";
        // MainDragSceneCounter++;



        if (itemsParen.childCount <= 0)
        {
            //You complete the Layers              
            //Now to the Next Scene
            audioSource.clip = completeThePuzzle;
            
            StartCoroutine(WaitTillShowTheWinPanel_MainDragScene());
        }

        else
        {
            // ImagesLayers[MainDragSceneCounter].gameObject.SetActive(true);
            //agesLayers[MainDragSceneCounter].gameObject.tag = "MainBoard";
        }
    }

    IEnumerator WaitTillShowTheWinPanel_MainDragScene()
    {
        yield return new WaitForSeconds(2);//put your time 3 or5 ..
        VocuplaryDonePanel.SetActive(true);
        Invoke("ShowMainDragPanel", 1.2f);
        audioSource.clip = winClip;
        audioSource.Play();



    } public void ShowMainDragPanel()
    {
        MainDragScene_WinPanel.SetActive(true);
    }
    //>>>>>>> Stashed changes
    private void Start()
    {
        if (sceneType == SceneType.Quiz)
        {
            currentWordNumber = 0;
            scoreText.text = score + " Points";
            tempTime = timeLeft;
            // currentLevel = PlayerPrefs.GetInt("Level");
            if (maxLevel < currentLevel || LevelsImages.Length - 1 < currentLevel)
            {
                currentLevel = 0;
                print("hhh");
            }
            if (sceneType == SceneType.Quiz)
            {
                HideAllTheWordsAndShowCurrentWords();
                CurrentImage.sprite = LevelsImages[currentLevel];
                WordslevelsPanel[currentLevel].SetActive(true);
            }
            ArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(0);
            UnArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(1);
        }
//<<<<<<< Updated upstream
        //HideAllTheWordsAndShowCurrentWords();
        //CurrentImage.sprite = LevelsImages[currentLevel];
        //WordslevelsPanel[currentLevel].SetActive(true);
        //ArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(0);
        //UnArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(1);
    }
//    public void OnClicImage(GameObject img)
//    {
//        if (!CheckIsHeIsRight(img))
//        {
//            //He Is un Correct
//            GameOver();
//            return;
//        }
//        if (img.transform.parent == ArrangedParent)
//        {
//            img.transform.parent = UnArrangedParent.transform;
//        }
//        else if (img.transform.parent == UnArrangedParent)
////=======
        
//    }
    public void OnClicImage(GameObject img)
    {
       
            audioSource.clip = clickClip;
            audioSource.Play();
        
        if (sceneType == SceneType.Quiz)
//>>>>>>> Stashed changes
        {
            if (!CheckIsHeIsRight(img))
            {
                if (score > 0)
                {
                    score--;
                    scoreText.text = score + " Points";
                }
                audioSource.clip = WrongWordClicked;
                audioSource.Play();
                img.GetComponent<Animator>().SetTrigger("shak");
                return;
            }

            if (img.transform.parent == ArrangedParent)
            {
                img.transform.parent = UnArrangedParent.transform;
            }
            else if (img.transform.parent == UnArrangedParent)
            {
                img.transform.parent = ArrangedParent.transform;

            }
            currentWordNumber++;

            if (CheckIfPlayerWin())
            {
                Invoke("WinPanl", 2);
            }
        }

    }


    public bool CheckIsHeIsRight(GameObject img)
    {
        if (currentWordNumber == img.gameObject.GetComponent<WordNumber>().number)
        {
            return true;
        }
        return false;
    }
    public bool CheckIfPlayerWin()
    {
        if (UnArrangedParent.childCount <= 0)
        {
            return true;
        }
        return false;
    }


    void Update()
    {
        if (sceneType == SceneType.Quiz)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                GameOver();
            }
            timerText.text = (int)timeLeft + " Seconds";
        }
    }

    public void GameOver()
    {
        
            audioSource.clip = LoseClip;
            audioSource.Play();
        if (score > 0)
        {
            score--;
            scoreText.text = score + " Points";
        }
        //  score = 0;
        scoreText.text = score + " Points";
        GameOverPanel.SetActive(true);
        WinPanel.SetActive(false);
        ResetWordsInGameOver();
        timeLeft = tempTime + 9;
        currentWordNumber = 0;
    }
    public void WinPanl()
    {
        
            audioSource.clip = winClip;
            audioSource.Play();
        

        winnerText.text = finalSentens[currentLevel];
        WinPanel.SetActive(true);
        GameOverPanel.SetActive(false);
       /* switch (currentLevel)
        {
            //you here can give each level a plus value 0 : n level when
            // if you use switch comment score += 5;
            case 0:
                {
                    score += 1;
                    break;
                }
            case 1:
                {
                    score += 2;
                    break;
                }
        }*/
        score += 5;
        scoreText.text = score + " Points";
//<<<<<<< Updated upstream
//        if (currentLevel == maxLevel)
//=======
//        ResetWordsInGameOver();
//        CurrentImage.sprite = null;
//        foreach (GameObject i in WordslevelsPanel)
//        {
//            i.SetActive(false);
//        }
        if (sceneType == SceneType.Quiz)
//>>>>>>> Stashed changes
        {
            currentLevel++;
        }
        if (currentLevel > maxLevel)
        {
            scoreThankYoupanel.text = "Congratulations! \n your score is : " + score;
            ThankyouPanel.SetActive(true);
           // StartCoroutine(GotoScene(0));
            return;
        }
//<<<<<<< Updated upstream
    //    currentLevel++;
        // PlayerPrefs.SetInt("level", currentLevel);
        timeLeft = tempTime + 9;
        HideAllTheWordsAndShowCurrentWords();
        CurrentImage.sprite = LevelsImages[currentLevel];
        currentWordNumber = 0;


    }
//=======
       
        // PlayerPrefs.SetInt("level", currentLevel);

    //    timeLeft = tempTime + 9;

    //    if (sceneType == SceneType.Quiz)
    //    {
    //        HideAllTheWordsAndShowCurrentWords();
    //        CurrentImage.sprite = LevelsImages[currentLevel];
    //    }
    //        currentWordNumber = 0;


    //}
    IEnumerator GotoScene(int index)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(index);
    }
   public void  ActivateMainBoardBlockRayCaster()
    {
        _MianBoard.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
//>>>>>>> Stashed changes
    public void HideAllTheWordsAndShowCurrentWords()
    {
        foreach (GameObject i in WordslevelsPanel)
        {
            i.SetActive(false);
        }
        print(currentLevel);
        print(WordslevelsPanel[currentLevel].name);
        WordslevelsPanel[currentLevel].SetActive(true);
        ArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(0);
        UnArrangedParent = WordslevelsPanel[currentLevel].transform.GetChild(1);
    }
    public void ResetWordsInGameOver()
    {
        for (int i = currentWordNumber - 1; i >= 0; i--)
        {
            ArrangedParent.GetChild(i).transform.SetParent(UnArrangedParent);
        }
    }

//<<<<<<< Updated upstream

//=======
    public void SetCurrentLevel(int level)
    {
        print("Change Level");
        currentLevel = level;
        timeLeft = tempTime + 9;
        HideAllTheWordsAndShowCurrentWords();
        CurrentImage.sprite = LevelsImages[currentLevel];
        currentWordNumber = 0;
    }
    public void GotoSceneNum(int index)
    {
        SceneManager.LoadScene(index);
    }
    ////>>>>>>> Stashed changes
    ///
    public AudioClip DragClip, ReleaseClip;
    public void PlayDragClip()
    {
        audioSource.clip = DragClip;
        audioSource.Play();
    }
    public void PlayreleaseClip()
    {
        audioSource.clip = ReleaseClip;
        audioSource.Play();
    }
}

