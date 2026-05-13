using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIndexManager : MonoBehaviour
{
    //si usamos string la selección del nivel se basara en su respectivo nombre
    //si usamos int la selección del nivel se basara en respecto a su posicionamiento del index del build profile
    public void LevelIndex(int index) 
    {
        SceneManager.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
