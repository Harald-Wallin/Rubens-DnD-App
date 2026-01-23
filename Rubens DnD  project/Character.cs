using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Rubens_DnD__project
{
    public class Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Level { get; set; } = 1;

        public int Strength { get; set; } = 0;
        public int Dexterity { get; set; } = 0;
        public int Intellect { get; set; } = 0;
        public int Charisma { get; set; } = 0;
        public int Spirit { get; set; } = 0;

        public int Armor { get; set; } = 0;
        public int MaxHP { get; set; } = 10;
        public int CurrentHP { get; set; } = 10;
        public int WizardLevel { get; set; } = 0;

        public string RaceClassLevel => $"{Race} {Class}, lvl {Level}";

        // Inventory / Abilities / Notes
        public string Notes { get; set; } = "";
        public ObservableCollection<Item> Inventory { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> Abilities { get; set; } = new ObservableCollection<Item>();

        // Inspiration
        public bool[] InspirationStatus { get; set; } = new bool[3];

        // Den modifierade IsSelected
        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Display => $"{Name} ({Description})";

        public override bool Equals(object obj)
        {
            if (obj is not Item other) return false;
            return Name == other.Name && Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
    }
}


