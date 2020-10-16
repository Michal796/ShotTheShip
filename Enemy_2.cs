using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasa dziedzicząca po klasie Enemy. Enemy2 porusza się wykorzystując interpolację liniową, 
//które jest wygładzona przez sinusoidę (w grze jest to wróg z żółtymi skrzydłami)
public class Enemy_2 : Enemy
{
    [Header("Panel inspekcyjny")]
    public float sinEccentricity = 0.6f; //stopień wygładzania przez sinusoidę
    public float lifeTime = 10;
    public GameObject projectile;

    [Header("Dynamicznie")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        //początkowe miejsce po lewej stronie ekranu, na losowej wysokości
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //po prawej
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        scoreForEnemy = 100;

        //losowo przenieś na drugą stronę
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        birthTime = Time.time;

    }
    public override void Move()
    {
        //interpolacja przy użyciu krzywej beziera opiera sie na wartosci parametru od 0 do 1
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        //interpolacja liniowa
        pos = (1 - u) * p0 + u * p1;
    }
    public void Shot()
    {
        //GameObject pocisk = Instantiate<GameObject>(projectile);

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
