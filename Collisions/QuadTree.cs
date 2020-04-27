using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TechSupportMario.Entity;
using TechSupportMario.Entity.MarioEntity;

namespace TechSupportMario.Collisions
{
    public class QuadTree
    {
        private const int NumLevels = 4;
        private const int MaxItems = 1;
        private readonly QuadTree[] nodes;
        private readonly QuadTree parent;

        public Rectangle Bounds { get; set; }
        public int Level { get; }
        private readonly ISet<IEntity> objects;

        public QuadTree(Rectangle bounds, int level)
        {
            Bounds = bounds;
            //split will intialize the nodes to actual quadtrees
            nodes = new QuadTree[4];
            objects = new HashSet<IEntity>();
            Level = level;
        }

        public ISet<IEntity> RetrieveAll()
        {
            ISet<IEntity> all = new HashSet<IEntity>();
            if(nodes[0] != null)
            {
                for(int i = 0; i < 4; i++)
                {
                    all.UnionWith(nodes[i].RetrieveAll());
                }
            }
            else
            {
                foreach(IEntity entity in objects)
                {
                    if (!all.Contains(entity))
                    {
                        all.Add(entity);
                    }
                }
            }
            return all;
        }

        /// <summary>
        /// this shouldn't be called outside of this class. Parent is only needed for the children of the root
        /// Node.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="level"></param>
        /// <param name="parent"></param>
        private QuadTree(Rectangle bounds, int level, QuadTree parent)
        {
            Bounds = bounds;
            //split will intialize the nodes to actual quadtrees
            nodes = new QuadTree[4];
            objects = new HashSet<IEntity>();
            Level = level;
            this.parent = parent;
        }

        /// <summary>
        /// Clears this node's objects and sets the children to null.
        /// </summary>
        public void Clear()
        {
            objects.Clear();
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// insert a sprite into the tree
        /// </summary>
        /// <param name="sprite"></param>
        public void Insert(IEntity entity)
        {

            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(entity.CollisionBox))
                    {
                        nodes[i].Insert(entity);
                    }
                }
            }
            else
            {
                if (!objects.Contains(entity))
                {
                    objects.Add(entity);
                    //it is possible that adding the object may cause the tree to need to split.
                    if (GetCollidable(Bounds).Count > MaxItems && NumLevels > Level)
                    {
                        Split();

                        IEntity[] entities = objects.ToArray();
                        for (int i = 0; i < entities.Length; i++)
                        {
                            for (int j = 0; j < nodes.Length; j++)
                            {
                                if (nodes[j].Bounds.Intersects(entities[i].CollisionBox))
                                {
                                    nodes[j].Insert(entities[i]);
                                }
                            }
                        }
                        objects.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Returns the nearest neighbors of the entity. This will only return collidable entities.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ISet<IEntity> Retrieve(IEntity entity)
        {
            ISet<IEntity> possibleCollisions = new HashSet<IEntity>();
            //add from the children nodes if they exist
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(entity.CollisionBox))
                    {
                        foreach (IEntity item in nodes[i].Retrieve(entity))
                        {
                            if (item.Collidable)
                            {
                                possibleCollisions.Add(item);
                            }
                        }
                    }

                }
            }
            else//this is a parent node
            {
                foreach (ISprite sprite in objects)
                {
                    if (sprite.Collidable)
                    {
                        possibleCollisions.Add((AbstractEntity)sprite);
                    }
                }
            }
            possibleCollisions.Remove(entity);//don't want the entity to collide with itself.
            return possibleCollisions;
        }

        /// <summary>
        /// Returns all the entities in the rectangular region that are in the quad tree.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public ISet<IEntity> AllEntities(Rectangle region)
        {
            ISet<IEntity> possibleCollisions = new HashSet<IEntity>();
            //add from the children nodes if they exist
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(region))
                    {
                        foreach (IEntity item in nodes[i].AllEntities(region))
                        {
                            if (!possibleCollisions.Contains(item))
                            {
                                possibleCollisions.Add(item);
                            }
                        }
                    }

                }
            }
            else
            {
                possibleCollisions.UnionWith(objects);
            }
            return possibleCollisions;
        }

        /// <summary>
        /// Returns all sprites that need to be updated that aren't moving in the rectangular region.
        /// </summary>
        /// <returns></returns>
        public ISet<IEntity> EntitiesNeedingUpdated(Rectangle region)
        {
            ISet<IEntity> sprites = new HashSet<IEntity>();
            foreach (IEntity sprite in AllEntities(region))
            {
                if (sprite.CollisionBox.Intersects(region) && sprite.NeedsUpdating)
                {
                    if (sprite.Velocity.X == 0 && sprite.Velocity.Y == 0)
                    {
                        sprites.Add(sprite);
                    }
                }
            }
            return sprites;
        }

        /// <summary>
        /// remove a single sprite from the quad tree and attempt to merge the sub trees.
        /// </summary>
        /// <param name="sprite"></param>
        public void Remove(IEntity entity)
        {
            
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(entity.CollisionBox))
                    {
                        nodes[i].Remove(entity);
                        //check if the children were merged.
                        if (nodes[i] == null)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                objects.Remove(entity);
                if (GetCollidable(Bounds).Count <= 1)
                {
                    parent?.TryMerge();
                }
            }
        }

        /// <summary>
        /// remove a single sprite from the quad tree with a large region to check for the entity than the collision box
        /// </summary>
        /// <param name="sprite"></param>
        public void Remove(IEntity entity, Rectangle checkRegion)//need if removing an object with a velocity, it may have moved out of a region it was in previously without calling remove.
        {
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(checkRegion))
                    {
                        nodes[i].Remove(entity, checkRegion);
                        //check if the children were merged.
                        if (nodes[i] == null)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                objects.Remove(entity);
                if (GetCollidable(Bounds).Count <= 1)
                {
                    parent?.TryMerge();
                }
            }
        }

        /// <summary>
        /// Will split the current quadrant into 4 equal parts and then move the elements in objects into them
        /// </summary>
        private void Split()
        {
            for (int i = 0; i < 4; i++)
            {
                int nextWidth = Bounds.Width / 2;
                int nextHeight = Bounds.Height / 2;
                Rectangle nextBounds = new Rectangle
                {
                    Size = new Point(nextWidth, nextHeight)
                };

                switch (i)
                {
                    case 0:
                        nextBounds.Location = new Point(Bounds.X, Bounds.Y);
                        break;
                    case 1:
                        nextBounds.Location = new Point(Bounds.X + nextWidth, Bounds.Y);
                        break;
                    case 2:
                        nextBounds.Location = new Point(Bounds.X, Bounds.Y + nextHeight);
                        break;
                    case 3:
                        nextBounds.Location = new Point(Bounds.X + nextWidth, Bounds.Y + nextHeight);
                        break;
                }
                nodes[i] = new QuadTree(nextBounds, Level + 1, this);
            }
        }

        /// <summary>
        /// return all sprites that have a collision box/Collidable property set to true.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public ISet<ISprite> GetCollidable(Rectangle region)
        {
            ISet<ISprite> collidables = new HashSet<ISprite>();
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(region))
                    {
                        collidables.UnionWith(nodes[i].GetCollidable(region));
                    }
                }
            }
            else
            {
                foreach (ISprite sprite in objects)
                {
                    if (sprite.Collidable && sprite.CollisionBox.Intersects(region))
                    {
                        collidables.Add(sprite);
                    }
                }
            }
            return collidables;
        }

        private void TryMerge()
        {
            if (GetCollidable(Bounds).Count <= MaxItems)
            {
                objects.UnionWith(AllEntities(Bounds));
                for (int i = 0; i < 4; i++)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
            if (GetCollidable(Bounds).Count <= MaxItems)
            {
                parent?.TryMerge();
            }
        }

        /// <summary>
        /// remove a list of objects from the tree.
        /// </summary>
        /// <param name="sprites"></param>
        public void Remove(ISet<IEntity> sprites)
        {
            foreach(IEntity sprite in sprites)
            {
                Remove(sprite);
            }
        }

        /// <summary>
        /// This method will get all the moving entities in the quadtree that can also participate in a collision
        /// </summary>
        /// <returns></returns>
        public ISet<IEntity> MovingCollidableEntities(Rectangle region)
        {
            ISet<IEntity> entities = new HashSet<IEntity>();
            if (nodes[0] != null)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(region))
                    {
                        entities.UnionWith(nodes[i].MovingCollidableEntities(region));
                    }
                }
            }
            else
            {
                foreach (ISprite sprite in objects)
                {
                    if (sprite.Collidable && sprite is AbstractEntity && !((AbstractEntity)sprite).Velocity.Equals(new Vector2()))
                    {
                        entities.Add((IEntity)sprite);
                    }else if(sprite is Mario)//for this sprint we always want to check mario
                    {
                        entities.Add((IEntity)sprite);
                    }
                }
            }
            return entities;
        }

        /// <summary>
        /// This method will get all the moving entities in the quadtree
        /// </summary>
        /// <returns></returns>
        public ISet<IEntity> MovingEntities(Rectangle region)
        {
            ISet<IEntity> sprites = new HashSet<IEntity>();
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[i].Bounds.Intersects(region))
                    {
                        sprites.UnionWith(nodes[i].MovingEntities(region));
                        
                    }
                }
            }
            else
            {
                foreach (ISprite sprite in objects)
                {
                    if (region.Intersects(sprite.CollisionBox))
                    {
                        if (sprite is AbstractEntity && !((AbstractEntity)sprite).Velocity.Equals(new Vector2()))
                        {
                            sprites.Add((IEntity)sprite);
                        }
                    }
                }
            }
            return sprites;
        }
    }
}
