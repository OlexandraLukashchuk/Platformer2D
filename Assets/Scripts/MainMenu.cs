using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartHandler()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Choose_Level_1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Choose_Level_2()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void Choose_Level_3()
    {
        SceneManager.LoadScene("Level_3");
    }

    public void ExitHandler()
    {
        Application.Quit();
    }
}
