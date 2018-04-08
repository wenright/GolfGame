// https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html

using System.Linq;
using UnityEngine;

public class MeshCombiner : MonoBehaviour {

	void Start () {
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		for (int i = 0; i < meshFilters.Length; i++) {
			if (meshFilters[i].sharedMesh == null) {
				continue;
			}

			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    Mesh mesh = new Mesh();
		mesh.name = "Level Mesh";
    mesh.CombineMeshes(combine);
		mesh.triangles = mesh.triangles.Reverse().ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
    GetComponent<MeshCollider>().sharedMesh = mesh;
	}

}
