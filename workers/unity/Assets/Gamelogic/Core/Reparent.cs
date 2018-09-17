using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparent : MonoBehaviour {

    public string folderName;

    private Transform folderTransform;
    private static Dictionary<string, Transform> folderMap = new Dictionary<string, Transform>();

    void Awake()
    {
        if (!folderMap.ContainsKey(folderName))
        {
            var folder = GameObject.Find(folderName);
            if (folder != null)
            {
                folderMap.Add(folderName, folder.transform);
            }
            else
            {
                folderMap.Add(folderName, null);
            }
        }
        folderTransform = folderMap[folderName];
    }

	void Start()
    {
        if (folderTransform != null)
        {
            transform.SetParent(folderTransform);
        }
	}
}
