using UnityEngine;

namespace Rts2DEngine
{
    namespace Map
    {
        // Using class not struct as the bool properties are not set at the constructor.
        public class Room
        {
            public Vector2 gridPos;
            public int type;
            public bool doorTop, doorBot, doorLeft, doorRight;

            public Room(Vector2 _gridPos, int _type)
            {
                gridPos = _gridPos;
                type = _type;
            }
        }
    }
}

