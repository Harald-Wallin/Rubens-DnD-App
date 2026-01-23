using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Rubens_DnD__project;

public partial class MainPage : ContentPage

{
    
    private List<Character> characters = new List<Character>();
    private Character? selectedCharacter = null;
    public List<Character> GetCharacters() => characters;


    public MainPage()
    {
        InitializeComponent();

        selectedCharacter = null;
        CharacterListView.SelectedItem = null;
        PlayBtn.IsEnabled = false;
        DeleteBtn.IsEnabled = false;
        LoadCharacters();
        UpdateCharacterList();
        NavigationPage.SetHasNavigationBar(this, false);
        // Ingen vald karaktär än
    }

    private async void LoadCharacters()
    {
        CharacterListView.ItemsSource = null; // nollställ först

        characters = await ProfileStorage.LoadProfilesAsync();

        foreach (var c in characters)
            c.IsSelected = false;

        selectedCharacter = null;
        //PlayBtn.IsEnabled = false;
        PlayBtn.IsEnabled = selectedCharacter != null;
        PlayBtn.BackgroundColor = selectedCharacter != null ? Color.FromArgb("#380F00") : Color.FromArgb("#251f1c");
        PlayBtn.TextColor = selectedCharacter != null ? Color.FromArgb("#D89651") : Color.FromArgb("#8e7d76");

        DeleteBtn.IsEnabled = false;

        CharacterListView.ItemsSource = characters; // sätt ItemsSource sist
    }


    // När man trycker på "Ny Profil"
    private async void NewProfileBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewProfilePage(this));
    }

    // När man trycker på "SPELA"
    private async void PlayBtnClicked(object sender, EventArgs e)
    {
        if (selectedCharacter != null)
        {
            await Navigation.PushAsync(new CharacterPage(selectedCharacter));
        }
        else
        {
            await DisplayAlert("Info", "Välj en karaktär först!", "OK");
        }
    }

    // Lägg till ny karaktär (från NewProfilePage)
    public async Task AddCharacter(Character character)
    {
        characters.Add(character);
        UpdateCharacterList();

        await ProfileStorage.SaveProfilesAsync(characters);
    }

    // Uppdatera en karaktär (från CharacterPage)
    public async Task UpdateCharacter(Character updatedCharacter)
    {
        var existing = characters.FirstOrDefault(c => c.Name == updatedCharacter.Name);
        if (existing != null)
        {
            int index = characters.IndexOf(existing);
            characters[index] = updatedCharacter;
        }
        else
        {
            characters.Add(updatedCharacter);
        }
        UpdateCharacterList();

        await ProfileStorage.SaveProfilesAsync(characters);
    }

    // Uppdatera ListView
    private void UpdateCharacterList()
    {
        if (characters != null)
        {
            foreach (var c in characters)
                c.IsSelected = false;

            selectedCharacter = null;
            //PlayBtn.IsEnabled = false;
            PlayBtn.IsEnabled = selectedCharacter != null;
            PlayBtn.BackgroundColor = selectedCharacter != null ? Color.FromArgb("#380F00") : Color.FromArgb("#251f1c");
            PlayBtn.TextColor = selectedCharacter != null ? Color.FromArgb("#D89651") : Color.FromArgb("#8e7d76");

            DeleteBtn.IsEnabled = false;

            // Force refresh
            CharacterListView.ItemsSource = null;
            CharacterListView.ItemsSource = characters;
        }
    }




    // Hantera val av karaktär
    /*private void CharacterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Ta bort native selection
        CharacterListView.SelectedItem = null;

        // Markera manuellt den valda karaktären
        var tappedCharacter = e.CurrentSelection.FirstOrDefault() as Character;
        if (tappedCharacter != null)
        {
            // Avmarkera tidigare
            if (selectedCharacter != null)
                selectedCharacter.IsSelected = false;

            // Markera ny
            selectedCharacter = tappedCharacter;
            selectedCharacter.IsSelected = true;
        }

        PlayBtn.IsEnabled = selectedCharacter != null;
        DeleteBtn.IsEnabled = selectedCharacter != null;
    }*/


    protected override void OnAppearing()
    {
        base.OnAppearing();

        foreach (var c in characters)
            c.IsSelected = false;

        selectedCharacter = null;
        //PlayBtn.IsEnabled = false;
        PlayBtn.IsEnabled = selectedCharacter != null;
        PlayBtn.BackgroundColor = selectedCharacter != null ? Color.FromArgb("#380F00") : Color.FromArgb("#251f1c");
        PlayBtn.TextColor = selectedCharacter != null ? Color.FromArgb("#D89651") : Color.FromArgb("#8e7d76");

        DeleteBtn.IsEnabled = false;

        CharacterListView.ItemsSource = null;
        CharacterListView.ItemsSource = characters;
    }


    //RADERA EN KARAKTÄR
    private async void DeleteBtnClicked(object sender, EventArgs e)
    {
        if (selectedCharacter != null)
        {
            var deletePage = new DeleteProfilePage(selectedCharacter, this);
            await Navigation.PushModalAsync(deletePage);
        }
    }


    public async Task RemoveCharacter(Character character)
    {
        if (characters.Contains(character))
        {
            characters.Remove(character);
            UpdateCharacterList();
            await ProfileStorage.SaveProfilesAsync(characters);
        }
    }

    private async void CharacterTapped(object sender, EventArgs e)
    {
        if (sender is Border border && border.BindingContext is Character tappedCharacter)
        {
            foreach (var c in characters)
                c.IsSelected = false;

            tappedCharacter.IsSelected = true;
            selectedCharacter = tappedCharacter;

            //PlayBtn.IsEnabled = true;
            PlayBtn.IsEnabled = selectedCharacter != null;
            PlayBtn.BackgroundColor = selectedCharacter != null ? Color.FromArgb("#380F00") : Color.FromArgb("#251f1c");
            PlayBtn.TextColor = selectedCharacter != null ? Color.FromArgb("#D89651") : Color.FromArgb("#8e7d76");

            DeleteBtn.IsEnabled = true;

            await border.FadeTo(0.7, 100); // fade out
            await border.FadeTo(1, 100);   // fade in

        }
    }



}


