using System;
using System.Collections.Generic;
namespace riverCrossing
{
    static class GameLogic
    {
        public static void move(int[] inputArr, ref List<Human> humans, ref Position boatPosition, ref Position targetPosition)
        {
            if (inputArr.Length != 2)
            { throw new Exception("Number of inputs is not 2"); }
            humans[inputArr[0]].position = targetPosition;
            humans[inputArr[1]].position = targetPosition;
            var temp = boatPosition;
            boatPosition = targetPosition;
            targetPosition = temp;
        }

        public static int[] getInput(Position boatPosition, List<Human> humans)
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
        public static bool GameIsEnd(List<Human> humans)
        {
            // Check for fail condition
            if (!isValidStateAt(Position.Source, humans) ||
                !isValidStateAt(Position.Destination, humans))
            {
                // Invalid state (Either one of the fail conditions)
                Console.WriteLine("You failed. Game Over");
                return true;
            }
            else // Previous if already checked for fail condition so no need check
            if (humans.TrueForAll(x => x.position == Position.Destination))
            {
                Console.WriteLine("Success. You win!");
                return true;
            }
            else { return false; }
        }
        private static bool isValidStateAt(Position position, List<Human> humans)
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