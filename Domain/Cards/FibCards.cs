namespace Domain.Cards
{
    public struct FibCards : ICardStore
    {
        private KeyValuePair<string, int>[] _cards;

        public FibCards()
        {
            _cards = new KeyValuePair<string, int>[11] {
                new KeyValuePair<string, int>("0",1),
                new KeyValuePair<string, int>("1/2",2),
                new KeyValuePair<string, int>("1",3),
                new KeyValuePair<string, int>("2",4),
                new KeyValuePair<string, int>("3",5),
                new KeyValuePair<string, int>("5",6),
                new KeyValuePair<string, int>("8",7),
                new KeyValuePair<string, int>("13",8),
                new KeyValuePair<string, int>("21",9),
                new KeyValuePair<string, int>("34",10),
                new KeyValuePair<string, int>("?",11)
            };
        }

        public KeyValuePair<string, int>[] getCards()
        {
            return _cards;
        }

        public string[] getCardsKey()
        {
            return _cards.Select(x => x.Key).ToArray();
        }
    }
}