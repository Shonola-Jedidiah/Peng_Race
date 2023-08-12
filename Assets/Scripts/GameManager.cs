using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private const int Coin_Score_Amount = 5;
    public static GameManager Instance { set; get; }


    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;



    //Hat Store
  // public GameObject[] Hats;


    //Theme
    public Material DarkT;
    public Material LightT;
    public Material SunnyT;

    public Light LightClr;
    public Color LightDarkColor = Color.black;
    public Color LightNormColor = Color.white;
    public Color LightSunnyColor = Color.yellow;

    public static Color LightCllr = new Color(73 / 255f, 151 / 255f, 210 / 255f);


    /* public static Color DarkSkyColor = new Color(118 / 255f, 118 / 255f, 118 / 255f);
     public static Color DarkEquatorColor = new Color(31 / 255f, 31 / 255f, 31 / 255f);
     public static Color DarkGroundColor = new Color(0, 0, 0);

     public static Color SunnySkyColor = new Color(191 / 255f, 146 / 255f, 0);
     public static Color SunnyEquatorColor = new Color(89 / 255f, 67 / 255f, 0 / 255f);
     public static Color SunnyGroundColor = new Color(90 / 255f, 57 / 255f, 0 / 255f);

     public static Color NormSkyColor = new Color(118/255f, 118 / 255f, 118 / 255f);
     public static Color NormEquatorColor = new Color(116 / 255f, 209 / 255f, 200 / 255f);
     public static Color NormGroundColor = new Color(124 / 255f, 183 / 255f, 190 / 255f);
    */

    public static bool ThemePanelB = false;
    public GameObject ThemePanelS;
    //Audio Control
    private static AudioSource AudioContoller;
    public AudioClip Jump;
    public AudioClip Click;
    public AudioClip CoinCollect;
    public AudioClip Dead;
    public AudioClip Slide;
    public AudioClip BgAudio;


    //UI
    public Animator GameCanvas, DefMenu, DefMenu1, Tap2Start;
    public Text scoreText, coinText, modifierText, HighScore, coinUpdate;
    private float score, coinScore, modifierScore;
    public GameObject DefaultScreen;
    public GameObject GamePanel;
    public GameObject MGamePanel;
    public GameObject DeathPanel;
    public GameObject PausePanel;


    private int lastScore;


    //Death Menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadCoinText;


    private void Awake()
    {
      
        /* PlayerPrefs.GetInt("hats");
         PlayerPrefs.GetInt("Un1");
         PlayerPrefs.GetInt("Un2");
         PlayerPrefs.GetInt("Un3");
        */
  
        Instance = this;
        modifierScore = 1;

        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");
        HighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        coinUpdate.text = PlayerPrefs.GetInt("Coin").ToString();


        #region
        /*  if(PlayerPrefs.GetInt("hats") == 0)
          {
              Hats[0].SetActive(true);
              Hats[1].SetActive(false);
              Hats[2].SetActive(false);
              Hats[3].SetActive(false);
          }
          else if(PlayerPrefs.GetInt("hats") == 1)
          {
              Hats[0].SetActive(false);
              Hats[1].SetActive(true);
              Hats[2].SetActive(false);
              Hats[3].SetActive(false);
          }
          else if(PlayerPrefs.GetInt("hats") == 2)
          {
              Hats[0].SetActive(false);
              Hats[1].SetActive(false);
              Hats[2].SetActive(true);
              Hats[3].SetActive(false);
          }
          else if(PlayerPrefs.GetInt("hats") == 3)
          {
              Hats[0].SetActive(false);
              Hats[1].SetActive(false);
              Hats[2].SetActive(false);
              Hats[3].SetActive(true);
          }
        */
        #endregion
    }

    private void Start()
    {
        Time.timeScale = 1;
        DefaultScreen.SetActive(true);
        DeathPanel.SetActive(false);
        GamePanel.SetActive(false);
        AudioContoller = GetComponent<AudioSource>();

        PausePanel.SetActive(false);

        ThemePanelB = false;

        AudioContoller.Play();

    }

    private void Update()
    {

        if (ThemePanelB)
        {
            ThemePanelS.SetActive(true);
        }
        else
        {
            ThemePanelS.SetActive(false);
        }
        if (MoblieInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CamFollow>().IsMoving = true;

            DefaultScreen.SetActive(false);
            DeathPanel.SetActive(false);
            GamePanel.SetActive(true);

            GameCanvas.SetTrigger("Show");
            DefMenu.SetTrigger("Hide");
            DefMenu1.SetTrigger("Hide");
            Tap2Start.SetTrigger("Hide");
        }
        if (isGameStarted && !IsDead)
        {
            //Increase Score
            score += (Time.deltaTime * modifierScore);

            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }

        }
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void ThemeAppear()
    {
        ThemePanelB = true;
    }

    public void GetCoin()
    {
        AudioContoller.PlayOneShot(CoinCollect, 1.0f);
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += Coin_Score_Amount;
        scoreText.text = score.ToString("0");
    }
    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;

        modifierText.text = "x" + modifierScore.ToString("0.0");
    }
    public void PauseMeenu()
    {
        Debug.Log("Clicked");
        AudioContoller.PlayOneShot(Click, 10.0f);
        Time.timeScale = 0;
        MGamePanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    public void ResumeButton()
    {
        AudioContoller.PlayOneShot(Click, 10.0f);
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        MGamePanel.SetActive(true);
    }

    public void OnPlayButton()
    {
        AudioContoller.PlayOneShot(Click, 10.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void LightTheme()
    {
        LightClr.color = LightNormColor;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = LightCllr;
        RenderSettings.skybox = LightT;
        ThemePanelB = false;
        /* RenderSettings.ambientSkyColor = NormSkyColor;
         RenderSettings.ambientEquatorColor = NormEquatorColor;
         RenderSettings.ambientGroundColor = NormGroundColor;*/
    }
    public void DarkTheme()
    {
        LightClr.color = LightDarkColor;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.skybox = DarkT;
        ThemePanelB = false;
        /* RenderSettings.ambientSkyColor = DarkSkyColor;
         RenderSettings.ambientEquatorColor = DarkEquatorColor;
         RenderSettings.ambientGroundColor = DarkGroundColor;
         */
    }
    public void SunnyTheme()
    {
        LightClr.color = LightSunnyColor;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

        RenderSettings.skybox = SunnyT;
        ThemePanelB = false;
        /* RenderSettings.ambientSkyColor = SunnySkyColor;
         RenderSettings.ambientEquatorColor = SunnyEquatorColor;
         RenderSettings.ambientGroundColor = SunnyGroundColor;
        */
    }

    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        DeathPanel.SetActive(true);
        deathMenuAnim.SetTrigger("Dead");
        GameCanvas.SetTrigger("Hide");
        DefaultScreen.SetActive(false);
        DeathPanel.SetActive(true);
        GamePanel.SetActive(false);

        //Check for new HghScore
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("HighScore", (int)score);
        }

        if (PlayerPrefs.HasKey("Coin"))
        {
            int oldValue = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", oldValue + (int)coinScore);
        }
        else
        {
            PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", (int)coinScore);
        }

        /* if( coinScore <PlayerPrefs.GetInt("CoinPrev")  || coinScore < PlayerPrefs.GetInt("CoinPrev"))
          {
  PlayerPrefs.SetInt("CoinPrev", (int)coinScore);
          }


          PlayerPrefs.GetInt("CoinNew", PlayerPrefs.GetInt("CoinPrev") + (int)coinScore); ;
  */
    }

  /*  public void EmptyHat()
    {
        PlayerPrefs.SetInt("hats", 0);
        Hats[0].SetActive(true);
        Hats[1].SetActive(false);
        Hats[2].SetActive(false);
        Hats[3].SetActive(false);
       
    }
    
    public void Hat1()
    {
        if (PlayerPrefs.GetInt("Un1") == 1)//Check if Hat is unlocked 1 means Unlocked while 0 which is the default means false
        {
        PlayerPrefs.SetInt("hats", 1);
        Hats[0].SetActive(false);
        Hats[1].SetActive(true);
        Hats[2].SetActive(false);
        Hats[3].SetActive(false);
        
            return;
        }

        else if(PlayerPrefs.GetInt("Coin")-25  >= 0)//If Hat is not Unlocked Subtract 50 from the Coin Value and Make the hat U2 Unlocked
        {
            PlayerPrefs.SetInt("Un1", 1);
            PlayerPrefs.SetInt("hats", 1);
            Hats[0].SetActive(false);
            Hats[1].SetActive(true);
            Hats[2].SetActive(false);
            Hats[3].SetActive(false);
           
            return;
        }

        else
        {
            Debug.Log("YOU DONT HAVE ENOUGH COIN"); //Line to be executed if the hat is not unlocked or if The Player doesnt have enough coin
        }
    }
    public void Hat2()
    {
        if (PlayerPrefs.GetInt("Un2") == 1)//Check if Hat is unlocked 1 means Unlocked while 0 which is the default means false
        {
        PlayerPrefs.SetInt("hats", 1);
        Hats[0].SetActive(false);
        Hats[1].SetActive(false);
        Hats[2].SetActive(true);
        Hats[3].SetActive(false);
        
            return;
        }

        else if(PlayerPrefs.GetInt("Coin")-50  >= 0)//If Hat is not Unlocked Subtract 50 from the Coin Value and Make the hat U2 Unlocked
        {
            PlayerPrefs.SetInt("Un2", 1);
            PlayerPrefs.SetInt("hats", 1);
            Hats[0].SetActive(false);
            Hats[1].SetActive(false);
            Hats[2].SetActive(true);
            Hats[3].SetActive(false);
           
            return;
        }

        else
        {
            Debug.Log("YOU DONT HAVE ENOUGH COIN"); //Line to be executed if the hat is not unlocked or if The Player doesnt have enough coin
        }
    }
    public void Hat3()
    {
        if (PlayerPrefs.GetInt("Un3") == 1)//Check if Hat is unlocked 1 means Unlocked while 0 which is the default means false
        {
        PlayerPrefs.SetInt("hats", 1);
        Hats[0].SetActive(false);
        Hats[1].SetActive(false);
        Hats[2].SetActive(false);
        Hats[3].SetActive(true);
        
            return;
        }

        else if(PlayerPrefs.GetInt("Coin")-50  >= 0)//If Hat is not Unlocked Subtract 50 from the Coin Value and Make the hat U2 Unlocked
        {
            PlayerPrefs.SetInt("Un2", 1);
            PlayerPrefs.SetInt("hats", 1);
            Hats[0].SetActive(false);
            Hats[1].SetActive(false);
            Hats[2].SetActive(false);
            Hats[3].SetActive(true);
           
            return;
        }

        else
        {
            Debug.Log("YOU DONT HAVE ENOUGH COIN"); //Line to be executed if the hat is not unlocked or if The Player doesnt have enough coin
        }
    }
  */
}
