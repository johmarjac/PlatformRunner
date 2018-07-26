using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformRunner
{
    public class Spieler
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Animations Animationen;
        public Animations Explosions;
        public SpriteEffects Effects;
        public SpriteBatch Batch;

        public Texture2D HerzTexture;
        public int Leben;

        public const float Geschwindigkeit = 0.5f;

        public Spieler()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Animationen = new Animations();
            Explosions = new Animations();
            Leben = 3;

            var stehanimation = new Animation("Stehen");
            stehanimation.Breite = 80;
            stehanimation.Höhe = 110;
            stehanimation.AddFrame(0);

            Animationen.AddAnimation(stehanimation);

            var laufanimation = new Animation("Laufen");
            laufanimation.Breite = 80;
            laufanimation.Höhe = 110;
            laufanimation.AddFrame(9);
            laufanimation.AddFrame(10);

            Animationen.AddAnimation(laufanimation);
            
            var wegtreten = new Animation("Treten");
            wegtreten.Breite = 80;
            wegtreten.Höhe = 110;
            wegtreten.AddFrame(15);

            Animationen.AddAnimation(wegtreten);

            var schlagen = new Animation("Schlagen");
            schlagen.Breite = 80;
            schlagen.Höhe = 110;
            schlagen.AddFrame(14);

            Animationen.AddAnimation(schlagen);

            var springen = new Animation("Springen");
            springen.Breite = 80;
            springen.Höhe = 110;
            springen.AddFrame(1);

            Animationen.AddAnimation(springen);


            var ducken = new Animation("Ducken");
            ducken.Breite = 80;
            ducken.Höhe = 110;
            ducken.AddFrame(3);

            Animationen.AddAnimation(ducken);


            var slide = new Animation("Slide");
            slide.Breite = 80;
            slide.Höhe = 110;
            slide.AddFrame(19);

            Animationen.AddAnimation(slide);

            var morden = new Animation("Morden");
            morden.Breite = 80;
            morden.Höhe = 110;
            morden.AddFrame(21);

            Animationen.AddAnimation(morden);

            var explosion = new Animation("Explosion");
            explosion.Breite = 320;
            explosion.Höhe = 332;
            for(int i = 0; i < 25; i++)
            {
                explosion.AddFrame(i);
            }

            Explosions.AddAnimation(explosion);
        }
        
        public void Move(Vector2 delta)
        {
            Position += delta;
        }
        
        public void Update(GameTime gameTime)
        {
            Animationen.Update(gameTime);
            //Explosions.Update(gameTime);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (Batch == null)
                Batch = new SpriteBatch(batch.GraphicsDevice);

            var frameIndex = Animationen.Aktuell.Frames[Animationen.Aktuell.AktuellerIndex];
            var rectangle = Animationen.Aktuell.GetFrame(frameIndex);

            batch.Draw(Animationen.Textur, 
                new Rectangle(Position.ToPoint(), new Point(Animationen.Aktuell.Breite, Animationen.Aktuell.Höhe)),
                rectangle, Color.White, 0f, Vector2.Zero, Effects, 0f);



            var frameIndex1 = Explosions.Aktuell.Frames[Explosions.Aktuell.AktuellerIndex];
            var rectangle1 = Explosions.Aktuell.GetFrame(frameIndex);

            Batch.Begin();
            for (int i = 0; i < Leben; i++)
            {
                Batch.Draw(HerzTexture, new Vector2(19 * i, 20), Color.White);
            }
            Batch.End();

            //batch.Draw(Explosions.Textur,
            //new Rectangle(Position.ToPoint(), new Point(Explosions.Aktuell.Breite, Explosions.Aktuell.Höhe)),
            //rectangle, Color.White, 0f, Vector2.Zero, Effects, 0f);
        }
    }
}