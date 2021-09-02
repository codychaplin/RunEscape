using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TickSystem.Create(this.transform);
        TickSystem.OnTick += delegate (object sender, TickSystem.OnTickEventArgs e)
        {
            //Debug.Log(TickSystem.GetTick());
        };
        TickSystem.OnBigTick += delegate (object sender, TickSystem.OnTickEventArgs e)
        {
            //Debug.Log("Big Tick: " + e.tick);
        };
    }
}
