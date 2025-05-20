using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
    private Block saveBlock = null;
    private bool canSave = true;
    private Block nextBlock;
    private int totalLines = 0;
    private bool gameOver = false;
    private List<ScoreEntry> leaderboard = new List<ScoreEntry>();
    private string leaderboardFile = "leaderboard.txt";
    private string pName = "";
    private bool enterName = false;
    Song song;
    SoundEffect kaching;
    Song lebron;
    SoundEffect taco;
    private bool pause = false;
   


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
        song = Content.Load<Song>("original-tetris-theme-tetris-soundtrack-made-with-Voicemod");
        kaching = Content.Load<SoundEffect>("ka-ching");
        taco = Content.Load<SoundEffect>("Voicy_Taco tuesday!!!");
        lebron = Content.Load<Song>("you-are-my-sunshine-lebron-james");
        MediaPlayer.Play(song);
        //pixel.SetData(new Color[]{Color.White});
        Spawn();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        newKState = Keyboard.GetState();
        fallTime += gameTime.ElapsedGameTime.TotalSeconds;
        if(MediaPlayer.State==MediaState.Stopped){
            MediaPlayer.Play(song);
        }
        if(!gameOver){
            if (newKState.IsKeyDown(Keys.Enter) && oldKstate.IsKeyUp(Keys.Enter))
            {
                pause = true;
            }
            if (pause)
            {
                if (newKState.IsKeyDown(Keys.Back) && oldKstate.IsKeyUp(Keys.Back))
                {
                    pause = false;
                }
                return;
            }
            if (fallTime >= fallSpeed)
            {
                if (!gameField.CheckCollision(newBlock, newBlock.X, newBlock.Y + 1))
                {
                    newBlock.Y++;
                }
                else
                {
                    gameField.Place(newBlock);
                    int lines = Clear();
                    if (totalLines / 4 == 0)
                    {
                        switch (lines)
                        {
                            case 1: score += 40; kaching.Play(); break;
                            case 2: score += 100; kaching.Play(); break;
                            case 3: score += 300; kaching.Play(); break;
                            case 4: score += 1200; taco.Play(); break;
                        }
                    }
                    if (totalLines / 4 == 1)
                    {
                        switch (lines)
                        {
                            case 1: score += 60; kaching.Play(); break;
                            case 2: score += 150; kaching.Play(); break;
                            case 3: score += 500; kaching.Play(); break;
                            case 4: score += 1800; taco.Play(); break;
                        }
                    }
                    if (totalLines / 4 == 2)
                    {
                        switch (lines)
                        {
                            case 1: score += 80; kaching.Play(); break;
                            case 2: score += 200; kaching.Play(); break;
                            case 3: score += 600; kaching.Play(); break;
                            case 4: score += 2100; taco.Play(); break;
                        }
                    }
                    if (totalLines / 4 == 3)
                    {
                        switch (lines)
                        {
                            case 1: score += 100; kaching.Play(); break;
                            case 2: score += 250; kaching.Play(); break;
                            case 3: score += 750; kaching.Play(); break;
                            case 4: score += 2400; taco.Play(); break;
                        }
                    }
                    if (totalLines / 4 >= 4)
                    {
                        switch (lines)
                        {
                            case 1: score += 120; kaching.Play(); break;
                            case 2: score += 300; kaching.Play(); break;
                            case 3: score += 900; kaching.Play(); break;
                            case 4: score += 2800; taco.Play(); break;
                        }
                    }
                    canSave = true;
                    if (gameField.CheckCollision(newBlock, 3, 0))
                    {
                        gameOver = true;
                        enterName = true;
                        pName = "";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(lebron);
                        return;
                    }
                    else
                    {
                        Spawn();
                    }
                }
                fallTime = 0;
            }
            Save();
            BlockUpdate();
        }
        else if(gameOver&&!enterName){
            if(newKState.IsKeyDown(Keys.R)){
                for (int i = 0; i < 24; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if(gameField.field[i, j]){
                            gameField.field[i, j] = false;
                        }
                    }
                }
                gameOver = false;
                score = 0;
                totalLines = 0;
            }
        }
        if(gameOver&&enterName){
            foreach(Keys key in newKState.GetPressedKeys()){
                if(oldKstate.IsKeyUp(key)){
                    if(key>=Keys.A&&key<=Keys.Z){
                        pName+=key;
                    }
                    else if(key == Keys.Back&&pName.Length>0){
                        pName=pName.Substring(0, pName.Length-1);
                    }
                    else if(key == Keys.Enter&&pName.Length>=3){
                        LoadLeaderboard();
                        leaderboard.Add(new ScoreEntry(pName, score));
                        leaderboard = leaderboard.OrderByDescending(s=>s.Score).Take(10).ToList();
                        SaveLeaderboard();
                        enterName = false;
                    }
                }
            }
        }
        oldKstate = newKState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        if(!gameOver){
            _spriteBatch.Draw(pixel, new Rectangle(0, 0, 200, 480), Color.MediumSlateBlue);
            _spriteBatch.Draw(pixel, new Rectangle(400, 0, 200, 480), Color.MediumSlateBlue);
            newBlock.Draw(_spriteBatch);
            for (int i = 1; i < 10; i++){
                _spriteBatch.Draw(pixel, new Rectangle(20*i+200, 0, 1, 480), Color.Black);
            }
            for (int j = 1; j < 24; j++)
            {
                _spriteBatch.Draw(pixel, new Rectangle(200, 20*j, 200, 1), Color.Black);
            }
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(gameField.field[i, j]){
                        _spriteBatch.Draw(pixel, new Rectangle(20*j+200, 20*i, 20, 20), Color.MediumVioletRed);
                    }
                }
            }
            if(saveBlock!=null){
                for (int i = 0; i < saveBlock.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < saveBlock.tiles.GetLength(1); j++)
                    {
                        if(saveBlock.tiles[i, j]){
                            _spriteBatch.Draw(pixel, new Rectangle(60+j*20, 130+i*20, 20, 20), Color.Aquamarine);
                        }
                    }
                }
            }
            if(nextBlock!=null){
                for (int i = 0; i < nextBlock.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < nextBlock.tiles.GetLength(1); j++)
                    {
                        if(nextBlock.tiles[i, j]){
                            _spriteBatch.Draw(pixel, new Rectangle(460+j*20, 130+i*20, 20, 20), Color.Aquamarine);
                        }
                    }
                }
            }
            _spriteBatch.DrawString(font, "Saved Block:", new Vector2(15, 80), Color.MonoGameOrange);
            _spriteBatch.DrawString(font, "Next Block", new Vector2(415, 80), Color.MonoGameOrange);
            _spriteBatch.DrawString(font, "Total Lines: " + Convert.ToString(totalLines), new Vector2(415, 15), Color.MonoGameOrange);
            _spriteBatch.DrawString(font, "Score: " + Convert.ToString(score), new Vector2(15, 15), Color.MonoGameOrange);
            _spriteBatch.DrawString(font, "Level: " + Convert.ToString(totalLines/4), new Vector2(15,45), Color.MonoGameOrange);
            }
        else{
            _spriteBatch.DrawString(font, "GAME OVER", new Vector2(220, 60), Color.Black);
            _spriteBatch.DrawString(font, "Score:" + Convert.ToString(score), new Vector2(220, 110), Color.Black);
            _spriteBatch.DrawString(font, "Rows Cleared:" + Convert.ToString(totalLines), new Vector2(220, 160), Color.Black);
            if(enterName){
                _spriteBatch.DrawString(font, "Enter Name(Minimum 3-letters): "+ pName, new Vector2(100, 230), Color.Black);
            }
            else{
                _spriteBatch.DrawString(font, "Leaderboard", new Vector2(100, 200), Color.MonoGameOrange);

                for (int i = 0; i < leaderboard.Count; i++)
                {
                    var entry = leaderboard[i];
                    _spriteBatch.DrawString(font, $"{i+1}.{entry.Name}-{entry.Score}", new Vector2(110, 225+i*25), Color.MonoGameOrange);
                }
            }
            _spriteBatch.DrawString(font, "After Entering Your Name Press R To Restart", new Vector2(15, 15), Color.MonoGameOrange);
        }
        if (pause)
        {
            _spriteBatch.Draw(pixel, new Rectangle(0, 0, 600, 480), Color.MediumSlateBlue);
            _spriteBatch.DrawString(font, "Press Backspace", new Vector2(200, 60), Color.Black);
            LoadLeaderboard();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                var entry = leaderboard[i];
                _spriteBatch.DrawString(font, $"{i + 1}.{entry.Name}-{entry.Score}", new Vector2(110, 225 + i * 25), Color.MonoGameOrange);
            }
            _spriteBatch.DrawString(font, "Score:" + Convert.ToString(score), new Vector2(220, 110), Color.Black);
            _spriteBatch.DrawString(font, "Rows Cleared:" + Convert.ToString(totalLines), new Vector2(220, 160), Color.Black);
        }
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private void BlockUpdate(){
        newKState = Keyboard.GetState();
        if(newKState.IsKeyDown(Keys.A)&&!gameField.CheckCollision(newBlock, newBlock.X-1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.A)||newKState.IsKeyDown(Keys.Left)&&!gameField.CheckCollision(newBlock, newBlock.X-1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.Left)){
            newBlock.X--;
        }
        if(newKState.IsKeyDown(Keys.D)&&!gameField.CheckCollision(newBlock, newBlock.X+1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.D)||newKState.IsKeyDown(Keys.Right)&&!gameField.CheckCollision(newBlock, newBlock.X+1, newBlock.Y)&&oldKstate.IsKeyUp(Keys.Right)){
            newBlock.X++;
        }
        if (newKState.IsKeyDown(Keys.S) || newKState.IsKeyDown(Keys.Down))
        {
            fallSpeed = 0.04;
        }
        else if (newKState.IsKeyDown(Keys.Space) && oldKstate.IsKeyUp(Keys.Space))
        {
            for (int i = 0; i < gameField.rows - 1; i++)
            {
                if (gameField.CheckCollision(newBlock, newBlock.X, newBlock.Y + i))
                {
                    newBlock.Y += i - 1;
                    Random rng = new Random();
                    score += rng.Next(20, 50);
                    break;
                }
            }
        }
        else if (totalLines <= 4)
        {
            fallSpeed = 0.5;
        }
        else if (totalLines > 4 && totalLines <= 8)
        {
            fallSpeed = 0.35;
        }
        else if (totalLines > 8 && totalLines <= 12)
        {
            fallSpeed = 0.25;
        }
        else if (totalLines > 12 && totalLines <= 16)
        {
            fallSpeed = 0.2;
        }
        else if (totalLines > 16 && totalLines <= 20)
        {
            fallSpeed = 0.15;
        }
        else if (totalLines > 20 && totalLines <= 24)
        {
            fallSpeed = 0.1;
        }
        else if (totalLines > 24 && totalLines <= 28)
        {
            fallSpeed = 0.08;
        }
        else if (totalLines > 28 && totalLines <= 32)
        {
            fallSpeed = 0.06;
        }
        else if (totalLines > 32)
        {
            fallSpeed = 0.04;
        }

        if (newKState.IsKeyDown(Keys.W) && oldKstate.IsKeyUp(Keys.W) || newKState.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
        {
            bool[,] rotatedTiles = newBlock.Rotate();
            if (!gameField.CheckCollisionRotate(rotatedTiles, newBlock.X, newBlock.Y))
            {
                newBlock.tiles = rotatedTiles;
            }
        }
    }


    private void Spawn(){
        if(nextBlock == null){
            newBlock = new Block(pixel, Block.RandomType());
            nextBlock = new Block(pixel, Block.RandomType());
        }
        else{
            newBlock = nextBlock;
            newBlock.X = 4;
            newBlock.Y = 0;

            nextBlock = new Block(pixel, Block.RandomType());
        }
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
        totalLines += clears;
        return clears;
    }

    private void Save(){
        if(newKState.IsKeyDown(Keys.C)&&oldKstate.IsKeyUp(Keys.C)&&canSave){
            if(saveBlock == null){
                saveBlock = newBlock.Clone(pixel);
                Spawn();
            }

            else{
                Block temp = newBlock;
                newBlock = new Block(pixel, saveBlock.Type);
                newBlock.X = 3;
                newBlock.Y = 0;
                saveBlock = new Block(pixel, temp.Type);
            }
            canSave = false;
        }
    }

    private void LoadLeaderboard(){
        leaderboard.Clear();

        if(File.Exists(leaderboardFile)){
            foreach(var line in File.ReadAllLines(leaderboardFile)){
                var parts = line.Split(",");
                if(parts.Length == 2&&int.TryParse(parts[1], out int score)){
                    leaderboard.Add(new ScoreEntry(parts[0], score));
                }
            }
        }
    }

    private void SaveLeaderboard(){
        File.WriteAllLines(leaderboardFile, leaderboard.Select(entry => $"{entry.Name},{entry.Score}"));
    }
}