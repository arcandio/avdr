using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ApplyMaterial: MonoBehaviour
{
    [MenuItem("AVDR/Content/Apply Material To Visual Meshes", priority = 21)]
    static void ApplyMaterialToSelection() {
        Debug.LogWarning("Applying Material to Selection");

        /* First, get setup from the selections the user made */
        Material material = null;
        List<GameObject> prefabs = new List<GameObject>();
        foreach(object o in Selection.objects){
            Debug.Log(o);
            Material tempMat = o as Material;
            if(tempMat) {
                material = tempMat;
                continue;
            }
            GameObject tempGO = o as GameObject;
            if (tempGO) {
                prefabs.Add(tempGO);
            }
        }

        /* guard clauses */
        if(prefabs.Count == 0) {
            Debug.LogError("No prefabs were selected.");
            return;
        }
        else if(material == null) {
            Debug.Log("No material was selected.");
            return;
        }

        /* Iterate through our prefabs */
        foreach(GameObject prefab in prefabs) {

            /* get the visual mesh */
            Transform visualMeshTransform = prefab.transform.Find("visual mesh");
            if(visualMeshTransform == null) {
                Debug.LogError($"no object named `visual mesh` in {visualMeshTransform.name}'s children.");
                return;
            }
            MeshRenderer meshRenderer = visualMeshTransform.GetComponent<MeshRenderer>();
            if(meshRenderer == null) {
                Debug.LogError("No mesh renderer on visual mesh game object.");
            }

            /* apply material */
            meshRenderer.material = material;

            /* apply changes to prefab */
            PrefabUtility.SavePrefabAsset(prefab, out bool saved);
            if(saved == false) {
                Debug.LogError("failed to save prefab " + prefab.name);
            }
        }

        Debug.LogWarning($"Applied {material.name} to {prefabs.Count} prefabs.");
    }
}
