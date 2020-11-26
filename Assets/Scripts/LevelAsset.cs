using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelAsset : ScriptableObject
{
    public bool blhe;
    [System.Serializable]
    public struct Lister 
    {
        public List<int> vs;

    }

    [SerializeField]
    public List<Lister> list;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
