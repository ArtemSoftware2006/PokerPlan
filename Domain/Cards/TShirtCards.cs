namespace Domain.Cards
{
    public struct TShirtCards : ICardStore
    {
        private KeyValuePair<string, int>[] _cards;

        public TShirtCards()
        {
            _cards = new KeyValuePair<string, int>[7] {
                new KeyValuePair<string, int>("XS",1),
                new KeyValuePair<string, int>("S",2),
                new KeyValuePair<string, int>("M",3),
                new KeyValuePair<string, int>("L",4),
                new KeyValuePair<string, int>("XL",5),
                new KeyValuePair<string, int>("XXL",6),
                new KeyValuePair<string, int>("?",0)
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