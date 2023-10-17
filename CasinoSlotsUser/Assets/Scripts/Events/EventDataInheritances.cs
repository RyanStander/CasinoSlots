namespace Events
{
    public class SendScore : EventData
    {
        public readonly int Score;
        public SendScore(int score):base(EventIdentifiers.SendScore)
        {
               Score = score;
        }
    }

    public class BossSummon : EventData
    {
        public BossSummon():base(EventIdentifiers.BossSummon)
        {
        }
    }
}
