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
using Map;
using Display;
using Enemy;

namespace Boulderdash
{
    
    class Program
    {
        //constante
        const int NbColonnes = 40;          // Nombre de cases par ligne
        const int NbLignes = 22;            // Nombre de lignes

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
        static void Main(string[] args)
        {
            //initialisation of sprites
            //initialise screen
            VideoMode mode = new VideoMode(NbColonnes * Displayable.NbPixelsParCase, NbLignes * Displayable.NbPixelsParCase);
            RenderWindow Screen = new RenderWindow(mode, "Boulder_Dash");

           
            //creation du personage
            Hero.Personnage RockFord = new Hero.Personnage("images/heros24.bmp",Screen, new Coord(3, 2));
            // On charge la carte dans map
            Map.Map map = new Map.Map(ChargerCarteJeu("Boulderdash.csv"), Screen, RockFord);
            //creation enemy moth
            enemy Moth = new Moth(new Coord(2, 15),Screen,map);
            //creation enemy mole
            enemy Mole = new Mole(new Coord(16,10), Screen,map);
            
            
            //initialise lost
            Texture lostTexture = new Texture("images/perdu24.bmp");
            Sprite lostSprite = new Sprite(lostTexture);
            lostSprite.Position=new Vector2f(14*Displayable.NbPixelsParCase, 9*Displayable.NbPixelsParCase);
            

            //loop to show screen
            while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
               
                //display the new screen 
                Screen.DispatchEvents();
                //clear the old sprite to make new one
                Screen.Clear();

                map.Afficher(); 

                //Maybe function

                
                if (map.GameContinue) { RockFord.Deplacement(map); }
                
              
                if (!map.GameContinue) { Screen.Draw(lostSprite); }
                //moth
                if (map.GameContinue) { Moth.Afficher(); }
                //mole
                if (map.GameContinue) { Mole.Afficher(); }
                //RockFord
                if (map.GameContinue) { RockFord.Afficher(); }
                //show on screen
                
                Screen.Display();

                //Console.WriteLine(RockFord.Monney);
                Thread.Sleep(100);

            }
        }
  
    }
}
