using System;

namespace GitDemo
{
    class Class1
    {
        private static readonly Random _random = new Random();
        private static readonly string[] _laughs = { "ха-ха-ха", "хи-хи-хи", "хо-хо-хо", "хе-хе-хе", "ху-ху-ху" };

        public string GenerateLaugh()
        {
            int index = _random.Next(_laughs.Length);
            return _laughs[index];
        }
    }
}
