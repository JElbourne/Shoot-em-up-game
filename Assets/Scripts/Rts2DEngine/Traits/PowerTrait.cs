using UnityEngine;

namespace Rts2DEngine
{
    [RequireComponent(typeof(PlayerStats))]
    public class PowerTrait : MonoBehaviour
    {

        PlayerStats m_Stats;
        MoveTrait m_MoveTrait;
        float m_PowerUsage = 0f;

        public int distancePerPowerUnit = 10;

        // Use this for initialization
        void Awake()
        {
            m_Stats = PlayerStats.instance;
            m_MoveTrait = GetComponent<MoveTrait>();
        }

        // Update is called once per frame
        void Update()
        {
            m_PowerUsage += m_MoveTrait.lastVelocity.magnitude * Time.deltaTime;
            if (m_PowerUsage > distancePerPowerUnit)
            {
                m_Stats.UsePower(1);
                m_PowerUsage = 0f;
            }
        }
    }

}