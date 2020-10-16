using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//typ wyliczeniowy WeaponType został zdefiniowany poza klasą, aby był dostępny dla każdego obiektu gry
public enum WeaponType
{
    none, //stan domyslny
    blaster,
    spread,
    phaser, //grupowe pociski
    missle,
    laser,
    shield, //zwiekszenie tarczy
}

//klasa definiujaca broń, to znaczy przechowująca wszystkie charakterystyczne dla danej broni wartości
//klasa edytowalna w panelu inspector
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

//klasa odpowiada za obiekty gry na statku bohatera będące bronią\
//generuje pociski oraz zawiera metodę Fire(), wywoływaną przy pomocy delegata
public class Weapon : MonoBehaviour
{
    public static Transform PROJECTILE_ANCHOR; //rodzic każdego obiektu Projectile

    [Header("Definiowane dynamicznie")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar; //obiekt statku bohatera, będący pojedynczą bronią
    public float lastShotTime;
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //w funkcji start pobierana jest transformacja rodzica obiektu Weapon (statku bohatera)
        //oraz następuje dodanie funkcji Fire() każdego obiektu Weapon do delegata
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        //obiekt Weapon jest aktywny tylko wtedy, gdy jego typ nie jest równy null
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }
    public void Fire()
    {
        //funkcja działa tylko gdy obiekt jest aktywny, przez co wywołanie delegata funkcji Fire()
        //powoduje wystrzał tylko z aktywnych obiektów Weapon
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0) //na wypadek gdyby broń była skierowana "w dół"
        {
            vel.y = -vel.y;
        }
        //w zależności od typu broni, stwórz pocisk, lub 3 pociski
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        // na wypadek implementacjji funkcji strzelania do któregoś z obiektów Enemy
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
