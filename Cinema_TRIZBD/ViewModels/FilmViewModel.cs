using System.Linq;

namespace Cinema_TRIZBD.ViewModels
{
    public class FilmViewModel
    {
        public Films Film { get; }

        public FilmViewModel(Films film)
        {
            Film = film;
        }

        // Все свойства для отображения
        public string Title => Film.Title;
        public string Image => Film.Image;
        public string StoryLine => Film.StotyLine;
        public string DurationString => $"{Film.Duration} мин";
        public string GenresString => Film.Genres != null
            ? string.Join(", ", Film.Genres.Select(g => g.Title))
            : "жанры не указаны";
    }
}
