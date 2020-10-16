using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa obsługuje przewijane tło w tle gry, które porusza sprawia wrażenie ruchu.
public class Parallax : MonoBehaviour
{
    public GameObject poi; //statek gracza
    public GameObject[] panels; //przewijane tło powodujące wrażenie ruchu
    public float scrollSpeed = -30f;

    public float motionMul = 0.25f; /*- wpływ ruchu gracza na zachowanie warstw*/
    private float panelHt;//położenie na osi Y panelu[0]
    private float depth;//głębokość
    // Start is called before the first frame update
    void Start()
    {
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;
        //dwa panele jeden nad drugim
        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHt, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);
        //przewijane tło porusza się w pewnym stopniu w tym samym kierunku co statek bohatera
        if (poi != null){
            tX = -poi.transform.position.x * motionMul;
        }
        panels[0].transform.position = new Vector3(tX, tY, depth);
        //pozycja panelu 1 ustawiana jest w zależności od wartości tY - w górnej lub w dolnej części ekranu,
        //tak aby oba panele razem przesłoniły całe ekran gry
        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - panelHt, depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + panelHt, depth);
        }
        //print(tY);
    }
}
