using UnityEngine;

[CreateAssetMenu(fileName = "Vfx_SO", menuName = "ScriptableObjects/Vfx_SO")]
public class Vfx_SO : ScriptableObject
{
    public ExplosionVfx explosionPrefab;
    public int poolSize = 5;
}
