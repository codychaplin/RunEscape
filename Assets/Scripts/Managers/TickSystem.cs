using System;
using UnityEngine;

public static class TickSystem
{
    public class OnTickEventArgs : EventArgs { public int tick; }

    public static event EventHandler<OnTickEventArgs> OnTick; // event on each tick
    public static event EventHandler<OnTickEventArgs> OnBigTick; // event on each tick

    public static GameObject TickGameObject;
    const float TICK_TIME = 1f; // x * 1000 milliseconds
    static int tick;

    public static int GetTick()
    {
        return tick;
    }

    public static void Create(Transform parent)
    {
        if (TickGameObject == null)
        {
            TickGameObject = new GameObject("TickSystem");
            TickGameObject.AddComponent<TickObject>();
            TickGameObject.transform.SetParent(parent);
        }
    }

    class TickObject : MonoBehaviour
    {
        float tickTimer; // used to increment tick

        // Start is called before the first frame update
        void Start()
        {
            tick = 0;
        }

        // Update is called once per frame
        void Update()
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= TICK_TIME)
            {
                tickTimer -= TICK_TIME;

                tick++;
                if (OnTick != null)
                    OnTick(this, new OnTickEventArgs { tick = tick });

                if (tick % 5 == 0)
                    if (OnBigTick != null)
                        OnBigTick(this, new OnTickEventArgs { tick = tick });
            }
        }
    }
}
