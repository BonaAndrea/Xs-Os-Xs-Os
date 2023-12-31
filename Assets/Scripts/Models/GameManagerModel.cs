using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Single,
    Local,
    Online,
    Uninitialized
};   
public class GameManagerModel : MonoBehaviour
{
 
    
    public List<int[]> matrixes = new List<int[]>();

    public GameMode GameMode =GameMode.Uninitialized; //0 single, 1 local, 2 online
// Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int[] matrix = new int[9];
            for (int j = 0; j < 9; j++)
            {
                matrix[j] = 0;
            }
            matrixes.Add(matrix);
        }
    }
}
