using GameLogic;

namespace GameCharacter
{
    public class Warrior : Job
    {
        public Warrior()
        {
            level = 1;
            chad = "전사";
            atk = 10;
            def = 5;
            health = 100;
            gold = 1500;
        }

    }

    public class Wizard : Job
    {
        public Wizard()
        {
            level = 1;
            chad = "마법사";
            atk = 10;
            def = 5;
            health = 100;
            gold = 1500;
        }
    }
}
