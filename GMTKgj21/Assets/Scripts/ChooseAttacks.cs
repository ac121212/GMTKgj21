using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseAttacks : MonoBehaviour
{
    private Color textColor;
    private bool threeWeapons;
    private bool tooFew;
    public bool isBullet;
    public bool isShield;
    public bool isGrenade;
    public bool isBack;
    // Start is called before the first frame update
    void Start()
    {
        tooFew = false;
        threeWeapons = false;
        textColor = gameObject.GetComponent<TextMesh>().color;
        if (PlayerPrefs.GetInt("isGrenade") == 1 && isGrenade) {
            gameObject.GetComponent<TextMesh>().color = Color.green;
        }
        if (PlayerPrefs.GetInt("isBullet") == 1 && isBullet) {
            gameObject.GetComponent<TextMesh>().color = Color.green;
        }
        if (PlayerPrefs.GetInt("isShield") == 1 && isShield) {
            gameObject.GetComponent<TextMesh>().color = Color.green;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseEnter()
    {
        if (PlayerPrefs.GetInt("isGrenade") == 0 && isGrenade)
        {
            gameObject.GetComponent<TextMesh>().color = Color.black;     
        }
        if (PlayerPrefs.GetInt("isBullet") == 0 && isBullet)
        {
            gameObject.GetComponent<TextMesh>().color = Color.black;
        }
        if (PlayerPrefs.GetInt("isShield") == 0 && isShield)
        {
            gameObject.GetComponent<TextMesh>().color = Color.black;
        }
        if (isBack)
        {
            gameObject.GetComponent<TextMesh>().color = Color.black;
        }
    }
    void OnMouseExit()
    {
        if (PlayerPrefs.GetInt("isGrenade") == 0 && isGrenade)
        {
            gameObject.GetComponent<TextMesh>().color = textColor;
        }
        if (PlayerPrefs.GetInt("isBullet") == 0 && isBullet)
        {
            gameObject.GetComponent<TextMesh>().color = textColor;
        }
        if (PlayerPrefs.GetInt("isShield") == 0 && isShield)
        {
            gameObject.GetComponent<TextMesh>().color = textColor;
        }
        if (isBack)
        {
            gameObject.GetComponent<TextMesh>().color = textColor;
        }
    }
    void OnMouseUp()
    {
        if (threeWeapons)
        {
            threeWeapons = false;
        }
        if (tooFew)
        {
            tooFew = false;
        }
        if (isGrenade)
        {
            if (PlayerPrefs.GetInt("isGrenade") == 1)
            {
                gameObject.GetComponent<TextMesh>().color = textColor;
                PlayerPrefs.SetInt("isGrenade", 0);
            }
            else
            {
                gameObject.GetComponent<TextMesh>().color = Color.green;
                PlayerPrefs.SetInt("isGrenade", 1);
            }
        }
        if (isShield)
        {
            if (PlayerPrefs.GetInt("isShield") == 1)
            {
                gameObject.GetComponent<TextMesh>().color = textColor;
                PlayerPrefs.SetInt("isShield", 0);
            }
            else
            {
                gameObject.GetComponent<TextMesh>().color = Color.green;
                PlayerPrefs.SetInt("isShield", 1);
            }
        }
        if (isBullet)
        {
            if (PlayerPrefs.GetInt("isBullet") == 1)
            {
                gameObject.GetComponent<TextMesh>().color = textColor;
                PlayerPrefs.SetInt("isBullet", 0);
            }
            else
            {
                gameObject.GetComponent<TextMesh>().color = Color.green;
                PlayerPrefs.SetInt("isBullet", 1);
            }
        }
        if (isBack)
        {
            if(PlayerPrefs.GetInt("isBullet") == 1 && PlayerPrefs.GetInt("isGrenade") == 1 && PlayerPrefs.GetInt("isShield") == 1)
            {
                threeWeapons = true;
            } else if (PlayerPrefs.GetInt("isBullet") + PlayerPrefs.GetInt("isGrenade") + PlayerPrefs.GetInt("isShield") <2)
            {
                tooFew = true;
            } else
            {
                SceneManager.LoadScene("MainMenu");
            }
            
        }
    }

    void OnGUI()
    {
        if (threeWeapons)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Can only choose 2 weapons");
            print("Can only choose 2 weapons");
        }
        if (tooFew)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Choose 2 Weapons");
            print("Choose 2 Weapons");
        }

    }
}

