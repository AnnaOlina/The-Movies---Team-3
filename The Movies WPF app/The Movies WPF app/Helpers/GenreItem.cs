using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Helpers
{
    // Hjælpeklasse til genrevalg
    public class GenreItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public MovieGenre Genre { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
