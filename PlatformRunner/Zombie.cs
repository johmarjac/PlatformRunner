using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformRunner
{
    public class Zombie : Angriffsgegner
    {
        public override void LoadContent(ContentManager content)
        {
            Animationen.Textur = content.Load<Texture2D>("Assets/zombie_tilesheet");
        }

        public override void Update(GameTime gameTime)
        {
            Animationen.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

        }
    }
}