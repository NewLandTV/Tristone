using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Grass Settings"), Space, SerializeField]
    private GameObject grassPrefab;
    [Range(0, 32), SerializeField]
    private int grassCount = 8;

    private GameObject[] grasses;

    private void Awake()
    {
        grasses = new GameObject[grassCount];

        for (int i = 0; i < grassCount; i++)
        {
            MakeGrass(i);
        }

        CombineMesh();
    }

    private void MakeGrass(int index)
    {
        float halfSize = transform.localScale.x * 5f;

        Vector3 center = transform.position;
        Vector3 position = new Vector3(Random.Range(center.x - halfSize, center.x + halfSize), 0.05f, Random.Range(center.z - halfSize, center.z + halfSize));
        Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

        GameObject instance = Instantiate(grassPrefab, Vector3.zero, rotation, transform);

        instance.transform.localPosition = position;

        grasses[index] = instance;
    }

    private void CombineMesh()
    {
        Mesh mesh = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combines = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        mesh.CombineMeshes(combines);

        meshFilters[0].sharedMesh = mesh;
    }

    public void SetActiveGrass(bool value)
    {
        for (int i = 0; i < grasses.Length; i++)
        {
            grasses[i].SetActive(value);
        }
    }
}
