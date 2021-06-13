using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour
{
    private Color textColor;
    private bool notTwoWeapons;
    public bool isStart;
    public bool isQuit;
    private AssetBundle myAssets;
    private string[] scenePaths;
    // Start is called before the first frame update
    void Start()
    {
        notTwoWeapons = false;
        textColor = gameObject.GetComponent<TextMesh>().color;
        //   myAssets = AssetBundle.LoadFromFile("Assets/Scenes");
        //   scenePaths = myAssets.GetAllScenePaths();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        gameObject.GetComponent<TextMesh>().color = Color.black;
    }

    void OnMouseUp()
    {
        if (isStart)
        {
            SceneManager.LoadScene("Game");
            SceneManager.UnloadScene("MainMenu");
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<TextMesh>().color = textColor;
    }
    void OnGUI()
    {
      
    }
}
