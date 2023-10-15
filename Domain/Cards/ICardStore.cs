using System.Collections.Generic;

namespace Domain.Cards
{
    public interface ICardStore
    {
        string[] getCardsKey();
        KeyValuePair<string, int>[] getCards();
    }
}