using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TechSupportMario.CameraItems;
using TechSupportMario.Collisions;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.CollisionZones;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.HeadsUpDisplay;
using TechSupportMario.Entity.Items;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.GameStates;

namespace TechSupportMario
{
    public class Level 
    {
        private QuadTree entities;
        private SortedSet<Layer> layers;
        private ISet<AbstractEnemy> enemies;
        private readonly Dictionary<int, LevelPart> levelAreas;
        private int currentLevelSection;
        private Dictionary<int, Vector2> marioPositions;
        private readonly LevelSaver savedState;
        private WarpZone interactBlock;

        public Timer Clock { get; set; }
        public Layer HUD { get; set; }
        public IGameState State { get; set; }
        public int Song { get; set; }
        public Level()
        {
            layers = new SortedSet<Layer>();
            enemies = new HashSet<AbstractEnemy>();
            levelAreas = new Dictionary<int, LevelPart>();
            marioPositions = new Dictionary<int, Vector2>();
            currentLevelSection = 0;
            savedState = new LevelSaver();
            Clock = new Timer();
        }

        public void GoToNewArea(int next, Camera camera)
        {            
            entities.Remove(Stage.mario);
            //pack the current level part
            LevelPart levelPart = new LevelPart()
            {
                Enemies = enemies,
                Layers = layers,
                Entities = entities,
                ExitPositions = marioPositions,
                Song = Song
            };
            //put it in the dictionary
            levelAreas.Add(currentLevelSection, levelPart);
            //retrieve the new level area
            LevelPart nextLevelPart = levelAreas[next];
            //remove it
            levelAreas.Remove(next);
            //set up the properties
            enemies = nextLevelPart.Enemies;
            entities = nextLevelPart.Entities;
            layers = nextLevelPart.Layers;
            marioPositions = nextLevelPart.ExitPositions;
            if(Song != nextLevelPart.Song)
            {
                SoundItems.SoundFactory.Instance.PlaySong(nextLevelPart.Song);
            }
            Song = nextLevelPart.Song;
            //give mario his new position
            Stage.mario.Position = marioPositions[currentLevelSection];
            Rectangle limits = entities.Bounds;
            //Set the height correctly as the blocks cover more height than the camera should.
            limits.Height -= 16;
            camera.Limits = limits;
            camera.LookAt(Stage.mario.Position);
            currentLevelSection = next;
            entities.Insert(Stage.mario);
            InternalSaveLevel();

        }

        public void SetUpLevel()
        {
            LevelPart nextLevelPart = levelAreas[0];
            levelAreas.Remove(0);
            enemies = nextLevelPart.Enemies;
            entities = nextLevelPart.Entities;
            layers = nextLevelPart.Layers;
            marioPositions = nextLevelPart.ExitPositions;
            Song = nextLevelPart.Song;
            currentLevelSection = 0;
            entities.Insert(Stage.mario);
            InternalSaveLevel();
            SoundItems.SoundFactory.Instance.PlaySong(Song);
        }

        public void AddArea(LevelPart levelPart, int order)
        {
            levelAreas.Add(order, levelPart);
        }

        public ISet<Layer> GetDrawLayers(Camera camera)
        {

            Layer entityLayer = new Layer(camera)
            {
                Order = layers.Count
            };
            
            foreach(IEntity entity in entities.AllEntities(camera.DrawRectangle))
            {
                entityLayer.Sprites.Add(entity);
            }

            ISet<Layer> returnLayers = new SortedSet<Layer>
            {
                entityLayer
            };
            returnLayers.UnionWith(layers);
            return returnLayers;
        }

        public void Update(GameTime gameTime, Camera camera)
        {
            Clock.Update(gameTime);
            //list to separate the two types of sprites that need to be updated
            //need to do this in two steps to make sure they don't call update on an entity that was moving but then stop due to a collision
            ISet<IEntity> moving = entities.MovingEntities(camera.DrawRectangle);
            moving.UnionWith(enemies);//add any enemies that are out of the screen
            ISet<IEntity> animated = entities.EntitiesNeedingUpdated(camera.DrawRectangle);
            
            foreach (IEntity entity in moving)
            {
                Point location = new Point(entity.CollisionBox.X - entity.TrueWidth(), entity.CollisionBox.Y - entity.TrueHeight());
                Point size = new Point(3 * entity.TrueWidth(), 3* entity.TrueHeight());
                Rectangle checkRegion = new Rectangle(location, size);//incase it somehow moved out of a square before removing itself.
                entities.Remove(entity, checkRegion);
                entity.Update(gameTime);
                //check for thecollision
                
                if (entity is Fireball || entity is AbstractItem || entity is BulletBill)
                {
                    if (entity.Position.X > camera.DrawRectangle.Right || entity.Position.X + entity.TrueWidth() < camera.DrawRectangle.Left)
                    {
                        entity.Collect = true;
                    }
                }
                else
                {
                    CollisionDetection.CheckAndFixEntityOutOfBounds(entity, entity.CollisionBox.Width, entity.CollisionBox.Height, (Rectangle)camera.Limits);
                }
                
                if (!entity.Collect)//check if the entity should be put back into the world
                {
                    entities.Insert(entity);
                    if (entity is AbstractEnemy)//try to add it to enemies, set will takes care of duplicates
                    {
                        enemies.Add((AbstractEnemy)entity);
                    }
                }
                if (entity.Collidable)
                {
                    CollisionDetection.SetOnGround(entity, entities);
                    if(entity is Goomba && !entity.OnGround)
                    {

                    }
                }
            }
            if (moving.Count > 0)
            {
                CollisionDetection.CollisionsCheckAndFix(moving, entities);
            }
            foreach (IEntity entity in animated)
            {
                entity.Update(gameTime);

                if (entity.Collect)//check if the entity should be removed from the world
                {
                    entities.Remove(entity);
                    if (entity is AbstractEnemy)
                    {
                        enemies.Remove((AbstractEnemy)entity);
                    }
                }
            }
            camera.LookAt(Stage.mario.Position);
           // Overlay.Update(gameTime);
            foreach(Layer layer in layers)
            {
                foreach(ISprite sprite in layer.Sprites)
                {
                    sprite.Update(gameTime);
                }
            }
            if (Clock.Time == 0)
            {
                OnRaiseLevelEvent(null);
            }
        }

        public void Add(IEntity entity)
        {
            entities.Insert(entity);
        }

        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }
        public void RemoveLayer(Layer layer)
        {
            if (layers.Contains(layer)) layers.Remove(layer);
        }

        public QuadTree GetTree()
        {
            return entities;
        }

        public void Clear()
        {
            entities.Clear();
            enemies.Clear();
            marioPositions.Clear();
            levelAreas.Clear();
            layers.Clear();
        }

        public void SaveLevelState(IEntity sender,CollisionZoneEventArgs eventArgs)
        {
            entities.Insert(sender);
            savedState.SaveLevel(this);
            savedState.ResetMarioPosition = eventArgs.Position;
        }

        private void InternalSaveLevel()
        {
            savedState.SaveLevel(this);
            savedState.ResetMarioPosition = Stage.mario.Position;
            Stage.player.SaveState();
        }

        public void ResetLevel()
        {
            entities.Remove(Stage.mario);
            Level level = this;
            savedState.ResetLevel(ref level);
            entities = level.entities;
            layers = level.layers;
            enemies = level.enemies;
            currentLevelSection = level.currentLevelSection;
            marioPositions = level.marioPositions;
            Stage.mario.Position = savedState.ResetMarioPosition;
            entities.Insert(Stage.mario);
            if(Clock.Time == 0)
            {
                Clock.Time = 400;
            }
        }

        public void HandleInteractEnter(object sender, CollisionZoneEventArgs args)
        {
            interactBlock = (WarpZone)sender;
        }

        public void HandleInteractLeave(object sender, CollisionZoneEventArgs args)
        {
            interactBlock = null;
        }

        public void Interact()
        {
            if(interactBlock != null)
            {
                interactBlock.Interact();
            }
        }

        internal class LevelSaver
        {
            private QuadTree entities;
            private SortedSet<Layer> layers;
            private ISet<AbstractEnemy> enemies;
            private int currentLevelSection;
            private Dictionary<int, Vector2> marioPositions;
            public Vector2 ResetMarioPosition { get; set; }

            public void SaveLevel(Level level)
            {
                entities = new QuadTree(level.entities.Bounds, 0);
                enemies = new HashSet<AbstractEnemy>();
                foreach (IEntity entity in level.entities.RetrieveAll())
                {
                    if (entity is AbstractEnemy || entity is AbstractItem || entity is BrickBlock || entity is QuestionBlock)
                    {
                        if(entity is AbstractEnemy enemy)
                        {
                            if (level.enemies.Contains(enemy))
                            {
                                IEntity entity1 = entity.Clone();
                                entities.Insert(entity1);
                                enemies.Add((AbstractEnemy)entity1);
                            }
                            else
                            {
                                entities.Insert(entity.Clone());
                            }
                        }
                        else
                        {
                            entities.Insert(entity.Clone());
                        }
                    }
                    else
                    {
                        entities.Insert(entity);
                    }
                }
                layers = level.layers;
                currentLevelSection = level.currentLevelSection;
                marioPositions = level.marioPositions;
            }

            public void ResetLevel(ref Level level)
            {
                level.entities.Clear();
                level.enemies.Clear();
                ISet<IEntity> all = entities.RetrieveAll();
                foreach (IEntity entity in all)
                {
                    if (entity is AbstractEnemy || entity is AbstractItem || entity is BrickBlock || entity is QuestionBlock || entity is ToggleBlock|| entity is DisappearingBlock)
                    {
                        if(entity is AbstractEnemy abstractEnemy)
                        {
                            if (enemies.Contains(abstractEnemy))
                            {
                                AbstractEnemy enemy = (AbstractEnemy)entity.Clone();
                                level.enemies.Add(enemy);
                                level.entities.Insert(enemy);
                            }
                            else
                            {
                                level.entities.Insert(entity.Clone());
                            }
                        }
                        else if (entity is IBlock)
                        {
                            if(entity is BrickBlock || entity is QuestionBlock)
                            {
                                level.entities.Insert(entity.Clone());
                            }   
                            else if (entity is ToggleBlock)
                            {
                                ((ToggleBlock)entity).Reset();
                                level.entities.Insert(entity);
                            }
                            else if( entity is DisappearingBlock)
                            {
                                ((DisappearingBlock)entity).Reset();
                                level.entities.Insert(entity);
                            }

                        }
                        else
                        {
                            level.entities.Insert(entity.Clone());
                        }
                        
                    }
                    else
                    {
                        level.entities.Insert(entity);
                    }
                }
                level.layers = layers;

                level.currentLevelSection = currentLevelSection;
                level.marioPositions = marioPositions;
            }

            
        }
        public virtual void OnRaiseLevelEvent(EventArgs e)
        {
            RaiseLevelEvent?.Invoke(this, e);
        }
        public delegate void LevelEventHandler(object sender);
        public event EventHandler<EventArgs> RaiseLevelEvent;
    }
}
