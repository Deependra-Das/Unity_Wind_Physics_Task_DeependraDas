using System.Collections;
using UnityEngine;

public class ExplosionVfx : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private VfxPoolService _poolService;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Initialize(VfxPoolService poolService)
    {
        _poolService = poolService;
    }

    public void Play()
    {
        _particleSystem.Clear();
        _particleSystem.Play();

        StartCoroutine(ReturnAfterDuration());
    }

    private IEnumerator ReturnAfterDuration()
    {
        ParticleSystem.MainModule main = _particleSystem.main;

        float duration = main.duration;

        if (main.startLifetime.mode == ParticleSystemCurveMode.Constant)
        {
            duration += main.startLifetime.constant;
        }

        yield return new WaitForSeconds(duration);

        _poolService.ReturnExplosion(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (_particleSystem == null)
            return;

        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
