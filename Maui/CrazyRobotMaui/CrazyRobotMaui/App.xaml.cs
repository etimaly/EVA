using CrazyRobotMod.model;
using CrazyRobotMod.Persistence;
using CrazyRobotMaui.Persistence;
using CrazyRobotMaui.ViewModel;

namespace CrazyRobotMaui;

public partial class App : Application
{
#nullable enable
    private const string SuspendedGameSavePath = "Suspended Game";

	private readonly AppShell _appShell;
	private readonly GameFromFile _gameFromFile;
	private readonly Game _gameModel;
	private readonly IStore _gameStore;
	private readonly CrazyRobotViewModel _crazyRobotViewModel;
	public App()
	{
		InitializeComponent();
		_gameStore = new CrazyRobotStore();
		_gameFromFile = new GameFromText(FileSystem.Current.AppDataDirectory);

		_gameModel = new Game(_gameFromFile);
		_crazyRobotViewModel = new CrazyRobotViewModel(_gameModel);

		_appShell = new AppShell(_gameStore, _gameFromFile, _gameModel, _crazyRobotViewModel)
		{
			BindingContext = _crazyRobotViewModel
		};

		MainPage = _appShell;
	}
protected override Window CreateWindow(IActivationState? activationState)
{
	Window window = base.CreateWindow(activationState);
    window.Created += (s, e) =>
    {
        _gameModel.NewGame(7);
        _appShell.StartTimer();
    };
    window.Activated += (s, e) =>
    {
        if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
            return;

        Task.Run(async () =>
        {

            try
            {
                await _gameModel.LoadGameAsync(SuspendedGameSavePath);

                _appShell.StartTimer();
            }
            catch
            {
            }
        });
    };

    window.Deactivated += (s, e) =>
    {
        Task.Run(async () =>
        {
            try
            {

                _appShell.StopTimer();
                await _gameModel.SaveGameAsync(SuspendedGameSavePath);
            }
            catch
            {
            }
        });
    };

    return window;
}
}
