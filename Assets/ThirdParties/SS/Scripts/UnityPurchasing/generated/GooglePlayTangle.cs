// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("pyQqJRWnJC8npyQkJfxQHd4j40j7zdvGu5vrPLZCwHRh143y/PNdpFirto8wD5OkxVZpIm2NYOYnHCjh5AOicALbJ6USkrXAAmaNmSX+CnwfkZDiC8oChbCQHXZwKXNbpOZoeFA8i6i+3KWmspedepvqoNCbqza4FackBxUoIywPo22j0igkJCQgJSb+evFv6gM4DaLVbJ38HCVapP6fgM5ZT3HRuh7DjAaRTWHcrVXdnavGXz6Do/EfT4ZiTLTHtS9dame4dGjwKSvu0u/2zXwMlcqN0ukPKn/Cxw4UsUBVN3LepzgEvNHm6Slhgk3GPkef5yLoK82GM3KkTtQCRmfgkFPXXzN2fm9hqh8XXwCQm47QUPQUImEp9pZS3ujgICcmJCUk");
        private static int[] order = new int[] { 1,8,6,4,9,6,8,7,10,12,13,11,13,13,14 };
        private static int key = 37;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
