// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash



using System;
using System.Threading;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Boulderdash
{
    
    class Program
    {
        //constante
        const int NbColonnes = 40;          // Nombre de cases par ligne
        const int NbLignes = 22;            // Nombre de lignes
        const int NbPixelsParCase = 24;     // Nombre de pixels d'une case
        const int MothSpeed = 20;           //le nombre de fois que le papillon prend de tour de boucle pour se deplacer
        int tour = 0;                       //le tour a lequel on est rendu pour introduire un futur timer



        // Représente ce que une case du tableau peut etre 
        // {Vide, Mur, Terre, Rocher, RocherTombant, Diamant}
        enum Objet { V, M, T, R, RT, D };

        // Représente les directions de déplacement de l'ennemi ==> **maybe other
        enum Direction { Gauche, Haut, Droite, Bas };

        class Cave
        {
            public Cave(string fichierImage)
            { 
                T = new Texture(fichierImage);
                S = new Sprite(T);
            }
            public void Afficher(RenderWindow Screen, float nbPixelsParCase,int X,int Y)
            {
                
                Vector2f position = new Vector2f(X * nbPixelsParCase, Y * nbPixelsParCase);
                S.Position = position;
                Screen.Draw(S);
            }

            
            public Sprite S { get; set; }
            public Texture T { get; set; }
        }



        class enemy
        {
            public enemy(string fichierImage, int x, int y, Direction dir)
            {
                X = x;
                Y = y;
                D = dir;
                T = new Texture(fichierImage);
                S = new Sprite(T);
              
            }

            private void Deplacement(Objet[,] carte, int tour)
            {
                

                if (tour%MothSpeed==0)
                {
                    Console.WriteLine(D);
                    if (D == Direction.Haut)
                    {
                        if (carte[Y - 1, X] == Objet.V) { Y--; }
                        else if (carte[Y, X + 1] == Objet.V) { X++; }
                        else if (carte[Y + 1, X] == Objet.V) { Y++; D = Direction.Droite; }
                    }
                    else if (D == Direction.Droite)
                    {
                        if (carte[Y, X + 1] == Objet.V) { X++; }
                        else if (carte[Y + 1, X] == Objet.V) { Y++; }
                        else if (carte[Y, X - 1] == Objet.V) { X--; D = Direction.Bas; }
                    }
                    else if (D == Direction.Bas)
                    {
                        if (carte[Y + 1, X] == Objet.V) { Y++; }
                        else if (carte[Y, X - 1] == Objet.V) { X--; }
                        else if (carte[Y - 1, X] == Objet.V) { Y--; D = Direction.Gauche; }
                    }
                    else if (D == Direction.Gauche)
                    {
                        if (carte[Y, X - 1] == Objet.V) { X--; }
                        else if (carte[Y - 1, X] == Objet.V) { Y--; }
                        else if (carte[Y, X + 1] == Objet.V) { X--; D = Direction.Haut; }
                    }
                    
                }
                
            }
            public void Afficher(RenderWindow Screen, float nbPixelsParCase, Objet[,] carte,int tour)
            {
                Deplacement(carte, tour);
                Vector2f position = new Vector2f(X * nbPixelsParCase, Y * nbPixelsParCase);
                S.Position = position;
                Screen.Draw(S);
            }

            public Direction D { get; set; }
            public int X { get; set; }
            public Sprite S { get; set; }

            public int Y { get; set; }
            public Texture T { get; set; }
        }
        /// <summary>
        /// La classe Personnage sert à créer Boulderdash et son ennemi ==> ** may change
        /// </summary>
        class Personnage
        {
            /// <summary>
            /// Constructeur du personnage 
            /// </summary>
            /// <param name="fichierImage">Nom du fichier image</param>
            /// <param name="x">Position initiale en x</param>
            /// <param name="y">Position initiale en y</param>
            /// <param name="dir">Direction intiale</param>
            public Personnage(string fichierImage, int x, int y)
            {
                X = x;
                Y = y;
               
                Texture = new Texture(fichierImage);
                Sprite = new Sprite(Texture);
                int monney = 0;
            }

            /// <summary>
            /// Utilisez la méthode Afficher du personnage pour afficher le personnage
            /// dans une fenêtre
            /// </summary>
            /// <param name="Screen">Objet représentant la fenêtre</param>
            /// <param name="nbPixelsParCase">Facteur multiplicatif permettant de passer de la position
            /// dans le tableau 2D à la position dans l'image</param>
            public void Afficher(RenderWindow Screen, float nbPixelsParCase)
            {
                Vector2f position = new Vector2f(X * nbPixelsParCase, Y * nbPixelsParCase);
                Sprite.Position = position;
                Screen.Draw(Sprite);
            }
            public int Monney { get; set; }
            /// <value>
            /// Utilisez la propriété X afin de modifier la position horizontale
            /// </value>     
            public int X { get; set; }
            /// <value>
            /// Utilisez la propriété Y afin de modifier la position verticale
            /// </value>     
            public int Y { get; set; }
            /// <value>
            /// Utilisez la propriété Direction afin de conserver la direction du personnage
            /// </value>   
            public void Deplacement(Objet[,] carte, Keyboard.Key touch, Personnage RockFord)
            {
                if (touch == (Keyboard.Key.S))
                {

                    if (carte[Y + 1,X] == Objet.V) {Y++; }
                    else if (carte[Y + 1,X] == Objet.T) { Y++; carte[Y, X] = Objet.V; }
                    else if (carte[Y + 1,X] == Objet.D) { Y++; carte[Y, X] = Objet.V; Monney++; }

                }
                else if (touch == (Keyboard.Key.W))
                {
                    if (carte[Y - 1,X] == Objet.V) { Y--; }
                    else if (carte[Y - 1,X] == Objet.T) {Y--; carte[Y,X] = Objet.V; }
                    else if (carte[Y - 1, X] == Objet.D) {Y--; carte[Y,X] = Objet.V; Monney++; }
                }
                else if (touch == (Keyboard.Key.A))
                {
                    if (carte[Y,X - 1] == Objet.V) {X--; }
                    else if (carte[Y,X - 1] == Objet.T) {X--; carte[Y,X] = Objet.V; }
                    else if (carte[Y,X - 1] == Objet.D) {X--; carte[Y,X] = Objet.V;Monney++; }
                    else if (carte[Y,X - 1] == Objet.R && carte[Y,X - 2] == Objet.V) { X--; carte[Y, X] = Objet.V; carte[Y,X - 1] = Objet.R; }
                }
                if (touch == (Keyboard.Key.D))
                {
                    if (carte[Y,X + 1] == Objet.V) {X++; }
                    else if (carte[Y, X + 1] == Objet.T) { X++; carte[Y,X] = Objet.V; }
                    else if (carte[Y, X + 1] == Objet.D) {X++; carte[Y,X] = Objet.V; Monney++; }
                    else if (carte[Y, X + 1] == Objet.R && carte[Y, X + 2] == Objet.V) { X++; carte[Y,X] = Objet.V; carte[Y,X + 1] = Objet.R; }
                }

            }
            public Direction Direction { get; set; }

            #region
            private Texture Texture { get; set; }
            private Sprite Sprite { get; set; }
                      
            #endregion
        }
        
        #region
        static Objet[,] ChargerCarteJeu(string map)
        {
            Objet[,] carte = new Objet[NbLignes, NbColonnes];
            using (StreamReader reader = new StreamReader(map))

            {
                for (int indiceLigne = 0; indiceLigne < NbLignes; indiceLigne++)
                {
                    string line = reader.ReadLine();
                    for (int indiceColonne = 0; indiceColonne < NbColonnes; indiceColonne++)
                    {
                        string[] values = line.Split(',');
                        carte[indiceLigne, indiceColonne] = (Objet)Enum.Parse(typeof(Objet), values[indiceColonne]);
                    }
                }

            }
            return carte;
        }
        #endregion
       static void TestMap(Objet[,] carte)
       {
            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    Console.Write(carte[i, j]);
                }
                Console.WriteLine();
            }
        }
       static void showMap(Objet[,] carte, RenderWindow Screen, Personnage RockFord, ref bool end,Cave  Wall,Cave Ground,Cave Rock,Cave Diamond)
        {

            bool test = true;
            //loop to draw every map part
            for (int i = 0; i < carte.GetLength(0); i++)
            {
                for (int j = 0; j < carte.GetLength(1); j++)
                {
                    //function maybe
                    if (carte[i, j] == Objet.M)
                    {
                        Wall.Afficher(Screen, NbPixelsParCase, j, i);
                    }

                    if (carte[i, j] == Objet.T)
                    {
                        Ground.Afficher(Screen, NbPixelsParCase, j, i);
                    }

                    if (carte[i, j] == Objet.R)
                    {
                        if (carte[i + 1, j] == Objet.V && !(RockFord.X == j && RockFord.Y == i + 1))
                        {
                            carte[i, j] = Objet.RT;
                            Rock.Afficher(Screen,NbPixelsParCase, j, i);
                        }
                        else if (carte[i + 1, j] == Objet.R && (carte[i + 1, j - 1] == Objet.V && carte[i, j - 1] == Objet.V))
                        {
                            carte[i, j - 1] = Objet.RT;
                            carte[i, j] = Objet.V;

                        }
                        else if (carte[i + 1, j] == Objet.R && (carte[i + 1, j + 1] == Objet.V && carte[i, j + 1] == Objet.V))
                        {

                            carte[i, j + 1] = Objet.RT;
                            carte[i, j] = Objet.V;
                        }
                        else
                        {
                            Rock.Afficher(Screen,NbPixelsParCase, j, i);
                        }
                    }

                    if (carte[i, j] == Objet.D)
                    {
                        Diamond.Afficher(Screen,NbPixelsParCase,j, i);
                    }
                    if (carte[i, j] == Objet.RT && test)
                    {
                        test = false;
                        if (carte[i + 1, j] == Objet.V)
                        {
                            if (i + 1 == RockFord.Y && j == RockFord.X)
                            {
                                end = false;
                                carte[i, j] = Objet.V;
                                carte[i + 1, j] = Objet.RT;

                            }
                            else
                            {
                                carte[i, j] = Objet.V;
                                carte[i + 1, j] = Objet.RT;
                            }
                        }
                        else
                        {
                            carte[i, j] = Objet.R;
                        }
                        Rock.Afficher(Screen,NbPixelsParCase,j, i);
                    }
                }
            }
        }

        
        static void Main(string[] args)
        {
            int tour=0;
            bool end = true;
            //creation du personage
            Personnage RockFord = new Personnage("images/heros24.bmp", 3,2);
            //creation du personage
            enemy Moth = new enemy("images/papillon24.bmp", 2, 15, Direction.Gauche);
            //creating the grid
            Objet[,] carte = ChargerCarteJeu("Boulderdash.csv"); // On charge la carte (fonction déjà écrite, cette ligne ne devrait pas changer)

            // **************************************************************************
            //initialisation of sprites
            //initialise screen
            VideoMode mode = new VideoMode(NbColonnes*NbPixelsParCase,NbLignes*NbPixelsParCase);
            RenderWindow Screen = new RenderWindow(mode, "Boulder_Dash");

            //initialise wall    
            Cave Wall = new Cave("images/mur24.bmp");

            //initialise Ground
            Cave Ground = new Cave("images/terre24.bmp");
 
            //initialise Rock
            Cave Rock = new Cave("images/roche24.bmp");
            
            //initialise diamond
            Cave Diamond = new Cave("images/diamant24.bmp");
            
            
 
            //initialise lost
            Texture lostTexture = new Texture("images/perdu24.bmp");
            Sprite lostSprite = new Sprite(lostTexture);
            lostSprite.Position=new Vector2f(14*NbPixelsParCase, 9*NbPixelsParCase);


            //loop to show screen
            while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                bool test = true;
               
                //display the new screen 
                Screen.DispatchEvents();
                //clear the old sprite to make new one
                Screen.Clear();
                showMap(carte,Screen, RockFord,ref end,Wall,Ground,Rock,Diamond);

                //Maybe function
                if (Keyboard.IsKeyPressed(Keyboard.Key.S)&&end)
                {
                    
                    RockFord.Deplacement(carte, Keyboard.Key.S, RockFord);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.W, RockFord);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.A, RockFord);
                  
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.D, RockFord);
                }
                if (!end) { Screen.Draw(lostSprite); }
                //moth
                Moth.Afficher(Screen, NbPixelsParCase,carte,tour);
                //RockFord
                if (end) { RockFord.Afficher(Screen, NbPixelsParCase); }
                //show on screen
                tour++;
                Screen.Display();
                Console.WriteLine(RockFord.Monney);
                Thread.Sleep(100);

            }
        }
  
    }
}
