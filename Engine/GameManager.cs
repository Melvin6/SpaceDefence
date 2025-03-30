using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class GameManager
    {
        private static GameManager gameManager;
        private List<GameObject> _gameObjects;
        private List<GameObject> _hubObjects;
        private List<GameObject> _toBeRemoved;
        private List<GameObject> _toBeAdded;
        public ContentManager _content {get; private set;}
        private GameState _state;
        private Menu _startMenu;
        private Menu _pauseMenu;
        private float _delay = -1f;

        public Level level { get; private set; }
        public Random RNG { get; private set; }
        public Ship Player { get; private set; }
        public InputManager InputManager { get; private set; }
        public Game Game { get; private set; }

        public int score {get; private set; }


        public static GameManager GetGameManager()
        {
            if(gameManager == null)
                gameManager = new GameManager();
            return gameManager;
        }
        public GameManager()
        {
            _gameObjects = new List<GameObject>();
            _hubObjects = new List<GameObject>();
            _toBeRemoved = new List<GameObject>();
            _toBeAdded = new List<GameObject>();
            _state = GameState.StartMenu;
            InputManager = new InputManager();
            RNG = new Random();

            _startMenu = new Menu(
                new List<string> { "Start Game", "Exit" },
                new List<Menu.MenuAction> { StartGame, ExitGame },
                new Vector2(300, 200)
            );

            _pauseMenu = new Menu(
                new List<string> { "Continue", "Quit" },
                new List<Menu.MenuAction> { ResumeGame, ExitGame },
                new Vector2(300, 200)
            );

            _hubObjects.Add(new Score(new Vector2(50, 30)));
            _hubObjects.Add(new CargoHud(new Vector2(150, 30)));
        }

        public void Initialize(ContentManager content, Game game, Ship player)
        {
            Game = game;
            _content = content;
            Player = player;
            AddGameObject(_startMenu);
            level = new Level1();
        }

        public void Load(ContentManager content)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Load(content);
            }

            foreach (GameObject gameObject in _hubObjects)
            {
                gameObject.Load(content);
            }
        }

        public void HandleInput(InputManager inputManager)
        {
            if (inputManager.IsKeyPress(Keys.Escape))
            {
                TogglePause();
            }

            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.HandleInput(this.InputManager);
            }

            foreach (GameObject hubObjects in _hubObjects)
            {
                hubObjects.HandleInput(this.InputManager);
            }
        }

        public void CheckCollision()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                for (int j = i+1; j < _gameObjects.Count; j++)
                {
                    if (_gameObjects[i].CheckCollision(_gameObjects[j]))
                    {
                        _gameObjects[i].OnCollision(_gameObjects[j]);
                        _gameObjects[j].OnCollision(_gameObjects[i]);
                    }
                }
            }
            
        }
        
        public void Update(GameTime gameTime) 
        {
            InputManager.Update();

            HandleInput(InputManager);

            if (_delay > 0)
            {
                _delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_delay <= 0)
                    RemoveGameOver();
            }

            switch(_state) {
                case GameState.StartMenu:
                UpdateStart(gameTime);
                break;

                case GameState.GameOver:
                UpdateGameOver(gameTime);
                break;

                case GameState.Running:
                CheckCollision();
                UpdateGame(gameTime);
                break;

                case GameState.Paused:
                UpdatePaused(gameTime);
                break;
            }
        }

        public void UpdateGame(GameTime gameTime)
        {
            // Update
            if (_state == GameState.Running)
            {
                foreach (GameObject gameObject in _gameObjects)
                {
                    gameObject.Update(gameTime);
                }
            }

            foreach (GameObject gameObject in _toBeAdded)
            {
                gameObject.Load(_content);
                if (gameObject is Menu) 
                    _hubObjects.Add(gameObject);
                else
                    _gameObjects.Add(gameObject);
            }
            _toBeAdded.Clear();

            foreach (GameObject gameObject in _toBeRemoved)
            {
                gameObject.Destroy();
                if (gameObject is Menu)
                    _hubObjects.Remove(gameObject);
                else
                    _gameObjects.Remove(gameObject);
            }
            _toBeRemoved.Clear();

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin(transformMatrix: Player?.Camera != null ? Player.Camera.Transform : Matrix.Identity);
            level.Draw(spriteBatch);
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            foreach (GameObject hubObject in _hubObjects)
            {
                hubObject.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }

        public void UpdateGameOver(GameTime gameTime)
        {
            UpdateGame(gameTime);
        }

        public void UpdateStart(GameTime gameTime)
        {
            UpdateGame(gameTime);
        }

        public void UpdatePaused(GameTime gameTime)
        {
            UpdateGame(gameTime);
        }

        private void StartGameObjects()
        {
            AddGameObject(Player);
            level.Start();
        }

        public void GameOver()
        {
            _delay = 3f;
            _state = GameState.GameOver;
        }

        private void RemoveGameOver()
        {
            AddScore(-score);
            Player.ResetCargo();
            UnloadObjects(typeof(Ship));
            UnloadListObjects(level.End());
            AddGameObject(_startMenu);
        }

        private void StartGame()
        {
            _state = GameState.Running;
            RemoveGameObject(_startMenu);
            StartGameObjects();
        }

        private void ExitGame()
        {
            Game.Exit();
        }

        private void ResumeGame()
        {
            _state = GameState.Running;
            RemoveGameObject(_pauseMenu);
        }

        private void TogglePause()
        {
            if (_state == GameState.Running)
            {
                AddGameObject(_pauseMenu);
                _state = GameState.Paused;
            }
            else if (_state == GameState.Paused)
            {
                ResumeGame();
            }
        }

        private void UnloadListObjects(List<GameObject> listObjects)
        {
            foreach(GameObject gameObject in listObjects){
                if(gameObject is Spawner spawner)
                    UnloadObjects(spawner._type);

                RemoveGameObject(gameObject);
            }
        }

        private void UnloadObjects(Type type)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                if(gameObject.GetType() == type)
                    RemoveGameObject(gameObject);
            }
        }

        public GameState GetState()
        {
            return _state;
        }

        public void AddScore(int number)
        {
            score += number;
            Console.WriteLine(score);
        }


        /// <summary>
        /// Add a new GameObject to the GameManager. 
        /// The GameObject will be added at the start of the next Update step. 
        /// Once it is added, the GameManager will ensure all steps of the game loop will be called on the object automatically. 
        /// </summary>
        /// <param name="gameObject"> The GameObject to add. </param>
        public void AddGameObject(GameObject gameObject)
        {
            _toBeAdded.Add(gameObject);
        }

        /// <summary>
        /// Remove GameObject from the GameManager. 
        /// The GameObject will be removed at the start of the next Update step and its Destroy() mehtod will be called.
        /// After that the object will no longer receive any updates.
        /// </summary>
        /// <param name="gameObject"> The GameObject to Remove. </param>
        public void RemoveGameObject(GameObject gameObject)
        {
            _toBeRemoved.Add(gameObject);
        }

        /// <summary>
        /// Get a random location on the screen.
        /// </summary>
        public Vector2 RandomScreenLocation()
        {
            return new Vector2(
                RNG.Next(level.Bounds.Left, level.Bounds.Right),
                RNG.Next(level.Bounds.Top, level.Bounds.Bottom));
        }

    }
}

public enum GameState
{
    StartMenu,
    Running,
    Paused,
    GameOver
}

