using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    float groundDistance = -4f;
    public GameConstants gameConstants;

    void Start()
    {
        gameConstants.spawned = 0;

        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.greenEnemy);

        GameManager.OnScore += spawnNewEnemy;

    }


    void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null)
        {
            //set position
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.position = new Vector3(Random.Range(-20f, 20f), groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("not enough items in the pool!");
        }
    }

    public void spawnNewEnemy()
    {
        if (gameConstants.spawned < 5)
        {
            ObjectType i = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
            spawnFromPooler(i);
            gameConstants.spawned += 1;
        }
    }

}
