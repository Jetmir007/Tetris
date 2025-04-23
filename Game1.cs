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
    private int score = 0;
    private SpriteFont font;
    private Block saveBlock;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 480;
        _graphics.PreferredBackBufferWidth = 600;
        _graphics.ApplyChanges();
        // TODO: Add your initialization logic here
        gameField = new GameField();
        base.Initialize();

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        pixel = Content.Load<Texture2D>("pixel");
        font = Content.Load<SpriteFont>("spritefont");
        //pixel.SetData(new Color[]{Color.White});
        Spawn();

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
                int lines = Clear();
                switch(lines){
                    case 1: score += 100; break;
                    case 2: score += 200; break;
                    case 3: score += 300; break;
                    case 4: score += 500; break;
                }
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
        _spriteBatch.Draw(pixel, new Rectangle(0, 0, 200, 480), Color.Brown);
        _spriteBatch.Draw(pixel, new Rectangle(400, 0, 200, 480), Color.Brown);
        newBlock.Draw(_spriteBatch);
        for (int i = 0; i < 24; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(gameField.field[i, j]){
                    _spriteBatch.Draw(pixel, new Rectangle(20*j+200, 20*i, 20, 20), Color.Black);
                }
            }
        }
        _spriteBatch.DrawString(font, "Score: " + Convert.ToString(score), new Vector2(15, 15), Color.MonoGameOrange);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private void BlockUpdate(){
        newKState = Keyboard.GetState();
        if(newKState.IsKeyDown(Keys.Left)&&!gameField.CheckCollision(newBlock, newBlock.X-1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.Left)){
            newBlock.X--;
        }
        if(newKState.IsKeyDown(Keys.Right)&&!gameField.CheckCollision(newBlock, newBlock.X+1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.Right)){
            newBlock.X++;
        }
        if(newKState.IsKeyDown(Keys.Down)&&score<=200){
            fallSpeed = 0.1;
        }
        else if(score>200){
            fallSpeed = 0.001;
        }
        else{
            fallSpeed = 0.5;
        }

        if(newKState.IsKeyDown(Keys.Up)&&oldKstate.IsKeyUp(Keys.Up)){
            bool[,] rotatedTiles = newBlock.Rotate();
            if(!gameField.CheckCollisionRotate(rotatedTiles, newBlock.X, newBlock.Y)){
                newBlock.tiles = rotatedTiles;
            }
        }
        oldKstate = newKState;
    }

    private void Spawn(){
        Random rng = new Random();
        newBlock = new Block(pixel, (BlockType)rng.Next(0,6));
    }

    private int Clear(){
        int i = gameField.rows-1;
        int clears = 0;
        while(i>=0){
            bool full = true;

            for (int j = 0; j < gameField.cols; j++)
            {
                if(!gameField.field[i, j]){
                    full = false;
                    break;
                }                
            }

            if(full){
                clears++;
                for (int x = 0; x < gameField.cols; x++){
                    gameField.field[i, x] = false;
                }
                for (int y = i; y > 0; y--)
                {
                    for (int x = 0; x < gameField.cols; x++)
                    {
                        gameField.field[y, x] = gameField.field[y-1, x];
                    }
                }

                for(int col = 0; col<gameField.cols; col++){
                    gameField.field[0,col] = false;
                }
            }
            else{
                i--;
            }
        }
        return clears;
    }
}