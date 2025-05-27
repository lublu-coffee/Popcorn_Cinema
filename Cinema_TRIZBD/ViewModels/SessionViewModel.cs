namespace Cinema_TRIZBD.ViewModels
{
    public class SessionViewModel
    {
        public Sessions Session { get; }
        public string CinemaName { get; }
        public string HallName { get; }
        public string Date => Session.Data;
        public string Time => Session.Time;
        public string Price => $"{Session.Price}";

        public SessionViewModel(Sessions session)
        {
            Session = session;

            // Загружаем связанные данные
            var db = My_CinemaEntities.GetContext();
            db.Entry(session).Reference(s => s.Halls).Load();
            db.Entry(session.Halls).Reference(h => h.Cinemas).Load();

            CinemaName = session.Halls.Cinemas.Title;
            HallName = session.Halls.Title;
        }
    }
}