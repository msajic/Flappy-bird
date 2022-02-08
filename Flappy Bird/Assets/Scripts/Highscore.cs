using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour
{

    public static void Start()
    {
        
        Ptica.get_instanca().smrt += smrt_ptice; //dodeljuje dogadjaj za smrt

    }


    private static void smrt_ptice(object sender, System.EventArgs e)
    {
        postaviHighscore(Level.Get_intanca().get_poeni());
    }

    public static int getHighscore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool postaviHighscore(int poeni)
    {
        int trenutniHighscore = getHighscore();
        if (poeni > trenutniHighscore)
        {
            PlayerPrefs.SetInt("highscore", poeni);
            PlayerPrefs.Save();
            return true;
        }
        else
            return false;
    }

}
