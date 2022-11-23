using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("---ARABA AYARLARI")]
    public GameObject[] Arabalar;
    public int KacArabaOlsun;
    int KalanAracSayisiDeger;
    int AktifAracIndex = 0;

    [Header("---CANVAS AYARLARI")]
    public GameObject[] ArabaCanvasGorseller;
    public Sprite AracGeldiGorseli;
    public TextMeshProUGUI KalanAracSayisi;
    public TextMeshProUGUI[] Textler;
    public GameObject[] Panellerim;
    public GameObject[] TapToButonlar;

    [Header("---PLATFROM AYARLARI")]
    public GameObject[] Platformlar;
    //public GameObject Platform_2;
    public float[] DonusHizlari;
    bool DonusVarmi;
    public bool YukselecekPlatformVarmi;

    [Header("---LEVEL AYARLARI")]
    public int ElmasSayisi;
    public ParticleSystem CarpmaEfekti;
    public AudioSource[] Sesler;
    bool DokunmaKilidi;

    void Start()
    {
        DokunmaKilidi = true;
        DonusVarmi = true;
        VarsayilanDegerleriKontrolEt();
        KalanAracSayisiDeger = KacArabaOlsun;
        //KalanAracSayisi.text = KalanAracSayisiDeger.ToString();
        for (int i = 0; i < KacArabaOlsun; i++)
        {
            ArabaCanvasGorseller[i].SetActive(true);
        }
    }
    public void YeniArabaGetir()
    {
        KalanAracSayisiDeger--;
        if (AktifAracIndex < KacArabaOlsun)
        {
            Arabalar[AktifAracIndex].SetActive(true);
        }
        else
        {
            Kazandýn();
        }
        ArabaCanvasGorseller[AktifAracIndex - 1].GetComponent<Image>().sprite = AracGeldiGorseli;
        //KalanAracSayisi.text = KalanAracSayisiDeger.ToString();
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase==TouchPhase.Began)
            {
                if (DokunmaKilidi)
                {
                    Panellerim[0].SetActive(false);
                    Panellerim[3].SetActive(true);
                    DokunmaKilidi = false;
                }
                else
                {
                    Arabalar[AktifAracIndex].GetComponent<Araba>().ilerle = true;
                    AktifAracIndex++;
                }
            }

        }
        if (DonusVarmi)
        {
            Platformlar[0].transform.Rotate(new Vector3(0, 0, -DonusHizlari[0]), Space.Self);//Self objenin kendi iç eksenini dikkate alarak dönmesi
            if (Platformlar[1] != null)
            {
                Platformlar[1].transform.Rotate(new Vector3(0, 0, DonusHizlari[1]), Space.Self);
            }
        }
    }
    public void Kaybettin()
    {
        DonusVarmi = false;
        Textler[6].text = PlayerPrefs.GetInt("Elmas").ToString();
        Textler[7].text = SceneManager.GetActiveScene().name;
        Textler[8].text = (KacArabaOlsun - KalanAracSayisiDeger).ToString();
        Textler[9].text = ElmasSayisi.ToString();
        Sesler[1].Play();
        Sesler[3].Play();
        Panellerim[1].SetActive(true);
        Panellerim[3].SetActive(false);
        Invoke("KaybettinButonuOrtayaCikart", 2f);//2 saniye sonra KaybettinButonuOrtayaCikart() fonksiyonu çalýþtýr.
    }
    void KaybettinButonuOrtayaCikart()
    {
        TapToButonlar[0].SetActive(true);
    }
    void KazandinButonuOrtayaCikart()
    {
        TapToButonlar[1].SetActive(true);
    }
    void Kazandýn()
    {
        PlayerPrefs.SetInt("Elmas", PlayerPrefs.GetInt("Elmas") + ElmasSayisi);
        Textler[2].text = PlayerPrefs.GetInt("Elmas").ToString();
        Textler[3].text = SceneManager.GetActiveScene().name;
        Textler[4].text = (KacArabaOlsun - KalanAracSayisiDeger).ToString();
        Textler[5].text = ElmasSayisi.ToString();
        Sesler[2].Play();
        Panellerim[2].SetActive(true);
        Panellerim[3].SetActive(false);
        Invoke("KazandinButonuOrtayaCikart", 2f);//2 saniye sonra KazandinButonuOrtayaCikart() fonksiyonu çalýþtýr.
    }
    //BELLEK YÖNETÝMÝ
    void VarsayilanDegerleriKontrolEt()
    {
        Textler[0].text = PlayerPrefs.GetInt("Elmas").ToString();
        Textler[1].text = SceneManager.GetActiveScene().name;//aktif sahnenin ismini atadýk
    }
    public void izleVeDevamEt()
    {
        //istersen yap
    }
    public void izleVeDahaFazlaKazan()
    {
        //istersen yap
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SonrakiLevel()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //public void Donustur()
    //{
    //    ArabaCanvasGorseller[i].gam
    //}
    //foreach (var item in Araba)
    //        {
    //            if (!item.activeInHierarchy)
    //            {
    //                item.GetComponent<Araba>().ilerle = true;
    //                item.SetActive(true);
    //            }
    //        }
}
