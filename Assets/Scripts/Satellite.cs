using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    public GameObject[] ShrapnelPrefabs;
    public GameObject ExplosionPrefab;
    public Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetInstanceID() > collision.transform.GetInstanceID())
        {
            int nr = Random.Range(2, 5) + 10;
            for (int i = 0; i < nr; i++)
            {
                int tr = Random.Range(0, ShrapnelPrefabs.Length);
                var obj = Instantiate(ShrapnelPrefabs[tr], transform.position, Quaternion.identity);
                obj.GetComponent<Physics>().Velocity = GetComponent<Physics>().Velocity + Random.insideUnitCircle * 2.0f;
            }

            Physics other = collision.transform.GetComponent<Physics>();
            if (other)
            {
                nr = Random.Range(2, 5) + 10;
                for (int i = 0; i < nr; i++)
                {
                    int tr = Random.Range(0, ShrapnelPrefabs.Length);
                    var obj = Instantiate(ShrapnelPrefabs[tr], transform.position, Quaternion.identity);
                    obj.GetComponent<Physics>().Velocity = other.Velocity + Random.insideUnitCircle * 2.0f;
                }
            }
        }

        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        player.RemoveSatellite(gameObject);
    }
}
