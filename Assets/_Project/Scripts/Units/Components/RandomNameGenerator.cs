using System.Collections.Generic;

namespace Units.Components
{
    public static class RandomNameGenerator
    {
        private static readonly string[] names = new string[]
        {   "Alice", "Bob", "Charlie", "Dave", "Eve",
            "Frank", "Grace", "Heidi", "Ivan", "Judy",
            "Kevin", "Liam", "Mia", "Nina", "Olivia",
            "Pete", "Quinn", "Randy", "Steve", "Tina",
            "Ursula", "Victor", "Wendy", "Xander", "Yvonne", "Zack" };


        private static readonly List<string> usedName = new List<string>();

        public static string GetRandomName() {
            var name = names[UnityEngine.Random.Range(0, names.Length)];
            name = CheckUsedName(name, usedName);
            usedName.Add(name);

            return name;
        }

        public static void CleanUsedName() =>
            usedName.Clear();
        

        private static string CheckUsedName(string name, List<string> usedNameList)
        {
            if (usedNameList.Contains(name)) {
                name = name + "_" + UnityEngine.Random.Range(0, 10);
                name = CheckUsedName(name, usedNameList);
            }

            return name;
        }
    }
}
