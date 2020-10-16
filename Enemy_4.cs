using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa pomocnicza do wykonania wroga Enemy_4, który został podzielony na części
[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector]
    public GameObject go;
    [HideInInspector]
    public Material mat;
}

//klasa Enemy4 dziedziczy po klasie Enemy. Wróg Enemy_4 składa się z kilku części. Kokpit jest chroniony przez kadłub,
//który jest z kolei chroniony przez skrzydła. Wróg utrzymuje się na ekranie dopóki nie zostanie do końca zniszczony
public class Enemy_4 : Enemy
{
    public Part[] parts;

    //dane do wyznaczenia ruchu
    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4; //czas trwania jednego przemieszczenia

    // Start is called before the first frame update
    void Start()
    {
        p0 = p1 = pos;
        InitMovement();
        //odcztanie obiektu gry oraz materiału każdej z części
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
        scoreForEnemy = 250;
    }//zainicjuj ruch z punktu 0 do puntku 1
    void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        timeStart = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration; // inicjacja ruchu co 4 sekundy (duration)
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }
    Part FindPart(string n)//znajdz część po nazwie
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)//znajdź część po obiekcie gry
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }
    // właściwości tylko do odczytu, określające czy statek został zniszony (czy zdrowie spadło ponizej zera)
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n) 
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            return (true);
        }
        return (prt.health <= 0);
    }
    //pokaż obrażenia trafionych części
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //znalezienie trafionego obiektu na podstawie punktu trafienia (contacts)
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                //jeśli odwłano się do pocisku zamiast do trafionej części wroga, skorzystaj z drugiego Collidera
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //jeśli trafiona część jest chroniona, znajdz nazwę chroniącej części 
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        //jeśli część chroniąca dalej istnieje, zniszcz pocisk i przerwij funkcję
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }
                //jeśli część chroniąca już została zniszczona
                //znajdz typ broni, którą został trafiony element i odejmij zdrowie
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                }
                bool allDestroyed = true;
                foreach (Part prt in parts)
                {
                    //jeśli pozostał co najmniej jeden element
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                //jeśli zniszczone zostały wszystkie elementy
                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
        // Update is called once per frame
        void Update()
        {
            Move();
            if (showingDamage && Time.time > damageDoneTime)
            {
                UnShowDamage();
            }
            if (bndCheck != null && bndCheck.offDown)
            {
                Destroy(gameObject);
            }
        }
    }
}
