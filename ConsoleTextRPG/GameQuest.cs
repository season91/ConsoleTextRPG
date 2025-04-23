using GameService;
using Manager;

namespace GameQuest
{
    public delegate void Function(); //GameService로 이동해야함

    public class Quest
    {
        public bool isClear { get; private set; }
        public bool acceptance { get; set; } //수락 여부
        public string[] info { get; init; } //퀘스트 정보

        public string[] itemName { get; init; } //지급할 아이템 이름
        public int[] rewardCount { get; init; } //지급할 아이템 갯수

        public int totalClear { get; private set; } //토탈 클리어 횟수
        public int[] count { get; private set; } //클리어 횟수
        public int[] maxCount { get; private set; } //클리어 조건 횟수
        public string[] condition { get; private set; } //퀘스트 조건

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

            count = new int[conditionLength];
            maxCount = new int[conditionLength];
            condition = new string[conditionLength];

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

            //보상
            textLength++;
            var itemText = questText[textLength].Split('/');
            var itemLength = itemText.Length == 0 ? 1 : itemText.Length;

            itemName = new string[itemLength];
            rewardCount = new int[itemLength];

            for (int i = 0; i < itemLength; i++)
            {
                var countText = itemText[i].Split(',');
                itemName[i] = countText[0];

                if (!int.TryParse(countText[1], out rewardCount[i]))
                {
                    Console.WriteLine($"{countText[1]}는 숫자가 아님");
                    break;
                }
            }
        }

        public void CheckCondition(string _objectName)
        {
            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i] == _objectName)
                {
                    if(count[i] < maxCount[i]) count[i]++;
                    else if (count[i] == maxCount[i]) totalClear++;
                }
            }
        }

        public bool CheckClear()
        {
            if (totalClear >= maxCount.Length) return true;
            else return false;
        }

        public void Save(int _index)
        {
            _index++;
            GameManager.data.boolen.Add($"Quest{_index}clear", isClear);
            GameManager.data.boolen.Add($"Quest{_index}accep", acceptance);
            GameManager.data.integer.Add($"Quest{_index}total", totalClear);

            for (int i = 0; i < count.Length; i++)
            {
                GameManager.data.integer.Add($"Quest({_index})count({i})", count[i]);
            }
        }

        public void Load(int _index)
        {
            isClear = GameManager.data.boolen.GetData($"Quest{_index}clear");
            acceptance = GameManager.data.boolen.GetData($"Quest{_index}accep");
            totalClear = GameManager.data.integer.GetData($"Quest{_index}total");

            for (int i = 0; i < count.Length; i++)
            {
                count[i] = GameManager.data.integer.GetData($"Quest({_index})count({i})");
            }
        }

        public void QuestClear() => isClear = true;
    }

    public class QuestManager
    {
        public Quest[] data { get; private set; }

        public void CheckCondition(string _objectName)
        {
            //event로 해야하지만 일단 그냥..
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].acceptance && data[i].totalClear < data[i].maxCount.Length)
                {
                    data[i].CheckCondition(_objectName);
                }
            }
        }

        public void Save()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i].Save(i);
            }
        }

        public void Load(bool _isLoad)
        {
            //현재 퀘스트 텍스트 갯수
            var questCount = 10;

            data = new Quest[questCount];

            for (int i = 0; i < questCount; i++)
            {
                data[i] = new Quest(i + 1);
                if (_isLoad) data[i].Load(i + 1);
            }
        }
    }

    public class QuestScene
    {
        private static string CheckState(Quest _quest)
        {
            if (_quest.acceptance)
            {
                if (_quest.isClear) return "";
                else if (_quest.CheckClear()) return " -완료 가능-";
                else return " -진행 중-";
            }

            else return "";
        }

        public static void ShowList()
        {
            var input = 0;
            var quest = GameManager.quest.data;
            var questLength = quest.Length;

            while (true)
            {
                Console.Clear();
                Mathod.FontColorOnce("Quest!!\n\n", ColorCode.Yellow);

                for (int i = 0; i < questLength; i++)
                {
                    //넘버링 색
                    Mathod.FontColorOnce($"{i + 1}. ", ColorCode.Magenta);

                    //퀘스트 이름
                    var text = quest[i].info[0];
                    Mathod.ChangeFontColor(quest[i].isClear ? ColorCode.DarkGray : ColorCode.None);
                    Console.Write(text.Replace("\r", ""));
                    Mathod.ChangeFontColor(ColorCode.None);

                    //현재 진행 상황
                    var questProgress = CheckState(quest[i]);

                    Mathod.FontColorOnce($"{questProgress}\n", ColorCode.Blue);
                }

                Mathod.MenuFont("\n0", "나가기\n");

                Console.WriteLine("\n원하시는 퀘스트를 선택해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input < 0 || questLength < input)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }

                    else if (input == 0)
                    {
                        break;
                    }

                    else
                    {
                        input--;

                        if (GameManager.quest.data[input].isClear)
                        {
                            Mathod.FontColorOnce("\n이미 완료된 퀘스트입니다.", ColorCode.Red);
                            Thread.Sleep(1000);
                        }

                        else
                        {
                            ShowInfo(input);
                        }
                    }
                }
            }
        }

        private static void ShowInfo(int _input)
        {
            var length = GameManager.quest.data.Length;
            var quest = GameManager.quest.data[_input];
            var questScreen = quest.info;
            var isClear = quest.CheckClear();

            while (true)
            {
                Console.Clear();
                Mathod.FontColorOnce("Quest!!\n\n", ColorCode.Yellow);

                //퀘스트 제목
                var title = questScreen[0];
                Mathod.FontColorOnce(title.Replace("\r", ""), ColorCode.DarkGray);

                //진행 상황
                Mathod.FontColorOnce($"{CheckState(quest)}\n", ColorCode.Blue);

                //퀘스트 내용
                var infoCount = 0;

                for (int i = 1; i < questScreen.Length; i++)
                {
                    if (questScreen[i].Contains("{정보}"))
                    {
                        var text = questScreen[i];
                        var questText = text.Replace("{정보}", "");
                        Console.Write(questText.Replace("\r", ""));

                        //처치 카운트
                        Mathod.FontColorOnce($" ({quest.count[infoCount]} / {quest.maxCount[infoCount]})\n", ColorCode.Red);

                        //다음 인덱스로
                        infoCount++;
                    }

                    else Console.WriteLine(questScreen[i]);
                }

                //퀘스트 상태인가?
                if (quest.acceptance)
                {
                    //완료 했는가? (완료 X => 출력 안함)
                    if (isClear)
                    {
                        Mathod.MenuFont("1", "보상받기", ColorCode.Green);
                    }
                }

                //아닐 경우
                else Mathod.MenuFont("1", "수락하기");

                Mathod.MenuFont($"\n{(quest.acceptance ? 0 : 2)}", "돌아가기\n");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out _input))
                {
                    if (_input == 1)
                    {
                        if (!quest.acceptance)
                        {
                            quest.acceptance = true;
                        }

                        else
                        {
                            if (isClear)
                            {
                                if (CheckReward(quest))
                                {
                                    quest.QuestClear();
                                }

                                else
                                {
                                    Mathod.FontColorOnce("\n동일한 아이템을 이미 소지 중입니다.", ColorCode.Red);
                                    Thread.Sleep(1000);
                                }

                                break;
                            }

                            else
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                                Thread.Sleep(1000);
                            }
                        }
                    }

                    else if ((quest.acceptance && _input == 0) || (!quest.acceptance && _input == 2))
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private static bool CheckReward(Quest _quest)
        {
            if (_quest.itemName.Length > 1) return RewardAny(_quest);
            else return RewardGold(_quest);
        }

        private static bool RewardGold(Quest _quest)
        {
            var player = GameManager.player;
            var ItemPooling = GameManager.ItemPooling[0];

            //보상이 골드일 경우
            if (_quest.itemName[0].Contains("골드"))
            {
                player.gold += _quest.rewardCount[0];
            }

            //아이템이 있을 경우
            else
            {
                for (int i = 0; i < GameManager.ItemPooling.Length; i++)
                {
                    ItemPooling = GameManager.ItemPooling[i];

                    if (ItemPooling.name == _quest.itemName[0])
                    {
                        if (player.item.Contains(GameManager.ItemPooling[i]))
                        {
                            return false;
                        }

                        else
                        {
                            player.item.Add(ItemPooling);
                        }
                    }
                }
            }

            return true;
        }

        private static bool RewardAny(Quest _quest)
        {
            for (int i = 0; i < _quest.itemName.Length; i++)
            {
                var item = GameManager.ItemPooling[i];

                if (_quest.itemName[i] == item.name)
                {
                    if (!GameManager.player.item.Contains(item)) GameManager.player.item.Add(item);
                    else return false;
                }

                else if(_quest.itemName[i] == "골드")
                {
                    GameManager.player.gold += _quest.rewardCount[i];
                }
            }

            return true;
        }
    }
}
