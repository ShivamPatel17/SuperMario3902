using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Background;

namespace TechSupportMario.CameraItems
{
    public class Layer : IComparable
    {
        private readonly Camera camera;
        public Vector2 Parallax { get; set; }
        public ISet<ISprite> Sprites { get; set; }
        public SamplerState SamplerEffect { get; set; }
        public int Order { get; set; }
        public bool FullLinearWrap { get; set; }
        public Layer(Camera camera)
        {
            this.camera = camera;
            Sprites = new HashSet<ISprite>();
            Parallax = Vector2.One;
            SamplerEffect = null;//defualt don't linear wrap
            Order = 0;
            FullLinearWrap = false;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerEffect, null, null, null, camera.GetViewMatrix(Parallax));
            foreach(ISprite sprite in Sprites)
            {
                if (SamplerEffect != null)
                {
                    if (FullLinearWrap)
                    {
                        ((Sprite)sprite).DrawArea = new Rectangle(new Point(0), camera.Limits.Value.Size);
                    }
                    else
                    {
                        ((Sprite)sprite).DrawArea = new Rectangle(new Point(0), new Point(((Rectangle)camera.Limits).Width, sprite.Texture.Height));
                    }
                    
                }
                sprite.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        public int CompareTo(object obj)
        {
            if (obj is Layer comp)
            {
                if (Order == comp.Order)
                {
                    return 0;
                }
                else if (Order > comp.Order)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Layer checking)
            {
                if (checking.Order == Order)
                {
                    if (checking.Parallax == Parallax)
                    {
                        if (checking.SamplerEffect == SamplerEffect)
                        {
                            if(checking.Sprites.Count == Sprites.Count)
                            {
                                return checking.Sprites.Equals(Sprites);
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Parallax.GetHashCode() + SamplerEffect.GetHashCode() + Sprites.GetHashCode();
        }
    }
}
