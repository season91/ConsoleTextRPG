using GameService;
using Manager;

namespace GameQuest
{
    public delegate void Function(); //GameService로 이동해야함

    public class Quest
    {
        public bool acceptance { get; set; } //수락 여부
        public string[] info { get; init; } //퀘스트 정보
        //public int[] itemID { get; init; } //보상을 줄 아이템 ID
        public string[] condition { get; private set; } //퀘스트 조건
        public int totalClear { get; set; } //토탈 클리어 횟수
        public int[] count { get; private set; } //해당 조건 클리어 횟수
        public int[] maxCount { get; set; } //해당 조건 클리어 조건 횟수

        public Quest(int _questNumber)
        {
            var questText = Mathod.LoadAllText($"Quest({_questNumber})");
            var textLength = questText.Length - 2;

            if (questText == null)
            {
                Console.WriteLine($"{_questNumber}번은 없는 퀘스트");
                return;
            }

            info = new string[textLength];

            for (int i = 0; i < textLength; i++)
            {
                info[i] = questText[i];
            }

            //조건 갯수 만큼 초기화
            var conditionText = questText[textLength].Split('/');
            var conditionLength = conditionText.Length == 0 ? 1 : conditionText.Length;

            condition = new string[conditionLength];
            count = new int[conditionLength];
            maxCount = new int[conditionLength];

            //보상
            textLength++;

            for (int i = 0; i < conditionLength; i++)
            {
                var countText = conditionText[i].Split(',');
                condition[i] = countText[0];

                if (!int.TryParse(countText[1], out maxCount[i]))
                {
                    Console.WriteLine($"{countText[1]}는 숫자가 아님");
                    break;
                }
            }
        }

        public void CheckCondition(string _objectName)
        {
            if (!acceptance || totalClear >= maxCount.Length) return;

            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i].Contains(_objectName))
                {
                    count[i]++;

                    if (count[i] == maxCount[i]) totalClear++;
                }
            }
        }

        public bool CheckClear()
        {
            if (totalClear > maxCount.Length) return true;
            else return false;
        }

        public void Save(int _index)
        {
            if (!acceptance) return;

            GameManager.data.boolen.Add($"Quest{_index}accep", acceptance);
            GameManager.data.integer.Add($"Quest{_index}clear", totalClear);

            for (int i = 0; i < count.Length; i++)
            {
                GameManager.data.integer.Add($"Quest({_index})data({i})", count[i]);
            }
        }

        public void Load(int _index)
        {
            acceptance = GameManager.data.boolen.GetData($"Quest{_index}accep");

            if (acceptance)
            {
                totalClear = GameManager.data.integer.GetData($"Quest{_index}clear");

                for (int i = 0; i < count.Length; i++)
                {
                    count[i] = GameManager.data.integer.GetData($"Quest({_index})data({i})");
                }
            }
        }
    }

    public class QuestManager
    {
        public Quest[] data { get; private set; }

        public void CheckCondition(string _objectName)
        {
            //event로 해야하지만 일단 그냥..
            for (int i = 0; i < data.Length; i++)
            {
                data[i].CheckCondition(_objectName);
            }
        }

        public void Save()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].acceptance) data[i].Save(i);
            }
        }

        public void Load()
        {
            //현재 퀘스트 텍스트 갯수
            var questCount = 1;

            data = new Quest[questCount];

            for (int i = 0; i < questCount; i++)
            {
                data[i] = new Quest(i + 1);
                data[i].Load(i + 1);
            }
        }
    }

    public class QuestScene
    {
        public static void ShowList()
        {
            var input = 0;
            var quest = GameManager.quest.data;
            var questLength = quest.Length;

            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("Quest!!\n");
                Mathod.ChangeFontColor(ColorCode.None);

                for (int i = 0; i < questLength; i++)
                {
                    //넘버링 색
                    Mathod.ChangeFontColor(ColorCode.Magenta);
                    Console.Write($"{i + 1}. ");
                    Mathod.ChangeFontColor(ColorCode.None);

                    //퀘스트 이름
                    Console.WriteLine($"{quest[i].info[0]}");
                }

                Console.WriteLine("\n원하시는 퀘스트를 선택해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input < 1 || questLength < input)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }

                    else
                    {
                        ShowInfo(--input);
                    }
                }
            }
        }

        private static void ShowInfo(int _input)
        {
            var quest = GameManager.quest.data[_input];
            var questScreen = quest.info;
            var isClear = quest.CheckClear();

            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("Quest!!\n");
                Mathod.ChangeFontColor(ColorCode.None);

                for (int i = 0; i < questScreen.Length; i++)
                {
                    Console.WriteLine(questScreen[i]);
                }

                //수락시
                if (quest.acceptance)
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                }

                //거절시
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                }

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out _input))
                {

                }
            }
        }

        public static void Reward()
        {

        }
    }
}
