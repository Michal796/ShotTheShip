using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//klasa odpowiada za spawn losowego wroga co określony czas nad krawędzią ekranu, oraz tworzenie instancji
//obiektu wzmacniającego w miejscu zniszczonego przez statek bohatera wroga
public class Main : MonoBehaviour
{
    static public Main S; //singleton
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Definiowanie w panelu inspekcyjnym")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f; //zasięg statku, wykorzystywane przy tworzeniu wrogów, 
    //aby ich statki pojawiały się bezpośrednio nad ekranem
    public WeaponDefinition[] weaponDefinitions; // klasa zdefiniowana w skrypcie Weapon, poza klasą Weapon
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield
    };
    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            //porzuć losowy obiekt wzmacniający
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
            HighScore.SCORE += e.scoreForEnemy;
        }
    }

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //słownik zwracający informacje definiujące broń na podstawie jej typu
        //słownik jest obsługiwany przez metodę GetWeaponDefinition()
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
    public void SpawnEnemy()
    {
        //losowy wróg respawnuje się co określony czas nad ekranem
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart (float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return ( WEAP_DICT[wt] );
        }
        return (new WeaponDefinition());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
