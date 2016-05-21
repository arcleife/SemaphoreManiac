using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    public void backToMenu()
    {
        HeartManager.reset();
        SceneManager.LoadScene("mainmenu");
    }

    public void retry()
    {
        HeartManager.reset();
        SceneManager.LoadScene("gameplay");
    }

    public void nextStage()
    {
        ScoreManager.reset();
        HeartManager.reset();

        Data.level += 1;
        if (Data.level > 15) Data.level = 1;

        SceneManager.LoadScene("gameplay");
    }
}