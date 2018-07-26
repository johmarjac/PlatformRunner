using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace PlatformRunner
{
    public class Zombie : Angriffsgegner
    {
        private Stopwatch Watch;
        int sekunden = 0;
        int richtung = 0;
                
        public Zombie()
        {
            Watch = new Stopwatch();
            Watch.Start();
            Bewegungsgeschwindigkeit = 0.25f;
            richtung = GameMain.Zufall.Next(0, 1);
            sekunden = GameMain.Zufall.Next(1000, 5000);

            Leben = 5;

            Position = new Vector2(GameMain.Zufall.Next(0, 12200), GameMain.Zufall.Next(0, 1000));
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            Animationen.Textur = content.Load<Texture2D>("Assets/zombie_tilesheet");

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

            Animationen.Play("Stehen");
        }

        public void Move(Vector2 delta)
        {
            Position += delta;

            Animationen.Play("Laufen");

            if(richtung == 0)
            {
                Effects = SpriteEffects.FlipHorizontally;
            }
            else if(richtung == 1)
            {
                Effects = SpriteEffects.None;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Animationen.Update(gameTime);

            Velocity += new Vector2(Velocity.X, Bewegungsgeschwindigkeit);
            Position += Velocity;

            if(Watch.ElapsedMilliseconds >= sekunden)
            {
                sekunden = GameMain.Zufall.Next(1000, 5000);

                if (richtung == 1)
                    richtung = 0;
                else richtung = 1;
                Watch.Restart();
            }

            if (richtung == 0)
                Move(new Vector2(-Bewegungsgeschwindigkeit * gameTime.ElapsedGameTime.Milliseconds, 0));
            else
                Move(new Vector2(Bewegungsgeschwindigkeit * gameTime.ElapsedGameTime.Milliseconds, 0));
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);

            var frameIndex = Animationen.Aktuell.Frames[Animationen.Aktuell.AktuellerIndex];
            var rectangle = Animationen.Aktuell.GetFrame(frameIndex);

            batch.Draw(Animationen.Textur,
                new Rectangle(Position.ToPoint(), new Point(Animationen.Aktuell.Breite, Animationen.Aktuell.Höhe)),
                rectangle, Color.White, 0f, Vector2.Zero, Effects, 0f);

            
        }
    }
}