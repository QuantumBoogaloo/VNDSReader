using System;

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
    public class StringCharReader : ICharReader
    {
        private int nextIndex = 0;
        private string text;

        public StringCharReader(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            this.text = text;
        }

        public bool HasNext()
        {
            return (this.nextIndex < text.Length);
        }

        public char PeekNext()
        {
            return this.text[this.nextIndex];
        }

        public char ReadNext()
        {
            char result = this.PeekNext();
            ++this.nextIndex;
            return result;
        }
    }
}
