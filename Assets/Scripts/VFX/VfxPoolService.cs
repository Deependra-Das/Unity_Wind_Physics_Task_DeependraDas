using UnityEngine;
using System.Collections.Generic;

public class VfxPoolService
{
    private readonly ExplosionVfx _explosionPrefab;
    private readonly Transform _vfxPoolParent;

    private readonly Queue<ExplosionVfx> _explosionVfxPool = new();

    public VfxPoolService(Vfx_SO vfx_SO, Transform poolParent)
    {
        _explosionPrefab = vfx_SO.explosionPrefab;
        _vfxPoolParent = poolParent;

        for (int i = 0; i < vfx_SO.poolSize; i++)
        {
            CreateExplosion();
        }
    }

    private ExplosionVfx CreateExplosion()
    {
        ExplosionVfx explosion = Object.Instantiate(_explosionPrefab, _vfxPoolParent);
        explosion.gameObject.SetActive(false);
        _explosionVfxPool.Enqueue(explosion);

        return explosion;
    }

    public ExplosionVfx GetExplosion(Vector3 position)
    {
        if (_explosionVfxPool.Count == 0)
        {
            CreateExplosion();
        }

        ExplosionVfx explosion = _explosionVfxPool.Dequeue();
        explosion.transform.position = position;
        explosion.gameObject.SetActive(true);
        explosion.Initialize(this);
        return explosion;
    }

    public void ReturnExplosion(ExplosionVfx explosion)
    {
        if (explosion == null)
            return;

        explosion.gameObject.SetActive(false);
        _explosionVfxPool.Enqueue(explosion);
    }
}
