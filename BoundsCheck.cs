using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//klasa odpowiada za utrzymywanie wybranych obiektów na ekranie (na podstawie wartości keepOnScreen)
//na podstawie zmiennej isOnScreen, obiekty które nie powinny zostać zatrzymane na ekranie są niszczone
public class BoundsCheck : MonoBehaviour
{
    [Header("Definiowanie ręczne w panelu inspector")]
    public float radius = 1f; //promień obiektu
    public bool keepOnScreen = true; 

    [Header("Definiowanie dynamiczne")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;
    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown; // strona, z której obiekt opuścił ekran

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }
    // Start is called before the first frame update
        // działa dla kamery Main Camera o rzucie prostokątnym transform 0,0,0

    // Update is called once per frame
    void LateUpdate()
    {
        // sprawdzenie czy obiekt opuścił ekran
        isOnScreen = true; //założenie, że nie opuścił ekranu
        Vector3 pos = transform.position;
        offDown = offLeft = offRight = offUp = false;
        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            offRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            offUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            offDown = true;
        }
        //jeśli obiekt opuścił ekran z którejkolwiek strony - isOnScreen = false
        isOnScreen = !(offRight || offLeft || offUp || offDown);

        //jeśli obiekt ma pozostać na ekranie, utrzymaj go na widoku kamery
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offDown = offLeft = offRight = offUp = false;
        }
    }
    void OnDrawGizmos()
    {
        //narysowanie delikatnej granicy ekranu
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
