using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TechSupportMario.Entity.Background
{
    class Changing_background : Sprite
    {
        private int lastChange;
        private readonly Queue<Texture2D> textureQueue;

        public Changing_background(Texture2D texture, Queue<Texture2D> others) : base(texture)
        {
            lastChange = 0;
            textureQueue = others;
        }

        public override void Update(GameTime gameTime)
        {
            if(lastChange < 120)
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                textureQueue.Enqueue(Texture);
                Texture = textureQueue.Dequeue();
                lastChange = 0;
            }
        }
    }
}
