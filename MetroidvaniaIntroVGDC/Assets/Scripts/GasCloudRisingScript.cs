using UnityEngine;

public class GasCloudRisingScript : MonoBehaviour
{
    public float risingSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (risingSpeed * Time.deltaTime));
    }
}
