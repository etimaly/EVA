using CrazyRobotMod.model;
using CrazyRobotMod.Persistence;
using CrazyRobotMaui.View;
using CrazyRobotMaui.ViewModel;
namespace CrazyRobotMaui;

public partial class AppShell : Shell
{
	private GameFromFile _gameFromFile;
	private readonly Game _gameModel;
	private readonly CrazyRobotViewModel _crazyRobotViewModel;
	private readonly IDispatcherTimer _timer;

	private readonly IStore _store;
	private readonly StoredGameBrowserModel _storedGamesBrowserModel;
	private readonly StoredGameBrowserViewModel _storedGamesBrowserViewModel;
	public AppShell(IStore gameStore, GameFromFile gameFromFile, Game gameModel, CrazyRobotViewModel crazyRobotViewModel)
	{
		InitializeComponent();

		_store = gameStore;
		_gameFromFile = gameFromFile;
		_gameModel = gameModel;
        _gameModel._file.Dir = FileSystem.Current.AppDataDirectory;
        _crazyRobotViewModel = crazyRobotViewModel;
		_timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, _) => _gameModel.round();

        _gameModel.GameOver += GameModel_GameOver;

        _crazyRobotViewModel.NewGame += CrazyRobotViewModel_NewGame;
        _crazyRobotViewModel.LoadGame += CrazyRobotViewModel_LoadGame;
        _crazyRobotViewModel.SaveGame += CrazyRobotViewModel_SaveGame;
        _crazyRobotViewModel.PauseGame += CrazyRobotViewModel_PauseGame;

        _storedGamesBrowserModel = new StoredGameBrowserModel(_store);
        _storedGamesBrowserViewModel = new StoredGameBrowserViewModel(_storedGamesBrowserModel);
        _storedGamesBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGamesBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }

    internal void StartTimer() => _timer.Start();
    internal void StopTimer() => _timer.Stop();

    private async void GameModel_GameOver(object? sender, GameEvent e)
    {
        StopTimer();

        if (e.IsWon)
        {
            await DisplayAlert("Crazy Robot játék",
                "Gratulálok, győztél!" + Environment.NewLine +
                "Összesen " + (e.Walls + e.BrokenWalls) + " falat raktál le és " +
                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                "OK");
        }
        else
        {
            await DisplayAlert("Crazy Robot játék", "Sajnálom, vesztettél!", "OK");
        }
    }

    private void CrazyRobotViewModel_NewGame(object? sender, EventArgs e)
    {
        _gameModel.NewGame(_gameModel._table.N);

        StartTimer();
    }
    private void CrazyRobotViewModel_PauseGame(object? sender, EventArgs e)
    {
        if (_timer.IsRunning)
        {
            StopTimer();
            foreach(CrazyRobotField field in _crazyRobotViewModel.Fields)
            {
                field.IsLocked = false;
            } 
        }
        else 
        {
            foreach (CrazyRobotField field in _crazyRobotViewModel.Fields)
            {
                field.IsLocked = true;
            }
            StartTimer(); 
        }
    }

    private async void CrazyRobotViewModel_LoadGame(object? sender, EventArgs e)
    {
        StopTimer();
        await _storedGamesBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new LoadGamePage
        {
            BindingContext = _storedGamesBrowserViewModel
        });
    }

    private async void CrazyRobotViewModel_SaveGame(object? sender, EventArgs e)
    {
        await _storedGamesBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new SaveGamePage
        {
            BindingContext = _storedGamesBrowserViewModel
        });
    }

    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();
        _gameModel._file.Dir = FileSystem.Current.AppDataDirectory;
        try
        {
            await _gameModel.LoadGameAsync(e.Name);


            await DisplayAlert("Crazy Robot játék", "Sikeres betöltés.", "OK");

            StartTimer();
        }
        catch
        {
            await DisplayAlert("Crazy Robot játék", "Sikertelen betöltés.", "OK");
        }
    }
    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();
        StopTimer();
        _gameModel._file.Dir = FileSystem.Current.AppDataDirectory;
        try
        {
            _gameModel.Save(e.Name);
            await DisplayAlert("Crazy Robot játék", "Sikeres mentés.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Crazy Robot játék", "Sikertelen mentés." + ex.Message + System.Environment.NewLine + _gameFromFile.Dir + System.Environment.NewLine + _gameModel._file.Dir, "OK");
        }
    }

}
