using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa odpowiadająca za najprostszy typ wroga, po której dziedziczą wszystkie klasy podrzędne Enemy(1-4)
public class Enemy : MonoBehaviour
{
    [Header("Definiowanie w panelu inspekcyjnym: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f; // strzał co tyle sekund
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f; //czas sygnalizacji obrażeń (zmiana koloru na czerwony)
    public float powerUpDropChance = 1f; //szansa na upuszczenie obiektu wzmacniajacego
    public int scoreForEnemy = 50;

    [Header("Definiowanie dynamiczne: enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool notifiedOfDestruction = false;

    protected BoundsCheck bndCheck; //do określenie czy obiekt ma zostać zatrzymany na ekranie
    void Awake()
    {
        //pobranie oryginalnych kolorów obiektu
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i ++)
        {
                originalColors[i] = materials[i].color;
        }
    }
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //jeśli czas sygnalizacji obrażeń minął, wyłącz ją
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)
        {
                Destroy(gameObject); //podstawowy wróg nie jest utrzymywany na ekranie
        }
    }
    public virtual void Move() //funkcja Move zostanie nadpisana w klasach podrzędnych
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag) {
            case "ProjectileHero":
                //jeśli wróg został trafiony, zasygnalizuj obrażenia, zniszcz pocisk oraz odejmij mu zdrowie
                ShowDamage();
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
             default:
                print("Trafiono innym obiektem niż pocisk");
                break;
        }
    }
    void ShowDamage()
    {
        //zmień kolor na czerwony na określony czas
        foreach (Material m in materials){
            m.color = Color.red;
            showingDamage = true;
            damageDoneTime = Time.time + showDamageDuration;
        }
    }
    //przywróć domyślne kolory
    public void UnShowDamage()
    {
        for ( int i = 0; i<materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
