using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechSupportMario.Entity.Block;
using TechSupportMario.State.BlockState;
using static TechSupportMario.Entity.AbstractEntity;
using static TechSupportMario.Entity.Block.AbstractBlock;

namespace TechSupportMario.Entity
{
    public class BlockFactory
    {
        private static BlockFactory _instance;

        private Texture2D PyramidBlockSpriteSheet;
        private Texture2D FloorBlockSpriteSheet;
        private Texture2D QuestionBlockSpriteSheet;
        private Texture2D BrickBlockSpriteSheet;
        private Texture2D PipeSpriteSheet;
        private Texture2D PipeTopSpriteSheet;
        private Texture2D PipeMiddleSpriteSheet;
        private Texture2D PipeBottomSpriteSheet;
        private Texture2D UsedBlockTexture;
        private Texture2D PipeTopLeftSpriteSheet;
        private Texture2D PipeTopRightSpriteSheet;
        private Texture2D UndergroundFloorTop;
        private Texture2D UndergroundFloorNormal;
        private Texture2D UndergroundFloorBlank;
        private Texture2D UndergroundFloorRight;
        private Texture2D UndergroundFloorLeft;
        private Texture2D CastleFloor;
        private Texture2D Rock;
        private Texture2D Button;
        private Texture2D Toggle;
        private Texture2D KeyHole;

        private BlockFactory() { }

        public static BlockFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BlockFactory();
                }
                return _instance;
            }
        }

        public void LoadFactory(Game game)
        {
            PyramidBlockSpriteSheet = game.Content.Load<Texture2D>("blocks/pyramid_block");
            FloorBlockSpriteSheet = game.Content.Load<Texture2D>("blocks/basic_floor_block");
            UsedBlockTexture = game.Content.Load<Texture2D>("blocks/used_block");
            QuestionBlockSpriteSheet = game.Content.Load<Texture2D>("blocks/question_block");
            BrickBlockSpriteSheet = game.Content.Load<Texture2D>("blocks/orignal_brown_brick");
            PipeSpriteSheet = game.Content.Load<Texture2D>("blocks/pipe");
            PipeTopSpriteSheet = game.Content.Load<Texture2D>("blocks/Pipe_Top");
            PipeMiddleSpriteSheet = game.Content.Load<Texture2D>("blocks/Pipe_middle");
            PipeBottomSpriteSheet = game.Content.Load<Texture2D>("blocks/Pipe_bottom");
            PipeTopRightSpriteSheet = game.Content.Load<Texture2D>("blocks/Pipe_Top_Right");
            PipeTopLeftSpriteSheet = game.Content.Load<Texture2D>("blocks/Pipe_Top_Left");
            UndergroundFloorTop = game.Content.Load<Texture2D>("blocks/Underground_floor_top");
            UndergroundFloorNormal = game.Content.Load<Texture2D>("blocks/Underground_floor");
            UndergroundFloorBlank = game.Content.Load<Texture2D>("blocks/bottom_underground");
            UndergroundFloorRight = game.Content.Load<Texture2D>("blocks/Underground_floor_right");
            UndergroundFloorLeft = game.Content.Load<Texture2D>("blocks/Underground_floor_left");
            CastleFloor = game.Content.Load<Texture2D>("blocks/castle_floor");
            Rock = game.Content.Load<Texture2D>("blocks/rockparts");
            Button = game.Content.Load<Texture2D>("blocks/button");
            Toggle = game.Content.Load<Texture2D>("blocks/toggle_block");
            KeyHole = game.Content.Load<Texture2D>("blocks/block_keyhole");
        }

        #region BlockSprites
        // Need to make this as function so that we can call as many as we prefer
        public IBlock PyramidBlock()
        {
            return new BasicBlock(PyramidBlockSpriteSheet, EntityType.Basic);
        }

        public IBlock FloorBlock()
        {
            return new BasicBlock(FloorBlockSpriteSheet, EntityType.Basic);
        }

        public IBlock CastleFloorBlock()
        {
            return new BasicBlock(CastleFloor, EntityType.Basic);
        }

        public IBlock QuestionBlock()
        {
            return new QuestionBlock(QuestionBlockSpriteSheet);
        }
        public QuestionBlock QuestionBlockQ()
        {
            return new QuestionBlock(QuestionBlockSpriteSheet);
        }

        public IBlock BrickBlock()
        {
            return new BrickBlock(BrickBlockSpriteSheet);
        }

        public IBlock HiddenBrickBlock()
        {
            BrickBlock b = new BrickBlock(BrickBlockSpriteSheet);
            b.BlockState = new BlockStateHidden(b);
            b.BlockState.Enter();
            return b;
        }

        public IBlock HiddenQuestionBlock()
        {
            QuestionBlock q = new QuestionBlock(QuestionBlockSpriteSheet);
            q.BlockState = new BlockStateHidden(q);
            q.BlockState.Enter();
            return q;
        }

        //Probably decide to delete this later. This one for now is just for show.
        public IBlock UsedBlock()
        {
            QuestionBlock q = new QuestionBlock(QuestionBlockSpriteSheet);
            q.StraightToUsed();
            return q;
        }

        public IBlock WarpUpTop()
        {
            return new WarpEntranceBlock(PipeTopSpriteSheet, Collisions.CollisionDetection.Direction.up);
        }

        public IBlock WarpUp()
        {
            return new WarpEntranceBlock(PipeSpriteSheet, Collisions.CollisionDetection.Direction.up);
        }

        public IBlock WarpEntityTop()
        {
            return new EntityWarpBlock(PipeTopSpriteSheet);
        }

        public IBlock WarpEntity()
        {
            return new EntityWarpBlock(PipeSpriteSheet);
        }

        public IBlock WarpRight()
        {
            return new WarpEntranceBlock(PipeTopRightSpriteSheet, Collisions.CollisionDetection.Direction.right);
        }

        public IBlock WarpLeft()
        {
            return new WarpEntranceBlock(PipeTopLeftSpriteSheet, Collisions.CollisionDetection.Direction.left);
        }

        public IBlock PipeTop()
        {
            return new BasicBlock(PipeTopSpriteSheet, EntityType.PipeTop);
        }

        public IBlock PipeBottom()
        {
            return new BasicBlock(PipeBottomSpriteSheet, EntityType.PipeBottom);
        }

        public IBlock PipeMiddle()
        {
            return new BasicBlock(PipeMiddleSpriteSheet, EntityType.PipeMiddle);
        }

        public Texture2D UsedTexture()
        {
            return UsedBlockTexture;
        }

        public IBlock UndergroundFloorTopBLock()
        {
            return new BasicBlock(UndergroundFloorTop, EntityType.Basic);
        }

        public IBlock UndergroundFloorNormalBLock()
        {
            return new BasicBlock(UndergroundFloorNormal, EntityType.Basic);
        }

        public IBlock UndergroundFloorLeftBLock()
        {
            return new BasicBlock(UndergroundFloorLeft, EntityType.Basic);
        }

        public IBlock UndergroundFloorRightBLock()
        {
            return new BasicBlock(UndergroundFloorRight, EntityType.Basic);
        }

        public IBlock UndergroundFloorBlankBLock()
        {
            return new BasicBlock(UndergroundFloorBlank, EntityType.Basic);
        }

        public IBlock RockBlock()
        {
            return new BasicBlock(Rock, EntityType.Basic);
        }

        public IBlock ButtonBlock()
        {
            return new Button(Button);
        }

        public IBlock ToggleBlock()
        {
            return new ToggleBlock(Toggle);
        }

        public IBlock KeyHoleBlock()
        {
            return new KeyHoleBlock(KeyHole);
        }

        public DisappearingBlock DisappearingBlockSkin(char type, char part = '0', char side = '0')
        {
            DisappearingBlock block = null;

            switch (type)
            {
                case 'B':
                    block = new DisappearingBlock(BrickBlockSpriteSheet)
                    {
                        SourceRectangle = new Rectangle(new Point(), new Point(32))
                    };
                    break;
                case 'F':
                    switch (part)
                    {
                        case 'C':
                            block = new DisappearingBlock(CastleFloor);
                            break;
                        case 'U':
                            switch (side)
                            {
                                case 'N':
                                    block = new DisappearingBlock(UndergroundFloorNormal);
                                    break;
                                case 'R':
                                    block = new DisappearingBlock(UndergroundFloorRight);
                                    break;
                                case 'L':
                                    block = new DisappearingBlock(UndergroundFloorLeft);
                                    break;
                                case 'T':
                                    block = new DisappearingBlock(UndergroundFloorTop);
                                    break;
                                default:
                                    block = new DisappearingBlock(UndergroundFloorBlank);
                                    break;
                            }
                            break;
                        default:
                            block = new DisappearingBlock(FloorBlockSpriteSheet);
                            break;
                    }
                    break;
                case 'R':
                    block = new DisappearingBlock(Rock);
                    Rectangle sr = new Rectangle()
                    {
                        Size = new Point(32)
                    };

                    switch (part)
                    {
                        case 'M':
                            sr.Location = new Point(32);
                            break;
                        case 'T':
                            sr.Location = new Point(32, 0);
                            break;
                        case 'B':
                            sr.Location = new Point(32, 64);
                            break;
                        case 'L':
                            switch (side)
                            {
                                case 'T':
                                    sr.Location = new Point(0, 0);
                                    break;
                                case 'B':
                                    sr.Location = new Point(0, 64);
                                    break;
                                default:
                                    sr.Location = new Point(0, 32);
                                    break;
                            }
                            break;
                        case 'R':
                            switch (side)
                            {
                                case 'T':
                                    sr.Location = new Point(64, 0);
                                    break;
                                case 'B':
                                    sr.Location = new Point(64, 64);
                                    break;
                                default:
                                    sr.Location = new Point(64, 32);
                                    break;
                            }
                            break;
                    }
                    block.SourceRectangle = sr;
                    break;
                case 'Y':
                    block = new DisappearingBlock(PyramidBlockSpriteSheet);
                    break;
            }
            return block;
        }
        #endregion


    }
}
