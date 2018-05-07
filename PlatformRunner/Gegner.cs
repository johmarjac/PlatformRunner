using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PlatformRunner
{
    public abstract class Gegner
    {
        public Animations Animationen { get; set; }
        public int Leben { get; set; }
        public Vector2 Position { get; set; }
        public float Bewegungsgeschwindigkeit { get; set; }

        public Gegner()
        {
            Animationen = new Animations();
            Position = new Vector2();
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}