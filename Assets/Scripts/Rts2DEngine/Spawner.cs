using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject enemy;

    public int amountPerWave = 1;
    public float WaveRate = 5.0f;
    public float SpawnDelay = 0.5f;

    public float GetWaveRate()
    {
        return WaveRate + (SpawnDelay * (amountPerWave-1));
    }

    public IEnumerator Spawn()
    {
        for (int i=0; i <= (amountPerWave-1); i++)
        {
            Instantiate(enemy, transform.position, transform.rotation);

            float m_RandomSpawnDelay = Random.Range(SpawnDelay * 0.5f, SpawnDelay * 1.5f);
            yield return new WaitForSeconds(m_RandomSpawnDelay);
        }
    }
}
