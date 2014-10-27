using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLineFormat(this StringBuilder sb, string text, 
                                                        params object[] args)
        {
            return sb.AppendFormat(text, args).AppendLine();
        }
    }
}
