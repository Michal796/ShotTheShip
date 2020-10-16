using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa definiująca obiekt wzmacniający, upuszczany losowo z pokonanych wrogów
//odpowiada za poruszanie się oraz znikanie po upływie określonego czasu
public class PowerUp : MonoBehaviour
{
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2); //przechowywanie predkosci maks i min w vectorze 2;
    public float lifeTime = 6f;
    public float fadeTime = 4f; //czas zanikania

    public WeaponType type;
    public GameObject cube;
    public TextMesh letter; //litera wyświetlana na obiekcie
    public Vector3 rotPerSecond;
    public float birthTime;
    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        //losowa prędkość liniowa
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        transform.rotation = Quaternion.identity; //brak obrotu, 0 0 0
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        //u jest większe od jeden po upływie czasu lifeTime+fadeTime
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //u jest mniejsze od zera przez czas lifeTime
        if (u > 0)
        {
            //zmniejszanie wartości alfa koloru obiektu PowerUp w zależnosci od wartości u
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        //jeśli obiekt znajdzie się poza ekranem
        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    //ustawienie typu obiektu wzmacniającego wykonuje funkcja Main
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }
    public void AbsorbedBy (GameObject target)
    {
        Destroy(this.gameObject);
    }
}
