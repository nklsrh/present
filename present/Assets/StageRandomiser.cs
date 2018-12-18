using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRandomiser : MonoBehaviour {

    public List<Transform> groups;

    internal void Randomise()
    {
        foreach (var group in groups)
        {
            EnableRandomChildInTransform(group);
        }
    }

    private void EnableRandomChildInTransform(Transform parent)
    {
        int randomIndex = UnityEngine.Random.Range(0, parent.childCount);
        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(i == randomIndex);
        }
    }
}
