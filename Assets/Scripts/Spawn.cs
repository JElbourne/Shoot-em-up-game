using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public List<Spawner> spawners = new List<Spawner>();

    Spawner m_CurrentSpawner;
    float m_WaveCooldown;

    private void Start()
    {
        int m_Level = 0; //TODO get from player;
        UpdateSpawnLevel(m_Level);
    }

    // Update is called once per frame
    void Update () {
        m_WaveCooldown -= Time.deltaTime;
        if (m_CurrentSpawner && m_WaveCooldown <= 0)
            NextWave();
    }

    void NextWave()
    { 
       StartCoroutine(m_CurrentSpawner.Spawn());
       m_WaveCooldown = m_CurrentSpawner.GetWaveRate();
    }

    public void UpdateSpawnLevel(int level)
    {
        if(spawners.Count > level)
        {
            m_CurrentSpawner = spawners[level];
            m_WaveCooldown = m_CurrentSpawner.GetWaveRate();
        }
    }
}
