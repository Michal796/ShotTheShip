using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa dziedzicząca po klasie Enemy
//wróg Enemy1 porusza się wykorzystując funkcję sinus
public class Enemy_1 : Enemy
{
    [Header("Enemy_1")]
    public float vaveFrequency = 2;
    public float vaveWidth = 4;
    public float vaveRotY = 45;

    private float x0; //położenie początkowe
    private float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        x0 = pos.x;
        birthTime = Time.time;
        scoreForEnemy = 150;
    }
    public override void Move()
    {
        //określenie ruchu sinusoidalnego względem osi x, oraz rotacji
        Vector3 tempPos = pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / vaveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + vaveWidth * sin;
        pos = tempPos;

        Vector3 rot = new Vector3(0, sin * vaveRotY+90, 90);
        this.transform.rotation = Quaternion.Euler(rot);
        //Enemy1 wykonuje również funkcję Move() Klasy podstawowej (porusza się w dół ekranu)
        base.Move();
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
