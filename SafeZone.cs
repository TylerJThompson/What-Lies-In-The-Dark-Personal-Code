using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private bool zoneIsSafe = false;

    // Update is called once per frame
    void Update()
    {
        if (zoneIsSafe)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                float deltaX = transform.position.x - monster.transform.position.x;
                float deltaY = transform.position.y - monster.transform.position.y;
                float deltaZ = transform.position.z - monster.transform.position.z;
                float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                if (distance < 10f) monster.GetComponent<EnemyController>().SetHitTrigger();
            }
        }
    }

    public void SetZoneSafe()
    {
        zoneIsSafe = true;
    }
}
