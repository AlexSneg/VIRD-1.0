
namespace TechnicalServices.Licensing
{
    public class HashCalculator
    {
        public string CalculateHash(string id, string randomSeed)
        {
            char[] result = new char[16];
            string randomizedId = id + randomSeed;

            short sum = 0;
            for (int i = 0; i < randomizedId.Length; i++)
            {
                sum += (byte)randomizedId[i];
            }

            for (short i = 1; i <= 16; i++)
            {
                result[i - 1] = (((sum * i) % 256) % 10).ToString()[0];
            }

            return new System.String(result);
        }
    }
}
