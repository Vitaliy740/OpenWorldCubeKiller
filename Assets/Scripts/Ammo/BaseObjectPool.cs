using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectPool : BaseObjectScene
{
    [SerializeField]
    protected List<ObjectPoolData> _datas=new List<ObjectPoolData>();

    protected List<GameObject> _objectPrefabs=new List<GameObject>();
    public static BaseObjectPool Instance;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) 
        {
            Instance = this;
        }
        foreach (var item in _datas)
        {
            for (int i = 0; i < item.NumberOfObjectsToCreate; i++)
            {
                var obj=MonoBehaviour.Instantiate(item.ObjectPrefab, this.transform);
                _objectPrefabs.Add( obj);
                obj.SetActive(false);
            }
        }
        
    }
    public virtual T InstantiateNewObject<T>(T original,Vector3 pos, Quaternion rot) 
    {
        for (int i = 0; i < _objectPrefabs.Count; i++)
        {
            if(!_objectPrefabs[i].activeInHierarchy &&  _objectPrefabs[i].GetComponent<T>() != null) 
            {
                _objectPrefabs[i].transform.position = pos;
                _objectPrefabs[i].transform.rotation = rot;
                _objectPrefabs[i].SetActive(true);
                return _objectPrefabs[i].GetComponent<T>();
            }
        }
        return default(T);
    }
    public virtual void DestroyObj(GameObject obj) 
    {
        var rb = obj.GetComponent<Rigidbody>();
        obj.SetActive(false);
    }
    public virtual void DestroyObj(GameObject obj, float t) 
    {
        StartCoroutine(SetForDestruction(obj, t));
    }
    private IEnumerator SetForDestruction(GameObject obj,float t) 
    {
        yield return new WaitForSeconds(t);
        DestroyObj(obj);
    }

}
[System.Serializable]
public class ObjectPoolData 
{
    public int NumberOfObjectsToCreate = 100;
    public GameObject ObjectPrefab;
}
