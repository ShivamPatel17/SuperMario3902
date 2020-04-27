using System;
using TechSupportMario.Commands;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.Items;
using TechSupportMario.Entity.MarioEntity;
using TechSupportMario.State.EnemyState;
using TechSupportMario.State.GameStates;
using TechSupportMario.State.MarioStates.MarioPowerStates;

namespace TechSupportMario
{
    public class Player
    {
        public int Lives { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public int Keys { get; 
            set; }
        public Mario PlayerMario { get; set; }
        private readonly PlayerSave playerSave;
        
        public Player()
        {
            Coins = 0;
            Score = 0;
            Lives = 3;
            Keys = 0;
            playerSave = new PlayerSave();
            SaveState();
        }

        public void HandleMarioCollision(object sender, MarioCollisionEventArgs eventArgs)
        {
            //collidedwith is null if takedamage is called
            if (eventArgs.CollidedWith is null)
            {
                if (eventArgs.TookDamage)
                {
                    if (PlayerMario?.PowerState is MarioPowerStateDead)
                    {
                        Lives -= 1;
                        if(Lives != 0)
                        {
                            Stage.level.State = new MarioDeadState(Stage.level);
                            Stage.level.State.Enter();
                        }
                        else
                        {
                            Stage.level.State.End();
                            if (!(Stage.level.State is GameOverState))
                            {
                                SoundItems.SoundFactory.Instance.Dead();
                            }
                        }
                    }
                }
            }
            else if(eventArgs.CollidedWith is AbstractEnemy)
            {
                if (eventArgs.CollidedWith is Goomba)
                {
                    Score += 100;
                }
                else if (eventArgs.CollidedWith is GreenKoopa && !(((GreenKoopa)eventArgs.CollidedWith).EnemyState is KoopaShellState))
                {
                    Score += 100;
                }
            }
            else if(eventArgs.CollidedWith is IItem && eventArgs.CollidedWith.Collect == false)
            {
                if(eventArgs.CollidedWith is Coin)
                {
                    AddCoin();
                }
                if(eventArgs.CollidedWith is SuperMushroom || eventArgs.CollidedWith is FireFlower || eventArgs.CollidedWith is Star)
                {
                    Score += 1000;
                }
                if(eventArgs.CollidedWith is OneUpMushroom)
                {
                    Lives += 1;
                }
                if(eventArgs.CollidedWith is Key)
                {
                    Keys += 1;
                    Stage.level.Clock.Time += 40;
                }
            } 
            else if(eventArgs.CollidedWith is IBlock)
            {
                if(((AbstractBlock)eventArgs.CollidedWith).Items.Count > 0)
                {
                    ICommand item = ((AbstractBlock)eventArgs.CollidedWith)?.Items?.Peek();
                    if (item is CoinItemCommand)
                    {
                        AddCoin();
                    }
                }
                
            }
        }

        public void HandleInstantDeathEvent(object sender, EventArgs e)
        {
            Lives -= 1;
            if (Lives != 0)
            {
                Stage.level.State = new MarioDeadState(Stage.level);
                Stage.level.State.Enter();
            }
            else
            {
                Stage.level.State.End();
                if (!(Stage.level.State is GameOverState))
                {
                    SoundItems.SoundFactory.Instance.Dead();
                }
            }
        }

        public void HandleFireballEvent(object sender, EventArgs e)
        {
            //right now takes regular event args, if enemies give different amounts of points, need to pass in the enemy type
            Score += 100;
        }

        public void AddPoints(int points)
        {
            Score += points;
        }

        public void Reset()
        {
            if (Lives == 0)
            {
                Lives = 3;
                Coins = 0;
                Score = 0;
                Keys = 0;
                playerSave.SavePlayer(this);
            }
            else
            {
                playerSave.LoadSave(this);
            }
            
        }
        public void Restart()
        {
            Lives = 3;
            Coins = 0;
            Score = 0;
            Keys = 0;
        }

        public void SaveState()
        {
            playerSave.SavePlayer(this);
        }

        private void AddCoin()
        {
            Score += 200;
            Coins += 1;
            if (Coins % 100 == 0)
            {
                Lives += 1;
            }
        }

        internal class PlayerSave
        {
            private int score;
            private int coins;

            public void SavePlayer(Player player)
            {
                score = player.Score;
                coins = player.Coins;
            }

            public void LoadSave(Player player)
            {
                player.Score = score;
                player.Coins = coins;
            }
        }
    }
}
