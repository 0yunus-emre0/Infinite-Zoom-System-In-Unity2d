using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    private float currentSize;
    private float scrollSpeed = 5.0f;
    private float scrollFadeDuration = 0.2f;
    private List<GameObject> objects;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        int totalObjectCount = inputOfObjects.Length;
        objects = new List<GameObject>();
        for (int i = 0; i < totalObjectCount; i++)
        {
            GameObject obj = Instantiate(objectPrefab, new Vector3(-100, 0, 0), Quaternion.identity);
            ObjectBehaviour objBehavior = obj.GetComponent<ObjectBehaviour>();
            objBehavior.infos = inputOfObjects[i];
            objBehavior.InitInfos();
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    private void Start()
    {
        WhichObjectShouldSpawn();
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            WhichObjectShouldSpawn();
            StartCoroutine(DoScroll(scrollInput));
        }
    }

    private void WhichObjectShouldSpawn()
    {
        foreach (var obj in objects)
        {
            ObjectBehaviour objBehavior = obj.GetComponent<ObjectBehaviour>();
            ObjectInfos objInfo = objBehavior.infos;
            if (currentSize - 5 < objInfo.objectSize && objInfo.objectSize < currentSize + 5)
            {
                obj.SetActive(true);
            }
            else if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }

    private IEnumerator DoScroll(float scrollInput)
    {
        currentSize = Mathf.Clamp(currentSize, -35f, 27.2f);
        
        float targetSize = currentSize + scrollInput * scrollSpeed;
        targetSize = Mathf.Clamp(targetSize, -34.9f, 27.1f);
        
        float counter = 0f;
        while (counter < scrollFadeDuration)
        {
            float newPos = Mathf.Lerp(currentSize, targetSize, counter / scrollFadeDuration);
            counter += Time.deltaTime;
            currentSize = newPos;
            yield return null;
        }
    }
}
