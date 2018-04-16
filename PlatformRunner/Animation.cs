using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PlatformRunner
{
    public class Animation
    {
        public int Breite;
        public int Höhe;

        public int BildBreite;
        public int BildHöhe;

        public List<int> Frames;
        public int AktuellerIndex;

        public int Dauer = 250;
        public double LetzteZeit = 0;

        public string Name { get; }

        public Animation(string name)
        {
            Frames = new List<int>();
            Name = name;
        }

        public void AddFrame(int frameIndex)
        {
            Frames.Add(frameIndex);
        }
        
        public Rectangle GetFrame(int index)
        {
            int X = index % (BildBreite / Breite);
            int Y = index / (BildBreite / Breite);

            return new Rectangle(X * Breite, Y * Höhe, Breite, Höhe);
        }

        public void Update(GameTime gameTime)
        {
            var aktuelleZeit = gameTime.TotalGameTime.TotalMilliseconds;

            if((aktuelleZeit - LetzteZeit) > Dauer)
            {
                if (AktuellerIndex + 1 == Frames.Count)
                    AktuellerIndex = 0;
                else
                    AktuellerIndex++;

                LetzteZeit = aktuelleZeit;
            }
        }
    }
}