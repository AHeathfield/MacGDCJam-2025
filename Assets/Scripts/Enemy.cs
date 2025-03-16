using UnityEngine;

// Not sure if this is best way lol who cares
public class Enemy : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    public float GetDamage() 
    {
        return damage;
    }
}
