using System;
using System.Collections.Generic;
using System.Text;

namespace riverCrossing
{
    enum HumanType { Dad = 1, Mum, Son, Daughter, Policeman, Criminal }
    enum Position { Destination = 1, Source }
    class Human
    {
        public bool canRowBoat()
        {
            if (this.type == HumanType.Dad ||
            this.type == HumanType.Mum ||
            this.type == HumanType.Policeman)
            { return true; }
            else { return false; }
        }
        public Human(HumanType type)
        {
            this.type = type;
            this.position = Position.Source;
        }
        public HumanType type;
        public Position position;
    }
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
            {
                human.position = Position.Source;
            }
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
                int[] input = getInput(boatPosition);
                move(input);
                // Check for fail condition
                if (!isValidStateAt(Position.Source) ||
                    !isValidStateAt(Position.Destination))
                {
                    // Invalid state (Either one of the fail conditions)
                    Console.WriteLine("You failed. Game Over");
                    GameReset();
                }
                // Previous if already checked for fail condition so no need check
                else if (humans.TrueForAll(x => x.position == Position.Destination))
                {
                    Console.WriteLine("Success. You win!");
                    GameReset();
                }
            }
        }
        void move(int[] inputArr)
        {
            if (inputArr.Length != 2)
            { throw new Exception("Number of inputs is not 2"); }
            humans[inputArr[0]].position = targetPosition;
            humans[inputArr[1]].position = targetPosition;
            var temp = boatPosition;
            boatPosition = targetPosition;
            targetPosition = temp;
        }

        int[] getInput(Position boatPosition)
        {
            List<int> result = new List<int>(2);
            Console.WriteLine(@"Select humans by keying their number, followed by the ""Enter"" key, once for each human. Type ""go"" to send current selected human alone.");
            while (result.Count < 2)
            {
                string input = Console.ReadLine();
                int index;
                if (string.Equals(input, "go"))
                {
                    if (result.Count == 1) { break; }
                    else { Console.WriteLine("You can only \"go\" with at least one human selected"); }
                }
                if (!Int32.TryParse(input, out index))
                { Console.WriteLine("Please choose a number between 0 - 7 or \"go\" to send with one human selected"); }
                else if (index > 7 || index < 0)
                { Console.WriteLine("Please choose a number between 0 - 7"); }
                else if (humans[index].position != boatPosition)
                { Console.WriteLine($"Please choose humans that are on the {boatPosition.ToString()} side"); }
                else if (result.Count == 1 && result[0] == index)
                { Console.WriteLine("Please choose another human that is not chosen"); }
                else if (result.Count == 1 && !(humans[result[0]].canRowBoat() || humans[index].canRowBoat()))
                { result.Clear(); Console.WriteLine("Please choose a combination with an adult (Mom, Dad, Policeman)"); }
                else { result.Add(index); }
            }
            return result.ToArray();
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
        private bool isValidStateAt(Position position)
        {
            List<Human> humansAtPosition = humans.FindAll(x => x.position == position);
            // Dad with Daughter without Mum
            if (humansAtPosition.Exists(x => x.type == HumanType.Dad)
            && humansAtPosition.Exists(x => x.type == HumanType.Daughter)
            && !humansAtPosition.Exists(x => x.type == HumanType.Mum))
            { return false; }
            // Mum with Son without Dad
            if (humansAtPosition.Exists(x => x.type == HumanType.Mum)
            && humansAtPosition.Exists(x => x.type == HumanType.Son)
            && !humansAtPosition.Exists(x => x.type == HumanType.Dad))
            { return false; }
            if (humansAtPosition.Exists(x => x.type == HumanType.Criminal)
            && !humansAtPosition.Exists(x => x.type == HumanType.Policeman)
            && humansAtPosition.Exists(x => x.type != HumanType.Policeman && x.type != HumanType.Criminal))
            { return false; }
            return true;
        }
    }
}
