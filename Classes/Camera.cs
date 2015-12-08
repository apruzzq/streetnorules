using System;
using Microsoft.Xna.Framework;

namespace Les_Loubards
{
    class Camera
    {
        public static Vector2 Position;

        public static Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return worldPosition - Camera.Position + 
                new Vector2(Game1.SCREEN_WIDHT/2, Game1.SCREEN_HEIGHT/2);
        }
    }
}
