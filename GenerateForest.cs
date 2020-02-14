using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateForest : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;
    [SerializeField] int numberOfTrees;
    [SerializeField] bool isFirst;
    [SerializeField] GenerateForest nextGenerator;
    [SerializeField] bool isTotem;

    private void Start()
    {
        if (isFirst) GenerateNewForest(new List<GameObject>());
    }

    public void GenerateNewForest(List<GameObject> allTrees)
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            int attempts = 0;
            GameObject tree = Instantiate(treePrefab);
            bool tooClose = true;
            while (tooClose && attempts < 10)
            {
                if (isTotem)  tree.transform.position = new Vector3(Random.Range(-30f, 30f), tree.transform.position.y, Random.Range(-30f, 30f));
                else tree.transform.position = new Vector3(Random.Range(-60f, 60f), tree.transform.position.y, Random.Range(-60f, 60f));
                tooClose = false;
                foreach (GameObject otherTree in allTrees)
                {
                    if (otherTree != null)
                    {
                        float deltaX = tree.transform.position.x - otherTree.transform.position.x;
                        float deltaY = tree.transform.position.y - otherTree.transform.position.y;
                        float deltaZ = tree.transform.position.z - otherTree.transform.position.z;
                        float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                        if (distance <= 10f) tooClose = true;
                    }
                }
                attempts++;
            }
            tree.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
            tree.transform.SetParent(transform);
            allTrees.Add(tree);

        }
        if (nextGenerator != null) nextGenerator.GenerateNewForest(allTrees);
    }
}
