using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poeni_prozor : MonoBehaviour
{
    private Text poeni;
    private Text highScore;
    private Text jump;
    private Text tezina;
    private static Poeni_prozor instanca;
    


    private void Awake()
    {
        instanca = this;
    }

    private void Update()
    {
        
        poeni.text = Level.Get_intanca().get_poeni().ToString(); //uzima trenutni broj ostvarenih poena i postavlja odgovarajucu tezinu
        if(Level.Get_intanca().get_poeni() == 12)
        {
            tezina.text = "Difficulty: MEDIUM";
        }
        if (Level.Get_intanca().get_poeni() == 25)
        {
            tezina.text = "Difficulty: HARD";
        }

        if (Level.Get_intanca().get_poeni() == 40)
        {
            tezina.text = "Difficulty: IMPOSIBLE";
        }
    }

    private void Start() {
        poeni = transform.Find("Poeni_tekst").GetComponent<Text>();
        highScore = transform.Find("HighScore").GetComponent<Text>();
        jump = transform.Find("Jump").GetComponent<Text>();
        tezina = transform.Find("Tezina").GetComponent<Text>();

        highScore.text = "Highscore: " + Highscore.getHighscore().ToString();
        tezina.text = "Difficulty: EASY";
    }

    public void jump_tekst(bool prikazi) { // prikazuje ili uklanja tekst JUMP iznad ptice
        if(prikazi)
            jump.gameObject.SetActive(true);
        else
            jump.gameObject.SetActive(false);
    }

    public static Poeni_prozor Getinstanca()
    {
        return instanca;
    }
}
