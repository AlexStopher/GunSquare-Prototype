using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPoolableObject
{
    void ReturnToPool();
    void FetchFromPool(Vector3 SpawnPoint);
}

