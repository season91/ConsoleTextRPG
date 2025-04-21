using GameLogic;

namespace GameQuest
{
    public delegate void Function(); //GameService로 이동해야함

    public struct Quest
    {
        public string name;
        public string[] info;
    }

    public class QuestManager
    {
        public Quest[] quests;

        public void Awake()
        {

        }

        public void CheckClear(Monster _monster)
        {

        }

        public void CheckClear(Item _item)
        {

        }
    }

    public class Monster
    {
        //test용 몬스터 클래스
        public string name;
    }
}
