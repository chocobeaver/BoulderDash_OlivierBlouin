// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash





using Display;
using Map;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.ComponentModel;

namespace Hero
{
    class Personnage:Displayable
    {

        public Personnage(string fichierImage,RenderWindow Screen, Coord XY) : base(fichierImage,Screen,XY)
        {
            Monney = 0;
        }

        public int Monney { get; set; }

        public void Deplacement(Map.Map map)
        {
            Direction dir = Direction.Null;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) { dir = Direction.Bas; }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.W)) { dir = Direction.Haut; }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) { dir = Direction.Gauche; }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D)) { dir = Direction.Droite; }


            Coord dirCoord = new Coord(dir);
            Coord NewXY = dirCoord.Clone().Add(XY);
            Objet RN = map.GetObjet(NewXY);
            if (RN == Objet.V || RN == Objet.T || RN == Objet.D) { XY = NewXY; map.SetObjet(XY, Objet.V); }
            if (RN == Objet.D) { Monney++; }
            if (dir == Direction.Gauche || dir == Direction.Droite)
            {
                Coord RockCoord = NewXY.Clone().Add(dirCoord);
                if (RN == Objet.R && map.IsEmpty(RockCoord)) { XY = NewXY; map.SetObjet(XY, Objet.V); map.SetObjet(RockCoord, Objet.R); }
            }


        }
    }



}
