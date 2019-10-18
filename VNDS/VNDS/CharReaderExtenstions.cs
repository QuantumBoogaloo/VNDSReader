using System.Collections.Generic;
using System.Text;

//
//  Copyright (C) 2019 Pharap (@Pharap)
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

namespace VNDS
{
    public static class CharReaderExtenstions
    {
        public static string ReadUntilAny(this ICharReader reader, params char[] charset)
        {
            return ReadUntilAny(reader, new HashSet<char>(charset));
        }

        public static string ReadUntilAny(this ICharReader reader, IEnumerable<char> charset)
        {
            return ReadUntilAny(reader, new HashSet<char>(charset));
        }

        public static string ReadUntilAny(this ICharReader reader, ISet<char> charset)
        {
            while (true)
            {
                if (!reader.HasNext())
                    return null;

                var next = reader.PeekNext();
                if (!charset.Contains(next))
                    break;

                reader.ReadNext();
            }

			var result = new StringBuilder();

            while (reader.HasNext())
			{
                var next = reader.ReadNext();
				if(charset.Contains(next))
					break;

				result.Append(next);
			}

			return result.ToString();
        }

        public static bool TryReadUntilAny(this ICharReader reader, out string result, params char[] charset)
        {
            return TryReadUntilAny(reader, out result, new HashSet<char>(charset));
        }

        public static bool ReadUntilAny(this ICharReader reader, out string result, IEnumerable<char> charset)
        {
            return TryReadUntilAny(reader, out result, new HashSet<char>(charset));
        }

        public static bool TryReadUntilAny(this ICharReader reader, out string result, ISet<char> charset)
        {
            while (true)
            {
                if (!reader.HasNext())
                {
                    result = string.Empty;
                    return false;
                }

                var next = reader.PeekNext();
                if (!charset.Contains(next))
                    break;

                reader.ReadNext();
            }

            var builder = new StringBuilder();

            while (reader.HasNext())
            {
                var next = reader.ReadNext();
                if (charset.Contains(next))
                    break;

                builder.Append(next);
            }

            result = builder.ToString();
            return true;
        }
    }
}
