using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolerManager : Singleton<ObjectPoolerManager>
{
    #region Public Fields

    public List<ObjectPoolItem> objectsToPool;

    public List<GameObject> pooledObjects;

    #endregion Public Fields

    #region Public Methods

    // Get pooled object by name.
    public GameObject GetPooledObject(string nameObject)
    {
        // Check if theres any pooled object active and its name.
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].name == nameObject)
                return pooledObjects[i];
        }

        // Add object to pool if possible.
        foreach (ObjectPoolItem item in objectsToPool)
        {
            if (item.objectToPool.name == nameObject && item.expandPoolAtRuntime)
                return GetGameObject(item);
        }

        return null;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion Protected Methods

    #region Private Methods

    private GameObject GetGameObject(ObjectPoolItem item)
    {
        item.objectToPool.SetActive(false);
        GameObject obj = Instantiate(item.objectToPool) as GameObject;
        obj.transform.parent = item.Pool.transform;
        obj.name = item.objectToPool.name;
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    private void Init()
    {
        GameObject pools = new GameObject("ObjectPools");
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in objectsToPool)
        {
            item.Pool = new GameObject(item.objectToPool.name + "_Pool");
            item.Pool.transform.parent = pools.transform;

            for (int i = 0; i < item.amountToPool; i++)
            {
                GetGameObject(item);
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "02 Game")
            Init();
    }

    #endregion Private Methods

    #region Public Classes

    [System.Serializable]
    public class ObjectPoolItem
    {
        #region Public Fields

        public int amountToPool;
        public bool expandPoolAtRuntime;
        public GameObject objectToPool;

        #endregion Public Fields

        #region Private Fields

        private GameObject pool;

        #endregion Private Fields

        #region Public Properties

        public GameObject Pool { get; set; }

        #endregion Public Properties
    }

    #endregion Public Classes
}