using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformRunner
{
    public abstract class Gegner
    {
        public Animations Animationen { get; set; }
        public SpriteEffects Effects;
        public int Leben { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Bewegungsgeschwindigkeit { get; set; }
        public Texture2D HerzTexture;

        public bool IstAmLeben
        {
            get
            {
                return Leben > 0;
            }
        }

        public Gegner()
        {
            Animationen = new Animations();
            Position = new Vector2();
        }

        public virtual void LoadContent(ContentManager content)
        {
            HerzTexture = content.Load<Texture2D>("Assets/herz");
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var widthTotal = HerzTexture.Width * Leben;
            var widthTotalDividedByTwo = widthTotal / 2f;

            for(int i = 0; i < Leben; i++)
            {
                batch.Draw(HerzTexture, new Vector2(Position.X - widthTotalDividedByTwo + (Animationen.Aktuell.Breite / 2) + (i * HerzTexture.Width), Position.Y), Color.White);
            }
        }
    }
}