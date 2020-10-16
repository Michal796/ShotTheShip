using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa odpowiada za zmiane wyglądu tarczy statku bohatera, w zależności od jej poziomu
public class Shield : MonoBehaviour
{
    [Header("Definiowanie w panelu inspekcyjnym")]
    public float rotationsPerSecond = 0.2f;

    [Header("Definiowanie dynamiczne")]
    public int levelShown = 0;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        int currentLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        if (levelShown != currentLevel)
        {
            levelShown = currentLevel;
            //zmiana wyglądu tarczy w zależności od poziomu
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
