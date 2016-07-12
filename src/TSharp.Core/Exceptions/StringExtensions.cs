namespace System
{
    /// <summary>
    /// Class StringExtensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Fills the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns>System.String.</returns>
        public static string Fill(this string format, params object[] args)
        {
            if (null == args)
                return format;
            switch (args.Length)
            {
                case 0:
                    return format;
                case 1:
                    return string.Format(format, args[0]);
                case 2:
                    return string.Format(format, args[0], args[1]);
                case 3:
                    return string.Format(format, args[0], args[1], args[2]);
                default:
                    return string.Format(format, args);
            }
        }


    }
}
