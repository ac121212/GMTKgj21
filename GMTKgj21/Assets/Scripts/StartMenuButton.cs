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
    public bool isWeapons;
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
        if (isStart && (PlayerPrefs.GetInt("isGrenade") + PlayerPrefs.GetInt("isBullet") + PlayerPrefs.GetInt("isShield") == 2))
        {
            SceneManager.LoadScene("GameMap1");
        } 
        else if (isStart && (PlayerPrefs.GetInt("isGrenade") + PlayerPrefs.GetInt("isBullet") + PlayerPrefs.GetInt("isShield") != 2))
        {
            notTwoWeapons = true;
        }
        if (isQuit)
        {
            Application.Quit();
        }
        if (isWeapons)
        {
            SceneManager.LoadScene("ChooseAttacks");
        }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<TextMesh>().color = textColor;
    }
    void OnGUI()
    {
        if (notTwoWeapons)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Must have exactly two weapon choices");
        }
    }
}
