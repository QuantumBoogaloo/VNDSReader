using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

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

namespace VNDS.Commands
{
    public abstract class Command
    {
        private static readonly ParseException[] empty = new ParseException[0];
        private static readonly ReadOnlyCollection<ParseException> emptyWrapper = new ReadOnlyCollection<ParseException>(empty);

        private ParseException[] exceptions;
        private ReadOnlyCollection<ParseException> exceptionsWrapper;

        protected Command()
        {
            this.exceptions = empty;
            this.exceptionsWrapper = emptyWrapper;
        }

        protected Command(IEnumerable<ParseException> exceptions)
        {
            var exceptionsArray = exceptions.ToArray();
            if (exceptionsArray.Length > 0)
            {
                this.exceptions = exceptionsArray;
                this.exceptionsWrapper = new ReadOnlyCollection<ParseException>(this.exceptions);
            }
            else
            {
                this.exceptions = empty;
                this.exceptionsWrapper = emptyWrapper;
            }
        }

        public ReadOnlyCollection<ParseException> Exception
        {
            get { return this.exceptionsWrapper; }
        }

        public bool IsErroneous
        {
            get { return (this.exceptions.Length > 0); }
        }

        public abstract void Accept(CommandVisitor visitor);
    }
}
