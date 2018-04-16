using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace PlatformRunner
{
    public class Animations
    {
        public Texture2D Textur;
        public List<Animation> AnimationList;

        public Animation Aktuell { get; set; }

        public Animations()
        {
            AnimationList = new List<Animation>();
        }

        public void AddAnimation(Animation animation)
        {
            
            AnimationList.Add(animation);
        }

        public void Play(string name)
        {
            var animation = AnimationList.FirstOrDefault(t => t.Name == name);
            Aktuell = animation ?? throw new System.Exception("Animation gibt es nicht.");

            Aktuell.BildBreite = Textur.Bounds.Width;
            Aktuell.BildHöhe = Textur.Bounds.Height;
        }

        public void Update(GameTime gameTime)
        {
            Aktuell?.Update(gameTime);
        }
    }
}