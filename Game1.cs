using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace Tetris;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D pixel;
    private Block newBlock;
    private GameField gameField;
    private double fallSpeed = 0.5;
    private double fallTime = 0;
    private KeyboardState newKState;
    private KeyboardState oldKstate;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 480;
        _graphics.PreferredBackBufferWidth = 200;
        _graphics.ApplyChanges();
        // TODO: Add your initialization logic here
        gameField = new GameField();
        base.Initialize();

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        pixel = Content.Load<Texture2D>("pixel");
        newBlock = new Block(pixel, BlockType.I);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        fallTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (fallTime>=fallSpeed){
            if(!gameField.CheckCollision(newBlock, newBlock.X, newBlock.Y+1)){
                newBlock.Y++;
            }
            else{
                gameField.Place(newBlock);
                Spawn();
            }
            fallTime = 0;
        }
        
        BlockUpdate();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        for (int i = 0; i < 24; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(gameField.field[i, j]){
                    _spriteBatch.Draw(pixel, new Rectangle(20*i, 20*j, 20, 20), Color.Red);
                }
            }
        }
        newBlock.Draw(_spriteBatch);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private void BlockUpdate(){
        newKState = Keyboard.GetState();
        if(newKState.IsKeyDown(Keys.Left)&&!gameField.CheckCollision(newBlock, newBlock.X-1, newBlock.Y)){
            newBlock.X--;
        }
        if(newKState.IsKeyDown(Keys.Right)&&!gameField.CheckCollision(newBlock, newBlock.X+1, newBlock.Y)){
            newBlock.X++;
        }
        if(newKState.IsKeyDown(Keys.Down)){
            fallSpeed = 0.1;
        }
        else{
            fallSpeed = 0.5;
        }

        if(newKState.IsKeyDown(Keys.Up)&&oldKstate.IsKeyUp(Keys.Up)){
            newBlock.Rotate();
        }
        oldKstate = newKState;
    }

    private void Spawn(){
        Random rng = new Random();
        newBlock = new Block(pixel, (BlockType)rng.Next(0,7));
    }


}