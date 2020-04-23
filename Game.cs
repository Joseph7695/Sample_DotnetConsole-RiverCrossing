using System;
using System.Collections.Generic;
using System.Text;

namespace riverCrossing
{
    enum HumanType { Dad = 1, Mum, Son, Daughter, Policeman, Criminal }
    enum Position { Destination = 1, Source }
    class Game
    {
        private List<Human> humans = new List<Human> {
                new Human(HumanType.Dad), new Human(HumanType.Mum),
                new Human(HumanType.Son), new Human(HumanType.Son),
                new Human(HumanType.Daughter), new Human(HumanType.Daughter),
                new Human(HumanType.Policeman), new Human(HumanType.Criminal)
            };
        Position boatPosition = Position.Source;
        Position targetPosition = Position.Destination;
        void GameReset()
        {
            Console.WriteLine("Resetting Game...");
            boatPosition = Position.Source;
            targetPosition = Position.Destination;
            foreach (var human in humans)
            { human.position = Position.Source; }
        }
        public void Run()
        {
            Console.WriteLine("\nWelcome to River Crossing!");
            // Infinite game loop
            while (true)
            {
                // Print current state
                Console.WriteLine("\nHumans at the source: ");
                PrintHumansAt(Position.Source);
                Console.WriteLine("\nHumans at the destination: ");
                PrintHumansAt(Position.Destination);
                int[] input = GameLogic.getInput(boatPosition, humans);
                GameLogic.move(input, ref humans, ref boatPosition, ref targetPosition);
                if (GameLogic.GameIsEnd(humans))
                { GameReset(); }
            }
        }

        private void PrintHumansAt(Position position)
        {
            StringBuilder humansString = new StringBuilder();
            for (int i = 0; i < humans.Count; i++)
            {
                var human = humans[i];
                if (human.position == position)
                { humansString.Append($"{i}:{human.type.ToString()} "); }
            }
            if (humansString.Length == 0)
            { Console.WriteLine("None"); }
            else
            { Console.WriteLine(humansString.ToString()); }
        }
    }
}
