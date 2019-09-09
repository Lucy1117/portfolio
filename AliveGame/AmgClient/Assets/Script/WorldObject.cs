using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour {

    public static Dictionary<int, WorldObject> m_worldObjects = new Dictionary<int, WorldObject>();
    int m_id_INTERNAL = 0;

    public int m_id
    {
        get
        {
            return m_id_INTERNAL;
        }
        set
        {
            if (m_id_INTERNAL == value)
            {
                return;
            }
            if (m_id_INTERNAL != 0)
            {
                m_worldObjects.Remove(m_id_INTERNAL);
            }
            if (value != 0)
            {
                m_worldObjects.Add(value, this);
            }
            m_id_INTERNAL = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
