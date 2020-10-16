using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa odpowiada za poruszanie się statkiem bohatera oraz podnoszenie przedmiotów wzmacniających
public class Hero : MonoBehaviour
{
    static public Hero S;
    [Header("Definiowane w panelu inspekcyjnym")]
    public float speed = 30;
    public float rollMult = -45; //prędkość obrotu statku podczas ruchu prawo-lewo
    public float pitchMult = 30; //prędkość obrotu statku podczas ruchu góra-dół
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40; //szybkość wystrzeliwanych pocisków
    public Weapon[] weapons; //tablica broni
    [Header("Definiowanie dynamiczne")]

    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null; //ostatni obiekt, który wywołał metodę OnTriggerEnter() statku 
    //bohatera; klasa wykorzystuje to pole aby ten sam statek nie zadał bohaterowi obrażeń kilkukrotnie

    //delegat do funkcji Fire() klasy Weapon, w celu umożliwienia oddawania strzałów z kilku broni jednocześnie
    //dodanie metody Fire() poszczególnej broni do delegata odbywa się w fukncji Start() klasy Weapon
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - próba przypisania drugiego singletona Hero.S");
        }
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //obsługiwanie ruchu statku
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //wywołanie delegata funkcji Fire() gdy naciśnięto spację
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }        
    private void OnTriggerEnter(Collider other)
    {
        //jeśli statek wszedł w kolizję z innym obiektem
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject; //root - do rodzica obiektu
        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;
        //zniszcz go jeśli to wróg, oraz odejmij poziom tarczy
        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        //jeśli to obiekt wzmacniający, pochłoń go
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {

        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                _shieldLevel++;
                break;
            default:
                //jeśli obiekt wzmacniający zawiera broń tego typu, dołącz ją jako kolejną broń (maksymalnie 5 broni tego samego typu)
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                //lub jeśli podniesiono broń innego typu - usuń wszystkie posiadane bronie i dodaj jedną broń innego typu
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
    Weapon GetEmptyWeaponSlot()
    {
        for  (int i=0; i<weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
