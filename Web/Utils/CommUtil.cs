namespace ShoppingMall.Web.Utils
{
    public static class CommUtil
    {
        public static string GetUserName(string? userName)
        {
            return string.IsNullOrEmpty(userName) ? "訪客" : userName;
        }

        public static string GetUserName(string? userName, string? name)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "訪客";
            }
            return string.IsNullOrEmpty(name) ? userName : $"{userName}({name})";
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringInA_Z(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringIn0_9(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}