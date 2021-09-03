using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using Siccity.GLTFUtility;

public class LoadModel : MonoBehaviour {

    [Tooltip("Can only load models from the Asset/Models folder per this script.")]
    [SerializeField] private string fileName = null;

    private List<Mesh> GetMeshes( Transform rootTransform )
    {
        List<Mesh> meshes = new List<Mesh>();

        for (int i=0; i<rootTransform.childCount; i++)
        {
            Transform child = rootTransform.GetChild(i);
            SkinnedMeshRenderer skin = child.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skin != null)
            {
                meshes.Add(skin.sharedMesh);
            }
            else
            {
                meshes.AddRange(GetMeshes(child));
            }
        }
        return meshes;
    }

    private void Start()
    {
        var assetLoaderOptions = new ImportSettings(); //Creates an Asset Loader Options object.
        var filename = fileName;
        Importer.ImportGLTFAsync(filename, assetLoaderOptions, OnFinishLoading);  //Loads our model.
    }

    private void OnFinishLoading(GameObject loadedModel, AnimationClip[] anims) 
    {
        GetMeshes(loadedModel.transform);
    }
}