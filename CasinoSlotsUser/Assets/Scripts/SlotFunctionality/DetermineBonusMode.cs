namespace SlotFunctionality
{
    public static class DetermineBonusMode
    {
        public static BonusMode GetBonusMode(string matchName)
        {
            return matchName switch
            {
                "BossSummon" => BonusMode.BossBattle,
                _ => BonusMode.None
            };
        }
    }
}
