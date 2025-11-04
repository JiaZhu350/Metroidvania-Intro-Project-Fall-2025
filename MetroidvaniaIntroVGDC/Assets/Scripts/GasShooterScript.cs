using System.Collections.Generic;
using UnityEngine;

public class GasShooterScript : MonoBehaviour
{
    private float _timer;
    
    public GameObject gasCloudPrefab;
    public GameObject spawnPoint;
    public float timeBetweenClouds;
    public float gasCloudLifetime;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenClouds)
        {
            var gasCloud = Instantiate(gasCloudPrefab, spawnPoint.transform.position, Quaternion.identity);
            
            Destroy(gasCloud, gasCloudLifetime);
            _timer = 0;
        }
    }
}
