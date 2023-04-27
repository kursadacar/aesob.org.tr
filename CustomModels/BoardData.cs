using System.Collections.Generic;

namespace aesob.org.tr.CustomModels
{
    public class BoardData
    {
        private static List<BoardData> _all = new List<BoardData>()
        {
            new BoardData("Yönetim Kurulu", 0),
            new BoardData("Denetim Kurulu", 1),
            new BoardData("Disiplin Kurulu", 2)
        };

        public static IEnumerable<BoardData> All
        {
            get
            {
                return _all;
            }
        }

        public string Name { get; private set; }
        public int ID { get; private set; }

        public BoardData(string name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
