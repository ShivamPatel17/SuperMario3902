using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.CollisionZones;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.Items;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.BlockState;

namespace TechSupportMario.Collisions
{
    public static class CollisionDetection
    {

        public enum Direction
        {
            up, 
            down,
            left,
            right,
            upnov,
            downnov
        }

        /// <summary>
        /// This method will check every entity in the list given and then 'fix' them if needed (so only collisions with items of type IBlock)
        /// If the collision is with a entity of type IMario then it will call the methods on mario and the entity as needed.  Should only use 
        /// moving entities, like mario, enemies, and items otherwise  using the other method is going run in about the same amount of time.
        /// </summary>
        /// <param name="checkEntities">the list of entities to check collisions for.</param>
        public static void CollisionsCheckAndFix(ISet<IEntity> checkEntities, QuadTree CollisionTree)
        {
            foreach (IEntity entity in checkEntities)//loop through the entities that need to be checked
            {
                bool cont = entity.Collidable;
                while (cont)//continue to check till there are no more collisions to resolve
                {
                    ISet<IEntity> possibleCollisions = CollisionTree.Retrieve(entity);//get nearest neighbor
                    if (possibleCollisions.Count > 0)//check if any exist
                    {
                        IList<IEntity> firstCollider = new List<IEntity>();
                        //time it happened, from 0 to 1 as a decimal representing a percentage
                        float time = 1;
                        foreach (IEntity possible in possibleCollisions)//loop through the possibilities
                        {
                            if (!((entity is IItem && possible is AbstractEnemy) || (entity is AbstractEnemy && possible is IItem)) && entity.CollisionBox.Intersects(possible.CollisionBox))//see if they intersect, items ignore enemies
                            {
                                //fix it so that the hidden block isn't even tried if it isn't from the bottom
                                //first check if it is a block, if it is a block check if it is in the hidden state
                                //if the block is in the hidden state then is the collision from the bottom
                                if (!(possible is IBlock && ((IBlock)possible).BlockState is BlockStateHidden && !HiddenBlockValid(entity, (IBlock)possible)))
                                {
                                    bool check = CheckMarioInvincible(entity, possible);
                                        
                                    if (check)
                                    {
                                        if (firstCollider.Count == 0)//first entity to collide with the checking entity
                                        {
                                            time = FindTime(entity, possible);
                                            if (time != 1)
                                            {
                                                AddToPossible(ref firstCollider, entity, possible, ref time);
                                            }                                            
                                        }
                                        else//see if it happened earlier in the update or not
                                        {
                                            AddToPossible(ref firstCollider, entity, possible, ref time);
                                        }
                                    }
                                }
                            }
                        }
                        if (firstCollider.Count > 0)//see if any collision needs to be resolved
                        {
                            bool allEnemiesItems = true;
                            foreach (IEntity collision in firstCollider)//resolve the collisions
                            {
                                if (entity is Mario || entity is IBlock)
                                {
                                    if (!(collision is AbstractEnemy) && !(collision is IItem) && allEnemiesItems)
                                    {
                                        allEnemiesItems = collision is CollisionZone;//enemies and items won't move mario so if they are all that is colliding there is no need to loop through this again with the entity
                                    }
                                }
                                CollisionTree.Remove(collision);
                                IEntity first = entity;
                                IEntity second = collision;
                                SetTheEntitiesInOrder(ref first, ref second);//method to simplify the checking later (mario first, if no mario moving entity first then blocks)
                                FixCollisions(first, second, time);//fix it and call the needed methods
                                CollisionTree.Insert(collision);
                            }
                            //checks to see if the while loop should continue or exit
                            if (allEnemiesItems)
                            {
                                cont = false;
                            }
                        }
                        else
                        {
                            cont = false;
                        }
                    }
                    else
                    {
                        cont = false;
                    }
                }
            }
        }

        private static bool CheckMarioInvincible(IEntity entity, IEntity possible)
        {
            bool check = true;
            if (entity is AbstractEnemy && possible is Mario)
            {
                if (((Mario)possible).Invincible)
                {
                    check = false;
                }

            }
            else if (entity is Mario && possible is AbstractEnemy)
            {
                if (((Mario)entity).Invincible)
                {
                    check = false;
                }
            }
            return check;
        }

        private static void AddToPossible(ref IList<IEntity> entities, IEntity entityOne, IEntity entityTwo, ref float time)
        {
            if(entityOne is PiranhaPlant && entityTwo is IBlock)//PiranhaPlants move without caring about blocks
            {
                return;
            }
            //if entity two is a CollisionZone then add it.
            if (entityTwo is CollisionZone hz)
            {
                if(hz.LookingFor == entityOne.Type)
                {
                    entities.Add(entityTwo);
                }
                
            }
            else if(entityOne is Boo)
            {
                if(entityTwo is Mario)
                {
                    entities.Add(entityOne);
                }
            }
            else
            {
                float time2 = FindTime(entityOne, entityTwo);
                bool allHidden = true;
                ISet<IEntity> nonHiddenEntities = new HashSet<IEntity>();
                foreach(IEntity entity in entities)//find if the only entity in entities is are collision zones then don't care about anything
                {
                    if(!(entity is CollisionZone))
                    {
                        nonHiddenEntities.Add(entity);
                        allHidden = false;
                    }
                }

                if (allHidden)
                {
                    time = time2;
                    entities.Add(entityTwo);
                }
                else
                {
                    if (entityTwo is AbstractBlock block)
                    {
                        //find out if the blocks are stacked, if they are make sure that only the left/right collision is present
                        Direction dir = CollisionDirection(entityOne, entityTwo);
                        if (entities.Count > 0)
                        {
                            IEntity removed = null;
                            bool stacked = false;
                            foreach (IEntity entity in entities)
                            {
                                if (entity is IBlock)
                                {
                                    if (entity.CollisionBox.Top == block.CollisionBox.Bottom || entity.CollisionBox.Bottom == block.CollisionBox.Top)
                                    {
                                        stacked = true;
                                        if (entity.CollisionBox.Left == block.CollisionBox.Left)
                                        {
                                            if (dir == Direction.right || dir == Direction.left)
                                            {
                                                removed = entity;
                                            }
                                        }

                                    }
                                }
                            }
                            if (removed != null)
                            {
                                entities.Remove(removed);
                                entities.Add(entityTwo);
                                return;
                            }
                            else if (stacked)
                            {
                                return;
                            }
                            
                        }
                    }
                    if (time > time2)
                    {
                        foreach(IEntity entity in nonHiddenEntities)
                        {
                            entities.Remove(entity);
                        }
                        entities.Add(entityTwo);
                        time = time2;
                    }
                    else if (time == time2)
                    {
                        entities.Add(entityTwo);
                    }
                }
            }
        }

        /// <summary>
        /// This method will check if mario is out side of the play area. If mario is outside the play area, defined
        /// by the quadtree bounds, then it will put mario back in the quad tree bounds changing his position by as 
        /// small amount as possible.
        /// </summary>
        /// <param name="entity"></param>
        public static void CheckAndFixEntityOutOfBounds(IEntity entity, int width, int height, Rectangle bounds)
        {

            Vector2 pos = new Vector2
            {
                X = MathHelper.Clamp(entity.Position.X, 0, bounds.Right - width),
                Y = MathHelper.Clamp(entity.Position.Y, height, bounds.Bottom)
            };
            if(pos.Y < entity.Position.Y)
            {
                if(entity is Mario mario)
                {
                    mario.InstantKill();
                }
                else if(entity is AbstractEnemy)
                {
                    ((AbstractEnemy)entity).TakeDamage();
                }
                else
                {
                    entity.Collect = true;
                }
            }
            entity.Position = pos;
        }

        /// <summary>
        /// this method will set the OnGround property for the entity.
        /// </summary>
        /// <param name="entity"></param>
        public static void SetOnGround(IEntity entity, QuadTree tree)
        {
            ISet<IEntity> possible = tree.Retrieve(entity);
            foreach(IEntity checking in possible)
            {
                if(checking is BulletBill || (checking is IBlock && !(((IBlock)checking).BlockState is BlockStateHidden)))
                {
                    if((checking.Position.X < entity.Position.X && checking.CollisionBox.Right > entity.Position.X) || (checking.Position.X < entity.CollisionBox.Right && checking.CollisionBox.Right > entity.CollisionBox.Right))
                    {
                        if(checking.CollisionBox.Top <= entity.Position.Y && checking.CollisionBox.Top + 1.1f > entity.Position.Y)
                        {
                            Direction dir = CollisionDirection(entity, checking);
                            entity.OnGround =  dir == Direction.down || dir == Direction.downnov;
                            return;
                        }
                    }
                    if(checking.Position.X == entity.Position.X && checking.CollisionBox.Right == entity.CollisionBox.Right)
                    {
                        if (checking.CollisionBox.Top <= entity.Position.Y && checking.CollisionBox.Bottom > entity.Position.Y)
                        {
                            entity.OnGround = true;
                            return;
                        }
                    }
                }
            }
            entity.OnGround = false;
        }

        /// <summary>
        /// the goal of this method is to make the first entity mario or the entity with velocity.
        /// If mario is one of the two, first will become mario otherwise first will have a none zero velocity.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private static void SetTheEntitiesInOrder(ref IEntity first, ref IEntity second)
        {
            if (first is Mario)
            {
                return;
            }
            else if (second is Mario)
            {
                IEntity temp = first;
                first = second;
                second = temp;
                return;
            }
            if(first is AbstractEnemy && second is AbstractEnemy && first.Position.X > second.Position.X)
            {
                IEntity temp = first;
                first = second;
                second = temp;
                return;
            }
            else if(first is IBlock && second is IItem)
            {
                IEntity temp = first;
                first = second;
                second = temp;
                return;
            }
            else if (first.Velocity.Length() == 0)
            {
                IEntity temp = first;
                first = second;
                second = temp;
                return;
            }
        }

        /// <summary>
        /// to fix when mario grows, can be jump or changing to super/fire state or even death, 
        /// though death state won't need it after sprint 2 (he will become uncollidable and be reset)
        /// </summary>
        /// <param name="mario"></param>
        /// <param name="changeInY"></param>
        public static void FixCollisionFromStateChange(IEntity mario, int changeInY)
        {
            ISet<IEntity> entities = Stage.level.GetTree().Retrieve(mario);
            foreach (IEntity block in entities)//only care about blocks so just naming it nice
            {
                if (block is IBlock)//check if it is a block
                {
                    Rectangle intersection = Rectangle.Intersect(mario.CollisionBox, block.CollisionBox);//find the intersection
                    if (!intersection.IsEmpty)//if they have some correction space to fix or not
                    {
                        if (!(((AbstractBlock)block).BlockState is BlockStateHidden))//check if there needs to be a fix
                        {
                            //so there is an intersection
                            Vector2 position = mario.Position;
                            if (changeInY >= intersection.Height)//check y over lap and which direction to move mario
                            {
                                    position.Y += intersection.Height;
                            }
                            else
                            {
                                position.X -= intersection.Width;
                            }
                            mario.Position = position;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// this method will set the positions of both items to a position so their Collision boxes are no longer intersecting yet still along their path.
        /// </summary>
        /// <param name="entityOne">this should be the entity with a velocity</param>
        /// <param name="entityTwo">if a block is in the collision it should be this one</param>
        private static void FixCollisions(IEntity entityOne, IEntity entityTwo, float time)
        {
            if(entityTwo is CollisionZone)
            {
                CallMethods(entityOne, entityTwo, CollisionDirection(entityOne, entityTwo));
                return;
            }
            Direction dir = CollisionDirection(entityOne, entityTwo);//find the direction before the entities are no longer colliding

            if (entityTwo is IBlock)
            {
                Vector2 newPosition = entityOne.Position;

                if (0f <= time && 1f >= time)//&& dir != Directions.downnov && dir != Directions.upnov)
                {//if outside this time something weird happened somewhere else and needs to be fixed there or it was part of a collision that happened at the same time as another that
                    //moved one or both the entities to no longer be touching, so no movement needed
                    Vector2 velocity = entityOne.Velocity;
                    if (entityOne is Mario mario)
                    {
                        if (Math.Abs(mario.OldVelocity.X) > mario.Velocity.X)
                        {
                            velocity = mario.OldVelocity;
                        }
                    }

                    if (dir == Direction.right || dir == Direction.left) 
                    {
                        newPosition.X = (float)Math.Round(newPosition.X - velocity.X * (1 - time));
                    }
                    else
                    {
                        newPosition.Y = (float)Math.Round(newPosition.Y - velocity.Y * (1 - time));
                    }
                    
                    entityOne.Position = newPosition;
                }
                
            }
            else//items or enemy collisions
            {
                if (!(entityOne is Mario))
                {
                    Vector2 newPosition = entityOne.Position;
                    Vector2 newPosition2 = entityTwo.Position;
                    if (0f <= time && 1f >= time)
                    {//if outside this time something weird happened somewhere else and needs to be fixed there or it was part of a collision that happened at the same time as another that
                     //moved one or both the entities to no longer be touching, so no movement needed
                        newPosition.X = (float)Math.Round(newPosition.X - entityOne.Velocity.X * (1 - time));
                        newPosition.Y = (float)Math.Round(newPosition.Y - entityOne.Velocity.Y * (1 - time));
                        entityOne.Position = newPosition;
                        newPosition2.X = (float)Math.Round(newPosition2.X - entityTwo.Velocity.X * (1 - time));
                        newPosition2.Y = (float)Math.Round(newPosition2.Y - entityTwo.Velocity.Y * (1 - time));
                        entityTwo.Position = newPosition2;
                    }
                }
            }
            CallMethods(entityOne, entityTwo, dir);//methods should always be called
        }

        /// <summary>
        /// The will return an integer from 0 to 1 unless the collision couldn't happen within one frame.
        /// If only one entity is moving then it should be entityOne in the call.
        /// </summary>
        /// <param name="entityOne"></param>
        /// <param name="entityTwo"></param>
        /// <returns>float between 0 and 1 inclusive if the collision happened in the frame</returns>
        private static float FindTime(IEntity entityOne, IEntity entityTwo)
        {
            float width = FindWidth(entityOne, entityTwo);
            float height = FindHeight(entityOne, entityTwo);
            Vector2 combinedVelocity;

            if (entityOne is Mario mario)
            {
                if (Math.Abs(mario.OldVelocity.X) > mario.Velocity.X)
                {
                    combinedVelocity = CombineVelocity(mario.OldVelocity, entityTwo.Velocity);
                }
                else
                {
                    combinedVelocity = CombineVelocity(mario.Velocity, entityTwo.Velocity);
                }
            }
            else
            {
                combinedVelocity = CombineVelocity(entityOne.Velocity, entityTwo.Velocity);
            }

            float t1 = 1 - Math.Abs(width / combinedVelocity.X);
            if (t1 > 1 || t1 < 0)
            {
                if(t1 > -1f && t1 < 0)
                {
                    return 0;
                }
                float time = 1 - Math.Abs(height / combinedVelocity.Y);
                if(time > -1f && time < 0)
                {
                    return 0;
                }
                return time;
            }
            else
            {
                return t1;
            }
        }

        /// <summary>
        /// this method will return an int 0-5, 0 is down, 1 is up, 2 is left, 3 is right, 4 the collision happened at the same time as another collision, yet it happened from the bott.
        /// 5 is it happened at the same time as another processed collision and from a direction other that the bottom. That is the direction that the collision came from.
        /// </summary>
        /// <param name="entityOne">this should be the moving entity in the pair, otherwise this method won't work correctly.</param>
        /// <returns> 0-5, 0 is down, 1 is up, 2 is left, 3 is right, 4 the collision happened at the same time as another collision, yet it happened from the bott That is the direction that the collision came from. </returns>
        private static Direction CollisionDirection(IEntity entityOne, IEntity entityTwo)
        {
            float width = FindWidth(entityOne, entityTwo);
            float height = FindHeight(entityOne, entityTwo);
            
            if (width > 0 || height > 0)
            {

                Vector2 combinedVelocity;
                Vector2 entityOneUsedV = entityOne.Velocity;
                if (entityOne is Mario mario)
                {
                    if (Math.Abs(mario.OldVelocity.X) > mario.Velocity.X)
                    {
                        combinedVelocity = CombineVelocity(mario.OldVelocity, entityTwo.Velocity);
                        entityOneUsedV = mario.OldVelocity;
                    }
                    else
                    {
                        combinedVelocity = CombineVelocity(mario.Velocity, entityTwo.Velocity);
                    }
                }
                else
                {
                    combinedVelocity = CombineVelocity(entityOne.Velocity, entityTwo.Velocity);
                }

                if (width > 0 && width - .0003 <= Math.Abs(combinedVelocity.X) && width / Math.Abs(combinedVelocity.X) < height / Math.Abs(combinedVelocity.Y))
                {
                    //so left or right
                    if (entityOneUsedV.X > 0)
                    {
                        return Direction.right;
                    }
                    else if (entityOneUsedV.X < 0)
                    {
                        return Direction.left;
                    }
                    //check entity two
                    if (entityTwo.Velocity.X > 0)
                    {
                        return Direction.right;
                    }
                    else if (entityTwo.Velocity.X < 0)
                    {
                        return Direction.left;
                    }
                }
                if (height > 0 && height - .0001 <= Math.Abs(combinedVelocity.Y))
                {
                    //so up or down
                    if (entityOne.Velocity.Y > 0)
                    {
                        return Direction.down;
                    }
                    else if (entityOne.Velocity.Y < 0)
                    {
                        if(entityOne.Position.Y > entityTwo.Position.Y)
                            return Direction.up;
                        else
                        {
                            //so left or right
                            if (entityOneUsedV.X > 0)
                            {
                                return Direction.right;
                            }
                            else if (entityOneUsedV.X < 0)
                            {
                                return Direction.left;
                            }
                        }
                    }
                    //check entity two
                    if (entityTwo.Velocity.Y < 0)
                    {
                        return Direction.up;
                    }
                    else if (entityTwo.Velocity.Y > 0)
                    {
                        return Direction.down;
                    }
                }
            }
            if(entityOne.Position.Y > entityTwo.Position.Y)
            {
                return Direction.upnov;
            }
            else
            {
                return Direction.downnov;
            }
        }

        private static float FindWidth(IEntity entityOne, IEntity entityTwo)
        {
            float width;
            if (entityOne.Position.X > entityTwo.Position.X)
            {
                width = entityTwo.Position.X + entityTwo.TrueWidth() - entityOne.Position.X;
            }
            else
            {
                width = entityOne.Position.X + entityOne.TrueWidth() - entityTwo.Position.X;
            }
            return width;
        }

        private static float FindHeight(IEntity entityOne, IEntity entityTwo)
        {
            float height;
            if (entityOne.Position.Y < entityTwo.Position.Y)
            {
                height = entityOne.Position.Y - entityTwo.Position.Y + entityTwo.TrueHeight();
            }
            else
            {
                height = entityTwo.Position.Y - entityOne.Position.Y + entityOne.TrueHeight();
            }
            return height;
        }

        private static Vector2 CombineVelocity(Vector2 v1, Vector2 v2)
        {
            Vector2 combinedVelocity = v1;
            if (combinedVelocity.X >= 0)
            {
                combinedVelocity.X += Math.Abs(v2.X);
            }
            else
            {
                combinedVelocity.X -= Math.Abs(v2.X);
            }

            if (combinedVelocity.Y >= 0)
            {
                combinedVelocity.Y += Math.Abs(v2.Y);
            }
            else
            {
                combinedVelocity.Y -= Math.Abs(v2.Y);
            }
            return combinedVelocity;
        }

        private static Direction OppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.up:
                    return Direction.down;
                case Direction.down:
                    return Direction.up;
                case Direction.left:
                    return Direction.right;
                case Direction.right:
                    return Direction.left;
                case Direction.upnov:
                    return Direction.downnov;
                case Direction.downnov:
                    return Direction.upnov;
                default:
                    return Direction.up;
            }
        }

        /// <summary>
        /// this method will call the collision response methods giving the direction to the entity.
        /// </summary>
        /// <param name="entityOne">this should be the moving entity unless both are moving. Should also be mario if mario is part of the collision</param>
        /// <param name="entityTwo">if a block is part of the collision it should be this one</param>
        private static void CallMethods(IEntity entityOne, IEntity entityTwo, Direction dir)
        {
            entityOne.CollisionResponse(entityTwo, dir);
            entityTwo.CollisionResponse(entityOne, OppositeDirection(dir));
        }

        private static bool HiddenBlockValid(IEntity mario, IBlock block)
        {
            bool possible = false;
            if (CollisionDirection(mario, block) == 0)
            {
                float time = FindTime(mario, block);
                if (time >= 0 && time <= 1)
                {
                    if (mario.Position.Y > block.Position.Y)
                    {
                        possible = true;
                    }
                }
            }
            return possible;
        }
    }
}
