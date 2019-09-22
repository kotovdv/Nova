using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 offset;
    public MeshFilter meshFilter;

    private void Start()
    {
        meshFilter.mesh = CreateGridMesh(11, offset);
    }
    
    private Mesh CreateGridMesh(int N, Vector2 offset)
    {
        //TODO BUG 
        var leftMostX = -N / 2;
        var rightMostX = N / 2;

        var topMostY = N / 2;
        var bottomMostY = -N / 2;

        var points = 4 * (N + 1);
        var indices = new int[points];
        var vertices = new Vector3[points];

        //Horizontal vertices.
        for (int line = 0, vertex = 0; line < N + 1; line++, vertex += 2)
        {
            indices[vertex] = vertex;
            indices[vertex + 1] = vertex + 1;
            vertices[vertex] = new Vector3(offset.x + leftMostX, offset.y + topMostY - line);
            vertices[vertex + 1] = new Vector3(offset.x + rightMostX, offset.y + topMostY - line);
        }

        //Vertical vertices.
        for (int line = 0, vertex = 2 * (N + 1); line < N + 1; line++, vertex += 2)
        {
            indices[vertex] = vertex;
            indices[vertex + 1] = vertex + 1;
            vertices[vertex] = new Vector3(offset.x + leftMostX + line, offset.y + topMostY);
            vertices[vertex + 1] = new Vector3(offset.x + leftMostX + line, offset.y + bottomMostY);
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);

        return mesh;
    }
}