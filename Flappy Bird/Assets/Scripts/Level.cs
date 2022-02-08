using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float VisinaEkrana = 50f;
    private const float SirinaCevi = 8.42f;
    private const float VisinaGlaveCevi = 4f;
    private static float brzina = 36f;
    private const float x_osa_obrisi = -125f;
    private const float x_osa_stvori = +125f;
    private const float x_osa_ptice = -45.6f;
    private const float x_osa_zemlja_obrisi = -237f;

    private static Level instanca;

    private int poeni = 0;
    private float timer = 0f;
    private float max_timer = 2f;
    private float sirina_razmaka = 50f;
    private List<Cev> Cevi = new List<Cev>();
    private List<Transform> lista_zemlje = new List<Transform>();
    

    private class Cev
    {
        private Transform glava, telo;
        
        public Cev(Transform glava, Transform telo) {
            this.glava = glava;
            this.telo = telo;
        }

        public void Pomeri() {
            glava.position += new Vector3(-1, 0, 0) * brzina * Time.deltaTime;
            telo.position += new Vector3(-1, 0, 0) * brzina * Time.deltaTime;
        }

        public float xOsa() {
            return glava.position.x;
        }

        public void Izbrisi() {
            Destroy(glava.gameObject);
            Destroy(telo.gameObject);
        }

    }



    private void Awake()
    {
        instanca = this;
        kreiranje_zemlje();
    }

    private void Start()
    {
       
        GameOver.Getinstanca().GameOver_ekran(false); //ne prikazuje GameOver prozor
    }

    private void Update()
    {
        if (!Ptica.get_instanca().udarac())
        {
            
            PomeranjeCevi();
            kreiranje_cevi();
            pomeranje_zemlje();
            
        }

        

        if (Ptica.get_instanca().get_stanje().Equals("kraj"))
        {
            max_timer = 2f;
            sirina_razmaka = 50f;
            brzina = 36f;
            GameOver.Getinstanca().GameOver_ekran(true);
            
        }

        

    }

    private void PomeranjeCevi()
    {
        for (int i = 0; i < Cevi.Count; i++) {

            bool ispred = true;

            if (Cevi[i].xOsa() < x_osa_ptice)
            {
                ispred = false;
            }

            Cevi[i].Pomeri();

            if (ispred && (Cevi[i].xOsa() < x_osa_ptice) && i%2 == 0) //proverava da li su cevi pre pomaranja bile isped ptica a nakon pomeranja iza.
            {
                poeni++;
                Sound.pustiZvuk(Sound.Zvuk.Poen);
                if (max_timer > 1f)
                {
                    max_timer -= 0.08f; 
                }
                if (sirina_razmaka > 20f)
                {
                    sirina_razmaka -= 0.8f;
                }
            }

            if (Cevi[i].xOsa() < x_osa_obrisi) //provaravamo da li su cevi napustile ekran i brisemo ako jesu
            {
                Cevi[i].Izbrisi();
                Cevi.RemoveAt(i);
                i--;
            }
        }


    }

    private void KreirajCev(float visina, float pozicijaX, bool donjaCev) {

        Transform teloCevi = Instantiate(GameAssets.GetInstancu().prefPipeBody); // keriramo telo cevi 

        float teloCeviYpozicija;

        if (donjaCev)// ako je donja cev vrednost je -50 ili dno ekrana u suprotnom je 50 ili vrh ekrana
        {
            teloCeviYpozicija = -VisinaEkrana;
        }
        else
        {
            teloCeviYpozicija = VisinaEkrana;
            teloCevi.localScale = new Vector3(1, -1, 1);// da cev ide na dole kad joj menjamo velicinu
        }

        teloCevi.position = new Vector3(pozicijaX, teloCeviYpozicija);// postavljamo telo cevi na tacnu poziciju ali nismo podesili visinu i sirinu

        Transform glavaCevi = Instantiate(GameAssets.GetInstancu().prefPipeHead);// sve isto samo je glava u pitanju
        float glavaCeviYpozicija;

        if (donjaCev)
        {
            glavaCeviYpozicija = visina - VisinaGlaveCevi / 2f - VisinaEkrana;
        }
        else
        {
            glavaCeviYpozicija = VisinaGlaveCevi / 2f + VisinaEkrana - visina;
        }
        glavaCevi.position = new Vector3(pozicijaX, glavaCeviYpozicija);

        SpriteRenderer teloCeviSpriteRenderer = teloCevi.GetComponent<SpriteRenderer>();
        teloCeviSpriteRenderer.size = new Vector2(SirinaCevi, visina);//ovim podesavamo dimenzije tela

        BoxCollider2D teloCeviKolajder = teloCevi.GetComponent<BoxCollider2D>();// ovo postavljamo da bi se dogodila kolizija 
        teloCeviKolajder.size = new Vector2(SirinaCevi, visina);
        teloCeviKolajder.offset = new Vector2(0f, visina / 2f);

        Cevi.Add(new Cev(glavaCevi, teloCevi));
        
    }

    private void Kreiraj_ParCevi(float pozicijaY, float razmak , float pozicijaX) {
        KreirajCev(pozicijaY - razmak  / 2f, pozicijaX, true);  
        KreirajCev(100 - pozicijaY - razmak  / 2f, pozicijaX, false);
        
    }

    private void kreiranje_cevi()
    {
        float y_osa = Random.Range(35, 65);
        timer -= Time.deltaTime;// oduzima se vreme koje prodje izmedju 2 frejma 
        if (timer < 0) {
            
            Kreiraj_ParCevi(y_osa, sirina_razmaka, x_osa_stvori);


            if (brzina < 96f)
            {
                brzina += 1.2f;
            }
            
            timer += max_timer;// timer se vraca na pocetak
        }



    }

    public int get_poeni() {
        return poeni;
    }

    public static Level Get_intanca() {
        return instanca;        
    }

    private void pomeranje_zemlje() {
        foreach (Transform zemlja in lista_zemlje) {
            zemlja.position -= new Vector3(1, 0, 0) * brzina * Time.deltaTime;// pomeranje zemlje

            if (zemlja.position.x < x_osa_zemlja_obrisi) {
                float x_pozicija = zemlja.position.x;
                for (int i = 0; i < lista_zemlje.Count; i++) {
                    if (lista_zemlje[i].position.x > x_pozicija) {
                        x_pozicija = lista_zemlje[i].position.x;
                    }
                }
                
                zemlja.position = new Vector3(x_pozicija + 219.4f, zemlja.position.y, zemlja.position.z);

            }

        }
        
    }

    private void kreiranje_zemlje() {
        Transform zemlja;
        float zemlja_y = -46.29f;
        float sirina_zemlje = 228.7f;

        zemlja = Instantiate(GameAssets.GetInstancu().prefGround, new Vector3(0, zemlja_y, 0), Quaternion.identity);
        lista_zemlje.Add(zemlja);
        zemlja = Instantiate(GameAssets.GetInstancu().prefGround, new Vector3(sirina_zemlje, zemlja_y, 0), Quaternion.identity);
        lista_zemlje.Add(zemlja);
        zemlja = Instantiate(GameAssets.GetInstancu().prefGround, new Vector3(sirina_zemlje * 2, zemlja_y, 0), Quaternion.identity);
        lista_zemlje.Add(zemlja);
    }
}
