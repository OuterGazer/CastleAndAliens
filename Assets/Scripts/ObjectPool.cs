using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] int poolSize = default;

    [SerializeField] Transform enemyPrefab;
    [SerializeField] float spawnTime = default;

    private Transform[] pool;


    private void Awake()
    {
        PopulatePool();
    }

    private void PopulatePool() 
    {
        this.pool = new Transform[this.poolSize];

        for(int i = 0; i < this.pool.Length; i++)
        {
            this.pool[i] = GameObject.Instantiate<Transform>(this.enemyPrefab, this.gameObject.transform);
            this.pool[i].gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        WaitForSeconds timeBetweenEnemies = new WaitForSeconds(this.spawnTime);

        while (Application.isPlaying)
        {
            for(int i = 0; i < this.pool.Length; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    pool[i].gameObject.SetActive(true);

                    yield return timeBetweenEnemies;
                }

                if (i == this.pool.Length)
                    i = 0;

                yield return null;
            }
        }
    }
}