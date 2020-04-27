using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Collisions;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.Items;
using TechSupportMario.State.MarioStates.MarioPowerStates;
using TechSupportMario.State.MarioStates.MarioActionStates;
using System;
using TechSupportMario.State.EnemyState;

namespace TechSupportMario.Entity.MarioEntity
{
    public class Mario : AbstractEntity
    {
        //private variables
        private const float decay = .85f;
        private int lastChange;//for changing SourceRectangle at the right time
        private int dir;//if the walking animation is moving forwards or backwards
        //variables to handle being invincible and the frame changes with that
        private int invincibleCurrentTime;
        private int sourceInvincibleChange = 0;
        private bool useSource = true;
        private bool holding = false;
        private const int SourceChangeTime = 80;
        private const int InvincibleTime = 1000;
        //variables to handle star state
        private const int starStateChange = 64;
        private int starChange = 0;
        private int starState = 1;
        private int iteration = 0;
        //handle fire ball sprite timing
        private int fireChange = 0;
        private const int FireTime = 80;

        public bool Falling { get; set; }
        private int lockDir;
        //so the powerstates can also change it as needed (taking damage and revive)
        private bool _invincible;
        public bool Invincible
        {
            get { return _invincible; }
            set
            {
                if (value)
                {
                    useSource = false;
                }
                else
                {
                    useSource = true;
                }
                _invincible = value;
            }
        }
        public bool trackSprint;
        public bool ShotFireBall { get; set; }
        public IMarioActionState ActionState { get; set; }
        public IMarioPowerState PowerState { get; set; }
        public ActionStateType ActionStateEnum { get; set; }
        public PowerStateType PowerStateEnum { get; set; }
        public int CurrentTexture { get; set; }
        public int CurrentFrame { get; set; }
        public Vector2 OldVelocity { get; set; }

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = SourceRectangle;
                box.Location = new Point((int)Position.X, (int)Position.Y - SourceRectangle.Height);
                if(box.Width > 28)
                {
                    box.Width = 28;
                }
                if (box.Bottom < Position.Y)
                {
                    box.Height++;
                }
                if (box.Right < Position.X + box.Width)
                {
                    box.Width++;
                }
                if (box.Left > Position.X)
                {
                    box.X--;
                    box.Width++;
                }
                return box;
            }
        }
        public override int TrueWidth()
        {
            if (SourceRectangle.Width > 28)
            {
                return 28;
            }
            else
            {
                return SourceRectangle.Width;
            }
        }

        public override int TrueHeight()
        {
            if(ActionStateEnum == ActionStateType.Jumping)
            {
                if (PowerState is MarioPowerStateNormal)
                {
                    return 44;
                }
                else if (PowerState is MarioPowerStateSuper || PowerState is MarioPowerStateFire)
                {
                    return 62;
                }
                else
                {
                    if(PowerState is MarioPowerStateStar)
                    {
                        if (((MarioPowerStateStar)PowerState).previousState is MarioPowerStateNormal)
                        {
                            return 44;
                        }
                        else
                        {
                            return 62;
                        }
                    }
                }
            }
            return SourceRectangle.Height;
        }

        //State enums
        public enum ActionStateType
        {
            Idle,
            WalkingLeft,
            WalkingRight,
            Crouching,
            Jumping,
            Dead,
            Decaying,
            HoldingShell
        }

        public enum PowerStateType
        {
            Normal,
            Super,
            Fire,
            Star,
            Dead
        }

            public Mario(Texture2D texture) : base(texture, EntityType.Mario)
        {
            CurrentTexture = 0;
            CurrentFrame = 0;
            dir = 1;
            SourceRectangle = new Rectangle();
            ActionState = new MarioActionStateIdle(this, null);
            ActionState.Enter(); 
            PowerState = new MarioPowerStateNormal(this);
            PowerState.Enter();
            lastChange = 0;
            invincibleCurrentTime = 0;
            Invincible = false;
            sourceInvincibleChange = 0;
            lockDir = 0;
            Order = 0f;
            trackSprint = false;
        }

        public void SuperMushroomAction(bool cheatCode)
        {
            //once cheat codes are taken out get rid of this if and only have the else block
            if (cheatCode && PowerState is MarioPowerStateFire) 
            {
                CurrentTexture = 0;
                PowerState = new MarioPowerStateSuper(this);
                PowerState.Enter();
            }
            else
            {
                int heightChange = SourceRectangle.Height;
                PowerState.SuperMushroom();
                heightChange = SourceRectangle.Height - heightChange;
                CollisionDetection.FixCollisionFromStateChange(this, heightChange);
            }

        }

        public void StarAction()
        {
            PowerState.StarPower();
            
        }

        public void ActionFireFlower()
        {
            int heightChange = SourceRectangle.Height;
            PowerState.RecievedFireFlower();
            heightChange = SourceRectangle.Height - heightChange;
            CollisionDetection.FixCollisionFromStateChange(this, heightChange);
        }

        public void ActionFireBall()
        {
            if (!ShotFireBall)
            {
                  PowerState.ThrowFire();
            }
        }

        public void StandardPowerState()
        {
            PowerState.Revive();
        }

        public void TakeDamage()
        {
            if (!Invincible)
            {
                PowerState.TakeDamage();
                //player may get life taken
                OnRaiseCollisionEvent(new MarioCollisionEventArgs(null, true));
            }
        }

        public void InstantKill()
        {
            if(!(PowerState is MarioPowerStateDead))
            {
                PowerState = new MarioPowerStateDead(this);
                PowerState.Enter();
                OnRaiseInstantDeathEvent(null);
            }
        }

        //Movement actions
        public void DownAction()
        {
            int heightChange = SourceRectangle.Height;
            if (!(ActionState is MarioActionStateJump))
            {
                ActionState.ActionDown();
            }
            heightChange = SourceRectangle.Height - heightChange;
            CollisionDetection.FixCollisionFromStateChange(this, heightChange);
        }

        public void ReleaseDown()
        {
            if(ActionStateEnum != ActionStateType.Idle)
                ActionState.ActionUp();
        }

        public void LeftAction()
        {
            if (lockDir == 0)
            {
                ActionState.ActionLeft();
                lockDir = -1;
            }
        }

        public void ReleaseLeft()
        {
            if (lockDir == -1)
            {
                lockDir = 0;
                ActionState.ActionRight();
            }
        }

        public void RightAction()
        {
            if (lockDir == 0)
            {
                ActionState.ActionRight();
                lockDir = 1;
            }
        }

        public void ReleaseRight()
        {
            if(lockDir == 1)
            {
                lockDir = 0;
                ActionState.ActionLeft();
            }
        }

        public void GoSprint()
        {
            if (ActionState is MarioActionStateMovingLeft)
                ActionState.Leave(new MarioActionStateSprintLeft(this));
            else if (ActionState is MarioActionStateMovingRight)
                ActionState.Leave(new MarioActionStateSprintRight(this));
        }

        public void ReleaseSprint()
        {
            if (ActionState is MarioActionStateSprintLeft)
                ActionState.Leave(new MarioActionStateMovingLeft(this));
            else if (ActionState is MarioActionStateSprintRight)
                ActionState.Leave(new MarioActionStateMovingRight(this));
        }

        public void ReleaseDirectionLock()
        {
            lockDir = 0;
        }

        public void UpAction()
        {
            int heightChange = SourceRectangle.Height;
            ActionState.ActionUp();
            heightChange = SourceRectangle.Height - heightChange;
            CollisionDetection.FixCollisionFromStateChange(this, heightChange);

            /*This is sound effect for jumping*/
            if (OnGround)
            {
                SoundItems.SoundFactory.Instance.JumpN();
            }
        }

        public void ReleaseUp()
        {
            if(ActionStateEnum == ActionStateType.Jumping)
            {
                //add the ability to change jump height here.
            }
        }

        public void HoldingCommand()
        {
            holding = true;
            if(ActionStateEnum == ActionStateType.HoldingShell)
            {
                ((MarioHoldingShellState)ActionState).Throw();
                holding = false;
            }
        }
        public void ReleaseHoldingCommand()
        {
            holding = false;
        }

        public void UpdateTexture()
        {
            Rectangle source = SourceRectangle;
            PowerStateType power = PowerStateEnum;
            if(PowerStateEnum == PowerStateType.Star)
            {
                power = PowerState.Previous;
            }
            MarioSpriteFactory.SetSourceRectangle(ref source, power, ActionStateEnum);
            SourceRectangle = source;
        }

        public void ChangeFacing(int direction)
        {
            Rectangle source = SourceRectangle;
            if (direction == 0 && SourceRectangle.X >= MarioSpriteFactory.RightFacingStart)
            {
                source.X -= MarioSpriteFactory.RightFacingStart;
            }
            else if (direction == 1 && SourceRectangle.X < MarioSpriteFactory.RightFacingStart)
            {
                source.X += MarioSpriteFactory.RightFacingStart;
            }
            SourceRectangle = source;
        }

        public override void CollisionResponse(IEntity entity, CollisionDetection.Direction direction)
        {

            if (entity is AbstractEnemy && !(entity is Canon))
            {
                if(PowerStateEnum != PowerStateType.Star)
                {
                    if (!((AbstractEnemy)entity is AbstractKoopa) && (entity is PiranhaPlant || (direction != CollisionDetection.Direction.down && direction != CollisionDetection.Direction.downnov)))
                    {
                        TakeDamage();
                    }
                    else
                    {
                        //player gets points
                        if(entity is BulletBill)
                        {
                            if (ActionStateEnum == ActionStateType.Jumping)
                            {
                                OnGround = true;
                                ActionState.ActionDown();
                            }
                                
                        }
                        else if(entity is AbstractKoopa)
                        {
                            IEnemyState koopaState = ((AbstractEnemy)entity).EnemyState;
                            if(!(koopaState is KoopaShellState) && !(direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov))
                            {
                                TakeDamage();
                            }
                            else if(koopaState is KoopaShellState && ((KoopaShellState)koopaState).GetDeadly() && !(direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov))
                            {
                                TakeDamage();
                            }
                            else if(ActionStateEnum != ActionStateType.HoldingShell && holding && koopaState is KoopaShellState && !((KoopaShellState)koopaState).GetDeadly())
                            {
                                ActionState.Leave(new MarioHoldingShellState(this, (AbstractKoopa)entity, ActionState));
                            }
                            else
                            {
                                if(direction == CollisionDetection.Direction.down || direction == CollisionDetection.Direction.downnov)
                                {
                                    Velocity = new Vector2(Velocity.X, -4f);
                                    OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));
                                }
                            }

                        }
                        else
                        {
                            Velocity = new Vector2(Velocity.X, -4f);
                            OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));
                        }
                        
                    }
                }
                else
                {
                    OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));//player gets points
                }
                
            }
            else if (entity is IBlock )
            {
                
                switch (direction)
                {
                    case CollisionDetection.Direction.down:
                        if(ActionStateEnum == ActionStateType.Jumping)
                            ActionState.ActionDown();
                        break;
                    case CollisionDetection.Direction.up:
                        ActionState.ActionDown();
                        OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));
                        break;
                    case CollisionDetection.Direction.downnov:
                        if(ActionState is MarioActionStateJump)
                            ActionState.ActionDown();
                        break;
                    case CollisionDetection.Direction.upnov:
                        ActionState.ActionDown();
                        OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));
                        break;
                }
            }
            else if (entity is IItem)
            {
                if (entity is Star)
                {
                    StarAction();
                }
                if (entity is FireFlower)
                {
                    ActionFireFlower();
                }
                if (entity is SuperMushroom)
                {
                    SuperMushroomAction(false);
                }
                //mario gets points
                if(entity is Key key)
                {
                    System.Diagnostics.Debug.WriteLine(key.KeyNum);
                }
                OnRaiseCollisionEvent(new MarioCollisionEventArgs(entity));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            OldVelocity = Velocity;

            if (Velocity.Y > 0 && !Falling)
            {
                Falling = true;
                if (!(ActionState is MarioActionStateJump))
                {
                    ActionState = new MarioActionStateJump(this, ActionState);
                    ActionStateEnum = ActionStateType.Jumping;
                    ActionState.Enter();
                }
                SourceRectangle = MarioSpriteFactory.FallingJump(SourceRectangle, ActionStateEnum);
            }
            
            if (ActionStateEnum == ActionStateType.Decaying || (ActionStateEnum == ActionStateType.Jumping && ActionState.Previous is MarioActionStateDecaying))
            {                    
                Velocity = new Vector2(decay * Velocity.X, Velocity.Y);
                if(Velocity.X < .1 && Velocity.X > -.1)
                {
                    Velocity = new Vector2(0, Velocity.Y);
                    if(ActionStateEnum == ActionStateType.Jumping)
                    {
                        ActionState.SetPrevious(new MarioActionStateIdle(this, null));
                    }
                    else
                    {
                        ActionState.Leave(new MarioActionStateIdle(this, null));
                    }
                }
            }
            //invulernability
            if (Invincible)
            {
                HandleInvincibilityUpdate(gameTime);
            }

            //handle walking
            if (ActionStateEnum == ActionStateType.WalkingLeft || ActionStateEnum == ActionStateType.WalkingRight || ActionStateEnum == ActionStateType.Decaying)
            {
                if (fireChange >= FireTime || !ShotFireBall) 
                {
                    if (ActionState is MarioActionStateMovingLeft || ActionState is MarioActionStateMovingRight || ActionState is MarioActionStateDecaying)
                        HandleWalkingUpdate(gameTime);
                    else
                        HandleRunningUpdate(gameTime);
                }
                
            }
            else
            {
                CurrentFrame = 0;
            }

            //powerstate handles the texture changes for mario
            if (PowerStateEnum == PowerStateType.Star)
            {
                HandleStarUpdate(gameTime);
            }

            if (ShotFireBall)
            {
                if(fireChange >= FireTime)
                {
                    Rectangle source = SourceRectangle;
                    MarioSpriteFactory.SetSourceRectangle(ref source, PowerStateEnum, ActionStateEnum);
                    SourceRectangle = source;
                    if(fireChange >= 4*FireTime)
                    {
                        ShotFireBall = false;
                        fireChange = 0;
                    }
                    else
                    {
                        fireChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                }
                else
                {
                    fireChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

            }

            //koopa shell holding
            if(ActionStateEnum == ActionStateType.HoldingShell)
            {
                ((MarioHoldingShellState)ActionState).UpdateState();
            }
        }

        //private methods for update
        private void HandleInvincibilityUpdate(GameTime gameTime)
        {
            if (invincibleCurrentTime >= InvincibleTime)
            {
                Invincible = false;
                useSource = true;
                Rectangle sourceInv = SourceRectangle;
                PowerStateType power = PowerStateEnum;
                if (PowerStateEnum == PowerStateType.Star)
                {
                    power = PowerState.Previous;
                }
                MarioSpriteFactory.SetSourceRectangle(ref sourceInv, power, ActionStateEnum);
                SourceRectangle = sourceInv;
                invincibleCurrentTime = 0;
                sourceInvincibleChange = 0;
            }
            else
            {
                invincibleCurrentTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (sourceInvincibleChange >= SourceChangeTime)
                {
                    PowerStateType power;
                    if (useSource)
                    {
                        if(PowerStateEnum != PowerStateType.Star) 
                        {
                            power = PowerStateEnum;
                        }
                        else
                        {
                            power = PowerState.Previous;
                        }
                        
                        useSource = false;
                    }
                    else
                    {
                        if (PowerStateEnum != PowerStateType.Star)
                        {
                            power = PowerState.Previous;
                        }
                        else
                        {
                            power = ((MarioPowerStateStar)PowerState).previousState.Previous;
                        }
                        useSource = true;
                    }
                    Rectangle sourceInv = SourceRectangle;
                    MarioSpriteFactory.SetSourceRectangle(ref sourceInv, power, ActionStateEnum);
                    if (ActionStateEnum == ActionStateType.WalkingLeft || ActionStateEnum == ActionStateType.WalkingRight)//need to handle walking as this will update faster than animation frame
                    {
                        MarioSpriteFactory.Walking(ref sourceInv, CurrentFrame, power, ActionStateEnum);
                    }

                    SourceRectangle = sourceInv;
                    sourceInvincibleChange = 0;
                }
                else
                {
                    sourceInvincibleChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        private void HandleWalkingUpdate(GameTime gameTime)
        {
            if (lastChange > 16)
            {
                if (CurrentFrame == 2)
                {
                    dir = -1;
                }
                else if (CurrentFrame == 0)
                {
                    dir = 1;
                }
                CurrentFrame += dir;
                Rectangle source = SourceRectangle;
                //to handle invincibility while walking
                PowerStateType power = PowerStateEnum;
                if (Invincible)
                {
                    if (useSource)//useSource was switched in invincable
                    {
                        power = PowerState.Previous;
                        useSource = true;
                    }
                }
                if (PowerStateEnum == PowerStateType.Star)
                {
                    power = PowerState.Previous;
                }
                if (ActionStateEnum == ActionStateType.Decaying && source.X >= MarioSpriteFactory.RightFacingStart)
                {
                    MarioSpriteFactory.Walking(ref source, CurrentFrame, power, ActionStateType.WalkingRight);
                }
                else
                {
                    MarioSpriteFactory.Walking(ref source, CurrentFrame, power, ActionStateEnum);
                }
                
                SourceRectangle = source;
                lastChange = 0;
            }
            else
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void HandleRunningUpdate(GameTime gameTime)
        {
            if (lastChange > 16)
            {
                if (CurrentFrame == 2)
                {
                    dir = -1;
                }
                else if (CurrentFrame == 0)
                {
                    dir = 1;
                }
                CurrentFrame += dir;
                Rectangle source = SourceRectangle;
                //to handle invincibility while walking
                PowerStateType power = PowerStateEnum;
                if (Invincible)
                {
                    if (useSource)//useSource was switched in invincable
                    {
                        power = PowerState.Previous;
                        useSource = true;
                    }
                }
                if (PowerStateEnum == PowerStateType.Star)
                {
                    power = PowerState.Previous;
                }
                if (ActionStateEnum == ActionStateType.Decaying && source.X >= MarioSpriteFactory.RightFacingStart)
                {
                    MarioSpriteFactory.Running(ref source, CurrentFrame, power, ActionStateType.WalkingRight);
                }
                else
                {
                    MarioSpriteFactory.Running(ref source, CurrentFrame, power, ActionStateEnum);
                }

                SourceRectangle = source;
                lastChange = 0;
            }
            else
            {
                lastChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void HandleStarUpdate(GameTime gameTime)
        {
            if (!Invincible)
            {
                if (starChange >= starStateChange)
                {
                    CurrentTexture += starState;
                    if (CurrentTexture == 0)
                    {
                        starState = 1;
                    }
                    else if (CurrentTexture == 3)
                    {
                        starState = -1;
                    }
                    starChange = 0;
                    iteration++;
                    if (iteration >= 40)
                    {
                        iteration = 0;
                        starState = 1;
                        ((MarioPowerStateStar)PowerState).Leave();
                    }
                }
                else
                {
                    starChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                Texture = MarioSpriteFactory.Instance.Star(CurrentTexture);
            }
        }
        public void Reset()
        {
            ActionState = new MarioActionStateIdle(this, null);
            PowerState = new MarioPowerStateNormal(this);
            ActionState.Enter();
            PowerState.Enter();
            Collidable = true;
            Velocity = Vector2.Zero;
            ChangeFacing(1);
            OnGround = true;
            Falling = false;
            lockDir = 0;
        }

        public override IEntity Clone()
        {
            return this;
        }

        //events
        public void HandleLevelEvent(object sender, EventArgs e)
        {
            InstantKill();
        }
        protected virtual void OnRaiseCollisionEvent(MarioCollisionEventArgs e)
        {
            RaiseCollisionEvent?.Invoke(this, e);
        }

        public delegate void CollisionEventHandler(object sender, MarioCollisionEventArgs e);
        public event EventHandler<MarioCollisionEventArgs> RaiseCollisionEvent;

        protected virtual void OnRaiseInstantDeathEvent(EventArgs e)
        {
            RaiseInstantDeathEvent?.Invoke(this, e);
        }
        public delegate void InstantDeathEventHandler(object sender, EventArgs e);
        public event EventHandler<EventArgs> RaiseInstantDeathEvent;
    }

}
