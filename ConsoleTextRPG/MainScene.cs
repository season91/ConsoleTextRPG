using BattleSystem;
using GameQuest;
using GameService;
using Inventory;
using Manager;
using Recovery;
using SaveWindow;
using Shop;
using StartScene;
using Status;

public static class MainScenes
{
    #region GameStart에 대해서
    //txt파일에 있는 내용 모두 출력하기 Mathod.PrintTextFIle("파일 이름");
    //txt파일에 있는 내용 배열로 가져오기 Mathod.LoadAllText("파일 이름");

    //게임 변수 저장하기(int일 경우) / 데이터 이름과 변수 입력. => gameData.integer.Add("데이터 이름", 변수);
    //게임 변수 불러오기(float일 경우) / 불러올 데이터 이름 입력. => float value = gameData.floating.GetData("데이터 이름");
    //위 주석이 이해가 잘 안되신다면 GameService.cs를 참고하세요~

    //해당 GameStart.cs는 Main에 동작하게 구현하는거 제외하고는 메서드를 만들거나 클래스를 만드시면 안됩니다~ (다른 분들을 위해)
    #endregion

    static void Main()
    {
        int input = 0;

        CsvData.ItemTable(); // ItemPooling에 들어가짐
        StartScenes.LoadScene();
        StartScenes.ShowStartText();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Mathod.MenuFont("1", "상태 보기\n", ColorCode.None);
            Mathod.MenuFont("2", "인벤토리\n", ColorCode.None);
            Mathod.MenuFont("3", "상점\n", ColorCode.None);
            Mathod.MenuFont("4", "전투 시작", ColorCode.Red);

            Mathod.ChangeFontColor(ColorCode.DarkGray);
            Console.WriteLine($" 현재 ({GameManager.DungeonFloor}층)");

            Mathod.MenuFont("5", "회복 아이템\n", ColorCode.Yellow);
            Mathod.MenuFont("6", "퀘스트\n", ColorCode.None);
            Mathod.MenuFont("7", "저장하기\n", ColorCode.Green);


            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            //선택지
            if (Mathod.CheckInput(out input))
            {
                switch (input)
                {
                    //상태보기
                    case 1:
                        StatusScene.ShowStatus();
                        break;

                    //인벤토리
                    case 2:
                        Inventorys.Show();
                        break;

                    //상점
                    case 3:
                        Shops.Show();
                        break;

                    //전투시작
                    case 4:
                        BattleSystems.Start();
                        break;

                    //회복아이템
                    case 5:
                        RecoveryItem.Show();
                        break;

                    //퀘스트
                    case 6:
                        QuestScene.ShowList();
                        break;

                    case 7:
                        //저장
                        SaveWindows.Show();
                        break;

                    default:
                        Console.WriteLine("\n1~7 사이의 숫자를 입력해주세요.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
    }
}