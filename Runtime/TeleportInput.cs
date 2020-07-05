using UnityEngine;

public class TeleportInput : MonoBehaviour
{
    public Teleportation m_targetToInteractWith;
    public KeyCode m_keyToUse = KeyCode.T;
    public bool m_switch;

    void Update()
    {
        if (Input.GetKeyDown(m_keyToUse))
        {
            m_switch = !m_switch;
            if (m_switch)
                m_targetToInteractWith.StartTeleport();
            else 
                m_targetToInteractWith.StopTeleport();

        }

    }

}
