using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateParticleSpeed : MonoBehaviour
{
    [SerializeField] private float scale = 1f;
    private ParticleSystem ps;
    private float currentPlaybackSpeed;

    void Start()
    {
        currentPlaybackSpeed = GlobalParams.speedScale;
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (currentPlaybackSpeed != GlobalParams.speedScale && ps != null)
        {
            currentPlaybackSpeed = GlobalParams.speedScale;
            var _ps = ps.main;
            _ps.simulationSpeed = currentPlaybackSpeed * scale;
        }
    }
}
