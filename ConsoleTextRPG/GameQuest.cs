using GameService;
using Manager;

namespace GameQuest
{
    public delegate void Function(); //GameService로 이동해야함

    public class Quest
    {
        public bool acceptance;  //수락 여부
        public string name { get; init; }
        public string[] info { get; init; }
        public int[] itemID { get; init; } //보상을 줄 아이템 ID
        public string[] condition { get; private set; }
        public int[] clearCount { get; private set; } //퀘스트 완료 조건 카운트
        public int[] conditionCount { get; set; } //퀘스트 완료 조건 카운트

        public Quest(int _questNumber)
        {
            var questText = Mathod.LoadAllText($"Quest({_questNumber})");
            var textLength = questText.Length - 2;

            if (questText == null)
            {
                Console.WriteLine($"{_questNumber}번은 없는 퀘스트");
                return;
            }

            name = questText[0];
            info = new string[questText.Length];

            for (int i = 0; i < textLength; i++)
            {
                info[i] = questText[i];
            }

            //조건 갯수 만큼 초기화
            textLength++;
            var conditionText = questText[textLength].Split('/');
            var conditionLength = conditionText.Length == 0 ? 1 : conditionText.Length;

            condition = new string[conditionLength];
            clearCount = new int[conditionLength];
            conditionCount = new int[conditionLength];

            for (int i = 0; i < conditionLength; i++)
            {
                var countText = conditionText[i].Split(',');
                condition[i] = countText[0];

                if (!int.TryParse(countText[1], out conditionCount[i]))
                {
                    Console.WriteLine($"{countText[1]}는 숫자가 아님");
                    break;
                }
            }
        }

        public void CheckCondition(string _objectName)
        {
            if (!acceptance) return;

            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i].Contains(_objectName)) clearCount[i]++;
            }
        }
    }

    public class QuestManager
    {
        public Quest[] data { get; private set; }

        public void CheckCondition(string _objectName)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i].CheckCondition(_objectName);
            }
        }

        public void LoadQuest()
        {
            //현재 퀘스트 텍스트 갯수
            var questCount = 1;

            data = new Quest[questCount];

            for (int i = 0; i < questCount; i++)
            {
                data[i] = new Quest(i);
            }
        }

        public void Save()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].acceptance)
                {
                    //GameManager.data.boolen.Add(data[i].name, data[i].acceptance);
                    //GameManager.data.integer.Add(data[i].name, data[i].conditionCount);
                }
            }
        }
    }

    public class QuestScene
    {
        public static void ShowQuestListScene()
        {
            var input = 0;
            var questLength = GameManager.quest.data.Length;

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
                    Console.WriteLine($"{GameManager.quest.data[i].name}");
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
                        ShowQuestScreen(--input);
                    }
                }
            }
        }

        public static void ShowQuestScreen(int _input)
        {
            var questScreen = GameManager.quest.data[_input].info;

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

                Console.WriteLine("");//보상 처리
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out _input))
                {

                }
            }
        }
    }
}
