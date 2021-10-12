using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    #region Singleton
    public static PlayerSkills instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Skill> skills = new List<Skill>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
