namespace Domain.Cards
{
    public struct NaturalNumbersCards : ICardStore
    {
         private KeyValuePair<string, int>[] _cards;

        public NaturalNumbersCards()
        {
            _cards = new KeyValuePair<string, int>[12] {
                new KeyValuePair<string, int>("0",1),
                new KeyValuePair<string, int>("1",2),
                new KeyValuePair<string, int>("2",3),
                new KeyValuePair<string, int>("3",4),
                new KeyValuePair<string, int>("4",5),
                new KeyValuePair<string, int>("5",6),
                new KeyValuePair<string, int>("6",7),
                new KeyValuePair<string, int>("7",8),
                new KeyValuePair<string, int>("8",9),
                new KeyValuePair<string, int>("9",10),
                new KeyValuePair<string, int>("10",11),
                new KeyValuePair<string, int>("?",12)
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