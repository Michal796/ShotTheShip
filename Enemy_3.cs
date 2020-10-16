using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasa Enemy3 dziedziczy po klasie Enemy. Ruch odbywa się przy użyciu interpolacji.
//W grze Enemy3 jest wrogiem z fioletowymi skrzydłami
public class Enemy_3 : Enemy
{
    [Header("Panel inspekcyjny")]
    public float lifeTime = 5;

    [Header("Dynamiczne")]
    public Vector3[] points; //punkty do wykonania interpolacji
    public float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        //minimalne oraz maksymalne położenie wroga na osi x
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        //losowe położenie na srodku w dolnej połowy
        v = Vector3.zero;
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        v.x = Random.Range(xMin, xMax);
        points[1] = v;

        //losowe położenie w górnej części ekranu
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
        scoreForEnemy = 150;
    }
    public override void Move()
    {
        //interpolacja ruchu na podstawie 3 punktów
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
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
