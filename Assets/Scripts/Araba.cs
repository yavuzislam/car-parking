using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Araba : MonoBehaviour
{
    public bool ilerle;
    bool DurusNoktasiDurumu = false;
    float YukselmeDeger;
    bool PlatformYukselt;
    //
    public Transform parent;
    public GameObject[] Tekerizleri;
    public GameManager _GameManager;
    public GameObject ParcPoint;

    void Update()
    {
        if (!DurusNoktasiDurumu)
        {
            transform.Translate(Vector3.forward * 7f * Time.deltaTime);
        }
        if (ilerle)
        {
            transform.Translate(Vector3.forward * 15f * Time.deltaTime);
        }
        if (PlatformYukselt)
        {
            if (YukselmeDeger> _GameManager.Platformlar[0].transform.position.y)
            {
                _GameManager.Platformlar[0].transform.position = Vector3.Lerp(_GameManager.Platformlar[0].transform.position, new Vector3(_GameManager.Platformlar[0].transform.position.x,
                _GameManager.Platformlar[0].transform.position.y + 1.3f, _GameManager.Platformlar[0].transform.position.z), .010f);
            }
            else
            {
                PlatformYukselt= false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DurusNoktasi"))
        {
            DurusNoktasiDurumu = true;
            //_GameManager.DurusNoktasi.SetActive(false); Rigidbody kalktý artýk buna gerek yok.Arabaya fiziksel iþlem yok çünkü
        }
        else if (other.gameObject.CompareTag("Elmas"))
        {
            _GameManager.Sesler[0].Play();
            other.gameObject.SetActive(false);
            _GameManager.ElmasSayisi++;
        }
        else if (other.gameObject.CompareTag("OrtaGobek"))
        {
            _GameManager.CarpmaEfekti.transform.position = ParcPoint.transform.position;
            _GameManager.CarpmaEfekti.Play();
            ArabaTeknikislem();
            _GameManager.Kaybettin();
        }
        else if (other.gameObject.CompareTag("On_Parking"))
        {
            //other.gameObject.GetComponent<On_Parking>().ParkingAktiflestir();
            other.gameObject.GetComponent<On_Parking>().Parking.SetActive(true);
        }
    }
    void ArabaTeknikislem()
    {
        ilerle = false;
        Tekerizleri[0].SetActive(false);
        Tekerizleri[1].SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parking"))
        {
            //foreach (var item in Tekerizleri)
            //{
            //    item.SetActive(false);    bu benim yaptýðým buda oluyor.Teker izlerini kapatýyor.
            //}
            ArabaTeknikislem();
            transform.SetParent(parent); //arabayý platforma taþýdýk.Yani onu platform içine atýp child yaptýk.
            //transform.SetParent(collision.gameObject.transform);//buda oluyor bunu ben yazdým
            if (_GameManager.YukselecekPlatformVarmi)
            {
                YukselmeDeger = _GameManager.Platformlar[0].transform.position.y + 1.3f;
                PlatformYukselt = true;
            }
            _GameManager.YeniArabaGetir();
        }
        else if (collision.gameObject.CompareTag("Araba"))
        {
            _GameManager.CarpmaEfekti.transform.position = ParcPoint.transform.position;
            _GameManager.CarpmaEfekti.Play();
            ArabaTeknikislem();
            _GameManager.Kaybettin();
        }
    }
}
