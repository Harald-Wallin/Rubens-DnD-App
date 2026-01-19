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
        UpdateCharacterList();
        LoadCharacters();
        PlayBtn.IsEnabled = false;
        NavigationPage.SetHasNavigationBar(this, false);
        // Ingen vald karaktär än
    }

    private async void LoadCharacters()
    {
        characters = await ProfileStorage.LoadProfilesAsync();
        CharacterListView.ItemsSource = characters;
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
        CharacterListView.ItemsSource = null;
        CharacterListView.ItemsSource = characters;
    }

    // Hantera val av karaktär
    private void CharacterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedCharacter = e.CurrentSelection.FirstOrDefault() as Character;

        PlayBtn.IsEnabled = selectedCharacter != null;
        DeleteBtn.IsEnabled = selectedCharacter != null;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UpdateCharacterList();
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





}


