using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ptica : MonoBehaviour
{
    private float Skok = 100f;
    private static Ptica instaca;
    private Stanje stanje;

    private Rigidbody2D ptica; //pristupa komponenti koji odredjuje da li je ptica staticna ili dinamicna

    public event System.EventHandler smrt;

    private void Awake()
    {
            ptica = GetComponent<Rigidbody2D>(); 
            instaca = this;
            stanje = Stanje.pocetak;
            ptica.bodyType = RigidbodyType2D.Static;
    }

    private enum Stanje
    {
        pocetak,
        igranje,
        kraj
    }
    
    void Start()
    {
        stanje = Stanje.pocetak;
    }


    void Update()
    {
        switch (stanje)
        {
            case Stanje.pocetak:
                {
                    Poeni_prozor.Getinstanca().jump_tekst(true);
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {

                        ptica.bodyType = RigidbodyType2D.Dynamic;
                        Poeni_prozor.Getinstanca().jump_tekst(false);
                        Jump();
                        stanje = Stanje.igranje;
                    }
                    break;
                }
            case Stanje.igranje:
                {
                   
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {
                        Jump();
                    }

                    transform.eulerAngles = new Vector3(0, 0, ptica.velocity.y * 0.2f); // rotacija ptice oko z ose
                    break;
                }
            case Stanje.kraj:
                {
                    break;
                }

        }


    }

    public static Ptica get_instanca()
    {
        return instaca;
    }

    


    private void Jump()
    {
        ptica.velocity = Vector2.up * Skok;
        Sound.pustiZvuk(Sound.Zvuk.Skok);
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        Sound.pustiZvuk(Sound.Zvuk.kraj);
        ptica.bodyType = RigidbodyType2D.Static;
        stanje = Stanje.kraj;
        if (smrt != null)
        {
            smrt(this, System.EventArgs.Empty); 
        }
    }

    public bool udarac()
    {
        if (ptica.bodyType == RigidbodyType2D.Static)
        {
            return true;
        }
        return false;
    }


    public string get_stanje()
    {
        return stanje.ToString();
    }

    
}
