﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using TechSupportMario.CameraItems;
using TechSupportMario.Collisions;
using TechSupportMario.Commands;
using TechSupportMario.Entity;
using TechSupportMario.Entity.Background;
using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.CollisionZones;
using TechSupportMario.Entity.Enemy;
using TechSupportMario.Entity.Items;

namespace TechSupportMario.LevelLoader
{
    class Loader
    {
        private Loader ()
        {

        }

        public static void LoadLevel(Camera camera, string fileName)
        {
            //load the background and then the level
            Stage.level = new Level();
            StreamReader reader = new StreamReader(fileName);
            LevelPart currentLevelPart = null;
            int order = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.StartsWith("B"))
                {
                    //background
                    int layerOrder = int.Parse(reader.ReadLine());
                    float para = float.Parse(reader.ReadLine());
                    currentLevelPart?.Layers.Add(LoadBackground(reader.ReadLine(), camera, para, layerOrder));
                }
                else if (line.StartsWith("A"))
                {
                    //entities in the game player level
                    int areaOrder = int.Parse(reader.ReadLine());
                    LevelPart lp = LoadArea(reader.ReadLine(), camera);
                    lp.Song = line[1] - '0';
                    if (currentLevelPart != null)
                    {
                        Stage.level.AddArea(currentLevelPart, order);
                    }
                    currentLevelPart = lp;
                    order = areaOrder;
                }
            }
            Stage.level.AddArea(currentLevelPart, order);
        }

        public static Layer LoadBackground(string filename, Camera camera, float par, int order)
        {
            Layer background = new Layer(camera)
            {
                Parallax = new Vector2(par, par),
                Order = order,
                SamplerEffect = SamplerState.LinearWrap
            };
            if(filename == "U")
            {
                background.Sprites.Add(BackgroundFactory.Instance.UndergroundPreset());
            }
            else if (filename == "CN")
            {
                background.Sprites.Add(BackgroundFactory.Instance.CastleNormal());
            }
            else if (filename == "CP")
            {
                background.Sprites.Add(BackgroundFactory.Instance.CastlePillarSprite());
            }
            else if (filename == "CW")
            {
                background.Sprites.Add(BackgroundFactory.Instance.CastleWindowsSprite());
                background.FullLinearWrap = true;
            }
            else
            {
                ReadCSV(filename, background);
            }
            
            return background;
        }

        public static LevelPart LoadArea(string filename, Camera camera)
        {
            QuadTree quad = new QuadTree((Rectangle)camera.Limits, 0);
            Dictionary<int, Vector2> dict = new Dictionary<int, Vector2>();
            ReadCSV(filename, null, quad, dict);
            LevelPart lp = new LevelPart
            {
                Entities = quad,
                ExitPositions = dict
            };
            return lp;
        }

        public static void ReadCSV(string filename, Layer layer, QuadTree quad = null, Dictionary<int, Vector2> dictionary = null)
        {
            int XDim = 0, YDim = 0, XPos = 0, YPos = 0, x;
            string cells;
            StreamReader reader = new StreamReader(filename);
            List<Button> buttons = new List<Button>();
            if (!reader.EndOfStream) reader.ReadLine();//read the first line as it has no information
            if (!reader.EndOfStream)
            {
                string[] inits = reader.ReadLine().Split(',');
                XDim = int.Parse(inits[1]);
                YDim = int.Parse(inits[3]);
                XPos = int.Parse(inits[5]);
                YPos = int.Parse(inits[7]);
                int buttonCount = int.Parse(inits[13]);
                for(int i = 0; i < buttonCount; i++)
                {
                    buttons.Add((Button)BlockFactory.Instance.ButtonBlock());
                }
                if (quad != null)
                {
                    int rows = int.Parse(inits[9]);
                    int columns = int.Parse(inits[11]);
                    Rectangle bounds = new Rectangle
                    {
                        Width = XDim * columns,
                        Height = YDim * rows
                    };
                    quad.Bounds = bounds;
                }
            }
            while (!reader.EndOfStream)
            {
                x = XPos;

                cells = reader.ReadLine();
                foreach (string cell in cells.Split(','))
                {
                    BuildEntity(cell, x, YPos, layer, quad, dictionary, buttons);
                    x += XDim;
                }
                YPos += YDim;
            }
        }

        public static void BuildEntity(string code, int XPos, int YPos, Layer layer, QuadTree quad, Dictionary<int, Vector2> dictionary, List<Button> buttons)
        {
            if (code.Length > 0)
            {
                char key1 = code[0], key2 = 'a', key3 = 'a';
                char key4 = '0';
                if (code.Length > 1)
                {
                    key2 = code[1];
                    if (code.Length > 2)
                    {
                        key3 = code[2];
                        if (code.Length > 3)
                        {
                            key4 = code[3];
                        }
                    }
                    else
                    {
                        key3 = '0';
                    }
                }
                
               


                //see 'Level Key.txt' for the Key!!
                switch (key1)
                {
                    //this is the case for blocks
                    case 'B':
                        IBlock block = BuildBlock(key2, key3, key4, XPos, YPos, quad, code, buttons);
                        
                        if (block != null)
                        {
                            block.Position = new Vector2(XPos, YPos);
                            quad.Insert(block);
                        }
                        break;

                    //this is the case for Enemies
                    case 'E':
                        //guaranteed to have a valid key2 to specify the type of enemy
                        IEntity e = BuildEnemy(key2);
                        if(e != null)
                        {
                            e.Position = new Vector2(XPos, YPos);
                            quad.Insert(e);
                            ((AbstractEnemy)e).RaiseFireballEvent += Stage.player.HandleFireballEvent;
                        }
                        break;

                    //this is the case for Items
                    
                    case 'I':
                        //guaranteed to have a valid key2 to specify the type of item
                        switch (key2)
                        {
                            case 'C':
                                IEntity c = (IEntity)ItemFactory.Instance.Coin();
                                c.Position = new Vector2(XPos, YPos);
                                quad.Insert(c);
                                break;
                            case 'K':
                                IEntity k = (IEntity)ItemFactory.Instance.KeyItem();
                                k.Position = new Vector2(XPos, YPos);
                                quad.Insert(k);
                                break;
                        }
                        break;                   
                    //this is the base for background images
                    case 'G':
                        ISprite s = null;
                        switch (key2)
                        {
                            case 'C':
                                s = BackgroundFactory.Instance.Cloud();
                                break;
                            case 'B':
                                s = BackgroundFactory.Instance.BigCloud();
                                break;
                            case 'U':
                                s = BackgroundFactory.Instance.Bush();
                                break;
                            case 'M':
                                s = BackgroundFactory.Instance.Mountain();
                                break;
                            case '1':
                                s = BackgroundFactory.Instance.MountainRange();
                                break;
                            case '2':
                                s = BackgroundFactory.Instance.MountainRange();
                                break;
                        }
                        if(s != null)
                        {
                            layer.Sprites.Add(s);
                            s.Position = new Vector2(XPos, YPos);
                            if(key2 == '2')
                            {
                                s.Position += new Vector2(0, 4);
                            }
                        }
                        break;
                    //this is the case for mario
                    case 'M':
                            Stage.mario.Position = new Vector2(XPos, YPos);
                            Stage.mario.SourceRectangle = new Rectangle(new Point(0, 0), new Point(28, 40));
                            Stage.mario.ChangeFacing(1);//make mario face the right instead of left.
                            quad.Insert(Stage.mario);
                        
                        break;
                    case 'C':
                        BuildZone(key2, XPos, YPos, quad, key3);
                        break;
                    case 'Z':
                        int exit = key2 - '0';
                        dictionary.Add(exit, new Vector2(XPos, YPos + 30));
                        break;
                    default:
                        break;
                }
            }

        }

        private static void BuildZone(int key2, int XPos, int YPos, QuadTree quad, int key3 = 0)
        {
            CollisionZone ca = null;
            switch (key2)
            {
                case 'C':
                    ca = CollisionZoneFactory.Instance.CheckpointZone(new Point(XPos, 0), new Point(40, quad.Bounds.Height));
                    ca.LookingFor = AbstractEntity.EntityType.Mario;
                    ca.Position = new Vector2(XPos, YPos);
                    ca.RaiseEnterEvent += Stage.HandleCheckpoint;
                    break;
                case 'F':
                    ca = CollisionZoneFactory.Instance.Flag(new Point(XPos + 16, YPos));
                    ca.Position = new Vector2(XPos + 16, YPos);
                    ca.RaiseEnterEvent += Stage.HandleGameEnd;
                    ca.LookingFor = AbstractEntity.EntityType.Mario;
                    break;
                case 'A':
                    ca = CollisionZoneFactory.Instance.Castle(new Point(XPos, YPos));
                    ca.Position = new Vector2(XPos, YPos);
                    break;
                case 'S':
                    ca = CollisionZoneFactory.Instance.SmallDoor(new Point(XPos, YPos));
                    ca.Position = new Vector2(XPos, YPos);
                    ((WarpZone)ca).NextArea = key3 - '0';
                    ca.RaiseEnterEvent += Stage.level.HandleInteractEnter;
                    ca.RaiseLeaveEvent += Stage.level.HandleInteractLeave;
                    ((WarpZone)ca).RaiseWarpEvent += Stage.HandleWarp;
                    ca.LookingFor = AbstractEntity.EntityType.Mario;
                    break;
                case 'L':
                    ca = CollisionZoneFactory.Instance.LockedDoorZone(new Point(XPos, YPos));
                    ca.Position = new Vector2(XPos, YPos);
                    for (int i = 0; i < 4; i++)
                    {
                        KeyHoleBlock b = (KeyHoleBlock)BlockFactory.Instance.KeyHoleBlock();
                        b.Position = new Vector2(XPos + 32 * (i - 1), YPos - b.Texture.Height - 32);
                        ((LockedDoor)ca).KeyHoles.Add(b);
                        quad.Insert(b);
                    }
                    ca.LookingFor = AbstractEntity.EntityType.Mario;
                    ca.RaiseEnterEvent += Stage.level.HandleInteractEnter;
                    ca.RaiseLeaveEvent += Stage.level.HandleInteractLeave;
                    ((WarpZone)ca).NextArea = key3 - '0'; 
                    ((WarpZone)ca).RaiseWarpEvent += Stage.HandleWarp;
                    break;
            }
            quad.Insert(ca);
        }

        private static IBlock BuildBlock(int key2, int key3, int key4, int XPos, int YPos, QuadTree quad, string code, List<Button> buttons)
        {
            IBlock block = null;
            switch (key2)
            {
                case 'A':
                    foreach(Button button in buttons)
                    {
                        if (!button.Set)
                        {
                            block = button;
                            button.Set = true;
                            break;
                        }
                    }
                    break;
                case 'B':
                    block = BlockFactory.Instance.BrickBlock();
                    if(code.Length == 4)
                    {
                        int i = 0;
                        while(code[3] - '0' > i)
                        {
                            BlockCommand(key3, block);
                            i++;
                        }
                    }
                    else
                    {
                        BlockCommand(key3, block);
                    }
                    break;
                case 'C':
                    if (code.Length == 6)
                    {
                        block = BlockFactory.Instance.DisappearingBlockSkin((char)key4, char.Parse(code.Substring(4,1)), char.Parse(code.Substring(5,1)));
                    }
                    else if (code.Length == 5)
                    {
                        block = BlockFactory.Instance.DisappearingBlockSkin((char)key4, char.Parse(code.Substring(4, 1)));
                    }
                    else
                    {
                        block = BlockFactory.Instance.DisappearingBlockSkin((char)key4);
                    }
                    buttons[key3 - '0'].RaisePressEvent += ((DisappearingBlock)block).HandleButtonPress;
                    break;
                case 'F':
                    switch (key3)
                    {
                        case 'U':
                            block = CreateUndergroundBlock(code[3]);
                            break;
                        case 'C':
                            block = BlockFactory.Instance.CastleFloorBlock();
                            break;
                        default:
                            block = BlockFactory.Instance.FloorBlock();
                            if(key3 == 'Z')
                            {
                                block.Collidable = false;
                            }
                            break;
                    }
                    break;
                case 'P':
                    block = BuildPipeBlock(key3);
                    break;
                
                case 'Q':
                    QuestionBlock qb = BlockFactory.Instance.QuestionBlockQ();
                    qb.Position = new Vector2(XPos, YPos);
                    ICommand qcmd = null;
                    //if key2 == Q, then there valid key3
                    switch (key3)
                    {
                        case 'C':
                            qcmd = new CoinItemCommand(qb);
                            break;
                        case 'M':
                            qcmd = new SuperMushroomItemCommand(qb);
                            break;
                        case 'F':
                            qcmd = new FireFlowerItemCommand(qb);
                            break;
                        case 'S':
                            qcmd = new StarItemCommand(qb);
                            break;
                        case 'O':
                            qcmd = new OneUpMushroomItemCommand(qb);
                            break;
                        case 'H':
                            block = BlockFactory.Instance.HiddenQuestionBlock();
                            BlockCommand(key3, block);
                            break;
                        case 'U':
                            qb.Position = new Vector2(XPos, YPos);
                            qb.StraightToUsed();
                            break;
                        default:
                            block = BlockFactory.Instance.QuestionBlock();
                            break;
                    }
                    if (qcmd != null)
                    {
                        quad.Insert(qb);
                        qb.AddCommand(qcmd);
                    }
                    break;
                case 'R':
                    block = BlockFactory.Instance.RockBlock();
                    Point size = new Point()
                    {
                        X = block.Texture.Width / 3,
                        Y = block.Texture.Height / 3
                    };

                    block.SourceRectangle = BlockPart(key3, key4, size);
                    break;
                case 'T':
                    block = BlockFactory.Instance.ToggleBlock();
                    for(int i = 3; i < code.Length; i++)
                    {
                        buttons[code[i] - '0'].RaisePressEvent += ((ToggleBlock)block).HandleButtonPress;
                        
                    }
                    if(code[2] == 'H')
                    {
                        block.SourceRectangle = new Rectangle(new Point(0), new Point(block.Texture.Width, block.Texture.Height / 2));
                        block.Collidable = false;
                        ((ToggleBlock)block).StartY = block.SourceRectangle.Y;
                    }
                    else
                    {
                        block.SourceRectangle = new Rectangle(new Point(0, block.Texture.Height / 2), new Point(block.Texture.Width, block.Texture.Height / 2));
                        ((ToggleBlock)block).StartY = 32;
                    }
                    break;
                case 'H':
                    block = BlockFactory.Instance.HiddenBrickBlock();
                    break;
                case 'W':
                    switch (key3)
                    {
                        case 'U':
                            block = BlockFactory.Instance.WarpUpTop();
                            break;
                        case 'L':
                            block = BlockFactory.Instance.WarpLeft();
                            break;
                        case 'R':
                            block = BlockFactory.Instance.WarpRight();
                            break;
                    }
                    ((WarpEntranceBlock)block).Warp = key4 - '0';
                    ((WarpEntranceBlock)block).RaiseWarpEvent += Stage.HandleWarp;
                    break;
                case 'I':
                    EntityWarpBlock warpBlock = (EntityWarpBlock)BlockFactory.Instance.WarpEntityTop();
                    ICommand wCommand = null;
                    switch (key3)
                    {
                        case 'C':
                            wCommand = new CoinItemCommand(warpBlock);
                            break;
                        case 'M':
                            wCommand = new SuperMushroomItemCommand(warpBlock);
                            break;
                        case 'F':
                            wCommand = new FireFlowerItemCommand(warpBlock);
                            break;
                        case 'S':
                            wCommand = new StarItemCommand(warpBlock);
                            break;
                        case 'O':
                            wCommand = new OneUpMushroomItemCommand(warpBlock);
                            break;
                        case 'G':
                            wCommand = new CreateEnemyCommand(AbstractEntity.EntityType.Goomba, warpBlock);
                            break;
                        case 'K':
                            wCommand = new CreateEnemyCommand(AbstractEntity.EntityType.GreenKoopa, warpBlock);
                            break;
                        case 'R':
                            wCommand = new CreateEnemyCommand(AbstractEntity.EntityType.RedKoop, warpBlock);
                            break;
                        case 'P':
                            wCommand = new CreateEnemyCommand(AbstractEntity.EntityType.PiranhaPlant, warpBlock);
                            break;
                    }
                    warpBlock.Command = wCommand;
                    warpBlock.Position = new Vector2(XPos, YPos);
                    Rectangle box = warpBlock.CollisionBox;
                    box.X -= box.Width / 2;
                    box.Width *= 2;
                    box.Y = warpBlock.CollisionBox.Y - warpBlock.TrueHeight();
                    box.Height = (int)(quad.Bounds.Height - YPos + warpBlock.CollisionBox.Height * 1.5);
                    CollisionZone hz = new CollisionZone(box)
                    {
                        LookingFor = AbstractEntity.EntityType.Mario
                    };
                    hz.RaiseEnterEvent += warpBlock.HandleEnterEvent;
                    hz.RaiseLeaveEvent += warpBlock.HandleLeaveEvent;
                    quad.Insert(warpBlock);
                    quad.Insert(hz);
                    break;
                case 'Y':
                    block = BlockFactory.Instance.PyramidBlock();
                    break;
            }
            return block;
        }

        private static void BlockCommand(int key3, IBlock block)
        {
            ICommand cmd = null;
            switch (key3)
            {
                case 'C':
                    cmd = new CoinItemCommand(block);
                    break;

                case 'M':
                    cmd = new SuperMushroomItemCommand(block);
                    break;
                case 'F':
                    cmd = new FireFlowerItemCommand(block);
                    break;
                case 'O':
                    cmd = new OneUpMushroomItemCommand(block);
                    break;

                case 'S':
                    cmd = new StarItemCommand(block);
                    break;
                default:
                    break;

            }
            if(cmd != null)
            {
                block.AddCommand(cmd);
            }
        }

        /// <summary>
        /// If a block has more than one part (like the rock) it has the textures in one file and needs to have a
        /// part of the texture selected passing in the keys works best for this.
        /// </summary>
        /// <param name="Key3"></param>
        /// <param name="key4"></param>
        private static Rectangle BlockPart(int key3, int key4, Point size)
        {
            Rectangle rectangle = new Rectangle()
            {
                Size = size
            };
            switch (key3)
            {
                case 'R':
                    rectangle.X = size.X * 2;
                    if (key4 == '0')
                    {
                        rectangle.Y = size.Y;
                    }
                    else if (key4 == 'B')
                    {
                        rectangle.Y = size.Y * 2;
                    }
                    break;
                case 'L':
                    if(key4 == '0')
                    {
                        rectangle.Y = size.Y;
                    }
                    else if (key4 == 'B')
                    {
                        rectangle.Y = size.Y * 2;
                    }
                    break;
                case 'T':
                    rectangle.X = size.X;
                    break;
                case 'B':
                    rectangle.X = size.X;
                    rectangle.Y = size.Y * 2;
                    break;
                case 'M':
                    rectangle.X = size.X;
                    rectangle.Y = size.Y;
                    break;
            }
            return rectangle;
        }

        /// <summary>
        /// Builds each enemy
        /// </summary>
        /// <param name="key2"></param>
        /// <returns></returns>
        private static IEntity BuildEnemy(int key2)
        {
            switch (key2)
            {
                case 'A':
                    return EnemyFactory.Instance.Boss();
                case 'G':
                    return EnemyFactory.Instance.Goomba();
                case 'K':
                    return EnemyFactory.Instance.GreenKoopa();
                case 'R':
                    return EnemyFactory.Instance.RedKoopa();
                case 'B':
                    return EnemyFactory.Instance.Boo();
                case 'C':
                    return EnemyFactory.Instance.Canon(1);
            }
            return null;
        }

        private static IBlock BuildPipeBlock(int key3)
        {
            switch (key3)
            {
                case 'M':
                    return BlockFactory.Instance.PipeMiddle();
                case 'B':
                    return BlockFactory.Instance.PipeBottom();
                default:
                    return BlockFactory.Instance.PipeTop();
            }
        }
        private static IBlock CreateUndergroundBlock(int key4)
        {
            IBlock block = null;
            switch (key4)
            {
                case 'N':
                    block = BlockFactory.Instance.UndergroundFloorNormalBLock();
                    break;
                case 'L':
                    block = BlockFactory.Instance.UndergroundFloorLeftBLock();
                    break;
                case 'R':
                    block = BlockFactory.Instance.UndergroundFloorRightBLock();
                    break;
                case 'T':
                    block = BlockFactory.Instance.UndergroundFloorTopBLock();
                    break;
                case 'Z':
                    block = BlockFactory.Instance.UndergroundFloorBlankBLock();
                    block.Collidable = false;
                    break;
            }
            return block;
        }
    }
}
