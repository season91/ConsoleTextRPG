using GameCharacter;
using GameService;
using MainScene;
using StartScene;

public static class GameStart
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
        var gameData = new GameData();
        int input = 0;

        var nickName = StartScenes.ShowStartScene();
        var player = StartScenes.SelectJob();

        player.SetName(nickName);
        StartScenes.StartGame(player);

        while (true)
        {
            MainScenes.ShowMainScene(player);
        }
    }
}