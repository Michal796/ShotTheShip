using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //klasa definiująca typ pocisku, w celu poinformowania obiektu Enemy z jakiego rodzaju broni został trafiony
    //(ile odjąć hp), oraz nadania odpowiedniego koloru pocisku
    private BoundsCheck bndCheck;
    private Renderer rend;

    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type; //modyfikowalne w panelu inspector

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
